using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetDataLogByIpCommentQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ipComment { get; set; }
        public string ticketId { get; set; }
        public string teamName { get; set; }

        public class GetDataLogByIpCommentHandler : IRequestHandler<GetDataLogByIpCommentQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetDataLogByIpCommentHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetDataLogByIpCommentQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetDataLogByIpComment(request.user, request.ip, request.ipComment, request.ticketId, request.teamName);
                return response;
            }
        }
    }
}
