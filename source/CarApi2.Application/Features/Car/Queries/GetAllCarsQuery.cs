using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarApi2.Application.Common;
using CarApi2.Domain.Entities;
using MediatR;

namespace CarApi2.Application.Features.Cars.Commands
{
    public class GetAllCarsQuery : BaseCqrsRequest<IEnumerable<Car>>
    {
        public GetAllCarsQuery()
            : base()
        {
        }
    }

    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, IEnumerable<Car>>
    {
        private readonly ICarReservationDbContext _context;

        public GetAllCarsQueryHandler(ICarReservationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        => await _context.GetAllCars(cancellationToken);
        
    }
}
