using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetChecklistQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetChecklistQueryHandler : IRequestHandler<GetChecklistQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetChecklistQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetChecklistQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChecklistData(request.ip);
                return reaponse;
            }
        }
    }
}
