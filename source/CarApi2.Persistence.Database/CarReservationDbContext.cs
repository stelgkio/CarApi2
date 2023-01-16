using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CarApi2.Application.Common;
using CarApi2.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CarApi2.Persistence.Database
{
    public class CarReservationDbContext : DbContext, ICarReservationDbContext
    {
        private IDbContextTransaction _currentTransaction;
        ILogger<CarReservationDbContext> logger;
        public CarReservationDbContext(DbContextOptions<CarReservationDbContext> options, ILogger<CarReservationDbContext> logger) :
            base(options)
        {
            this.logger = logger;
        }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Car> Cars { get; set; }

        public async Task<Car> GetExistingCar(string mark, string model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(mark) || string.IsNullOrWhiteSpace(model))
            {
                return null;
            }
            return await Cars.Where(x => x.Model == model && x.Mark == mark).FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<Car> AddCarAsync(Car car, CancellationToken token)
        {
            var data = await Cars.AddAsync(car,token);

          return data.Entity;
        }

        public async Task<Car> GetCarbyId(long Id, CancellationToken token)
        {
            return await Cars.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<Car>> GetAllCars(CancellationToken token)
        => await Cars.Include(x => x.Reservations).ToArrayAsync(cancellationToken: token);

        public async Task<IEnumerable<Reservation>> GetAllReservations(CancellationToken token)
         => await Reservations.ToArrayAsync(cancellationToken: token);

        public async Task<List<Reservation>> FindReservations(DateTime ReservationDate, long CarId, CancellationToken token)
        {
           return await Reservations.Where(x => x.ReservationDate.Date == ReservationDate.Date && x.CarId == CarId).OrderBy(x => x.StartDuration).ToListAsync();
        }

        public async Task<Reservation> AddReservationAsync(Reservation car, CancellationToken token)
        {
            var data = await Reservations.AddAsync(car, token);

            return data.Entity;
        }
    }
}
