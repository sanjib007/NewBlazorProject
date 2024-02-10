using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.Ticket.CommandQuery.Ticket.CommandObject.Queries
{
    public class GetTicketByIdQuery : IRequest<TicketEntry>
    {
        public long Id { get; set; }
        public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdQuery, TicketEntry>
        {
            private readonly ITicketService _context;
            public GetTicketByIdHandler(ITicketService context)
            {
                _context = context;
            }
            public async Task<TicketEntry> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _context.GetTicketById(query.Id);
                if (product == null) return null;
                return product;
            }
        }
    }
}
