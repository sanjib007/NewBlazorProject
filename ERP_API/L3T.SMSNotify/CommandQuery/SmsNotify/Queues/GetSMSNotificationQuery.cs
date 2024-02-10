using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.SMSNotify;
using MediatR;

namespace L3T.SMSNotify.CommandQuery.SmsNotify.Queues
{
    public class GetSMSNotificationQuery : IRequest<ApiResponse>
    {
        public class GetUserInfoFromMikrotikQueryHandler : IRequestHandler<GetSMSNotificationQuery, ApiResponse>
        {
            private readonly ISMSNotifyService _context;

            public GetUserInfoFromMikrotikQueryHandler(ISMSNotifyService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetSMSNotificationQuery request, CancellationToken cancellationToken)
            {
                 var reaponse = await _context.GetSMSNotification();
                 return reaponse;
            }
        }
    }
}
