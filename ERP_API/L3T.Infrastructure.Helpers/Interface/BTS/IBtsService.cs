using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.BTS
{
    public interface IBtsService
    {
        Task<ApiResponse> AddBtsInfo(BtsInfo requestModel);
        Task<ApiResponse> UpdateBTS(long id, BtsInfo model);
        Task<BtsInfo> GetBtsById(long Id);

        Task<ApiResponse> AddBrand(Brand requestModel);
        Task<bool> UpdateBrand(long id, Brand model);
        Task<Brand> GetBrandById(long Id);
        Task<List<Brand>> GetAllBrandQuery();
        Task<bool> DeleteBrand(long id);

        Task<ApiResponse> AddSupportOffice(SupportOffice requestModel);
        Task<bool> UpdateSupportOffice(long id, SupportOffice model);
        Task<SupportOffice> GetSupportOfficeById(long Id);
        Task<List<SupportOffice>> GetAllSupportOfficeQuery();
        Task<bool> DeleteSupportOffice(long id);

        Task<ApiResponse> AddBTSType(BTSType requestModel);
        Task<bool> UpdateBTSType(long id, BTSType model);
        Task<BTSType> GetBTSTypeById(long Id);
        Task<List<BTSType>> GetAllBTSTypeQuery();
        Task<bool> DeleteBTSType(long id);

    }
}
