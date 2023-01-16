using CarApi2.Application.Common;
using CarApi2.Application.Features.Cars.Commands;
using CarApi2.Application.Features.Reservations.Commands;
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
    public class ReservetionCommandHandlerTest
    {

        [Fact]
        public async Task CreateReservation_Handler_Test_ThrowException()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var timeServiceMock = new Mock<IDateTimeService>();
            var logger = new Mock<ILogger<CreateReservationCommandHandler>>();
            var handler = new CreateReservationCommandHandler(repoMock.Object, logger.Object, timeServiceMock.Object);
            Car car = new Car("Test", "TestModel");

            var data = new CreateReservationCommand(new DateTime(2023, 1, 15, 10, 0, 0), 60);

            repoMock.Setup(c => c.GetAllCars(CancellationToken.None));
               
                

            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(data, CancellationToken.None));
            repoMock.Verify(v=>v.SaveChangesAsync(CancellationToken.None),Times.Never());
        }

        [Fact]
        public async Task CreateReservation_Handler_Test_Success()
        {
            var repoMock = new Mock<ICarReservationDbContext>();
            var timeServiceMock = new Mock<IDateTimeService>();
            var logger = new Mock<ILogger<CreateReservationCommandHandler>>();
            var handler = new CreateReservationCommandHandler(repoMock.Object, logger.Object, timeServiceMock.Object);

            Car car = new Car("Test", "TestModel");
            IEnumerable<Car> cars= new List<Car>() { car };
            List<Reservation> reservations = new List<Reservation>() ;
            var data = new CreateReservationCommand(new DateTime(2023, 1, 15, 10, 0, 0), 60);

            timeServiceMock.Setup(x=>x.GetTimeSpan(It.IsAny<TimeOnly>())).Returns(TimeSpan.FromMinutes(400));
            timeServiceMock.Setup(x => x.GetEndOfReservation(It.IsAny<TimeOnly>(),It.IsAny<int>())).Returns(TimeSpan.FromMinutes(450));
            repoMock.Setup(c => c.GetAllCars(CancellationToken.None)).Returns(async () => await Task.FromResult(cars));
            repoMock.Setup(c => c.FindReservations(It.IsAny<DateTime>(), It.IsAny<long>(),CancellationToken.None))
                .Returns(async () => await Task.FromResult(reservations));
            repoMock.Setup(c => c.AddReservationAsync(It.IsAny<Reservation>(), CancellationToken.None));

            CreateReservationCommandResponse result = await handler.Handle(data, CancellationToken.None);
            Assert.Equal("Test TestModel", result.CarNameModel);
            Assert.NotNull(result.EndDuration);
            Assert.NotNull(result.StartDuration);
            Assert.NotNull(result.ReservationDate);
            
            repoMock.Verify(v => v.SaveChangesAsync(CancellationToken.None), Times.Once());
        }

    }
}
