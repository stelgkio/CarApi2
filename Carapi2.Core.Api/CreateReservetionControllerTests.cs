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
using CarApi2.Application.Features.Reservations.Commands;

namespace NetCore.API.UnitTests.Controllers;

public class CreateReservetionControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    public CreateReservetionControllerTests(WebApplicationFactory<Startup> fixure)
    {
        _client = fixure.CreateClient();
    }

    [Fact]
    public void ReservetionController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(ReservationController)
            .Should()
            .BeDerivedFrom<ApiController>();
    }


    [Fact]
    public void CreateReservetion_Should_BeDecoratedWith_HttpUpdateAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ReservationController)
            .Methods()
            .First(x => x.Name == "CreateReservation")
            .Should()
            .BeDecoratedWith<HttpPostAttribute>(attribute => attribute.Template == "/v1/reservation/");
    }

    [Fact]
    public void CreateReservetion_Should_BeDecoratedWith_ProducesResponseTypeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ReservationController)
            .Methods()
            .First(x => x.Name == "CreateReservation")
            .Should()
            .BeDecoratedWith<ProducesResponseTypeAttribute>(x=>x.StatusCode.Equals(200));
    }


    [Fact]
    public async Task CreateReservationReqeust_Should_Return_InternalServerError()
    {
        // Arrange

        string payload = JsonConvert.SerializeObject(new CreateReservationReqeust {ReservationDate= new DateTime(2099,1,15,10,0,0) ,Duration=60 });

        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync("/v1/reservation", httpContent);


        // Assert
        response.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.InternalServerError);

    }


    [Fact]
    public async Task CreateReservationReqeust_Should_Return_CreateReservationCommandResponse_Success()
    {
        // Arrange

        string carPayload = JsonConvert.SerializeObject(new CreateCarReqeust { Mark = "test", Model = "testModel" });
        // Act
        var carHttpContent = new StringContent(carPayload, Encoding.UTF8, "application/json");
        var carResponse = await _client.PostAsync("/v1/car", carHttpContent);

        carResponse.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);


        string payload = JsonConvert.SerializeObject(new CreateReservationReqeust { ReservationDate = new DateTime(2099, 1, 15, 10, 0, 0), Duration = 60 });

        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync("/v1/reservation", httpContent);
        var readstream = await response.Content.ReadAsStringAsync();
        CreateReservationCommandResponse data = JsonConvert.DeserializeObject<CreateReservationCommandResponse>(readstream);

        // Assert
        response.StatusCode
         .Should()
         .Be(System.Net.HttpStatusCode.OK);

        data.Should().NotBeNull();

    }

    [Fact]
    public async Task CreateReservationReqeust_Should_Return_NoAvailabetime()
    {
        // Arrange

        string carPayload = JsonConvert.SerializeObject(new CreateCarReqeust { Mark = "test", Model = "testModel" });
        // Act
        var carHttpContent = new StringContent(carPayload, Encoding.UTF8, "application/json");
        var carResponse = await _client.PostAsync("/v1/car", carHttpContent);

        carResponse.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);


        string payload = JsonConvert.SerializeObject(new CreateReservationReqeust { ReservationDate = new DateTime(2099, 1, 15, 10, 0, 0), Duration = 60 });

        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync("/v1/reservation", httpContent);
        var readstream = await response.Content.ReadAsStringAsync();
        CreateReservationCommandResponse data = JsonConvert.DeserializeObject<CreateReservationCommandResponse>(readstream);

        // Assert
        response.StatusCode
         .Should()
         .Be(System.Net.HttpStatusCode.OK);

        data.Should().NotBeNull();


        // Same time this will return notfound
        string payload2 = JsonConvert.SerializeObject(new CreateReservationReqeust { ReservationDate = new DateTime(2099, 1, 15, 10, 0, 0), Duration = 60 });

        var httpContent2 = new StringContent(payload2, Encoding.UTF8, "application/json");
        // Act
        var response2 = await _client.PostAsync("/v1/reservation", httpContent2);

        // Assert
        response2.StatusCode
         .Should()
         .Be(System.Net.HttpStatusCode.NotFound);

        

    }

}
