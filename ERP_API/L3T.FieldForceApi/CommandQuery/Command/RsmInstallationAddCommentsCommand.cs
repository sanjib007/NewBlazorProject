using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class RsmInstallationAddCommentsCommand : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public RsmInstallationAddCommentsRequestModel model { get; set; }

        public class RsmInstallationInfoQueryHandler : IRequestHandler<RsmInstallationAddCommentsCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public RsmInstallationInfoQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(RsmInstallationAddCommentsCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.AddCommentData(request.userId, request.ip, request.model);
                return response;
            }
        }
    }
}
