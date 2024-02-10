using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetBrandByIdQuery : IRequest<Brand>
    {
        public long Id { get; set; }
        public class GetBrandByIdHandler : IRequestHandler<GetBrandByIdQuery, Brand>
        {
            private readonly IBtsService _context;
            public GetBrandByIdHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<Brand> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
            {
                var brand = await _context.GetBrandById(query.Id);
                if (brand == null) 
                    return null;
                else
                return brand;
            }
        }
    }
}
