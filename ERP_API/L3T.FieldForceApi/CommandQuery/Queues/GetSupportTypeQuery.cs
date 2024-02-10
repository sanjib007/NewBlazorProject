using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetSupportTypeQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string UserId { get; set; }
        public class GetSupportTypeQueryHandler : IRequestHandler<GetSupportTypeQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetSupportTypeQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetSupportTypeQuery request, CancellationToken cancellationToken)
            {
                ApiResponse reaponse = await _context.GetSupportType(request.UserId, request.ip);
                return reaponse;
            }
        }
    }
}
