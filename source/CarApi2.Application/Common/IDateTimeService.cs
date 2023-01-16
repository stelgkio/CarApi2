using System;

namespace CarApi2.Application.Common
{
    public interface IDateTimeService
    {
        DateTime Now { get; }

        TimeSpan GetTimeSpan(TimeOnly timeOnly);

        TimeSpan GetEndOfReservation(TimeOnly start,int mins);
    }
}
