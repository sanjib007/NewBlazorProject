using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetSupportOfficeDataQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }

        public class GetSupportOfficeDataQueryHandler : IRequestHandler<GetSupportOfficeDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetSupportOfficeDataQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetSupportOfficeDataQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetSupportOfficeData(request.ip, request.user);
                return reaponse;
            }
        }
    }
}
