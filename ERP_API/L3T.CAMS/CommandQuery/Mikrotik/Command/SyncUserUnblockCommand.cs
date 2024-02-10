using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command
{
    public class SyncUserUnblockCommand : IRequest<ApiResponse>
    {
        public UnblockUserFromMikrotikRouterRequestModel model { get; set; }
        public class SyncUserUnblockCommandHandler : IRequestHandler<SyncUserUnblockCommand, ApiResponse> {

            private readonly ICamsService _context;
            public SyncUserUnblockCommandHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(SyncUserUnblockCommand command, CancellationToken cancellationToken)
            {
                ApiResponse response = _context.UnblockUserInfoFromMikrotikRouterSyncMethod(command.model);
                return response;
            }
        }
    }
}
