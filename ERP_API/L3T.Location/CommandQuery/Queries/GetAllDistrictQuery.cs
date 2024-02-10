using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;

namespace L3T.Location.CommandQuery.Queries
{
    public class GetAllDistrictQuery : IRequest<IEnumerable<District>>
    {
        public class GetAllDistrictQueryHandler : IRequestHandler<GetAllDistrictQuery, IEnumerable<District>>
        {

            private readonly ILocationService _context;

            public GetAllDistrictQueryHandler(ILocationService context)
            {
                _context = context;
            }


            public async Task<IEnumerable<District>> Handle(GetAllDistrictQuery query, CancellationToken cancellationToken)
            {
                var districtList = await _context.GetAllDistrictQuery();
                if (districtList == null)
                {
                    return null;
                }
                return districtList.AsReadOnly();
            }

        }
    }
}

