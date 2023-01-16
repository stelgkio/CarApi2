using System.Reflection;
using AutoMapper;
using CarApi2.Application.Common;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CarApi2.Api.Infrastructure
{
    public static class ApplicationDependencyExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assemblyList = new[]
            {
                typeof(BaseCqrsRequest<>).Assembly,
                Assembly.GetExecutingAssembly()
            };

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(assemblyList);


            return services;
        }
    }
}
