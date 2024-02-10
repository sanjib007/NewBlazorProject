using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command
{
    public class AddUserInfoMikrotikSyncCommand : IRequest<ApiResponse>
    {
        public AddUserInfoInMikrotikRouterRequestModel model { get; set; }

        public class AddUserInfoMikrotikSyncCommandHandler : IRequestHandler<AddUserInfoMikrotikSyncCommand, ApiResponse>
        {
            private readonly ICamsService _context;
            public AddUserInfoMikrotikSyncCommandHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddUserInfoMikrotikSyncCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = _context.AddUserInMikrotikRouterSyncMethod(request.model);
                return response;
            }
        }
    }
}
