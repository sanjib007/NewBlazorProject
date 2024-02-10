using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command
{
    public class SyncListsUserBlockCommand : IRequest<ApiResponse>
    {
        public BlockUserListsFromMikrotikRouterRequestModel model { get; set; }

        public class SyncListsUserBlockCommondHandler : IRequestHandler<SyncListsUserBlockCommand, ApiResponse>
        {
            private readonly ICamsService _context;
            public SyncListsUserBlockCommondHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(SyncListsUserBlockCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = _context.BlockListsUserInfoFromMikrotikRouterSyncMethod(request.model);
                return response;
            }
        }
    }
}