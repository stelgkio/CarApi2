using CarApi2.Application.Common;
using CarApi2.Application.Features.Cars.Commands;
using CarApi2.Domain.Entities;
using CarApi2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CorApi2.Application.Test
{
    public class CarCommandHandlerTest
    {

        [Fact]
        public async Task CreateReservation_Handler_Test_ThrowException()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<AddNewCarCommandHandler>>();
            var handler = new AddNewCarCommandHandler(repoMock.Object, logger.Object);
            Car car = new Car("Test", "TestModel");

            var data = new AddNewCarCommand("Test", "TestModel");

           repoMock.Setup( c =>c.GetExistingCar(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .Returns(async () => await Task.FromResult(car));
                

            await Assert.ThrowsAsync<CarsAlreadyExistsException>(() => handler.Handle(data, CancellationToken.None));
            repoMock.Verify(v=>v.SaveChangesAsync(CancellationToken.None),Times.Never());
        }

        [Fact]
        public async Task ACreateReservation_Handler_Handler_Test_Success()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<AddNewCarCommandHandler>>();
            var handler = new AddNewCarCommandHandler(repoMock.Object, logger.Object);

            Car car = new Car("Test", "TestModel");
            var data = new AddNewCarCommand("Test", "TestModel");

            repoMock.Setup(c => c.GetExistingCar(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None));
            repoMock.Setup(c => c.AddCarAsync(It.IsAny<Car>(), CancellationToken.None)).Returns(async ()=> await Task.FromResult(car));

            string carId = await handler.Handle(data, CancellationToken.None);
            Assert.Equal("C_0", carId);
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Fact]
        public async Task DeleteCarCommandHandler_Handler_Test_ThrowException()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<DeleteCarCommandHandler>>();
            var handler = new DeleteCarCommandHandler(repoMock.Object, logger.Object);
            Car car = new Car("Test", "TestModel");

            var data = new DeleteCarCommand(1);

            repoMock.Setup(c => c.GetCarbyId(It.IsAny<long>(), CancellationToken.None));               


            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(data, CancellationToken.None));
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Never());
        }

        [Fact]
        public async Task DeleteCarCommandHandler_Handler_Test_Success()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<DeleteCarCommandHandler>>();
            var handler = new DeleteCarCommandHandler(repoMock.Object, logger.Object);            

            var data = new DeleteCarCommand(1);
            Car car = new Car("Test", "TestModel");
          

            repoMock.Setup(c => c.GetCarbyId(It.IsAny<long>(), CancellationToken.None)).Returns(async () => await Task.FromResult(car));
            repoMock.Setup(c => c.Cars.Remove(It.IsAny<Car>()));

            bool isDeleted = await handler.Handle(data, CancellationToken.None);
            Assert.True(isDeleted);
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Fact]
        public async Task UpdateCarCommandHandler_Handler_Test_ThrowException()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<UpdateCarCommandHandler>>();
            var handler = new UpdateCarCommandHandler(repoMock.Object, logger.Object);
            Car car = new Car("Test", "TestModel");

            var data = new UpdateCarCommand(1,"Test", "TestModel");

            repoMock.Setup(c => c.GetCarbyId(It.IsAny<long>(), CancellationToken.None));


            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(data, CancellationToken.None));
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Never());
        }

        [Fact]
        public async Task UpdateCarCommandCommandHandler_Handler_Test_Success()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<UpdateCarCommandHandler>>();
            var handler = new UpdateCarCommandHandler(repoMock.Object, logger.Object);

            var data = new UpdateCarCommand(1, "Test", "TestModel");
            Car car = new Car("Test", "TestModel");


            repoMock.Setup(c => c.GetCarbyId(It.IsAny<long>(), CancellationToken.None)).Returns(async () => await Task.FromResult(car));
            

            bool isDeleted = await handler.Handle(data, CancellationToken.None);
            Assert.True(isDeleted);
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Once());
        }



        [Fact]
        public async Task GetCarsByIdCommandHandler_Handler_Test_ThrowException()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<UpdateCarCommandHandler>>();
            var handler = new UpdateCarCommandHandler(repoMock.Object, logger.Object);
            Car car = new Car("Test", "TestModel");

            var data = new UpdateCarCommand(1, "Test", "TestModel");

            repoMock.Setup(c => c.GetCarbyId(It.IsAny<long>(), CancellationToken.None));


            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(data, CancellationToken.None));
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Never());
        }

        [Fact]
        public async Task GetCarsByIdCommandHandler_Handler_Test_Success()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var logger = new Mock<ILogger<GetCarsByIdQueryHandler>>();
            var handler = new GetCarsByIdQueryHandler(repoMock.Object, logger.Object);

            var data = new GetCarsByIdQuery(1);
            Car car = new Car("Test", "TestModel");

            repoMock.Setup(c => c.GetCarbyId(It.IsAny<long>(), CancellationToken.None)).Returns(async () => await Task.FromResult(car));

            Car result = await handler.Handle(data, CancellationToken.None);
            Assert.NotNull(result);
            
        }


        [Fact]
        public async Task GetCarsCommandHandler_Handler_Test_ReturnNull()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var handler = new GetAllCarsQueryHandler(repoMock.Object);
            Car car = new Car("Test", "TestModel");

            var data = new GetAllCarsQuery();

            IEnumerable<Car> result = await handler.Handle(data, CancellationToken.None);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCarsCommandHandler_Handler_Test_ArrayOfCar()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var handler = new GetAllCarsQueryHandler(repoMock.Object);
            Car car = new Car("Test", "TestModel");

            List<Car> list = new() { car };
            var data = new GetAllCarsQuery();

            repoMock.Setup(c => c.GetAllCars(CancellationToken.None)).Returns(async ()=> await Task.FromResult(list.AsEnumerable()));

            IEnumerable<Car> result = await handler.Handle(data, CancellationToken.None);
            Assert.NotEmpty(result);
            Assert.Single(result);
        }
    }
}
