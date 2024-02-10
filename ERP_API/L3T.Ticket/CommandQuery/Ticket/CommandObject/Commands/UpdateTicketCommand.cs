using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.Ticket.CommandQuery.Ticket.CommandObject.Commands
{
    public class UpdateTicketCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public TicketEntry TicketEntry { get; set; }

        public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, bool>
        {
            private readonly ITicketService _context;
            public UpdateTicketCommandHandler(ITicketService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(UpdateTicketCommand command, CancellationToken cancellationToken)
            {
                var ticketobj = await _context.GetTicketById(command.Id);
                if (ticketobj != null)
                {
                    ticketobj.brCliCode = command.TicketEntry.brCliCode;
                    ticketobj.brSlNo = command.TicketEntry.brSlNo;
                    ticketobj.MqID = command.TicketEntry.MqID;
                }
                var ticket = await _context.UpdateTicket(command.Id, ticketobj);
                return ticket;
            }
        }
    }
}
