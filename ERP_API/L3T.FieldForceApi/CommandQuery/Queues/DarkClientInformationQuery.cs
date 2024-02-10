using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class DarkClientInformationQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }
        public class DarkClientInformationHandler : IRequestHandler<DarkClientInformationQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public DarkClientInformationHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(DarkClientInformationQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetDarkClientInformation(request.ip, request.user, request.brSerialNumber, request.brClientCode);
                return response;
            }
        }

    }
}
