using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace IPService.CommandQuery.Queues
{
    public class GetGatewayIpByIdQuery : IRequest<ApiResponse>
    {
        public long Id { get; set; }

        public class GetGatewayIpByIdHandler : IRequestHandler<GetGatewayIpByIdQuery, ApiResponse>
        {
            private readonly IGateWayIpService _getewayIpservice;
            public GetGatewayIpByIdHandler(IGateWayIpService getewayIpservice)
            {
                _getewayIpservice = getewayIpservice;
            }
            public async Task<ApiResponse> Handle(GetGatewayIpByIdQuery request, CancellationToken cancellationToken)
            {
                var respomse = await _getewayIpservice.GetGateWayIpAddressById(request.Id);

                return respomse;
            }
        }
    }
}
