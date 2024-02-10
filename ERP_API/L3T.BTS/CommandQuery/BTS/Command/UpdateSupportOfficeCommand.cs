using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class UpdateSupportOfficeCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public SupportOffice supportOffice { get; set; }

        public class UpdateSupportOfficeCommandHandler : IRequestHandler<UpdateSupportOfficeCommand, bool>
        {
            private readonly IBtsService _context;
            public UpdateSupportOfficeCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(UpdateSupportOfficeCommand command, CancellationToken cancellationToken)
            {
                var supportOfficeobj = await _context.GetSupportOfficeById(command.Id);
                if (supportOfficeobj != null)
                {
                    if (!string.IsNullOrEmpty((command.supportOffice.Name).Trim()))
                    {
                        supportOfficeobj.Name = command.supportOffice.Name;
                    }
                }
                var office = await _context.UpdateSupportOffice(command.Id, supportOfficeobj);
                return office;
            }
        }
    }
}
