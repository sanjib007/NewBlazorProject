using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command
{
    public class SetQueueIntoMikrotikRouterCommand : IRequest<ApiResponse>
    {
        public SetQueueIntoMikrotikRouterRequestModel model { get; set; }
        public class SetQueueIntoMikrotikRouterCommandHandler : IRequestHandler<SetQueueIntoMikrotikRouterCommand, ApiResponse>
        {
            private readonly ICamsService _context;
            public SetQueueIntoMikrotikRouterCommandHandler(ICamsService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(SetQueueIntoMikrotikRouterCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = _context.SetQueueInfoIntoMikrotikRouter(request.model);
                return response;
            }
        }
    }
}
