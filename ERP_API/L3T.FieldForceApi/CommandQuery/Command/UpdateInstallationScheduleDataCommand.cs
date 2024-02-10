using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class UpdateInstallationScheduleDataCommand : IRequest<ApiResponse>
    {
        public string userId;
        public installationScheduleRequestModel model { get; set; }
        public string ip { get; set; }

        public class UpdateInstallationScheduleDataCommandHandler : IRequestHandler<UpdateInstallationScheduleDataCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public UpdateInstallationScheduleDataCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateInstallationScheduleDataCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.UpdateInstallationScheduleData(request.userId,request.model, request.ip);
                return reaponse;
            }
        }
    }
}
