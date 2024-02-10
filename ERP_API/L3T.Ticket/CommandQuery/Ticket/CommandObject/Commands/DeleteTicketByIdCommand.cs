using L3T.Infrastructure.Helpers.Interface.Ticket;
using MediatR;

namespace L3T.Ticket.CommandQuery.Ticket.CommandObject.Commands
{
    public class DeleteTicketByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public class DeleteTicketByIdCommandHandler : IRequestHandler<DeleteTicketByIdCommand, bool>
        {
            private readonly ITicketService _context;
            public DeleteTicketByIdCommandHandler(ITicketService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(DeleteTicketByIdCommand command, CancellationToken cancellationToken)
            {
                var ticket = await _context.DeleteTicket(command.Id);
                return ticket;
            }
        }
    }
}
