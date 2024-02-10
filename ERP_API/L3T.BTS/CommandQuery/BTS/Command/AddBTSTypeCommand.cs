using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class AddBTSTypeCommand : IRequest<ApiResponse>
    {
        public BTSType model { get; set; }

        public class AddBTSTypeCommandHandler : IRequestHandler<AddBTSTypeCommand, ApiResponse>
        {
            private readonly IBtsService _context;
            public AddBTSTypeCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddBTSTypeCommand request, CancellationToken cancellationToken)
            {
                request.model.InsertedBy = "Admin";
                request.model.InsertedDateTime = DateTime.Now;
                ApiResponse response = await _context.AddBTSType(request.model);
                return response;
            }
        }
    }
}
