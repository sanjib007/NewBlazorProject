using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.Ticket.CommandQuery.Ticket.CommandObject.Queries
{
    public class GetAllTicketQuery : IRequest<IEnumerable<TicketEntry>>
    {
        public class GetAllTicketQueryHandler : IRequestHandler<GetAllTicketQuery, IEnumerable<TicketEntry>>
        {
            private readonly ITicketService _context;
            public GetAllTicketQueryHandler(ITicketService context)
            {
                _context = context;
            }
            public async Task<IEnumerable<TicketEntry>> Handle(GetAllTicketQuery query, CancellationToken cancellationToken)
            {
                var productList = await _context.GetAllTicket();
                if (productList == null)
                {
                    return null;
                }
                return productList.AsReadOnly();
            }
        }
    }
}
