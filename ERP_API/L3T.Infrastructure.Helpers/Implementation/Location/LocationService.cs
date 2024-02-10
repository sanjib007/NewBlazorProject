using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.LocationDBContext;
using L3T.Infrastructure.Helpers.Interface.Location;
using L3T.Infrastructure.Helpers.Models.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Implementation.Location
{
    public class LocationService : ILocationService
    {
        private readonly LocationDataWriteContext _context;
        private readonly ILogger<LocationService> _logger;
        private readonly LocationDataReadContext _context4read;
        public LocationService(LocationDataWriteContext context, ILogger<LocationService> logger, LocationDataReadContext context4read)
        {
            _context = context;
            _logger = logger;
            _context4read = context4read;
        }

        public async Task<List<Zone>> GetAllZoneQuery()
        {
            try
            {
                var item = await _context4read.Zone.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllDivisionQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }

        public async Task<List<Division>> GetAllDivisionQuery()
        {
            try
            {
                var item = await _context4read.Division.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllDivisionQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<List<District>> GetAllDistrictQuery()
        {
            try
            {
                var item = await _context4read.District.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllDistrictQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }

        public async Task<List<Upazila>> GetAllUpazilaQuery()
        {
            try
            {
                var item = await _context4read.Upazila.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllUpazilaQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }


        public async Task<ApiResponse> AddZone(Zone requestModel)
        {
            try
            {
                _context.Zone.Add(requestModel);
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

        public async Task<List<Upazila>> GetUpazilaByDistrictIdQuery(long Id)
        {
            try
            {
                var item = await _context4read.Upazila.Where(u=> u.DistrictId == Id).ToListAsync().ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllUpazilaQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }

        public async Task<List<District>> GetDistrictByDivisionIdQuery(long Id)
        {
            try
            {
                var item = await _context4read.District.Where(u => u.DivisionId == Id).ToListAsync().ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllUpazilaQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }


        public async Task<Zone> GetZoneById(long Id)
        {
            _logger.LogInformation("Fetching GetTicketById by Id" + Id);
            try
            {
                var Zoneobj = await _context4read.Zone.FirstOrDefaultAsync(f => f.Id == Id).ConfigureAwait(false);


                return Zoneobj;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the TicketEntry  id is :" + Id + " Exception: ", ex);

            }
            return null;
        }

        public async Task<ApiResponse> UpdateZone(long id, Zone model)
        {
            _logger.LogInformation("updating TicketEntry id is : " + id);
            try
            {
                var OldEntity = await _context4read.Zone.FirstOrDefaultAsync(x => x.Id == id);
                if (OldEntity != null)
                {
                    _context.Zone.Update(model);
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

        public async Task<bool> DeleteZone(long id)
        {
            _logger.LogInformation("Fetching  Brand Data by id : " + id);
            try
            {
                var item = await _context.Zone.SingleOrDefaultAsync(f => f.Id == id);
                if (item != null)
                {
                    _context.Zone.Remove(item);
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


    }
}
