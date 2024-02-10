using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetBtsByIdQuery:IRequest<BtsInfo>
    {
        public long Id { get; set; }

        public class GetBtsByIdHandler : IRequestHandler<GetBtsByIdQuery, BtsInfo>
        {
            private readonly IBtsService _context;
            public GetBtsByIdHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<BtsInfo> Handle(GetBtsByIdQuery query, CancellationToken cancellationToken)
            {
                var bts = await _context.GetBtsById(query.Id);
                if (bts == null) return null;
                return bts;
            }
        }


    }
}
