using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetAllBrandQuery : IRequest<IEnumerable<Brand>>
    {
        public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery,IEnumerable<Brand>> 
        {
            private readonly IBtsService _context;

            public GetAllBrandQueryHandler(IBtsService context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Brand>> Handle(GetAllBrandQuery query, CancellationToken cancellationToken)
            {
                var brandList = await _context.GetAllBrandQuery();
                if (brandList == null)
                {
                    return null;
                }
                return brandList.AsReadOnly();
            }
        }
    }
}
