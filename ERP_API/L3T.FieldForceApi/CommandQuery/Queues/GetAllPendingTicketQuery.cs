using L3T.FieldForceApi.CommandQuery.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetAllPendingTicketQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public class GetAllPendingTicketQueryHandler : IRequestHandler<GetAllPendingTicketQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetAllPendingTicketQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetAllPendingTicketQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetAllAssignTicketForUser(request.userId, request.ip);
                return reaponse;
            }
        }
    }
}
