using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class AllDataForRSMCheckListQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string clientId { get; set; }
        public string userId { get; set; }
        public class AllDataForRSMCheckListQueryHandler : IRequestHandler<AllDataForRSMCheckListQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public AllDataForRSMCheckListQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AllDataForRSMCheckListQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.AllDataForRSMCheckList(request.clientId, request.ip, request.userId);
                return reaponse;
            }
        }
    }
}
