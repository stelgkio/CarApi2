using CarApi2.Domain.Common;
using System;

namespace CarApi2.Domain.Entities
{
    public class Reservation : Entity
    {
        public DateTime ReservationDate { get; private set; }
        public TimeSpan StartDuration { get; private set; }
        public TimeSpan EndDuration { get; private set; }

        public long CarId { get; private set; }
        public virtual Car Car { get; private set; }

        public Reservation(DateTime reservationDate, TimeSpan startDuration, TimeSpan endDuration)
        {
            CheckDuration(startDuration, endDuration);
            ValidateDatePeriod(reservationDate);
            ReservationDate = reservationDate;
            StartDuration = startDuration;
            EndDuration = endDuration;
        }

        public Reservation(DateTime reservationDate, TimeSpan startDuration, TimeSpan endDuration, long carId)
        {
            CheckDuration(startDuration, endDuration);
            ValidateDatePeriod(reservationDate);
            ReservationDate = reservationDate;
            StartDuration = startDuration;
            EndDuration = endDuration;
            CarId = carId;

        }

        public Reservation WithCarId(long carId)
        {
            CarId = carId;
            return this;
        }

        public void CheckDuration(TimeSpan startDuration, TimeSpan endDuration)
        {
            if (endDuration - startDuration > new TimeSpan(2, 0, 0))
            {
                throw new ArgumentException("Time of duration is grater than 2 hours");
            }
        }

        public void ValidateDatePeriod(DateTime reservationDate)
        {
            if (reservationDate.Date == DateTime.UtcNow.Date || reservationDate.Date < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Invalid date of reservation");
            }
        }
        public Reservation()
        {

        }
    }
}
