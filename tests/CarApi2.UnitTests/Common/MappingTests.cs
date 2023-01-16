using System;
using AutoMapper;
using CarApi2.Application.Features.Forecast;
using CarApi2.Application.Features.Forecast.Commands;
using CarApi2.Domain.Entities.Forecasts;
using NUnit.Framework;

namespace CarApi2.UnitTests.Common
{
    internal class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WeatherForecastMappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(AddNewForecastCommand), typeof(WeatherForecast))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source, "User");

            _mapper.Map(instance, source, destination);
        }
    }
}
