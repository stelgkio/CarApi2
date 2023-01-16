using System;
using System.Linq;
using CarApi2.Api.Features;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace NetCore.API.UnitTests.Controllers;

public class ApiControllerTests
{
    [Fact]
    public void ApiControllerBase_Should_BeDerivedFrom_ControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiController)
            .Should()
            .BeDerivedFrom<ControllerBase>();
    }

  
    [Fact]
    public void ApiController_Should_BeDecoratedWith_ApiControllerAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiController)
            .Should()
            .BeDecoratedWith<ApiControllerAttribute>();
    }

  

}
