using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.OltDBContext;
using L3T.Infrastructure.Helpers.Interface.OLT;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using L3T.Infrastructure.Helpers.Models.OLT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tik4net;

namespace L3T.Infrastructure.Helpers.Implementation.OLT
{
    public class OltService : IOltService
    {
        private readonly OltDataWriteContext _context;
        private readonly ILogger<OltService> _logger;
        private readonly OltDataReadContext _context4read;
        public OltService(OltDataWriteContext context, ILogger<OltService> logger, OltDataReadContext context4read)
        {
            _context = context;
            _logger = logger;
            _context4read = context4read;
        }

        public async Task<ApiResponse> AddOLTInfo(OltInfo requestModel)
        {
            try
            {
                _context.OltInfo.Add(requestModel);
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

        public async Task<ApiResponse> GetOltInfo(long Id)
        {
            var methordName = "OltService/GetOltInfo";

            try
            {
                OltInfo oltobj = await _context4read.OltInfo.FirstOrDefaultAsync(f => f.ID == Id).ConfigureAwait(false);

                if (oltobj == null)
                    {
                        throw new Exception("OLT information not found.");
                    }

                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = oltobj
                };
                return apiResponse;
            }
            catch (Exception ex)
            {
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                return response;
            }
        }
    }
}
