using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class UpdateCommentQuery : IRequest<ApiResponse>
    {
        public UpdateCommentRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class UpdateCommentQueryHandler : IRequestHandler<UpdateCommentQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public UpdateCommentQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateCommentQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.UpdateCommentData(request.model,request.user,request.ip);
                return reaponse;
            }
        }
    }
}
