using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetCrUploadFileQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public int brSerialNumber { get; set; }
        public string brClientCode { get; set; }

        public class GetCrUploadFileHandler : IRequestHandler<GetCrUploadFileQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetCrUploadFileHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetCrUploadFileQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetCrUploadFile(request.ip, request.user, request.brSerialNumber, request.brClientCode);
                return response;
            }
        }
    }
}
