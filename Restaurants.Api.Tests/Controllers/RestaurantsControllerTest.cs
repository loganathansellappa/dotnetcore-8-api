using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Restaurants.API.Controllers;
using Xunit;

namespace Restaurants.Api.Tests.Controllers;

[TestSubject(typeof(RestaurantsController))]
public class RestaurantsControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RestaurantsControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ForValidRequest_Return200Ok()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/api/Restaurants?pageNumber=1&pageSize=10");

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK); //200()
    }
    
    [Fact]
    public async Task GetAll_ForValidRequest_Return400Ok()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/api/Restaurants");

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest); //400()
    }
}