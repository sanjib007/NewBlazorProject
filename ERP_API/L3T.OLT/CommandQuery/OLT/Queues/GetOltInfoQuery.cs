using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.OLT;
using L3T.Infrastructure.Helpers.Models.OLT;
using MediatR;

namespace L3T.OLT.CommandQuery.OLT.Queues
{
    public class GetOltInfoQuery : IRequest<ApiResponse>
    {
        //public OltInfo model { get; set; }
        public OltInfo model { get; set; }
        public class GetOltInfoQueryHandler : IRequestHandler<GetOltInfoQuery, ApiResponse>
        {
            private readonly IOltService _context;

            public GetOltInfoQueryHandler(IOltService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetOltInfoQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetOltInfo(request.model.ID);
                return reaponse;
            }
        }
    }
}
