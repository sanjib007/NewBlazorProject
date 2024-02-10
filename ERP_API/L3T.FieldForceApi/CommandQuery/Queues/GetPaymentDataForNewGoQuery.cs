using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetPaymentDataForNewGoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string ticketId { get; set; }

        public class GetPaymentDataForNewGoHandler : IRequestHandler<GetPaymentDataForNewGoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetPaymentDataForNewGoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetPaymentDataForNewGoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetPaymentDataForNewGo(request.ip, request.user, request.ticketId);
                return response;
            }
        }
    }
}
