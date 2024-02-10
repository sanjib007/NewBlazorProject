using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;

namespace L3T.Location.CommandQuery.Command
{

    public class AddZoneCommand : IRequest<ApiResponse>
    {
        public Zone model { get; set; }

        public class AddZoneCommandHandler : IRequestHandler<AddZoneCommand, ApiResponse>
        {
            private readonly ILocationService _context;
            public AddZoneCommandHandler(ILocationService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddZoneCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _context.AddZone(request.model);
                return response;
            }
        }


    }
}
