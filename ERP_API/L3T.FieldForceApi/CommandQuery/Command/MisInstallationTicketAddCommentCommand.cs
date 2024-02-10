using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class MisInstallationTicketAddCommentCommand : IRequest<ApiResponse>
    {
        public MisInstallationTickeAddCommentRequestModel model { get; set; }

        public class MisInstallationTicketAddCommentCommandHandler : IRequestHandler<MisInstallationTicketAddCommentCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public MisInstallationTicketAddCommentCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(MisInstallationTicketAddCommentCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SeveAddComment(request.model);
                return reaponse;
            }
        }
    }
}
