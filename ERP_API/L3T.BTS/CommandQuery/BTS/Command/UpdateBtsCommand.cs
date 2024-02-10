using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.BTS;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3T.BTS.CommandQuery.BTS.Command
{
    public class UpdateBtsCommand:IRequest<ApiResponse>
    {

        public long Id { get; set; }
        public BtsInfo BtsObj { get; set; }
        public class UpdateBtsCommandHandler : IRequestHandler<UpdateBtsCommand, ApiResponse>
        {
            private readonly IBtsService _context;
            public UpdateBtsCommandHandler(IBtsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateBtsCommand command, CancellationToken cancellationToken)
            {                
                var btsmodelobj = await _context.GetBtsById(command.Id);
                if (btsmodelobj != null)
                {
                    if (command.BtsObj.ZoneId > 0)
                    {
                        btsmodelobj.ZoneId = command.BtsObj.ZoneId;
                    }
                    if (command.BtsObj.DivisionId > 0)
                    {
                        btsmodelobj.DivisionId = command.BtsObj.DivisionId;
                    }
                    if (command.BtsObj.DistrictId > 0)
                    {
                        btsmodelobj.DistrictId = command.BtsObj.DistrictId;
                    }
                    if (command.BtsObj.ThanaId > 0)
                    {
                        btsmodelobj.ThanaId = command.BtsObj.ThanaId;
                    }
                    if (!string.IsNullOrEmpty(command.BtsObj.Area))
                    {
                        btsmodelobj.Area = command.BtsObj.Area;
                    }
                    if (command.BtsObj.SupportOfficeId > 0)
                    {
                        btsmodelobj.SupportOfficeId = command.BtsObj.SupportOfficeId;
                    }
                    if (command.BtsObj.BTSTypeId > 0)
                    {
                        btsmodelobj.BTSTypeId = command.BtsObj.BTSTypeId;
                    }
                    if (!string.IsNullOrEmpty(command.BtsObj.OldBTSName))
                    {
                        btsmodelobj.OldBTSName = command.BtsObj.OldBTSName;
                    }
                    if (!string.IsNullOrEmpty(command.BtsObj.BTSName))
                    {
                        btsmodelobj.BTSName = command.BtsObj.BTSName;
                    }
                    btsmodelobj.Status = command.BtsObj.Status;
                    if (command.BtsObj.BTSModeId > 0)
                    {
                        btsmodelobj.BTSModeId = command.BtsObj.BTSModeId;
                    }



                    btsmodelobj.UpdatedBy = "Admin";
                    btsmodelobj.UpdatedDateTime = DateTime.Now;
                }                              

                ApiResponse response = await _context.UpdateBTS(command.Id, btsmodelobj);
                return response;
            }
        }

    }
}
