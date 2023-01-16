using CarApi2.Domain.Entities;
using System;

namespace CarApi2.Api.Contracts
{
    public class ReservationResponse
    {
        public DateTime ReservationDate { get;  set; }
        public TimeSpan StartDuration { get;  set; }
        public TimeSpan EndDuration { get;  set; }
        public string CarName { get;  set; }
    }
}
