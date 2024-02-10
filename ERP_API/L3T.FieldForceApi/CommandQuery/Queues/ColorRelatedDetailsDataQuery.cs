using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ColorRelatedDetailsDataQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }

        public class ColorRelatedDetailsDataHandler : IRequestHandler<ColorRelatedDetailsDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ColorRelatedDetailsDataHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ColorRelatedDetailsDataQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetColorRelatedDetailsData(request.ip, request.user, request.brSerialNumber, request.brClientCode);
                return response;
            }
        }
    }
}
