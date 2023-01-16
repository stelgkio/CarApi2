using CarApi2.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace CarApi2.Services.System
{
    public static class SystemServicesDependencyExtensions
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}
