using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.DataContext.BtsDBContext;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Common;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tik4net;

namespace L3T.Infrastructure.Helpers.Implementation.BTS
{
    public class BtsService:IBtsService
    {
        private readonly BtsDataWriteContext _context;
        private readonly ILogger<BtsService> _logger;
        private readonly BtsDataReadContext _context4read;
        //private readonly ISystemService _systemService;
        //private readonly IMailSenderService _mailrepo;

        public BtsService(BtsDataWriteContext context,ILogger<BtsService> logger,BtsDataReadContext context4read)
        {
            _context = context;
            _logger = logger;
            _context4read= context4read;          
        }

        public async Task<ApiResponse> AddBtsInfo(BtsInfo requestModel)
        {
            try
            {
                _context.BtsInfo.Add(requestModel);
                await _context.SaveChangesAsync();
                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error saving TicketEntry SubmitAsync-", ex);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                return response;
            }

        }

        public async Task<BtsInfo> GetBtsById(long Id)
        {
            _logger.LogInformation("Fetching GetTicketById by Id" + Id);
            try
            {
                var btsobj = await _context4read.BtsInfo.FirstOrDefaultAsync(f => f.Id == Id).ConfigureAwait(false);
                

                return btsobj;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the TicketEntry  id is :" + Id + " Exception: ", ex);
                
            }
            return null;
        }

        public async Task<ApiResponse> UpdateBTS(long id, BtsInfo model)
        {
            _logger.LogInformation("updating TicketEntry id is : " + id);
            try
            {
                var OldEntity = await _context4read.BtsInfo.FirstOrDefaultAsync(x => x.Id == id);
                if (OldEntity != null)
                {
                    _context.BtsInfo.Update(model);
                    await _context.SaveChangesAsync();
                    var response = new ApiResponse()
                    {
                        Message = "Update data",
                        Status = "success",
                        StatusCode = 200
                    };

                    return response;
                }
                else
                {
                    var response = new ApiResponse()
                    {
                        Message = "Update failed",
                        Status = "Error",
                        StatusCode = 400
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the TicketEntry  id is :" + id + " Exception: ", ex);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                return response;
            }
        }
        //public async Task<ApiResponse> AddBtsInfo(BtsInfoDTO requestModel)
        //{
        //    var methordName = "BtsService/AddBtsInfo";
        //    try
        //    {
        //        await _btsContext.BtsInfo.AddAsync(requestModel);
        //        await _btsContext.SaveChangesAsync();

        //        var response = new ApiResponse()
        //            {
        //                Message = "create data",
        //                Status = "success",
        //                StatusCode = 200
        //            };

        //            return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        string errormessage = await errorMethord(ex, methordName);
        //        var response = new ApiResponse()
        //        {
        //            Message = errormessage,
        //            Status = "Error",
        //            StatusCode = 400
        //        };
        //        await InsertRequestResponse(requestModel, response, requestModel.TXTRealIP, requestModel.RouterIp, methordName, requestModel.CallerId);
        //        return response;
        //    }
        //}

        #region Brand
        public async Task<ApiResponse> AddBrand(Brand requestModel)
        {
            try
            {
                requestModel.InsertedBy = "Admin";
                requestModel.InsertedDateTime = DateTime.Now;
                _context.Brands.Add(requestModel);
                await _context.SaveChangesAsync();
                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error saving TicketEntry SubmitAsync-", ex);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                return response;
            }

        }
        public async Task<bool> UpdateBrand(long id, Brand model)
        {
            _logger.LogInformation("updating Brand id is : " + id);
            try
            {
                var OldEntity = await _context4read.Brands.FirstOrDefaultAsync(x => x.Id == id);
                if (OldEntity != null)
                {
                    model.UpdatedBy = "Admin";
                    model.UpdatedDateTime = DateTime.Now;
                    _context.Brands.Update(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the Brand  id is :" + id + " Exception: ", ex);
                return false;
            }
        }
        public async Task<Brand> GetBrandById(long Id)
        {
            _logger.LogInformation("Fetching GetTicketById by Id" + Id);
            try
            {
                var item = await _context4read.Brands.FirstOrDefaultAsync(b => b.Id == Id).ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetBrandById by id:" + Id + " Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<List<Brand>> GetAllBrandQuery()
        {
            try
            {
                var item = await _context4read.Brands.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllBrandQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<bool> DeleteBrand(long id)
        {
            _logger.LogInformation("Fetching  Brand Data by id : " + id);
            try
            {
                var item = await _context.Brands.SingleOrDefaultAsync(f => f.Id == id);
                if (item != null)
                {
                    _context.Brands.Remove(item);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when deleteing the Brand by id:" + id + " Exception: ", ex);
                return false;
            }
        }
        #endregion

        #region Support Office
        public async Task<ApiResponse> AddSupportOffice(SupportOffice requestModel)
        {
            try
            {
                //requestModel.InsertedBy = "Admin";
                //requestModel.InsertedDateTime = DateTime.Now;
                _context.SupportOffice.Add(requestModel);
                await _context.SaveChangesAsync();
                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error saving TicketEntry SubmitAsync-", ex);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                return response;
            }

        }
        public async Task<bool> UpdateSupportOffice(long id, SupportOffice model)
        {
            _logger.LogInformation("updating Support Office id is : " + id);
            try
            {
                var OldEntity = await _context4read.SupportOffice.FirstOrDefaultAsync(x => x.Id == id);
                if (OldEntity != null)
                {
                    model.UpdatedBy = "Admin";
                    model.UpdatedDateTime = DateTime.Now;
                    _context.SupportOffice.Update(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the Support Office  id is :" + id + " Exception: ", ex);
                return false;
            }
        }
        public async Task<SupportOffice> GetSupportOfficeById(long Id)
        {
            _logger.LogInformation("Fetching GetSupportOfficeById by Id" + Id);
            try
            {
                var item = await _context4read.SupportOffice.FirstOrDefaultAsync(b => b.Id == Id).ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetSupportOfficeById by id:" + Id + " Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<List<SupportOffice>> GetAllSupportOfficeQuery()
        {
            try
            {
                var item = await _context4read.SupportOffice.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllSupportOfficeQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<bool> DeleteSupportOffice(long id)
        {
            _logger.LogInformation("Fetching  Support Office Data by id : " + id);
            try
            {
                var item = await _context.SupportOffice.SingleOrDefaultAsync(f => f.Id == id);
                if (item != null)
                {
                    _context.SupportOffice.Remove(item);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when deleteing the Support Office by id:" + id + " Exception: ", ex);
                return false;
            }
        }
        #endregion

        #region BTS Type
        public async Task<ApiResponse> AddBTSType(BTSType requestModel)
        {
            try
            {
                //requestModel.InsertedBy = "Admin";
                //requestModel.InsertedDateTime = DateTime.Now;
                _context.BTSType.Add(requestModel);
                await _context.SaveChangesAsync();
                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error saving TicketEntry SubmitAsync-", ex);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                return response;
            }

        }
        public async Task<bool> UpdateBTSType(long id, BTSType model)
        {
            _logger.LogInformation("updating BTS Type id is : " + id);
            try
            {
                var OldEntity = await _context4read.BTSType.FirstOrDefaultAsync(x => x.Id == id);
                if (OldEntity != null)
                {
                    model.UpdatedBy = "Admin";
                    model.UpdatedDateTime = DateTime.Now;
                    _context.BTSType.Update(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the Support Office  id is :" + id + " Exception: ", ex);
                return false;
            }
        }
        public async Task<BTSType> GetBTSTypeById(long Id)
        {
            _logger.LogInformation("Fetching GetBTSTypeById by Id" + Id);
            try
            {
                var item = await _context4read.BTSType.FirstOrDefaultAsync(b => b.Id == Id).ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetBTSTypeById by id:" + Id + " Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<List<BTSType>> GetAllBTSTypeQuery()
        {
            try
            {
                var item = await _context4read.BTSType.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllBTSTypeQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<bool> DeleteBTSType(long id)
        {
            _logger.LogInformation("Fetching  Support Office Data by id : " + id);
            try
            {
                var item = await _context.BTSType.SingleOrDefaultAsync(f => f.Id == id);
                if (item != null)
                {
                    _context.BTSType.Remove(item);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when deleteing the Support Office by id:" + id + " Exception: ", ex);
                return false;
            }
        }
        #endregion

    }
}
