using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Command;

public class DeleteUserFromMikrotikCommand : IRequest<ApiResponse>
{
    public DeleteUserFromMikrotikRouerRequestModel model { get; set; }
    
    public class DeleteUserFromMikrotikCommandHandler : IRequestHandler<DeleteUserFromMikrotikCommand, ApiResponse>
    {
        private readonly ICamsService _context;
        public DeleteUserFromMikrotikCommandHandler(ICamsService context)
        {
            _context = context;
        }
        public async Task<ApiResponse> Handle(DeleteUserFromMikrotikCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = await _context.DeleteUserInfoFromMikrotikRouter(request.model);
            return response;
        }
    }
}