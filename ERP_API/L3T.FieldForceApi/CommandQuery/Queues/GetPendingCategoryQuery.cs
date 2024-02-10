using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetPendingCategoryQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetPendingCategoryQueryHandler : IRequestHandler<GetPendingCategoryQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetPendingCategoryQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetPendingCategoryQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetPendingCategoryList(request.ip);
                return reaponse;
            }
        }
    }
}
