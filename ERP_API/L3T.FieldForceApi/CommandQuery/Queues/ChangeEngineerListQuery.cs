using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ChangeEngineerListQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public string UserId { get; set; }
        public class ChangeEngineerListQueryHandler : IRequestHandler<ChangeEngineerListQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ChangeEngineerListQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ChangeEngineerListQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ChangeEngineer(request.ticketId, request.UserId, request.ip);
                return reaponse;
            }
        }
    }
}
