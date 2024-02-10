using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class AddBrandCommand : IRequest<ApiResponse>
    {
        public Brand model { get; set; }

        public class AddBrandCommandHandler : IRequestHandler<AddBrandCommand, ApiResponse>
        {
            private readonly IBtsService _context;
            public AddBrandCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddBrandCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _context.AddBrand(request.model);
                return response;
            }
        }


    }
}
