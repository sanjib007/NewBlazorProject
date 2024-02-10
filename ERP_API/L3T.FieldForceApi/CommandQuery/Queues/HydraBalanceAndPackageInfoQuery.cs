using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class HydraBalanceAndPackageInfoQuery : IRequest<ApiResponse>
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public class HydraBalanceAndPackageInfoQueryHandler : IRequestHandler<HydraBalanceAndPackageInfoQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public HydraBalanceAndPackageInfoQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(HydraBalanceAndPackageInfoQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _rSMComplainTicketService.GetBillInformationFromHydra(request.CustomerId, request.UserId, request.Ip);
                return response;
            }
        }
    }
}
