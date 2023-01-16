using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CarApi2.Application.Common;

namespace CarApi2.Persistence.Database
{
    public static class CarReservationDbContextSeed
    {
        public static async Task SeedBaseDataAsync(ICarReservationDbContext context)
        {
            await context.Cars.AddAsync(new Domain.Entities.Car("Tesla","Model x"));
            await context.Cars.AddAsync(new Domain.Entities.Car("Ferrary", "Monza"));
            await context.Cars.AddAsync(new Domain.Entities.Car("BMW", "320i "));

            await context.SaveChangesAsync(default);

            await context.Reservations.AddAsync(new Domain.Entities.Reservation(
                DateTime.ParseExact("2099-01-19", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                new TimeSpan(12, 0, 0), 
                new TimeSpan(13, 0, 0), 1));
            await context.Reservations.AddAsync(new Domain.Entities.Reservation(
                DateTime.ParseExact("2099-01-19", "yyyy-MM-dd", CultureInfo.InvariantCulture), 
                new TimeSpan(13, 0, 0), 
                new TimeSpan(15, 0, 0), 2));
            await context.SaveChangesAsync(default);

        }
    }
}
