using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class MISInstallationTicketListQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }

        public class MISInstallationTicketListQueryHandler : IRequestHandler<MISInstallationTicketListQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public MISInstallationTicketListQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(MISInstallationTicketListQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetAssignedMisInstallationTicketList(request.userId, request.ip);
                return response;
            }
        }
    }
}
