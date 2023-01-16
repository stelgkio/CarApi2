using AutoMapper;
using CarApi2.Application.Common;
using CarApi2.Domain.Entities;
using CarApi2.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarApi2.Application.Features.Cars.Commands
{

    public class AddNewCarCommand : BaseCqrsRequest<string>
    {

        public AddNewCarCommand(string mark, string model) : base()
        {
            Mark = mark ;
            Model = model;
        }

        public string Mark { get; set; }
        public string Model { get; set; }


    }

    public class AddNewCarCommandHandler : IRequestHandler<AddNewCarCommand, string>
    {
        private readonly ICarReservationDbContext _context;
        private readonly ILogger<AddNewCarCommandHandler> logger;

        public AddNewCarCommandHandler(ICarReservationDbContext DbContext, ILogger<AddNewCarCommandHandler> logger)
        {
            _context = DbContext;
            this.logger = logger;
        }

        public async Task<string> Handle(AddNewCarCommand request, CancellationToken cancellationToken)
        {

            var insertCar = new Car(request.Mark, request.Model);
            Car car = await _context.GetExistingCar(request.Mark, request.Model, CancellationToken.None);

            if (car != null)
            {
                this.logger.LogError($"Car already exist");
                throw new CarsAlreadyExistsException();
            }

            Car entityEntry = await _context.AddCarAsync(insertCar, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entityEntry.CarId.ToString();
        }
    }

}
