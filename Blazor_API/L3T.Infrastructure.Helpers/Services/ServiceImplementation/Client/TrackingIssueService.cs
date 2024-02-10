using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Repositories.Implementation.Client;
using L3T.Infrastructure.Helpers.Repositories.Interface.Client;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Client
{
    public class TrackingIssueService : ITrackingIssueService
    {
        private readonly IClientReqResService _reqResService;
        private readonly ITrackingIssueRepository _trackingIssueRepository;
        private readonly ILogger<MISCustomerInformationService> _logger;
        public TrackingIssueService(IClientReqResService reqResService, ITrackingIssueRepository trackingIssueRepository, ILogger<MISCustomerInformationService> logger)
        {
            _reqResService = reqResService;
            _trackingIssueRepository = trackingIssueRepository;
            _logger = logger;
        }

        public async Task<ApiResponse> AllTicketInfo(int day, string getUserid, string ip)
        {
          
            var methordName = "TrackingIssueService/AllTicketInfo";
            try
            {
                var toDate = DateTime.Now.AddDays(- day).ToString("yyyy-MM-dd"); 
                var fromDate = DateTime.Now.ToString("yyyy-MM-dd");

                var model = new TicketListRequestModel()
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    DateTimeSearchTable = "InitiateDate",
                    AssignEmpId = "",
                    AssignEmpName = "",
                    TicketId = "",
                    CustomerId = "",
                    CustomerName = "",
                    CustomerPhone = "",
                    CustomerEmail = "",
                    Area = "",
                    SupportOffice = "",
                    Status = "",
                    TicketCatagory = "",
                    OrderByTableName = "InitiateDate",
                    DescOrAsc = "DESC",
                    PageNumber =1,
                    PageSize =20000
                };

                var data = await _trackingIssueRepository.GetAllTicketWithFilter(model);

                if (data != null)
                {
                    _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(data)}");
                    return await _reqResService.CreateResponseRequest(model, data, ip, methordName, getUserid, "Ok");
                }
                throw new ApplicationException("Ticket data not found");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(day, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());

            }

        }

        public async Task<ApiResponse> AllTicketInfoByCustomer(string customerId, string getUserid, string ip)
        {
           
            var methordName = "TrackingIssueService/AllTicketInfoByCustomer";
            try
            {
                var preId = "";
                var aCustomerID = "";
                var upperCustomerId = customerId.ToUpper();
                if (upperCustomerId.Contains("L3R"))
                {
                    aCustomerID = customerId;
                }
                else
                {                   

                    if (customerId.Length == 5 || customerId.Length == 6)
                    {
                        aCustomerID = "08.01.001." + customerId.ToString();
                    }
                    else if (customerId.Contains("08.01.001."))
                    {
                        aCustomerID = customerId.ToString();
                    }
                    else if (customerId.Length == 8)
                    {
                        aCustomerID = customerId.ToString();
                    }

                }

                

                var model = new TicketListRequestModel()
                {
                    FromDate = "",
                    ToDate = "",
                    DateTimeSearchTable = "",
                    AssignEmpId = "",
                    AssignEmpName = "",
                    TicketId = "",
                    CustomerId = aCustomerID,
                    CustomerName = "",
                    CustomerPhone = "",
                    CustomerEmail = "",
                    Area = "",
                    SupportOffice = "",
                    Status = "",
                    TicketCatagory = "",
                    OrderByTableName = "InitiateDate",
                    DescOrAsc = "DESC",
                    PageNumber = 1,
                    PageSize = 500
                };

                var data = await _trackingIssueRepository.GetAllTicketWithFilter(model);

                if (data != null)
                {
                    _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(data)}");
                    return await _reqResService.CreateResponseRequest(model, data, ip, methordName, getUserid, "Ok");
                }
                throw new ApplicationException("Ticket data not found");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(customerId, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());

            }

        }


        public async Task<ApiResponse> AddTicketRsmOrMis(TicketCreateReqModel ReqModel, string getUserid, string ip)
        {
            var response = new ApiResponse();
            var methordName = "TrackingIssueService/AddTicketRsmOrMis";
            var subscriber_code = ReqModel.subscriberCode.Replace(" ", "");
            var subscriber_codeUpper = subscriber_code.ToUpper();
            var CustomerPrefix = subscriber_codeUpper.Substring(0, 3).ToLower();

            try
            {

                if (CustomerPrefix == "L3R") // RSM
                {

                    var cdata = await _trackingIssueRepository.RsmProfileView(subscriber_codeUpper.Trim());
                    if (cdata == null)
                    {
                        throw new ApplicationException("subscriber not found.");
                    }
                    else
                    {
                        var getResRsm = await _trackingIssueRepository.AddRepositoryTicketRsm(ReqModel);

                        _reqResService.CreateResponseRequest(subscriber_codeUpper, getResRsm, ip, methordName, getUserid, "Ok");
                        _logger.LogInformation(methordName + getResRsm.ToString());
                        return getResRsm;
                    }
                }
                else
                {
                    //MIS
                    string misSubscriberCode = "";

                    if (ReqModel.subscriberCode.Length == 5 || ReqModel.subscriberCode.Length == 6)
                    {
                        misSubscriberCode = "08.01.001." + ReqModel.subscriberCode.ToString();
                    }
                    else if (ReqModel.subscriberCode.Contains("08.01.001."))
                    {
                        misSubscriberCode = ReqModel.subscriberCode.ToString();
                    }
                    else if (ReqModel.subscriberCode.Length == 8)
                    {
                        misSubscriberCode = ReqModel.subscriberCode.ToString();
                    }

                    ReqModel.subscriberCode = misSubscriberCode;

                    var getResMis = await _trackingIssueRepository.AddRepositoryTicketMis(ReqModel);

                    _reqResService.CreateResponseRequest(ReqModel, getResMis, ip, methordName, getUserid, "Ok");                 
                    _logger.LogInformation(methordName + getResMis.ToString());
                    return getResMis;
                }
            }
            catch (Exception ex)
            {              
                _logger.LogInformation("Exception " + ex.Message.ToString());
                return await _reqResService.CreateResponseRequest(ReqModel.subscriberCode, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());
              

            }

        }


        public async Task<ApiResponse> RSMComplainTicketLogs(string ticketNo, string getUserid, string ip)
        {
            var methodName = "TrackingIssueService/RSMComplainTicketLogs";
            try
            {
                var logsData = await _trackingIssueRepository.GetRSMComplainTicketLogs(ticketNo);
                if (logsData != null)
                {
                    _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(logsData)}");
                    return await _reqResService.CreateResponseRequest(ticketNo, logsData, ip, methodName, getUserid, "Ok");
                }
                throw new ApplicationException("Ticket data not found");
               
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(ticketNo, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());

            }
        }



        public async Task<ApiResponse> GetAllTicketsNature(string systemType, string getUserid, string ip)
        {
            var response = new ApiResponse();
            var methordName = "TrackingIssueService/GetAllTicketsNature";

            if (systemType == null || !(systemType == "RSM" || systemType == "MIS"))
            {
                throw new ApplicationException("Please provide systemType like RSM OR MIS.");
            }
            List<TicketNature> tNaturedata = new List<TicketNature>();
            try
            {               

                if (systemType == "RSM")   // if system is RSM
                {

                    var categoryListdata = await _trackingIssueRepository.GetRSM_NatureSetup();
                    
                        if (categoryListdata != null)
                        {
                            foreach (var category in categoryListdata)
                            {
                                tNaturedata.Add(new TicketNature { natureId = category.NatureID.ToString(), natureName = category.NatureName });
                            }

                        _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(tNaturedata)}");
                            return await _reqResService.CreateResponseRequest(systemType, tNaturedata, ip, methordName, getUserid, "Ok");
                        }
                        throw new ApplicationException("Category data not found");
                   
                   
                }
                else
                {
                    var categoryListMisdata = await _trackingIssueRepository.GetMis_NatureSetup();

                    if (categoryListMisdata != null)
                    {

                        foreach (var categoryData in categoryListMisdata)
                        {
                            tNaturedata.Add(new TicketNature { natureId = categoryData.C_id.ToString(), natureName = categoryData.SelfCategory });
                        }

                        _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(tNaturedata)}");
                        return await _reqResService.CreateResponseRequest(systemType, tNaturedata, ip, methordName, getUserid, "Ok");
                    }
                    throw new ApplicationException("Category data not found");

                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(systemType, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());

            }
        }


        public async Task<ApiResponse> GetAssignedPackageByCustomer(string customerId, string getUserid, string ip)
        {

            var methordName = "TrackingIssueService/GetAssignedPackageByCustomer";
            try
            {
                var preId = "";
                var aCustomerID = "";
                object data = "";
                var upperCustomerId = customerId.ToUpper();
                if (upperCustomerId.Contains("L3R"))
                {
                    aCustomerID = customerId;
                    data = await _trackingIssueRepository.GetHydraNetworkInformation(aCustomerID);
                }
                else
                {
                    //throw new ApplicationException("Assigned Package data not found");

                    if (customerId.Length == 5 || customerId.Length == 6)
                    {
                        aCustomerID = "08.01.001." + customerId.ToString();
                    }
                    else if (customerId.Contains("08.01.001."))
                    {
                        aCustomerID = customerId.ToString();
                    }
                    else if (customerId.Length == 8)
                    {
                        aCustomerID = customerId.ToString();
                    }

                   data = await _trackingIssueRepository.GetMisNetworkInformationResponseModel(customerId); 
                }

                if (data != null)
                {
                    _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(data)}");
                    return await _reqResService.CreateResponseRequest(customerId, data, ip, methordName, getUserid, "Ok");
                }
                throw new ApplicationException("Assigned Package data not found");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _reqResService.CreateResponseRequest(customerId, ex, ip, methordName, getUserid, "Error", ex.Message.ToString());

            }

        }




    }
}
