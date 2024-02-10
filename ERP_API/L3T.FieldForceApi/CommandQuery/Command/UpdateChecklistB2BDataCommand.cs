using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
   
    public class UpdateChecklistB2BDataCommand : IRequest<ApiResponse>
    {
        public MisCheckListRequestModel model { get; set; }
        public string ip { get; set; }

        public class UpdateChecklistB2BDataCommandHandler : IRequestHandler<UpdateChecklistB2BDataCommand, ApiResponse>
        {
            private readonly IChecklistService _context;

            public UpdateChecklistB2BDataCommandHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateChecklistB2BDataCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SaveChecklistB2BData(request.model, request.ip);
                return reaponse;
            }
        }
    }
}
