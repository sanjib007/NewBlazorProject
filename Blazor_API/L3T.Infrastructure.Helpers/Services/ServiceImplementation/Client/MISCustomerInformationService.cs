using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Repositories.Interface.Client;
using L3T.Infrastructure.Helpers.Services.ServiceImplementation.ThirdParty;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Client
{
    public class MISCustomerInformationService: IMISCustomerInformationService
    {
        
        private readonly IClientReqResService _reqResService;
        private readonly IMISCustomerInformationRepository _misCustomerRepository;
        private readonly ILogger<MISCustomerInformationService> _logger;
        public MISCustomerInformationService(IClientReqResService reqResService, IMISCustomerInformationRepository misCustomerRepository, ILogger<MISCustomerInformationService> logger)
        {
          
            _reqResService = reqResService;
            _misCustomerRepository = misCustomerRepository;
            _logger = logger;

        }
        public async Task<ApiResponse> MisCustomerPhone(string customerId, string getUserid, string ip)
        {
            var response = new ApiResponse();
            var methordName = "MisCustomerService/MisCustomerPhoneNumber";
            try
            {
                var customerPrefix = customerId.Substring(0, 3).ToLower();
                if(customerPrefix == "l3r")
                {                 

                    var data = await _misCustomerRepository.RSMCustomerPhoneNumber(customerId);

                    if (data != null)
                    {
                        _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(data)}");
                        return await _reqResService.CreateResponseRequest(customerId, data, ip, methordName, getUserid, "Ok");
                    }
                }
                else
                {
                    string preId = "";
                    if (customerId.Contains("08.01.001."))
                    {
                        preId = customerId.Substring(0, 10);
                    }
                    var CustomerID = ((preId == "08.01.001.") ? customerId : "08.01.001." + customerId);

                    var data = await _misCustomerRepository.CustomerPhoneNumber(CustomerID);

                    if (data != null)
                    {
                        _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(data)}");
                        return await _reqResService.CreateResponseRequest(CustomerID, data, ip, methordName, getUserid, "Ok");
                    }
                }               
                throw new ApplicationException("Customer data not found");
               
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(customerId, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());

            }

        }



        public async Task<ApiResponse> MisCustomerCode(string mobileNo, string getUserid, string ip)
        {
            var response = new ApiResponse();
            var methordName = "MisCustomerService/MisCustomerCustomerCode";
            try
            {
                string area = mobileNo.Substring(0, 3);
                string major = mobileNo.Substring(3, 3);
                string minor = mobileNo.Substring(6);
                string customerPhone = "";

               if(area != "+88" && mobileNo.Length < 11)
                {
                    throw new Exception("Your provided mobile number is not valid");
                }

                if (area == "+88")
                {
                    customerPhone = major + minor;

                }
                else if (mobileNo.Length == 11)
                {
                    customerPhone = mobileNo;
                }
                var  data = await _misCustomerRepository.CustomerCode(customerPhone);
                if (data != null)
                {
                    _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(data)}");
                    return await _reqResService.CreateResponseRequest(mobileNo, data, ip, methordName, getUserid, "Ok");
                }
                throw new Exception("Data not found.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(mobileNo, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());
                
            }

        }


    }
}
