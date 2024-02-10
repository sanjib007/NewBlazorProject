using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using MediatR;
using tik4net.Objects;

namespace IPService.CommandQuery.Command
{
    public class ChangeGatewayWiseIpCommand : IRequest<ApiResponse>
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public String LastModifiedBy { get; set; }

        public class ChangeGatewayWiseIpCommandHandler : IRequestHandler<ChangeGatewayWiseIpCommand, ApiResponse>
        {
            private readonly IGatewayWiseClientIpService _gatewayIpservice;

            public ChangeGatewayWiseIpCommandHandler(IGatewayWiseClientIpService gatewayIpservice)
            {
                _gatewayIpservice = gatewayIpservice;
            }

            public  Task<ApiResponse> Handle(ChangeGatewayWiseIpCommand request, CancellationToken cancellationToken)
            {
                var changeStatusGatewayIp =  _gatewayIpservice.GatewayWiseClientIpAddressStatusChange(request.Id,request.Status, request.LastModifiedBy);
                return changeStatusGatewayIp;
            }
        }
    }
}
