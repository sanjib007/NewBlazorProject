﻿using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{

    public class UpdateHardwareInfoCommand : IRequest<ApiResponse>
    {
        public HardwareInfoUpdateRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class UpdateIpTelephonyInfoCommandHandler : IRequestHandler<UpdateHardwareInfoCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public UpdateIpTelephonyInfoCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateHardwareInfoCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.UpdateHardwareInfoData(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}
