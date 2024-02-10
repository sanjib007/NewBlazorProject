using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using MediatR;

namespace IPService.CommandQuery.Command
{
    public class UpdateGatewayIpCommand : IRequest<ApiResponse>
    {
        public UpdateGatewayIpReq model { get; set; }

        public class UpdateGatewayIpCommandHandler : IRequestHandler<UpdateGatewayIpCommand, ApiResponse>
        {
            private readonly IGateWayIpService _gatewayIpservice;

            public UpdateGatewayIpCommandHandler(IGateWayIpService gatewayIpservice)
            {
                _gatewayIpservice = gatewayIpservice;
            }

            public async Task<ApiResponse> Handle(UpdateGatewayIpCommand request, CancellationToken cancellationToken)
            {
                var gatewayIp =  await _gatewayIpservice.UpdateGateWayIpAddress(request.model);
                return gatewayIp;
            }
        }
    }
}
