using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ForwardToListQuery : IRequest<ApiResponse>
    {
        public string ip {  get; set; }
        public string UserId { get; set; }
        public class ForwardToListQueryHandler : IRequestHandler<ForwardToListQuery, ApiResponse>
        {
            private readonly IForwardTicketService _context;

            public ForwardToListQueryHandler(IForwardTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ForwardToListQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.FowardToList(request.ip, request.UserId);
                return reaponse;
            }
        }
    }
}
