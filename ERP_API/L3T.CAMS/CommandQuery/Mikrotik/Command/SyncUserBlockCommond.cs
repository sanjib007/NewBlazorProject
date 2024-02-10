using CAMS.CommandQuery.Mikrotik.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;
using static CAMS.CommandQuery.Mikrotik.Command.SyncUserBlockCommand;

namespace CAMS.CommandQuery.Mikrotik.Command
{
    public class SyncUserBlockCommand : IRequest<ApiResponse>
    {
        public BlockUserFromMikrotikRouterRequestModel model { get; set; }

        public class SyncUserBlockCommondHandler : IRequestHandler<SyncUserBlockCommand, ApiResponse>
        {
            private readonly ICamsService _context;
            public SyncUserBlockCommondHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(SyncUserBlockCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = _context.BlockUserInfoFromMikrotikRouterSyncMethod(request.model);
                return response;
            }
        }
    }
}

