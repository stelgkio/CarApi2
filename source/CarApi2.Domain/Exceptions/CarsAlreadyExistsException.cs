using System;
using System.Collections.Generic;
using System.Text;

namespace CarApi2.Domain.Exceptions
{
    [Serializable]
    public class CarsAlreadyExistsException : Exception
    {

        public CarsAlreadyExistsException() : base(String.Format("Car already exist"))
        {

        }
    }
}
