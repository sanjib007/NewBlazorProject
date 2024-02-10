using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class PostInstallationDataByServiceIdQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ticketId { get; set; }
        public int serviceId { get; set; }

        public class PostInstallationDataByServiceIdHandler : IRequestHandler<PostInstallationDataByServiceIdQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public PostInstallationDataByServiceIdHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(PostInstallationDataByServiceIdQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetPostMainInstallationDataByServiceId(request.ticketId, request.serviceId, request.ip);
                return response;
            }
        }
    }
}
