using System.ComponentModel.DataAnnotations;

namespace CarApi2.Api.Contracts
{
    /// <summary>
    /// Request model used by Createcars api endpoint
    /// </summary>
    public class CreateCarReqeust
    {
        /// <example>Name of cars created</example>
        [Required]
        public string Mark { get; set; }

        /// <example>Description of cars created</example>
        [Required]
        public string Model { get; set; }

      
      
    }
}
