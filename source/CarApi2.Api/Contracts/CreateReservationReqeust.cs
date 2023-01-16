using System;
using System.ComponentModel.DataAnnotations;

namespace CarApi2.Api.Contracts
{
    /// <summary>
    /// Request model used by CreateReservationReqeust api endpoint
    /// </summary>
    public class CreateReservationReqeust
    {
        ///Date of the reservation
        [Required]
        public DateTime ReservationDate { get; set; }
        
        ///Duration of the reservation in min
        [Required]        
        public int Duration { get; set; }



    }
}
