using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class AllServiceDataForNewGoQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ticketId { get; set; }
        public string serviceId { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }
        public int btsId { get; set; }

        public class AllServiceDataForNewGoHandler : IRequestHandler<AllServiceDataForNewGoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public AllServiceDataForNewGoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(AllServiceDataForNewGoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetAllServiceDataForNewGo(request.ip, request.user, request.ticketId,
                    request.serviceId, request.brClientCode, request.brSerialNumber, request.btsId);
                return response;
            }
        }
    }
}
