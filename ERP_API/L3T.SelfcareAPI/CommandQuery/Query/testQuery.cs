using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.SelfCare;
using MediatR;

namespace L3T.SelfcareAPI.CommandQuery.Query
{
    public class testQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public class testQueryHandler : IRequestHandler<testQuery, ApiResponse>
        {
            private readonly ISelfCareService _context;

            public testQueryHandler(ISelfCareService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(testQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.testMethod(request.Id);
                return reaponse;
            }
        }
    }
}
