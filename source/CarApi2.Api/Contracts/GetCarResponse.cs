using System.ComponentModel.DataAnnotations;

namespace CarApi2.Api.Contracts
{
    public class GetCarResponse
    {
        /// <example>1</example>       
        public long Id { get; private set; }

        /// <example>cars name</example>        
        public string Mark { get; private set; }

        /// <example>cars model</example>
        public string Model { get; private set; }

        public GetCarResponse(long id, string mark, string model)
        {
            Id = id;
            Mark = mark;
            Model = model;
        }

        ///// <summary>
        ///// Represents the categories contained in the cars
        ///// </summary>
        //public List<Category> Categories { get; private set; }


    }


}
