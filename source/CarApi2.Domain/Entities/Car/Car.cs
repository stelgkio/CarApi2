using CarApi2.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarApi2.Domain.Entities
{
    public class Car : IAgrregatesRoot<long>
    {
        public long Id { get; protected set; }
        public string Mark { get; private set; }
        public string Model { get; private set; }
        public string CarId { get { return "C_" + Id.ToString(); } private set { } }
        // best way is to add trigger in the database
        public virtual ICollection<Reservation> Reservations { get; private set; }
        public Car(string mark, string model)
        {
            Mark = mark;
            Model = model;
           
        }

        public Car WithMark(string mark)
        {
            if(string.IsNullOrEmpty(mark))
                throw new ArgumentNullException(nameof(mark),"Mark value should not be null or empty"); 
            this.Mark = mark;
            return this;
        }

        public Car WithModel(string model)
        {
            if (string.IsNullOrEmpty(model))
                throw new ArgumentNullException(nameof(model), "Model value should not be null or empty");
            this.Model = model;
            return this;
        }
    }
}
