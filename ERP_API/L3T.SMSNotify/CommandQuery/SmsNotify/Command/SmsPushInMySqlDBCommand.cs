using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.SMSNotify;
using MediatR;

namespace L3T.SMSNotify.CommandQuery.SmsNotify.Command
{
    public class SmsPushInMySqlDBCommand : IRequest<ApiResponse>
    {
        public class SmsPushInMySqlDBCommandHandler : IRequestHandler<SmsPushInMySqlDBCommand, ApiResponse>
        {
            private readonly ISMSNotifyService _context;

            public SmsPushInMySqlDBCommandHandler(ISMSNotifyService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(SmsPushInMySqlDBCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SmsPushInMySqlDB();
                return reaponse;
            }
        }
    }
}
