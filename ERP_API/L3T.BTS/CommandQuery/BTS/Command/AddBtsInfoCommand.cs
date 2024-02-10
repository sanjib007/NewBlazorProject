using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class AddBtsInfoCommand : IRequest<ApiResponse>
    {
        public BtsInfo model { get; set; }

        public class AddBtsInfoCommandHandler : IRequestHandler<AddBtsInfoCommand, ApiResponse>
        {
            private readonly IBtsService _context;
            public AddBtsInfoCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddBtsInfoCommand request, CancellationToken cancellationToken)
            {
                request.model.InsertedBy = "Admin";
                request.model.InsertedDateTime = DateTime.Now;
                ApiResponse response = await _context.AddBtsInfo(request.model);
                return response;
            }
        }
    

    }
}
