using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetAllSupportOfficeQuery : IRequest<IEnumerable<SupportOffice>>
    {
        public class GetAllSupportOfficeQueryHandler : IRequestHandler<GetAllSupportOfficeQuery, IEnumerable<SupportOffice>>
        {
            private readonly IBtsService _context;

            public GetAllSupportOfficeQueryHandler(IBtsService context)
            {
                _context = context;
            }

            public async Task<IEnumerable<SupportOffice>> Handle(GetAllSupportOfficeQuery query, CancellationToken cancellationToken)
            {
                var supportOfficeList = await _context.GetAllSupportOfficeQuery();
                if (supportOfficeList == null)
                {
                    return null;
                }
                return supportOfficeList.AsReadOnly();
            }
        }
    }
}
