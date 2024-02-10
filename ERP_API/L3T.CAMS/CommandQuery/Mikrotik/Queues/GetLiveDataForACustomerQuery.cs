using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Queues
{
    public class GetLiveDataForACustomerQuery : IRequest<ApiResponse>
    {
        public GetUserInfoFromMikrotikRequestModel model { get; set; }

        public class GetLiveDataForACustomerQueryHandler : IRequestHandler<GetLiveDataForACustomerQuery, ApiResponse>
        {
            private readonly ISocketService _context;

            public GetLiveDataForACustomerQueryHandler(ISocketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetLiveDataForACustomerQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetLiveDataFormMikrotikRouter(request.model);
                return reaponse;
            }
        }
    }
}
