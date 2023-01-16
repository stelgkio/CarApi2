using CarApi2.Api.Contracts;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carapi2.Core.Api
{
    public class UpdateCarsRequestTest
    {
        [Fact]
        public void UpdateCarRequestTest_Model_Validation()
        {
            // Arrange
            // Act
            // Assert
            typeof(UpdateCarsRequest)
                .Properties()
                .First(x => x.Name == "Mark")
                .Should()
                .BeDecoratedWith<RequiredAttribute>();
            typeof(UpdateCarsRequest)
                     .Properties()
                     .First(x => x.Name == "Mark")
                     .Should()
                     .Return<string?>();
            typeof(UpdateCarsRequest)
                .Properties()
                .First(x => x.Name == "Model")
                .Should()
                .BeDecoratedWith<RequiredAttribute>();

            typeof(UpdateCarsRequest)
                   .Properties()
                   .First(x => x.Name == "Model")
                   .Should()
                   .Return<string?>();

           
                  
        }
    }
}
