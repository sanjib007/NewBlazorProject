using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetClosingNatureQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string UserId { get; set; }
        public class GetClosingNatureQueryHandler : IRequestHandler<GetClosingNatureQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetClosingNatureQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetClosingNatureQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetClosingNature(request.UserId, request.ip);
                return reaponse;
            }
        }
    }
}
