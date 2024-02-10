using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class InstallationCompletionDataQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }
        public string clientName { get; set; }

        public class InstallationCompletionDataHandler : IRequestHandler<InstallationCompletionDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public InstallationCompletionDataHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(InstallationCompletionDataQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetInstallationCompletionFormData(request.brClientCode,
                    request.brSerialNumber, request.userId, request.ip, request.clientName);
                return response;
            }
        }
    }
}
