using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetBTSTypeByIdQuery : IRequest<BTSType>
    {
        public long Id { get; set; }
        public class GetBTSTypeByIdHandler : IRequestHandler<GetBTSTypeByIdQuery, BTSType>
        {
            private readonly IBtsService _context;
            public GetBTSTypeByIdHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<BTSType> Handle(GetBTSTypeByIdQuery query, CancellationToken cancellationToken)
            {
                var btsType = await _context.GetBTSTypeById(query.Id);
                if (btsType == null)
                    return null;
                else
                    return btsType;
            }
        }
    }
}
