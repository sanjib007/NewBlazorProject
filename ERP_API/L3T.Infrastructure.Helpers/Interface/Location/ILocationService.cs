using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.Location
{
    public interface ILocationService
    {
        Task<List<Zone>> GetAllZoneQuery();
        Task<List<Division>> GetAllDivisionQuery();
        Task<List<District>> GetAllDistrictQuery();
        Task<List<Upazila>> GetAllUpazilaQuery();
        Task<ApiResponse> AddZone(Zone requestModel);
        Task<Zone> GetZoneById(long Id);
        Task<ApiResponse> UpdateZone(long id, Zone model);
        Task<bool> DeleteZone(long id);
        Task<List<Upazila>> GetUpazilaByDistrictIdQuery(long Id);
        Task<List<District>> GetDistrictByDivisionIdQuery(long Id);
    }
}
