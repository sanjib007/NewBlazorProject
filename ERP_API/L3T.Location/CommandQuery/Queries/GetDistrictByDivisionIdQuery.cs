using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3T.Location.CommandQuery.Queries
{

    public class GetDistrictByDivisionIdQuery : IRequest<IEnumerable<District>>
    {
        public long divisionId { get; set; }
        public class GetAllDistrictQueryHandler : IRequestHandler<GetDistrictByDivisionIdQuery, IEnumerable<District>>
        {

            private readonly ILocationService _context;

            public GetAllDistrictQueryHandler(ILocationService context)
            {
                _context = context;
            }


            public async Task<IEnumerable<District>> Handle(GetDistrictByDivisionIdQuery query, CancellationToken cancellationToken)
            {
                var DistrictList = await _context.GetDistrictByDivisionIdQuery(query.divisionId);
                if (DistrictList == null)
                {
                    return null;
                }
                return DistrictList.AsReadOnly();
            }

        }
    }
}
