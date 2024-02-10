using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetLocationForAllUserQuery : IRequest<ApiResponse>
    {
        public string Date { get; set; }
        public string formTime { get; set; }
        public string toTime { get; set; }
        public string ip { get; set; }
        public class ChangeEngineerListQueryHandler : IRequestHandler<GetLocationForAllUserQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ChangeEngineerListQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetLocationForAllUserQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetLocationForAllUser(request.Date, request.formTime, request.toTime, request.ip);
                return reaponse;
            }
        }
    }
}
