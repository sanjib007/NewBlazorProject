using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;




namespace L3T.Location.CommandQuery.Queries
{

    public class GetAllZoneQuery : IRequest<IEnumerable<Zone>>
    {
        public class GetAllZoneQueryHandler : IRequestHandler<GetAllZoneQuery, IEnumerable<Zone>>
        {

            private readonly ILocationService _context;

            public GetAllZoneQueryHandler(ILocationService context)
            {
                _context = context;
            }


            public async Task<IEnumerable<Zone>> Handle(GetAllZoneQuery query, CancellationToken cancellationToken)
            {
                var ZoneList = await _context.GetAllZoneQuery();
                if (ZoneList == null)
                {
                    return null;
                }
                return ZoneList.AsReadOnly();
            }

        }
    }
}
