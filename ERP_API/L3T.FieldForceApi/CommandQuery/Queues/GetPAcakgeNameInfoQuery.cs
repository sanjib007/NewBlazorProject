using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetPAcakgeNameInfoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }

        public class GetPAcakgeNameInfoHandler : IRequestHandler<GetPAcakgeNameInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetPAcakgeNameInfoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetPAcakgeNameInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetPAcakgeNameInfoForNewGo(request.ip, request.user, request.brSerialNumber, request.brClientCode);
                return response;
            }
        }
    }
}
