namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class MisInstP2MTicketCloseRequestModel
    {
        public string branchidOrCliCode { get; set; }
        public int slnoOrCustomerCodeSlNo { get; set; }
        public string customerName { get; set; }
        public string customerBranchName { get; set; }
        public string customerAddressline1 { get; set; }
        public int cablnetwork_CablePathID { get; set; }
        public int typeofp2mlink_Typeofp2mlinkID { get; set; }
        public string splitterName { get; set; }
        public string fiberLaser { get; set; }
        public int cableNumberFiber { get; set; }
        public int nTTNID { get; set; }
        public string teamName { get; set; }
        public string comments { get; set; }
        public string ticketId { get; set; }
        public int teamId_CategorySetupId { get; set; }
        public string distributorId_From_ClientDataBasemain { get; set; }
        public decimal otcAmount_ClientDbMain { get; set; }
        public string serviceNarration_ClientDbMain { get; set; }
        public decimal monthlyAmount_Amount_ClientDbMain { get; set; }
        public string entityName_Hostname { get; set; }
        public string realIp_ClientTechnicalInfo { get; set; }
        public string nTTN_Name { get; set; }
        public DateTime installationDate_ClientDbMain { get; set; }
        public string designationName { get; set; }
        public string departmentName { get; set; }
        public DateTime billingDate { get; set; }
        public string installation_MktBilling_comment { get; set; }
        public string linkidp2m_SummitLinkId { get; set; }
        public string scridp2m_FiberAtHomeSCRID { get; set; }
        public string bahoncoreid { get; set; }
        public string bahonlinkid { get; set; }
        public int btsId_FIBERMEDIADETAILSP2M { get; set; }
        public string btsName_FIBERMEDIADETAILSP2M { get; set; }
        public string fiberPon_FiberMediaDetailsP2M { get; set; }
        public int fiberPort_FiberMediaDetailsP2M { get; set; }
        public string fiberoltbrand_FiberMediaDetailsP2M { get; set; }
        public string fiberoltname_FiberMediaDetailsP2M { get; set; }
        public string fiberlaser_FiberMediaDetailsP2M { get; set; }
        public string fiberPortCapacity_FiberMediaDetailsP2M { get; set; }
        public string linkpathFb_ConnectivityDetailsP2M { get; set; }
        public string remarksFb_ConnectivityDetailsP2M { get; set; }
        public string latitude { get; set; }
        public string longiTude { get; set; }
    }
}
