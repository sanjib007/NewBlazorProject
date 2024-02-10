using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetKh_IpAddressByHostNameQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string hostName { get; set; }


        public class GetKh_IpAddressByHostNameHandler : IRequestHandler<GetKh_IpAddressByHostNameQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetKh_IpAddressByHostNameHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetKh_IpAddressByHostNameQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetKh_IpAddressByHostNameForNewGo(request.ip, request.user, request.hostName);
                return response;
            }
        }
    }
}
