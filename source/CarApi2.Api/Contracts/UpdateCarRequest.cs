using System.ComponentModel.DataAnnotations;

namespace CarApi2.Api.Contracts
{
    public class UpdateCarsRequest
    {
        ///<example>cars name</example>
        [Required]
        public string Mark { get; set; }

        ///<example>car model</example>
        [Required]
        public string Model { get; set; }

      
    }

}
