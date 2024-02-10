using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class AddSupportOfficeCommand : IRequest<ApiResponse>
    {
        public SupportOffice model { get; set; }

        public class AddSupportOfficeCommandHandler : IRequestHandler<AddSupportOfficeCommand, ApiResponse>
        {
            private readonly IBtsService _context;
            public AddSupportOfficeCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddSupportOfficeCommand request, CancellationToken cancellationToken)
            {
                request.model.InsertedBy = "Admin";
                request.model.InsertedDateTime = DateTime.Now;
                ApiResponse response = await _context.AddSupportOffice(request.model);
                return response;
            }
        }
    }
}
