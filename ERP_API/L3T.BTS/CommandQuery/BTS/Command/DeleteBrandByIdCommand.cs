using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class DeleteBrandByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public class DeleteBrandByIdCommandHandler : IRequestHandler<DeleteBrandByIdCommand, bool>
        {
            private readonly IBtsService _context;
            public DeleteBrandByIdCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(DeleteBrandByIdCommand command, CancellationToken cancellationToken)
            {
                var brand = await _context.DeleteBrand(command.Id);
                return brand;
            }
        }
    }
}
