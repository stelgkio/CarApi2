using System;
using System.Reflection;
using System.Threading.Tasks;
using CarApi2.Persistence.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CarApi2.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<CarReservationDbContext>();
                    if (context.Database.IsSqlServer())
                    {
                        await context.Database.MigrateAsync();
                    }

                    await CarReservationDbContextSeed.SeedBaseDataAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, serilog) =>
                {
                    serilog
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("CarApi2", Assembly.GetEntryAssembly()?.GetName().Version);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.ConfigureAppConfiguration((context, configuration) =>
                    {
                        if (args != null)
                            configuration.AddCommandLine(args);

                        var env = context.HostingEnvironment;

                        configuration
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                                reloadOnChange: true);

                        configuration.AddEnvironmentVariables();
                    });
                });

    }
}
