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
    public class GetCarResponseTest
    {
        [Fact]
        public void GetCarResponseTest_Model_Validation()
        {
            // Arrange
            // Act
            // Assert            
            typeof(GetCarResponse)
                     .Properties()
                     .First(x => x.Name == "Mark")
                     .Should()
                     .Return<string?>();

            typeof(GetCarResponse)
                   .Properties()
                   .First(x => x.Name == "Model")
                   .Should()
                   .Return<string?>();
            typeof(GetCarResponse)
                  .Properties()
                  .First(x => x.Name == "Id")
                  .Should()
                  .Return<long>();          
      
            // Away to test constructo arguments type and order 
            ConstructorInfo constructor = typeof(GetCarResponse).GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                       null, new Type[] { typeof(long), typeof(string), typeof(string) }, null);

            constructor.GetParameters().Count().Should().Be(3);

            foreach (var parameter in constructor.GetParameters())
            {
                if (parameter.Name == "id")
                {
                    parameter.Position.Should().Be(0);
                    parameter.ParameterType.Should().Be(typeof(long));
                }
                else if (parameter.Name == "model")
                {
                    parameter.Position.Should().Be(2);
                    parameter.ParameterType.Should().Be(typeof(string));
                }
                else if (parameter.Name == "mark")
                {
                    parameter.Position.Should().Be(1);
                    parameter.ParameterType.Should().Be(typeof(string));
                }
                else
                {
                    parameter.Name.Should().NotMatch(parameter.Name);
                }
            }
        }
    }
}
