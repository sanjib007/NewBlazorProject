using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Interface.SelfCare;
using L3T.Infrastructure.Helpers.Models.SelfCare.RequestModel;
using MediatR;

namespace L3T.SelfcareAPI.CommandQuery.Command
{
    public class CreateServiceCommand : IRequest<ApiResponse>
    {
        public ServiceCreateRequestModel model { get; set; }
        public string UserIP { get; set; }
        public string UserId { get; set; }
        public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ApiResponse>
        {
            private readonly ISelfCareService _context;

            public CreateServiceCommandHandler(ISelfCareService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ServiceCreate(request.model, request.UserIP, request.UserId);
                return reaponse;
            }
        }
    }
}
