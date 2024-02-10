using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class AddCoordinatesCommand : IRequest<ApiResponse>
    {
        public AddCoordinatesRequestModel model { get; set; }
        public string ip { get; set; }

        public class AddCoordinatesCommandHandler : IRequestHandler<AddCoordinatesCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public AddCoordinatesCommandHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(AddCoordinatesCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.AddCoordinates(request.model, request.ip);
                return reaponse;
            }
        }
    }
}
