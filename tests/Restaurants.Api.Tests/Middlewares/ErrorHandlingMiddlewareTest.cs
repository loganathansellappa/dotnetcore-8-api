using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.Api.Tests.Middlewares;

[TestSubject(typeof(ErrorHandlingMiddleware))]
public class ErrorHandlingMiddlewareTest
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly ErrorHandlingMiddleware _middleware;
    private readonly DefaultHttpContext _httpContext;
    
    public ErrorHandlingMiddlewareTest()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _middleware = new ErrorHandlingMiddleware(_loggerMock.Object);
        _httpContext = new DefaultHttpContext();
        _httpContext.Response.Body = new MemoryStream();
    }

  
    [Fact]
    public async Task InvokeAsync_ShouldCallNextMiddlewareWithoutExceptions()
    {
        // Arrange
        var next = new Mock<RequestDelegate>();

        // Act
        await _middleware.InvokeAsync(_httpContext, next.Object);

        // Assert
        next.Verify(next => next.Invoke(_httpContext), Times.Once);
    }
    
    
    private async Task<string> GetResponseBodyAsync()
    {
        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(_httpContext.Response.Body);
        return await reader.ReadToEndAsync();
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleNotFoundException()
    {
        // Arrange
        RequestDelegate next = _ => throw new NotFoundException("Resource not found", "100");

        // Act
        await _middleware.InvokeAsync(_httpContext, next);
        var responseBody = await GetResponseBodyAsync();

        // Assert
        _httpContext.Response.StatusCode.Should().Be(404);
       
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleForbiddenException()
    {
        // Arrange
        RequestDelegate next = _ => throw new ForbiddenException("Access denied", "100");

        // Act
        await _middleware.InvokeAsync(_httpContext, next);
        var responseBody = await GetResponseBodyAsync();

        // Assert
        _httpContext.Response.StatusCode.Should().Be(403);
    
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleGeneralException()
    {
        // Arrange
        RequestDelegate next = _ => throw new Exception("Unexpected error");

        // Act
        await _middleware.InvokeAsync(_httpContext, next);
        var responseBody = await GetResponseBodyAsync();

        // Assert
        _httpContext.Response.StatusCode.Should().Be(500);
    
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNextMiddlewareWithoutExceptions_()
    {
        // Arrange
        RequestDelegate next = _ => Task.CompletedTask;

        // Act
        await _middleware.InvokeAsync(_httpContext, next);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(200);
        _httpContext.Response.Body.Length.Should().Be(0); // No response body
        _loggerMock.VerifyNoOtherCalls();
    }
}