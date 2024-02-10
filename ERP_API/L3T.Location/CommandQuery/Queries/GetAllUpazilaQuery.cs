using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;

namespace L3T.Location.CommandQuery.Queries
{

    public class GetAllUpazilaQuery : IRequest<IEnumerable<Upazila>>
    {
        public class GetAllUpazilaQueryHandler : IRequestHandler<GetAllUpazilaQuery, IEnumerable<Upazila>>
        {

            private readonly ILocationService _context;

            public GetAllUpazilaQueryHandler(ILocationService context)
            {
                _context = context;
            }


            public async Task<IEnumerable<Upazila>> Handle(GetAllUpazilaQuery query, CancellationToken cancellationToken)
            {
                var UpazilaList = await _context.GetAllUpazilaQuery();
                if (UpazilaList == null)
                {
                    return null;
                }
                return UpazilaList.AsReadOnly();
            }

        }
    }

}
