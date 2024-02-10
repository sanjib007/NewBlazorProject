using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;

namespace L3T.Location.CommandQuery.Queries
{
    public class GetAllDivisionQuery : IRequest<IEnumerable<Division>>
    {
        public class GetAllDivisionQueryHandler : IRequestHandler<GetAllDivisionQuery, IEnumerable<Division>>
        {
            private readonly ILocationService _context;

            public GetAllDivisionQueryHandler(ILocationService context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Division>> Handle(GetAllDivisionQuery query, CancellationToken cancellationToken)
            {
                var brandList = await _context.GetAllDivisionQuery();
                if (brandList == null)
                {
                    return null;
                }
                return brandList.AsReadOnly();
            }
        }
    }
}

