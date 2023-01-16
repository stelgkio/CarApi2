using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarApi2.Application.Common;
using CarApi2.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarApi2.Application.Features.Cars.Commands
{
    public class GetCarsByIdQuery : BaseCqrsRequest<Car>
    {
        public GetCarsByIdQuery(long Id)
            : base()
        {
            this.Id = Id;    
        }
        public long Id { get; set; }
    }

    public class GetCarsByIdQueryHandler : IRequestHandler<GetCarsByIdQuery, Car>
    {
        private readonly ICarReservationDbContext _context;
        private readonly ILogger<GetCarsByIdQueryHandler> _logger;
        public GetCarsByIdQueryHandler(ICarReservationDbContext context, ILogger<GetCarsByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Car> Handle(GetCarsByIdQuery request, CancellationToken cancellationToken)
        {
            Car result = await _context.GetCarbyId(request.Id,cancellationToken);
            if (result is null)
            {
                _logger.LogError($"Car with id={request.Id} does not exist ");
                throw new ArgumentNullException(nameof(result), $"Car with id={request.Id} does not exist ");
            }
            return result;
        }
    }
}
