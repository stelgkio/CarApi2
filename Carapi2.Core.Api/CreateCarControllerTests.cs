using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using CarApi2.Api.Features;
using CarApi2.Api.Controllers;
using CarApi2.Api.Contracts;
using CarApi2.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using NSubstitute.ExceptionExtensions;
using CarApi2.Domain.Exceptions;

namespace NetCore.API.UnitTests.Controllers;

public class CreateCarControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    public CreateCarControllerTests(WebApplicationFactory<Startup> fixure)
    {
        _client = fixure.CreateClient();
    }

    [Fact]
    public void AddcarsItemController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(CarController)
            .Should()
            .BeDerivedFrom<ApiController>();
    }


    [Fact]
    public void CreatecarsItem_Should_BeDecoratedWith_HttpPostAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CarController)
            .Methods()
            .First(x => x.Name == "CreateCar")
            .Should()
            .BeDecoratedWith<HttpPostAttribute>(attribute => attribute.Template == "/v1/car/");
    }

    [Fact]
    public void CreatecarsItem_Should_BeDecoratedWith_ProducesResponseTypeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CarController)
            .Methods()
            .First(x => x.Name == "CreateCar")
            .Should()
            .BeDecoratedWith<ProducesResponseTypeAttribute>();
    }


    [Fact, Trait("CreateCar", "Integration")]
    public async Task CreateCar_Should_Return_StatusCreated()
    {
        // Arrange
        // Act
      
        string payload = JsonConvert.SerializeObject(new CreateCarReqeust { Mark = "test", Model = "testModel" });

        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/v1/car", httpContent);
        // Assert
        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);
    }


    [Fact, Trait("CreateCar", "Integration")]
    public async Task CreateCar_Should_Return_InternalServerError()
    {
        // Arrange        
        string payload = JsonConvert.SerializeObject(new CreateCarReqeust { Mark = "test", Model = "testModel" });
        // Act
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/v1/car", httpContent);

        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);
      
        var response2 = await _client.PostAsync("/v1/car", httpContent);
        // Assert
        response2.StatusCode
         .Should()
         .Be(System.Net.HttpStatusCode.InternalServerError);
        
    }
}
