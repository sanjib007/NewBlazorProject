using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ComplainTicektImportentDataQuery : IRequest<ApiResponse>
    {
        public string TicketId { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class ComplainTicektImportentDataQueryHandler : IRequestHandler<ComplainTicektImportentDataQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public ComplainTicektImportentDataQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(ComplainTicektImportentDataQuery request, CancellationToken cancellationToken)
            {
                var response = await _rSMComplainTicketService.GetComplainTicektImportentData(request.TicketId, request.userId, request.ip);
                return response;
            }
        }


    }
}
