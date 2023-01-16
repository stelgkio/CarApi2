using CarApi2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carapi2.Domain.Test
{
    public class CarTest
    {
        [Fact]
        public void Car_Create_Object_Success()
        {
            Car car = new Car(mark:"Test", model:"Model");
            Assert.NotNull(car);
            Assert.IsType<Car>(car);
            Assert.Equal("Test",car.Mark);
            Assert.Equal("Model", car.Model);
        }

        [Fact]
        public void Car_Create_Object_WithMark()
        {
            Car car = new Car(mark: "Test", model: "Model");
            car.WithMark("Test2");
            Assert.NotNull(car);
            Assert.IsType<Car>(car);
            Assert.Equal("Test2", car.Mark);
            Assert.Equal("Model", car.Model);
        }

        [Fact]
        public void Car_Create_Object_WithModel()
        {
            Car car = new Car(mark: "Test", model: "Model");
            car.WithModel("Model2");
            Assert.NotNull(car);
            Assert.IsType<Car>(car);
            Assert.Equal("Test", car.Mark);
            Assert.Equal("Model2", car.Model);
        }

        [Fact]
        public void Car_Create_Object_WithModel_Throws()
        {
            Car car = new Car(mark: "Test", model: "Model");
            Assert.Throws<ArgumentNullException>(()=> car.WithModel(""));
            
        }
        [Fact]
        public void Car_Create_Object_WithMark_Throws()
        {
            Car car = new Car(mark: "Test", model: "Model");
            Assert.Throws<ArgumentNullException>(() => car.WithMark(""));

        }
    }
}
