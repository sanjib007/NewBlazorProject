using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command;

public class BlockUserFromMikrotikCommand : IRequest<ApiResponse>
{
    public BlockUserFromMikrotikRouterRequestModel model { get; set; }
    
    public class BlockUserInfoFromMikrotikCommandHandler : IRequestHandler<BlockUserFromMikrotikCommand, ApiResponse>
    {
        private readonly ICamsService _context;
        public BlockUserInfoFromMikrotikCommandHandler(ICamsService context)
        {
            _context = context;
        }
        public async Task<ApiResponse> Handle(BlockUserFromMikrotikCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = await _context.BlockUserInfoFromMikrotikRouter(request.model);
            return response;
        }
    }
}