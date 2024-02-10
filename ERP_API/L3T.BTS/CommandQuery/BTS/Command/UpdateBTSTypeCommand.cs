using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class UpdateBTSTypeCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public BTSType btsType { get; set; }

        public class UpdateBTSTypeCommandHandler : IRequestHandler<UpdateBTSTypeCommand, bool>
        {
            private readonly IBtsService _context;
            public UpdateBTSTypeCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<bool> Handle(UpdateBTSTypeCommand command, CancellationToken cancellationToken)
            {
                var btsTypeObj = await _context.GetBTSTypeById(command.Id);
                if (btsTypeObj != null)
                {
                    if (!string.IsNullOrEmpty((command.btsType.TypeName).Trim()))
                    {
                        btsTypeObj.TypeName = command.btsType.TypeName;
                    }
                }
                var office = await _context.UpdateBTSType(command.Id, btsTypeObj);
                return office;
            }
        }
    }
}
