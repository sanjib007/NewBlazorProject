using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class UpdateChecklistDataCommand : IRequest<ApiResponse>
    {
        public MisCheckListRequestModel model { get; set; }
        public string ip { get; set; }

        public class UpdateChecklistDataCommandHandler : IRequestHandler<UpdateChecklistDataCommand, ApiResponse>
        {
            private readonly IChecklistService _context;

            public UpdateChecklistDataCommandHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateChecklistDataCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SaveChecklistData(request.model, request.ip);
                return reaponse;
            }
        }
    }
}
