using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace IPService.CommandQuery.Queues
{
    public class GetGatewayWiseClientIpByIdQuery : IRequest<ApiResponse>
    {
        public long Id { get; set; }

        public class GetGatewayWiseClientIpByIdQueryHandler : IRequestHandler<GetGatewayWiseClientIpByIdQuery, ApiResponse>
        {
            private readonly IGatewayWiseClientIpService _getewayIpservice;
            public GetGatewayWiseClientIpByIdQueryHandler(IGatewayWiseClientIpService getewayIpservice)
            {
                _getewayIpservice = getewayIpservice;
            }
            public async Task<ApiResponse> Handle(GetGatewayWiseClientIpByIdQuery request, CancellationToken cancellationToken)
            {
                var respomse = await _getewayIpservice.GetGatewayWiseClientIpAddressById(request.Id);

                return respomse;
            }
        }
    }
}
