using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ServiceWisePermissionQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public string ticketId { get; set; }
        public class ServiceWisePermissionHandler : IRequestHandler<ServiceWisePermissionQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ServiceWisePermissionHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ServiceWisePermissionQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetServiceWisePermissionInfo(request.ip, request.userId, request.ticketId);
                return response;
            }
        }

    }
}
