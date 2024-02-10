using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetLocationForAUserQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string? Date { get; set; }
        public string? FromTime { get; set; }
        public string? ToTime { get; set; }
        public string ip { get; set; }
        public class ChangeEngineerListQueryHandler : IRequestHandler<GetLocationForAUserQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ChangeEngineerListQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetLocationForAUserQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetLocationForAUser(request.userId, request.Date, request.FromTime, request.ToTime, request.ip);
                return reaponse;
            }
        }
    }
}
