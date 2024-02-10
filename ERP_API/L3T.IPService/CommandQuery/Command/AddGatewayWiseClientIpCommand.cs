using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace IPService.CommandQuery.Command
{
   
    public class AddGatewayWiseClientIpCommand : IRequest<ApiResponse>
    {
        public AddGatewayWiseClientIpReq model { get; set; }
        public class AddGatewayWiseClientIpCommandHandler : IRequestHandler<AddGatewayWiseClientIpCommand, ApiResponse>
        {
            private readonly IGatewayWiseClientIpService _context;
            public AddGatewayWiseClientIpCommandHandler(IGatewayWiseClientIpService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddGatewayWiseClientIpCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _context.AddGatewayWiseClientIpAddress(request.model);
                return response;
            }
        }
    }
}
