using AutoMapper;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;

namespace L3T.BTS.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //CreateMap<BtsInfo, BtsInfoDTO>();
            //CreateMap<Brand, BrandDTO>(); // means you want to map from StudentModel to Student
            //CreateMap<SupportOffice, SupportOfficeDTO>();
            //CreateMap<BTSType, BTSTypeDTO>();
            CreateMap<BtsInfoDTO, BtsInfo>();
            CreateMap<BtsUpdateDTO, BtsInfo>();
            CreateMap<BrandDTO, Brand>(); // means you want to map from StudentModel to Student
            CreateMap<BrandUpdateDTO, Brand>();
            CreateMap<SupportOfficeDTO, SupportOffice>();
            CreateMap<BTSTypeDTO, BTSType>();
        }
    }
}
