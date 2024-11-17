using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Restaurants.API.Controllers;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Api.Tests.Controllers;

[TestSubject(typeof(RestaurantsController))]
public class RestaurantsControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;

    public RestaurantsControllerTest(WebApplicationFactory<Program> factory)
    {
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                    _ => _restaurantsRepositoryMock.Object));
            });
        });
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
    public async Task GetAll_ForValidRequest_Return400Error()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/api/Restaurants");

        // assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest); //400()
    }

    [Fact]
    public async Task GetById_ForNonExistentId_Return404()
    {
        //arrange
        var id = 1;
        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant)null); //()
        var client = _factory.CreateClient();

        //act
        var response = await client.GetAsync($"/api/Restaurants/{id}");
        
        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetById_ForExistingtId_Return200()
    {
        //arrange
        var id = 1;
        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "test",
            Description = "test",
        };
        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant); //()
        var client = _factory.CreateClient();

        //act
        var response = await client.GetAsync($"/api/Restaurants/{id}");
        var responseContent = await response.Content.ReadFromJsonAsync<RestaurantDto>();
        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Name.Should().Be(restaurant.Name);
    }
}