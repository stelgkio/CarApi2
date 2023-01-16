using System;
using CarApi2.Application.Common;

namespace CarApi2.Services.System
{
   
    public class DateTimeService: IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public TimeSpan ConvertMinToTimeSpan(int mins)
        { 
            if(mins <= 0)
            {
                throw new ArgumentOutOfRangeException("Duration can not be negative or zero");
            }
            var time = TimeSpan.FromMinutes(mins);
            
            return time;
        }

        public TimeSpan GetEndOfReservation(TimeOnly start, int mins)
        {
           
            return new TimeSpan(start.Hour, start.Minute, start.Second) + ConvertMinToTimeSpan(mins);
        }

        public TimeSpan GetTimeSpan(TimeOnly timeOnly)
        {
           return new TimeSpan(timeOnly.Hour, timeOnly.Minute, timeOnly.Second);
        }

        
    }
}
