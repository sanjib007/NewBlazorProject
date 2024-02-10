using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.OLT;
using L3T.Infrastructure.Helpers.Models.OLT;
using MediatR;

namespace L3T.OLT.CommandQuery.OLT.Command
{
    public class AddOLTCommand : IRequest<ApiResponse>
    {
        public OltInfo model { get; set; }

        public class AddOLTCommandHandler : IRequestHandler<AddOLTCommand, ApiResponse>
        {
            private readonly IOltService _context;
            public AddOLTCommandHandler(IOltService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddOLTCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _context.AddOLTInfo(request.model);
                return response;
            }
        }
    }
}
