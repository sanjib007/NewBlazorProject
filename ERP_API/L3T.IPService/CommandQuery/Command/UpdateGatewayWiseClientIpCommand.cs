using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using MediatR;

namespace IPService.CommandQuery.Command
{
    public class UpdateGatewayWiseClientIpCommand : IRequest<ApiResponse>
    {
        public UpdateGatewayWiseClientIpReq model { get; set; }

        public class UpdateGatewayWiseClientIpCommandHandler : IRequestHandler<UpdateGatewayWiseClientIpCommand, ApiResponse>
        {
            private readonly IGatewayWiseClientIpService _gatewayIpservice;

            public UpdateGatewayWiseClientIpCommandHandler(IGatewayWiseClientIpService gatewayIpservice)
            {
                _gatewayIpservice = gatewayIpservice;
            }

            public async Task<ApiResponse> Handle(UpdateGatewayWiseClientIpCommand request, CancellationToken cancellationToken)
            {
                var gatewayIp =  await _gatewayIpservice.UpdateGatewayWiseClientIpAddress(request.model);
                return gatewayIp;
            }
        }
    }
}
