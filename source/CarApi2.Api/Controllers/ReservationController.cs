using CarApi2.Api.Contracts;
using CarApi2.Api.Features;
using CarApi2.Application.Features.Cars.Commands;
using CarApi2.Application.Features.Reservations.Commands;
using CarApi2.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarApi2.Api.Controllers
{
    public class ReservationController : ApiController
    {
        public ReservationController()
        {

        }
        /// <summary>
        /// Create a reserevation
        /// </summary>
        /// <remarks>Adds new reserevation</remarks>
        /// <param name="body">reservation being created</param>
        /// <response code="200">Reservation details</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("/v1/reservation/")]
        [ProducesResponseType(typeof(CreateReservationCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReservation([Required][FromBody] CreateReservationReqeust body)
        {
            var command = new CreateReservationCommand(body.ReservationDate,body.Duration);
            var result = await Mediator.Send(command);
            if(result is null)
            {
                return StatusCode(404, new { message= "There was not availble car for this serten time" });
            }
            return Ok(result);
        }

        /// <summary>
        /// Get list of reservations
        /// </summary>         
        /// <response code="201">Resource</response>
        /// <response code="400">Bad Request</response>        
        [HttpGet("/v1/reservation/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(IEnumerable<Reservation>), 200)]
        public async Task<IActionResult> GetAllReservasions()
        {
            var command = new GetAllReservationsQuery();
            var result = await Mediator.Send(command);

            return Ok(result);
        }

    }
}
