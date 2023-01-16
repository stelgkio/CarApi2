using CarApi2.Application.Common;
using CarApi2.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarApi2.Application.Features.Reservations.Commands
{
    public class CreateReservationCommand : BaseCqrsRequest<CreateReservationCommandResponse>
    {
        private DateTime time;

        public CreateReservationCommand(DateTime reservationDate, int duration) : base()
        {
            ReservationDate = reservationDate;
            Duration = duration;
            TimeOfReservation = TimeOnly.FromDateTime(reservationDate);

        }

        public DateTime ReservationDate { get; private set; }
        public TimeOnly TimeOfReservation { get; private set; }
        public int Duration { get; private set; }


    }

    public class CreateReservationCommandResponse
    {
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartDuration { get; set; }
        public TimeSpan EndDuration { get; set; }
        public string CarNameModel { get; set; }

    }
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationCommandResponse>
    {
        private readonly ICarReservationDbContext _context;
        private readonly ILogger<CreateReservationCommandHandler> logger;
        private readonly IDateTimeService _dateTimeService;

        public CreateReservationCommandHandler(ICarReservationDbContext DbContext, ILogger<CreateReservationCommandHandler> logger, IDateTimeService dateTimeService)
        {
            _context = DbContext;
            this.logger = logger;
            _dateTimeService = dateTimeService;
        }

        public async Task<CreateReservationCommandResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {

            var reservation = new Reservation(request.ReservationDate, _dateTimeService.GetTimeSpan(request.TimeOfReservation),
               _dateTimeService.GetEndOfReservation(request.TimeOfReservation, request.Duration)
               );
            var cars = await _context.GetAllCars(cancellationToken);

            if (cars.Count() == 0)
            {
                logger.LogError("Wihtout cars we can not make new reservation");
                throw new ArgumentNullException(nameof(cars), "Wihtout cars we can not make new reservation");
            }

            foreach (var item in cars.OrderBy(x => x.Id))
            {
                var allReservations = await _context.FindReservations(reservation.ReservationDate, item.Id, cancellationToken);

                if (allReservations.Count() == 0)
                {
                    reservation.WithCarId(item.Id);
                   await _context.AddReservationAsync(reservation,cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new CreateReservationCommandResponse
                    {
                        CarNameModel = item.Mark + " " + item.Model,
                        EndDuration = reservation.EndDuration,
                        ReservationDate = reservation.ReservationDate,
                        StartDuration = reservation.StartDuration,
                    };
                }


                for (var i = 0; i < allReservations.Count(); i++)
                {
                    var endTimeDefault = new TimeSpan(23, 59, 59);
                    var startTimeDefault = TimeSpan.Zero;

                    if (allReservations.ElementAtOrDefault(i + 1) != null && allReservations.ElementAtOrDefault(i - 1) != null)
                    {
                        if (reservation.StartDuration >= allReservations[i - 1].EndDuration && reservation.EndDuration <= allReservations[i + 1].StartDuration)
                        {
                            reservation.WithCarId(item.Id);
                            await _context.AddReservationAsync(reservation, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            return new CreateReservationCommandResponse
                            {
                                CarNameModel = item.Mark + " " + item.Model,
                                EndDuration = reservation.EndDuration,
                                ReservationDate = reservation.ReservationDate,
                                StartDuration = reservation.StartDuration,
                            };
                        }

                    }
                    if (allReservations.ElementAtOrDefault(i + 1) == null && allReservations.ElementAtOrDefault(i - 1) == null)
                    {
                        if (reservation.StartDuration >= allReservations[i].EndDuration || reservation.EndDuration <= allReservations[i].StartDuration)
                        {
                            reservation.WithCarId(item.Id);
                            await _context.AddReservationAsync(reservation, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            return new CreateReservationCommandResponse
                            {
                                CarNameModel = item.Mark + " " + item.Model,
                                EndDuration = reservation.EndDuration,
                                ReservationDate = reservation.ReservationDate,
                                StartDuration = reservation.StartDuration,
                            };
                        }

                    }


                    if (allReservations.ElementAtOrDefault(i - 1) == null)
                    {
                        if (reservation.StartDuration >= startTimeDefault && reservation.EndDuration <= allReservations[i].StartDuration)
                        {
                            reservation.WithCarId(item.Id);
                            await _context.AddReservationAsync(reservation, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            return new CreateReservationCommandResponse
                            {
                                CarNameModel = item.Mark + " " + item.Model,
                                EndDuration = reservation.EndDuration,
                                ReservationDate = reservation.ReservationDate,
                                StartDuration = reservation.StartDuration,
                            };
                        }

                    }

                    if (allReservations.ElementAtOrDefault(i + 1) == null)
                    {
                        if (reservation.StartDuration >= allReservations[i].EndDuration && reservation.EndDuration <= endTimeDefault)
                        {
                            reservation.WithCarId(item.Id);
                            await _context.AddReservationAsync(reservation, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            return new CreateReservationCommandResponse
                            {
                                CarNameModel = item.Mark + " " + item.Model,
                                EndDuration = reservation.EndDuration,
                                ReservationDate = reservation.ReservationDate,
                                StartDuration = reservation.StartDuration,
                            };
                        }

                    }

                };
            };
            logger.LogInformation($"There is no availabe car for reservation for date: {reservation.ReservationDate} start: {reservation.StartDuration} end:{reservation.EndDuration}  ");
            return null;
        }



    }
}
