using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class UpdateRsmChecklistDataCommand : IRequest<ApiResponse>
    {
        public string userId;
        public RsmCheckListRequestModel model { get; set; }
        public string ip { get; set; }

        public class UpdateRsmChecklistDataCommandHandler : IRequestHandler<UpdateRsmChecklistDataCommand, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public UpdateRsmChecklistDataCommandHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateRsmChecklistDataCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SaveRsmChecklistData(request.model,request.userId, request.ip);
                return reaponse;
            }
        }
    }
}
