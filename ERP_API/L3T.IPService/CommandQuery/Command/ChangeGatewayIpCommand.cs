using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using MediatR;
using tik4net.Objects;

namespace IPService.CommandQuery.Command
{
    public class ChangeGatewayIpCommand : IRequest<ApiResponse>
    {
        public long Id { get; set; }
        public bool Status { get; set; }

        public class ChangeGatewayIpCommandHandler : IRequestHandler<ChangeGatewayIpCommand, ApiResponse>
        {
            private readonly IGateWayIpService _gatewayIpservice;

            public ChangeGatewayIpCommandHandler(IGateWayIpService gatewayIpservice)
            {
                _gatewayIpservice = gatewayIpservice;
            }

            public  Task<ApiResponse> Handle(ChangeGatewayIpCommand request, CancellationToken cancellationToken)
            {
                var changeStatusGatewayIp =  _gatewayIpservice.GateWayIpAddressStatusChange(request.Id,request.Status);
                return changeStatusGatewayIp;
            }
        }
    }
}
