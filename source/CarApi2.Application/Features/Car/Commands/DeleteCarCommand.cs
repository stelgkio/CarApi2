using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarApi2.Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarApi2.Application.Features.Cars.Commands
{
    public class DeleteCarCommand : BaseCqrsRequest<bool>
    {
        public DeleteCarCommand(long Id)
            : base()
        {
            this.Id = Id;
        }
        public long Id { get; set; }
    }

    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, bool>
    {
        private readonly ICarReservationDbContext _context;
        private readonly ILogger<DeleteCarCommandHandler> logger;
        public DeleteCarCommandHandler(ICarReservationDbContext context, ILogger<DeleteCarCommandHandler> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _context.GetCarbyId( request.Id, cancellationToken);
            if (car is null)
            throw new ArgumentNullException(nameof(car), $"Car with id={request.Id} does not exist ");


            try
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return false;
            }

            logger.LogInformation($"Entity deleted: {car.Id}");
            return true;
        }


    }
}
