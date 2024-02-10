using AspNetCoreHero.Results;
using AutoMapper;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.ResponseModel;
using MediatR;

namespace IPService.CommandQuery.Queues
{
    public class GetAllGatewayWiseClientIpQuery : IRequest<ApiResponse>
    {
        //public GetAllGateWayIpQuery() { }

        public class GetAllGatewayWiseClientIpQueryHandler : IRequestHandler<GetAllGatewayWiseClientIpQuery, ApiResponse>
        {
            private readonly IGatewayWiseClientIpService _context;
            private readonly IMapper _mapper;
            public GetAllGatewayWiseClientIpQueryHandler(IGatewayWiseClientIpService context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<ApiResponse> Handle(GetAllGatewayWiseClientIpQuery request, CancellationToken cancellationToken)
            {
                var reaponse = _context.GetAllGatewayWiseClientIpAddressQuery();
                return reaponse;
            }
        }
    }
}
