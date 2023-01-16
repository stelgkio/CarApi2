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

    public class UpdateCarCommand : BaseCqrsRequest<bool>
    {

        public UpdateCarCommand(long id, string mark, string model) : base()
        {
            Mark = mark;
            Model = model;
            Id= id;
        }

        public string Mark { get; set; }
        public string Model { get; set; }
        public long Id { get; set; }


    }

    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, bool>
    {
        private readonly ICarReservationDbContext _context;
        private readonly ILogger<UpdateCarCommandHandler> _logger;

        public UpdateCarCommandHandler(ICarReservationDbContext context, ILogger<UpdateCarCommandHandler> logger)
        {
            _context = context;           
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            Car car = await _context.GetCarbyId(request.Id,cancellationToken);

            if (car is null)
            {
                _logger.LogError($"Car with id={request.Id} does not exist");
                throw new ArgumentNullException(nameof(car), $"Car with id={request.Id} does not exist");
            }

            car.WithMark(request.Mark)
               .WithModel(request.Model);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
