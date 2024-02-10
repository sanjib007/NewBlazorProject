using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class MediaAndsupportDataQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }

        public class MediaAndsupportDataQueryHandler : IRequestHandler<MediaAndsupportDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public MediaAndsupportDataQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(MediaAndsupportDataQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetInternetTechnologyDataFromMedia(request.ip, request.user);
                return reaponse;
            }
        }
    }
}
