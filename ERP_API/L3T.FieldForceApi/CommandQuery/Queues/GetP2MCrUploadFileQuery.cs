using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetP2MCrUploadFileQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public int autoODFID { get; set; }


        public class GetP2MCrUploadFileHandler : IRequestHandler<GetP2MCrUploadFileQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetP2MCrUploadFileHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetP2MCrUploadFileQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetPAcakgeNameInfoForNewGo(request.ip, request.user, request.autoODFID);
                return response;
            }
        }
    }
}
