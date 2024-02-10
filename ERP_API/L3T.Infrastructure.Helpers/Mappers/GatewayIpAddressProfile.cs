using AutoMapper;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.ResponseModel;
using L3T.Infrastructure.Helpers.Models.Ipservice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Mappers
{
    public class GatewayIpAddressProfile : Profile
    {
        public GatewayIpAddressProfile()
        {
            //CreateMap<CreateGatewayCommand, GatewayIpAddress>().ReverseMap();

            CreateMap<GatewayIpAddress,GetGatewayIpAddressByIdResponse >();
            CreateMap<GatewayIpAddress,GetAllGatewayIpAddressResponse>();
        }
    }
}
