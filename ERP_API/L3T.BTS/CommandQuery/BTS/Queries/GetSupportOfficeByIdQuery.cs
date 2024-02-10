using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Queries
{
    public class GetSupportOfficeByIdQuery : IRequest<SupportOffice>
    {
        public long Id { get; set; }
        public class GetSupportOfficeByIdHandler : IRequestHandler<GetSupportOfficeByIdQuery, SupportOffice>
        {
            private readonly IBtsService _context;
            public GetSupportOfficeByIdHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<SupportOffice> Handle(GetSupportOfficeByIdQuery query, CancellationToken cancellationToken)
            {
                var supportOffice = await _context.GetSupportOfficeById(query.Id);
                if (supportOffice == null)
                    return null;
                else
                    return supportOffice;
            }
        }
    }
}
