using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{
    public class GetRouterModelListQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public int brandId { get; set; }

        public class GetRouterModelListQueryHandler : IRequestHandler<GetRouterModelListQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public GetRouterModelListQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetRouterModelListQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.RouterModelData(request.userId, request.ip, request.brandId);
                return response;
            }
        }
    }
}
