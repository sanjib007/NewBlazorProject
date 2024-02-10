using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Implementation.IP;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.IP;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Queues
{
    public class IpWhiteListedCheckQuery : IRequest<ApiResponse>
    {
        public string Ip { get; set; }
        public string RequestStr { get; set; }

        public class IpWhiteListedCheckQueryHandler : IRequestHandler<IpWhiteListedCheckQuery, ApiResponse>
        {
            private readonly IIPWhiteListedService _context;

            public IpWhiteListedCheckQueryHandler(IIPWhiteListedService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(IpWhiteListedCheckQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.CheckIPWhiteList(request.Ip, request.RequestStr);
                return reaponse;
            }
        }
    }
}
