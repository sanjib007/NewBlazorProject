using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRsmControllerOwnerQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmControllerOwnerQueryHandler : IRequestHandler<GetRsmControllerOwnerQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmControllerOwnerQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmControllerOwnerQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetControllerOwnerData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}
