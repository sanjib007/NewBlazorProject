using L3T.Infrastructure.Helpers.Interface.Location;
using MediatR;

namespace L3T.Location.CommandQuery.Command
{
    public class DeleteZoneByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public class DeleteZoneByIdCommandHandler : IRequestHandler<DeleteZoneByIdCommand, bool>
        {
            private readonly ILocationService _context;
            public DeleteZoneByIdCommandHandler(ILocationService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(DeleteZoneByIdCommand command, CancellationToken cancellationToken)
            {
                var brand = await _context.DeleteZone(command.Id);
                return brand;
            }
        }
    }
}
