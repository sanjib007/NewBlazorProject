using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetChannelbetween149_161Query : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetChannelbetween149_161QueryHandler : IRequestHandler<GetChannelbetween149_161Query, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetChannelbetween149_161QueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetChannelbetween149_161Query request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChannelbetween149_161Data(request.ip);
                return reaponse;
            }
        }
    }
}
