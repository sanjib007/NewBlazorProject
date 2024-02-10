using AutoMapper;
using L3T.Infrastructure.Helpers.Models.Test;
using L3T.Infrastructure.Helpers.Models.Test.TestDTO;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using L3T.Infrastructure.Helpers.Models.TicketEntity.TicketDTO;


namespace L3T.Infrastructure.Helpers.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<TicketEntryDTO, TicketEntry>(); // means you want to map from TicketEntryDTO to TicketEntry 
            CreateMap<TicketUpdateDTO, TicketEntry>();
            CreateMap<InsertTestDataRequestDTOModel, Test>().ReverseMap();
        }
    }
}
