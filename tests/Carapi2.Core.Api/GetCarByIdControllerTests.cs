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
using CarApi2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace NetCore.API.UnitTests.Controllers;

public class GetCarByIdControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    public GetCarByIdControllerTests(WebApplicationFactory<Startup> fixure)
    {
        _client = fixure.CreateClient();
    }

    [Fact]
    public void CarController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(CarController)
            .Should()
            .BeDerivedFrom<ApiController>();
    }


    [Fact]
    public void GetCarById_Should_BeDecoratedWith_HttpGetAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CarController)
            .Methods()
            .First(x => x.Name == "GetCar")
            .Should()
            .BeDecoratedWith<HttpGetAttribute>(attribute => attribute.Template == "/v1/car/{id}");
    }

    [Fact]
    public void GetCarById_Should_BeDecoratedWith_ProducesResponseTypeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CarController)
            .Methods()
            .First(x => x.Name == "GetCar")
            .Should()
            .BeDecoratedWith<ProducesResponseTypeAttribute>(x=>x.StatusCode.Equals(200));

     
        typeof(CarController)
            .Methods()
            .First(x => x.Name == "GetCar")
            .Should()
            .BeDecoratedWith<ProducesResponseTypeAttribute>(x => x.Type== typeof(GetCarResponse));
    }

    [Fact]
    public async Task GetCarById_Should_Return_IntervalServerError()
    {
        // Arrange

        string payload = JsonConvert.SerializeObject(new CreateCarReqeust { Mark = "test", Model = "testModel" });

        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/v1/car", httpContent);
        response.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.OK);
        // Act
        var carAll = await _client.GetAsync("/v1/car/10");
        // Assert
        carAll.StatusCode
             .Should()
             .Be(System.Net.HttpStatusCode.InternalServerError);


    }

    [Fact]
    public async Task GetCarById_Should_Return_Car()
    {
        // Arrange

        string payload = JsonConvert.SerializeObject(new CreateCarReqeust { Mark = "test", Model = "testModel" });

        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/v1/car", httpContent);
        response.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.OK);
        // Act
        var carAll = await _client.GetAsync("/v1/car/1");
        // Assert
        carAll.StatusCode
             .Should()
             .Be(System.Net.HttpStatusCode.OK);
        var data = await carAll.Content.ReadAsStringAsync();

        GetCarResponse car = JsonConvert.DeserializeObject<GetCarResponse>(data);
        
       car.Should().NotBeNull();
       car.Mark.Should().Contain("test");        
       car.Model.Should().Contain("testModel");


    }


   
}
