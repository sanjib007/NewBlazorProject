using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ParentDataQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class ParentDataHandler : IRequestHandler<ParentDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ParentDataHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ParentDataQuery request, CancellationToken cancellationToken)
            {
                //var reaponse = await _context.GetParentData(request.user, request.ip);
                //return reaponse;
                return null;
            }
        }

    }
}
