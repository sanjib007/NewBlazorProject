using AspNetCoreHero.Results;
using AutoMapper;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.ResponseModel;
using MediatR;

namespace IPService.CommandQuery.Queues
{
    public class GetAllGateWayIpQuery : IRequest<ApiResponse>
    {
        //public GetAllGateWayIpQuery() { }

        public class GetAllGateWayIpQueryHandler : IRequestHandler<GetAllGateWayIpQuery, ApiResponse>
        {
            private readonly IGateWayIpService _context;
            private readonly IMapper _mapper;
            public GetAllGateWayIpQueryHandler(IGateWayIpService context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<ApiResponse> Handle(GetAllGateWayIpQuery request, CancellationToken cancellationToken)
            {
                var reaponse = _context.GetAllGateWayIpAddressQuery();
                return reaponse;
            }
        }
    }
}
