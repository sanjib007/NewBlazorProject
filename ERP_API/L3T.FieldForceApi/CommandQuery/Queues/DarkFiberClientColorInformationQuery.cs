using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class DarkFiberClientColorInformationQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }
        public int noOfCore { get; set; }

        public class DarkFiberClientColorInformationHandler : IRequestHandler<DarkFiberClientColorInformationQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public DarkFiberClientColorInformationHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(DarkFiberClientColorInformationQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetDarkFiberClientColorInformation(request.ip, request.user, request.brSerialNumber,
                    request.brClientCode, request.noOfCore);
                return response;
            }
        }
    }
}
