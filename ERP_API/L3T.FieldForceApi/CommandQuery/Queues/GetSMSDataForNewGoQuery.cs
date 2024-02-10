using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetSMSDataForNewGoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string ticketId { get; set; }

        public class GetSMSDataForNewGoHandler : IRequestHandler<GetSMSDataForNewGoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetSMSDataForNewGoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetSMSDataForNewGoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetSMSDataForNewGo(request.ip, request.user, request.ticketId);
                return response;
            }
        }
    }
}
