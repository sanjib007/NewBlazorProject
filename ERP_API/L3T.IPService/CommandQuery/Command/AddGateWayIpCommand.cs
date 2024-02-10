using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace IPService.CommandQuery.Command
{
   
    public class AddGateWayIpCommand : IRequest<ApiResponse>
    {
        public AddGateWayIpReq model { get; set; }
        public class AddGateWayIpCommandHandler : IRequestHandler<AddGateWayIpCommand, ApiResponse>
        {
            private readonly IGateWayIpService _context;
            public AddGateWayIpCommandHandler(IGateWayIpService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddGateWayIpCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _context.AddGateWayIpAddress(request.model);
                return response;
            }
        }
    }
}
