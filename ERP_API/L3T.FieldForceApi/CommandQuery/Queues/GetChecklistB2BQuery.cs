using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetChecklistB2BQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetChecklistB2BQueryHandler : IRequestHandler<GetChecklistB2BQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetChecklistB2BQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetChecklistB2BQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChecklistB2BData(request.ip);
                return reaponse;
            }
        }
    }
}
