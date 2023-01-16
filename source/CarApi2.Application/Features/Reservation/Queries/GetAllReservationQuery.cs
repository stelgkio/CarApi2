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
    public class GetAllReservationsQuery : BaseCqrsRequest<IEnumerable<Reservation>>
    {
        public GetAllReservationsQuery()
            : base()
        {
        }
    }

    public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, IEnumerable<Reservation>>
    {
        private readonly ICarReservationDbContext _context;

        public GetAllReservationsQueryHandler(ICarReservationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        => await _context.GetAllReservations(cancellationToken);
        
    }
}
