using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CarApi2.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CarApi2.Application.Common
{
    public interface ICarReservationDbContext
    {
        DbSet<Reservation> Reservations { get; set; }

        DbSet<Car> Cars { get; set; }

        Task<Car> GetExistingCar(string mark, string model, CancellationToken token);
        Task<Car> GetCarbyId(long Id, CancellationToken token);
       
        Task<Car> AddCarAsync(Car car, CancellationToken token);

        Task<int> SaveChangesAsync(CancellationToken token);

        Task<IEnumerable<Car>> GetAllCars(CancellationToken token);
        Task<IEnumerable<Reservation>> GetAllReservations(CancellationToken token);

        Task<List<Reservation>> FindReservations(DateTime ReservationDate,long CarId , CancellationToken token);
        Task<Reservation> AddReservationAsync(Reservation car, CancellationToken token);

    }
}