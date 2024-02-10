using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class UpdateBrandCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public Brand brand { get; set; }

        public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, bool>
        {
            private readonly IBtsService _context;
            public UpdateBrandCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
            {
                var brandobj = await _context.GetBrandById(command.Id);
                if (brandobj != null)
                {
                    if (!string.IsNullOrEmpty((command.brand.Name).Trim()))
                    {
                        brandobj.Name = command.brand.Name;
                    }
                    if (!string.IsNullOrEmpty((command.brand.ProductType).Trim()))
                    {
                        brandobj.ProductType = command.brand.ProductType;
                    }
                }
                var ticket = await _context.UpdateBrand(command.Id, brandobj);
                return ticket;
            }
        }
    }
}
