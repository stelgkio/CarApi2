using CarApi2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carapi2.Domain.Test
{
    public class ReservationTest
    {

        [Fact]
        public void Reservation_Create_Object_Success()
        {
            Reservation res = new Reservation(reservationDate: DateTime.Now.AddDays(1), startDuration: new TimeSpan(10, 0, 0), endDuration: new TimeSpan(11, 0, 0));
            Assert.NotNull(res);
            Assert.IsType<Reservation>(res);

        }

        [Fact]
        public void Reservation_Create_Object_withCardId()
        {
            Reservation res = new Reservation(reservationDate: DateTime.Now.AddDays(1), startDuration: new TimeSpan(10, 0, 0), endDuration: new TimeSpan(11, 0, 0));
            res.WithCarId(1);
            Assert.NotNull(res);
            Assert.IsType<Reservation>(res);
            Assert.Equal(1, res.CarId);


        }

        [Fact]
        public void Reservation_Create_Object_withBadDate()
        {
           Assert.Throws< ArgumentException >(()=> new Reservation(reservationDate: DateTime.Now, startDuration: new TimeSpan(10, 0, 0), endDuration: new TimeSpan(11, 0, 0)));
         


        }
        [Fact]
        public void Reservation_Create_Object_withBadDuration()
        {
            Assert.Throws<ArgumentException>(() => new Reservation(reservationDate: DateTime.Now.AddDays(1), startDuration: new TimeSpan(10, 0, 0), endDuration: new TimeSpan(13, 0, 0)));



        }
    }
}
