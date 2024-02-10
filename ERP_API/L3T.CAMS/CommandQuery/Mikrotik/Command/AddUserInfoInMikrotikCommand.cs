using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command;

public class AddUserInfoInMikrotikCommand : IRequest<ApiResponse>
{
    public AddUserInfoInMikrotikRouterRequestModel model { get; set; }
    
    public class AdduserInfoInMikrotikCommandHandler : IRequestHandler<AddUserInfoInMikrotikCommand, ApiResponse>
    {
        private readonly ICamsService _context;
        public AdduserInfoInMikrotikCommandHandler(ICamsService context)
        {
            _context = context;
        }
        public async Task<ApiResponse> Handle(AddUserInfoInMikrotikCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = await _context.AddUserInMikrotikRouter(request.model);
            return response;
        }
    }
}