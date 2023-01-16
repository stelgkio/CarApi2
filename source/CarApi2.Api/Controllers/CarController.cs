using CarApi2.Api.Contracts;
using CarApi2.Api.Features;
using CarApi2.Application.Features.Cars.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarApi2.Api.Controllers
{
    public class CarController : ApiController
    {
        public CarController()
        {

        }
        /// <summary>
        /// Create a cars
        /// </summary>
        /// <remarks>Adds a cars</remarks>
        /// <param name="body">cars being created</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("/v1/car/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCar([Required][FromBody] CreateCarReqeust body)
        {
            var command = new AddNewCarCommand(body.Mark, body.Model);
            string result = await Mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get list of cars
        /// </summary>         
        /// <response code="201">Resource</response>
        /// <response code="400">Bad Request</response>        
        [HttpGet("/v1/cars/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(IEnumerable<GetCarResponse>), 200)]
        public async Task<IActionResult> GetAllCars()
        {
            var command = new GetAllCarsQuery();
            var result = await Mediator.Send(command);

            return Ok(result.Select(x=> new GetCarResponse(x.Id,x.Mark,x.Model)).ToArray());
        }

        /// <summary>
        /// Get a cars
        /// </summary>
        /// <remarks>By passing the cars id, you can get access to available reservations </remarks>
        /// <param name="id">cars id</param>
        /// <response code="200">cars</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpGet("/v1/car/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetCarResponse), 200)]
        public async Task<IActionResult> GetCar([FromRoute][Required] long id)
        {

            var command = new GetCarsByIdQuery(id);
            var result = await Mediator.Send(command);

            if (result == null)
                return NotFound();

            return Ok(new GetCarResponse ( result.Id,result.Mark, result.Model));
        }

        /// <summary>
        /// Update a cars
        /// </summary>
        /// <remarks>Update a cars with new information</remarks>
        /// <param name="id">cars id</param>
        /// <param name="body">car being updated</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/car/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCar([FromRoute][Required] long id, [FromBody] UpdateCarsRequest body)
        {
            var command = new UpdateCarCommand(id,body.Mark,body.Model);
            bool result = await Mediator.Send(command);

            if (result == false)
                return NotFound();

            return StatusCode(204);
        }

        /// <summary>
        /// Removes a car with all its reservation 
        /// </summary>
        /// <remarks>Remove a car</remarks>
        /// <param name="id">car id</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/car/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCar([FromRoute][Required] long id)
        {
            var command = new DeleteCarCommand(id);
            var result = await Mediator.Send(command);
            return StatusCode(204);
        }
    }
}
