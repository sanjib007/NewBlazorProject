using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetControllerOwnerQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetControllerOwnerQueryHandler : IRequestHandler<GetControllerOwnerQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetControllerOwnerQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetControllerOwnerQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetControllerOwnerData(request.ip);
                return reaponse;
            }
        }
    }
}
