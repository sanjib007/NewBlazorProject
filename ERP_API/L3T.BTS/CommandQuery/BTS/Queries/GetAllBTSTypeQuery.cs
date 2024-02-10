using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetAllBTSTypeQuery : IRequest<IEnumerable<BTSType>>
    {
        public class GetAllBTSTypeQueryHandler : IRequestHandler<GetAllBTSTypeQuery, IEnumerable<BTSType>>
        {
            private readonly IBtsService _context;

            public GetAllBTSTypeQueryHandler(IBtsService context)
            {
                _context = context;
            }

            public async Task<IEnumerable<BTSType>> Handle(GetAllBTSTypeQuery query, CancellationToken cancellationToken)
            {
                var btsTypeList = await _context.GetAllBTSTypeQuery();
                if (btsTypeList == null)
                {
                    return null;
                }
                return btsTypeList.AsReadOnly();
            }
        }
    }
}
