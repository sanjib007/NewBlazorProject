/*
using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.MessageBroker.Models.Ticket;
using MassTransit;

namespace L3T.Ticket.Consumers.MessageBroker
{
    public class TicketEntryConsumer : IConsumer<MB_TicketEntry>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TicketEntryConsumer> _logger;
        private readonly ITicketService _context;

        public TicketEntryConsumer(
            IConfiguration configuration,
            ILogger<TicketEntryConsumer> logger,
            ITicketService context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<MB_TicketEntry> context)
        {
            var message = context.Message;
            //var abc = message.brSlNo;

            throw new NotImplementedException();
        }
    }
}
*/
