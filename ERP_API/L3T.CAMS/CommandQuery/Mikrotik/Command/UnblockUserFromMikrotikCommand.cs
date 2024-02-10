using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command;

public class UnblockUserFromMikrotikCommand : IRequest<ApiResponse>
{
    public UnblockUserFromMikrotikRouterRequestModel model { get; set; }
    
    public class UnblockUserFromMikrotikCommandHandler : IRequestHandler<UnblockUserFromMikrotikCommand, ApiResponse>
    {
        private readonly ICamsService _context;
        public UnblockUserFromMikrotikCommandHandler(ICamsService context)
        {
            _context = context;
        }
        public async Task<ApiResponse> Handle(UnblockUserFromMikrotikCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = await _context.UnblockUserInfoFromMikrotikRouter(request.model);
            return response;
        }
    }
}