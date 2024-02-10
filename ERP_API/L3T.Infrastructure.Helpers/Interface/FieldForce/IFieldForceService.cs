using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System.Security.Claims;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IFieldForceService
    {
        Task<ApiResponse> AddCoordinates(AddCoordinatesRequestModel model, string ip);
        Task<ApiResponse> GetATicketByTicketId(string ticketid, string userId, string ip);
        Task<ApiResponse> GetAllAssignTicketForUser(string userId, string ip);
        Task<ApiResponse> GetAllAssignTicketForUserWithCount(string userId, string ip, int LastDays = 7);
        Task<ApiResponse> GetClosingNature(string userId, string ip);
        Task<ApiResponse> GetReasonForOutage(long closingNatureId, string userId, string ip);
        Task<ApiResponse> GetSupportDealyReason(string userId, string ip);
        Task<ApiResponse> GetSupportType(string userId, string ip);
        Task<ApiResponse> ResolvedTicket(ResolvedTicketRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> ChangeEngineer(string ticketId, string userId, string ip);
        Task<ApiResponse> InitialServiceRestoredNotification(string ticketId, ClaimsPrincipal user, string ip);
        Task<ApiResponse> ResolvedDetailsMail(string ticketId, ClaimsPrincipal user, string ip);
        Task<ApiResponse> GetTicketLog(string ticketId, string userId, string ip);
        Task<ApiResponse> GetLocationForAllUser(string date, string formTime, string toTime, string ip);
        Task<ApiResponse> GetLocationForAUser(string userId, string date, string? fromTime, string? toTime, string ip);
        Task<ApiResponse> GetInternetTechnologyDataFromMedia(string ip, ClaimsPrincipal user);
        Task<ApiResponse> GetClientDatabseTechnologySetupData(string ip, ClaimsPrincipal user);
        Task<ApiResponse> GetClientDatabaseMediaSetupData(string ip, ClaimsPrincipal user);
        Task<ApiResponse> GetSupportOfficeData(string ip, ClaimsPrincipal user);
        Task<ApiResponse> GetAssignedMisInstallationTicketList(string userId, string ip);
        Task<ApiResponse> UpdateMediaInfoDetails(MediaInformationRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> SupportofficeDataUpdateInClientDatabase(SupportOfficeInfoRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> GetNetworkTypeList(ClaimsPrincipal user, string ip);
        Task<ApiResponse> GetClientDatabaseMainInfoByAddressCode(ClaimsPrincipal user, string ip, string clientAddressCode);
        Task<ApiResponse> GetClientBillingAddressInfoByClientCodeAndSerialNo(string userId, string ip,
            string brClientCode, int brSerialNumber);
        Task<ApiResponse> GetClientTechnicalInfo(string brClientCode, int brSerialNumber, string userId, string ip);
        Task<ApiResponse> GetInstallationCompletionFormData(string brClientCode,
                    int brSerialNumber, string userId, string ip, string clientName);
        Task<ApiResponse> GetTicketPriorityListByTicketId(string ip, ClaimsPrincipal user, string ticketId);
        Task<ApiResponse> UpdateTicketPriorityStatusByTcketId(string userId, string ip, string ticketId,
                    int priorityStatus, int pendingListSlNo, string serviceType);
        Task<ApiResponse> GetMailLogByTicketId(ClaimsPrincipal user, string ip, string ticketId);
        Task<ApiResponse> GetParentData(string ip, ClaimsPrincipal user, string userId, string ticketId);
        Task<ApiResponse> GetClientInformationForNewGo(string ip, ClaimsPrincipal user, string subscriberId, int serialNo);
        Task<ApiResponse> GetP2PAddressForNewGo(string ip, ClaimsPrincipal user, string subscriberId);
        Task<ApiResponse> GetPostMainInstallationDataByServiceId(string ticketId, int serviceId, string ip);
        Task<ApiResponse> GetAllServiceDataForNewGo(string ip, ClaimsPrincipal user, string ticketId, string serviceId,
            string brClientCode, int brSerialNumber, int btsId);
        Task<ApiResponse> GetServiceWisePermissionInfo(string ip, string userId, string ticketId);
        Task<ApiResponse> GetEmployeeTicketPriorityInfo(string ip, string userId, string ticketId);
        Task<ApiResponse> GetConnectivityTrayList(string ip, ClaimsPrincipal user);
        Task<ApiResponse> GetConnectivityPortList(string ip, ClaimsPrincipal user);
        Task<ApiResponse> GetColorRelatedDetailsData(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode);
        Task<ApiResponse> GetP2PFiberDetailsData(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode);
        Task<ApiResponse> GetDarkClientInformation(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode);
        Task<ApiResponse> GetDarkFiberClientColorInformation(string ip, ClaimsPrincipal user, int brSerialNumber,
                    string brClientCode, int noOfCore);
        Task<ApiResponse> GetSMSDataForNewGo(string ip, ClaimsPrincipal user, string ticketId);
        Task<ApiResponse> GetPaymentDataForNewGo(string ip, ClaimsPrincipal user, string ticketId);
        Task<ApiResponse> GetIpTvInfoForNewGo(string ip, ClaimsPrincipal user, string brClientCode);
        Task<ApiResponse> GetPAcakgeNameInfoForNewGo(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode);
        Task<ApiResponse> GetHost_IPInfoForNewGo(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode);
        Task<ApiResponse> GetKh_IpAddressByHostNameForNewGo(string ip, ClaimsPrincipal user, string hostName);
        Task<ApiResponse> GetColorInfo(string ip, ClaimsPrincipal user);
        Task<ApiResponse> AddToODF_JoincolorEntryCommand(string ip, ClaimsPrincipal user, tbl_Splitter_JoincolorEntryRequestModel model);
        Task<ApiResponse> GetCrUploadFile(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode);
        Task<ApiResponse> GetPAcakgeNameInfoForNewGo(string ip, ClaimsPrincipal user, int autoODFID);
        Task<ApiResponse> AddTotbl_ODF_JoincolorEntry(string ip, ClaimsPrincipal user, tbl_ODF_JoincolorEntryRequestModel model);
        Task<ApiResponse> GetAllSpliterInfoForNewGo(string ip, ClaimsPrincipal user, string prefixText, int count, string btsId);
        Task<ApiResponse> GetDataLogByIpComment(ClaimsPrincipal user, string ip, string ipComment, string ticketId, string teamName);
        Task<ApiResponse> UpdateP2MDataForTicketCloseForNewGo(string ip, ClaimsPrincipal user, string splitterNameFiber,
        string fiberoltbrand, string fiberoltname, int fiberpon, int fiberport, int portcapfiber, string encloserno,
        string refnoOrTicketId, string branchidOrCliCode, int slnoOrCustomerCodeSlNo, string customerName,
                    string customerBranchName, string customerAddressline1, int btsSetupId, string fiberLaser,
        string btsName, int cableNumber, string linkPathFiber, string remarksFiber, string emailBody);
        Task<ApiResponse> UpdateP2PDataForTicketCloseForNewGo(string ip, ClaimsPrincipal user, string refnoOrTicketId,
            string branchidOrCliCode, int slnoOrCustomerCodeSlNo, string emailBody, int cablePathID_DDLcablnetwork,
            string Typeofp2mlink_DDLtypeofp2mlinkText, string p2pSwitchRouIP, string p2pSwRouPortNew,
        string p2pLaserNew, string p2PMCTypeInfo, string btsSetupName, int btsSetupId, string customerName, string customerBranchName,
        string customerAddressline1, string linkpathp2p_GooglePath, string remarksp2pText, int autoOFIID_IncrementID);
        Task<ApiResponse> DoneP2MDataForTicketCloseForNewGo(string ip, ClaimsPrincipal user, string branchidOrCliCode,
                int slnoOrCustomerCodeSlNo, string customerName, string customerBranchName, string customerAddressline1,
                int cablnetwork_CablePathID, int typeofp2mlink_Typeofp2mlinkID, string splitterName, string fiberLaser,
                    int cableNumberFiber, int nTTNID, string teamName, string comments, string ticketId, int teamId_CategorySetupId,
                    string distributorId_From_ClientDataBasemain, decimal otcAmount_ClientDbMain, string serviceNarration_ClientDbMain,
                    decimal monthlyAmount_Amount_ClientDbMain, string entityName_Hostname, string realIp_ClientTechnicalInfo,
                    string nTTN_Name, DateTime installationDate_ClientDbMain, string designationName, string departmentName,
                    DateTime billingDate, string installation_MktBilling_comment, string linkidp2m_SummitLinkId,
                    string scridp2m_FiberAtHomeSCRID, string bahoncoreid, string bahonlinkid, int btsId_FIBERMEDIADETAILSP2M,
                    string btsName_FIBERMEDIADETAILSP2M, string fiberPon_FiberMediaDetailsP2M, int fiberPort_FiberMediaDetailsP2M,
                    string fiberoltbrand_FiberMediaDetailsP2M, string fiberoltname_FiberMediaDetailsP2M, string fiberlaser_FiberMediaDetailsP2M,
                    string fiberPortCapacity_FiberMediaDetailsP2M, string linkpathFb_ConnectivityDetailsP2M, string remarksFb_ConnectivityDetailsP2M,
                    string latitude, string longiTude);





    }
}
