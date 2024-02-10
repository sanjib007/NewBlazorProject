using L3T.Infrastructure.Helpers.Interface.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class DeleteSupportOffice : IRequest<bool>
    {
        public int Id { get; set; }
        public class DeleteSupportOfficeHandler : IRequestHandler<DeleteSupportOffice, bool>
        {
            private readonly IBtsService _context;
            public DeleteSupportOfficeHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(DeleteSupportOffice command, CancellationToken cancellationToken)
            {
                var supportOffice = await _context.DeleteSupportOffice(command.Id);
                return supportOffice;
            }
        }
    }
}
