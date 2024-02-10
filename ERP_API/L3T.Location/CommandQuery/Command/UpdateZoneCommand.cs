using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Location;
using MediatR;

namespace L3T.Location.CommandQuery.Command
{
    public class UpdateZoneCommand : IRequest<ApiResponse>
    {

        public long Id { get; set; }
        public Zone model { get; set; }
        public class UpdateZoneCommandHandler : IRequestHandler<UpdateZoneCommand, ApiResponse>
        {
            private readonly ILocationService _context;
            public UpdateZoneCommandHandler(ILocationService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateZoneCommand command, CancellationToken cancellationToken)
            {
                var Zoneobj = await _context.GetZoneById(command.Id);
                if (Zoneobj != null)
                {
                    //btsmodelobj.ID = command.BtsInfoo.ID;
                    //Zoneobj.Id = command.Zone.Id;
                    Zoneobj.ZoneName = command.model.ZoneName;
                }

                ApiResponse response = await _context.UpdateZone(command.Id, Zoneobj);
                return response;
            }
        }

    }
}
