using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
  
    public class GetRsmChecklistQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmChecklistQueryHandler : IRequestHandler<GetRsmChecklistQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmChecklistQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmChecklistQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChecklistData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}
