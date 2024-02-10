using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3T.Location.CommandQuery.Queries
{
    public class GetUpazilaByDistrictIdQuery : IRequest<IEnumerable<Upazila>>
    {
        public long districtId { get; set; }
        public class GetAllUpazilaQueryHandler : IRequestHandler<GetUpazilaByDistrictIdQuery, IEnumerable<Upazila>>
        {

            private readonly ILocationService _context;

            public GetAllUpazilaQueryHandler(ILocationService context)
            {
                _context = context;
            }


            public async Task<IEnumerable<Upazila>> Handle(GetUpazilaByDistrictIdQuery query, CancellationToken cancellationToken)
            {
                var UpazilaList = await _context.GetUpazilaByDistrictIdQuery(query.districtId);
                if (UpazilaList == null)
                {
                    return null;
                }
                return UpazilaList.AsReadOnly();
            }

        }
    }
}
