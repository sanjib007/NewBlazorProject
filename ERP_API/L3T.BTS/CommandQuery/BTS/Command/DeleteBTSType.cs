using L3T.Infrastructure.Helpers.Interface.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class DeleteBTSType : IRequest<bool>
    {
        public int Id { get; set; }
        public class DeleteBTSTypeHandler : IRequestHandler<DeleteBTSType, bool>
        {
            private readonly IBtsService _context;
            public DeleteBTSTypeHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(DeleteBTSType command, CancellationToken cancellationToken)
            {
                var btsType = await _context.DeleteBTSType(command.Id);
                return btsType;
            }
        }
    }
}
