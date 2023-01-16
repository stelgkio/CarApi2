using CarApi2.Api.Contracts;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carapi2.Core.Api
{
    public class CreaeCardRequestTest
    {
        [Fact]
        public void CreaeCardRequestTest_Model_Validation()
        {
            // Arrange
            // Act
            // Assert
            typeof(CreateCarReqeust)
                .Properties()
                .First(x => x.Name == "Mark")
                .Should()
                .BeDecoratedWith<RequiredAttribute>();
            typeof(CreateCarReqeust)
                     .Properties()
                     .First(x => x.Name == "Mark")
                     .Should()
                     .Return<string?>();
            typeof(CreateCarReqeust)
                .Properties()
                .First(x => x.Name == "Model")
                .Should()
                .BeDecoratedWith<RequiredAttribute>();

            typeof(CreateCarReqeust)
                   .Properties()
                   .First(x => x.Name == "Model")
                   .Should()
                   .Return<string?>();
        }
    }
}
