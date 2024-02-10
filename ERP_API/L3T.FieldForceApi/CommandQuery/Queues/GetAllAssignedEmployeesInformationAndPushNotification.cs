using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetAllAssignedEmployeesInformationAndPushNotification : IRequest<ApiResponse>
    {
        public PushNotificationRequestModel? PushNotificationRequestModel { get; set; }
        public string? Ip { get; set; }

        public class GetAllAssignedEmployeesInformationAndPushNotificationhandler :
            IRequestHandler<GetAllAssignedEmployeesInformationAndPushNotification, ApiResponse>
        {
            private readonly IForwardTicketService _service;
            public GetAllAssignedEmployeesInformationAndPushNotificationhandler(IForwardTicketService service)
            {
                _service = service;
            }

            public Task<ApiResponse> Handle(GetAllAssignedEmployeesInformationAndPushNotification request, CancellationToken cancellationToken)
            {
                var response = _service.PushNotificationForTicketAssignOrForward(request?.PushNotificationRequestModel, request?.Ip);
                throw new NotImplementedException();
            }
        }

    }
}
