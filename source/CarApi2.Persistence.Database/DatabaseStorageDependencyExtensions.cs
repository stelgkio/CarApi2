using CarApi2.Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarApi2.Persistence.Database
{
    public static class DatabaseStorageDependencyExtensions
    {
        public static IServiceCollection AddDbStorage(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<CarReservationDbContext>(options =>
                    options.UseInMemoryDatabase("CarApi2Db"));
            }
            else
            {
                services.AddDbContext<CarReservationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("CarReservationDbConnection"),
                        b => b.MigrationsAssembly(typeof(CarReservationDbContext).Assembly.FullName)));
            }

            services.AddScoped<ICarReservationDbContext>(provider => provider.GetService<CarReservationDbContext>());

            return services;
        }
    }
}
