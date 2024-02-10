using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain;
using L3T.Infrastructure.Helpers.Models.MISInstallation.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex
{
    public class MisDBContext : DbContext
    {
        public MisDBContext(DbContextOptions<MisDBContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior =
            QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public DbSet<GetAllPendingTicketByAssignUserResponseModel> ViewAllTicketForFieldForce { get; set; }
        public DbSet<tbl_ClosingNatureResponseModel> tbl_ClosingNature { get; set; }
        public DbSet<tbl_SupportDelayResonResopnseModel> tbl_SupportDelayReson { get; set; }
        public DbSet<tbl_ReasonForOutageResoponseModel> tbl_ReasonForOutage { get; set; }
        public DbSet<GetTeamIdModel> GetTeamId { get; set; }
        public DbSet<GetChangeEngListModel> GetChangeEmginnerList { get; set; }
        public DbSet<SubscriptionInfoResponse> GetSubscriptionInfos { get; set; }
        public DbSet<HardwareInfoResponseModel> GetHardwareInfos { get; set; }
        public DbSet<CustomerInfoResponseModel> GetCustomerInfos { get; set; }
        public DbSet<BandwidthModel> GetBandwidthInfos { get; set; }
        public DbSet<BtsSetupInfoModel> GetBtsSetupInfos { get; set; }
        public DbSet<MediaInfoModel> GetMediaInfos { get; set; }
        public DbSet<IpTelephonyInfoModel> GetIpTelephonyInfos { get; set; }
        public DbSet<MktAndBillingResponseModel> GetMktAndBillingInfos { get; set; }
        public DbSet<InstallationCommentResponseModel> GetInstallationCommentInfo { get; set; }
        public DbSet<MaxIdModel> GetMaxIdInfo { get; set; }
        public DbSet<ClientDatabaseIPDetailsModel> GetIpDetailsInfo { get; set; }
        public DbSet<SupportOfficeModel> GetSupportOfficeInfo { get; set; }
        public DbSet<WireSetupModel> GetWireSetupInfo { get; set; }
        public DbSet<TechnologySetupModel> GetTechnologySetupInfo { get; set; }
        public DbSet<MediaSetupModel> GetMediaSetupInfo { get; set; }
        public DbSet<GeneralInfoResponseModel> GetGeneralInfo { get; set; }
        public DbSet<IpTelephonyResponseModel> GetIpTelephonyEditInfo { get; set; }
        public DbSet<IntranetInfoResponseModel> GetIntranetInfo { get; set; }
        public DbSet<BtsSelectedItemResponseModel> GetBtsSelectedId { get; set; }
        public DbSet<BtsSetupListResponseModel> GetBtsSetupList { get; set; }
        public DbSet<TeamNameResponseModel> GetTeamNames { get; set; }
        public DbSet<GetPendingCategoryResponseModel> GetPendingCategories { get; set; }
        public DbSet<PendingReasonResponseModel> GetPendingResons { get; set; }
        public DbSet<ServiceCheckboxListResponseModel> GetCheckboxList { get; set; }
        public DbSet<MisChecklistDetailsModel> GetMisChecklistInfos { get; set; }
        public DbSet<MisChecklistDetailsB2BResponseModel> GetMisChecklistB2BInfos { get; set; }
        public DbSet<ChecklistResponseModel> GetChecklist { get; set; }
        public DbSet<CheckListB2BResponseModel> GetChecklistB2BData { get; set; }
        public DbSet<RouterTypeResponseModel> GetRouterType { get; set; }
        public DbSet<ControllerOwnerResponseModel> GetControllerOwner { get; set; }
        public DbSet<SingleApResponseModel> GetSingleAp { get; set; }
        public DbSet<MultipleApResponseModel> GetMultipleAp { get; set; }
        public DbSet<ChannelWidth20MHzResponseModel> GetChannelWidth20MHz { get; set; }
        public DbSet<GhzEnabledResponseModel> GetGhzEnabled { get; set; }
        public DbSet<ChannelWidthAutoResponseModel> GetChannelWidthAuto { get; set; }
        public DbSet<Channelbetween149_161ResponseModel> GetChannelbetween149_161 { get; set; }
        public DbSet<ClientBillingAddressModel> GetClientBillingAddressInfos { get; set; }
        public DbSet<ClientP2PAddressModel> GetClientP2PAddressInfos { get; set; }
        public DbSet<InitialServiceRestoredNotificationModel> InitialServiceRestoredNotification { get; set; }
        public DbSet<ComplainAccessPermissionModel> ComplainAccessPermission { get; set; }
        public DbSet<tbl_complain_info_Model> TblComplainInfoModel { get; set; }
        public DbSet<TblComplainInfoModel> TblComplainInfo { get; set; }
        public DbSet<TProjectModel> TProject { get; set; }
        public DbSet<SP_MaxTicketIdModel> SP_MaxTicketId { get; set; }
        public DbSet<GetProjectByRefNoModel> GetProjectByRefNo { get; set; }
        public DbSet<TMultiTaskModel> t_MultiTask { get; set; }
        public DbSet<TblComplainMailLoopingModel> TblComplainMailLooping { get; set; }
        public DbSet<tblComplainEmailFormatModel> tblComplainEmailFormat { get; set; }
        public DbSet<EmailBodyResponseModel> getMailInfo { get; set; }
        public DbSet<TeamEmailResponseModel> getTeamEmail { get; set; }
        public DbSet<CompanyNameResponseModel> getCompanyName { get; set; }
        public DbSet<TblTeamMemPermission> TblTeamMemPermission { get; set; }
        public DbSet<t_EmployeeProjectModel> t_EmployeeProject { get; set; }
        public DbSet<SP_CommonResponseModel> ResolvedTicketForApi { get; set; }
        public DbSet<FowardTicketDetailsResponseModel> FowordTicketDetails { get; set; }
        public DbSet<TblComCategoryModel> tblComCategory { get; set; }
        public DbSet<TblTeamInfoModel> TblTeamInfo { get; set; }
        public DbSet<TblCloseComplainDetailsModel> tblCloseComplainDetails { get; set; }
        public DbSet<ClientDatabaseMainModel> ClientDatabaseMain { get; set; }
        public DbSet<SP_ForwardTicketApiModel> ForwardTicketApi { get; set; }
        public DbSet<DistributorInfoModel> dDistributorInfo { get; set; }
        public DbSet<SP_ComplainTicketInformationAPIModel> ComplainTicketInformationAPI { get; set; }
        public DbSet<tbl_clientDatabaseItemDetModel> tbl_ClientDatabaseItemDetModels { get; set; }
        public DbSet<clientDatabaseFileResponseModel> ClientDatabaseFiles { get; set; }
        public DbSet<ClientDatabaseWireSetupResponseModel> ClientDatabaseWireSetup { get; set; }
        public DbSet<ClientDatabseTechnologySetupResponseModel> ClientDatabseTechnologySetup { get; set; }
        public DbSet<ClientDatabaseMediaSetupResponseModel> ClientDatabaseMediaSetup { get; set; }
        public DbSet<SupportOfficeResponseModel> SupportOffice { get; set; }
        public DbSet<MisInstallationTicketListViewModel> MisInstallationTicketList { get; set; }
        public DbSet<ClientDatabaseMediaDetailsResponseModel> ClientDatabaseMediaDetails { get; set; }
        public DbSet<BackboneCablePathTypeSetupResponseModel> BackboneCablePathTypes { get; set; }
        public DbSet<ClientDatabaseMainResponseModel> ClientDatabaseMainResponseModel { get; set; }
        public DbSet<ClientBillingAddressResponseModel> ClientBillingAddress { get; set; }
        public DbSet<ClientTechnicalInfoResponseModel> ClientTechnicalInfos { get; set; }
        public DbSet<ClientDatabaseEquipmentResponseModel> ClientDatabaseEquipments { get; set; }
        public DbSet<ClientDatabaseItemDetResponseModel> ClientDatabaseItemDets { get; set; }
        public DbSet<tbl_SubSpliterEntryResponseModel> tbl_SubSpliterEntrys { get; set; }
        public DbSet<tbl_darkfiber_clientResponseModel> tbl_darkfiber_clients { get; set; }
        public DbSet<tbl_NttnDetailsResponseModel> tbl_NttnDetails { get; set; }
        public DbSet<InstallationCompletionMediaInfo> InstallationCompletionMediaInfos { get; set; }
        public DbSet<Cli_PendingResponseModel> Cli_PendingModel { get; set; }
        public DbSet<ViewPendingServiceResponseModel> ViewPendingServiceResponseModel { get; set; }
        public DbSet<MailLogResponseModel> MailLogModel { get; set; }
        public DbSet<AreaGroupResponseModel> AreaGroups { get; set; }
        public DbSet<AreaResponseModel> Areas { get; set; }
        public DbSet<ClientStatusSlaResponseModel> ClientStatusSlas { get; set; }
        public DbSet<ClientNatureSetupResponseModel> ClientNatureSetups { get; set; }
        public DbSet<ClientCategorySetupResponseModel> ClientCategorySetups { get; set; }
        public DbSet<ClientGroupSetupResponseModel> ClientGroupSetups { get; set; }
        public DbSet<BackboneNTTNSetupResponseModel> BackboneNTTNSetups { get; set; }
        public DbSet<Typeofp2mlinkSetupResponseModel> Typeofp2mlinkSetups { get; set; }
        public DbSet<MisInstallationServicePackageInfo> MisInstallationServicePackageInfos { get; set; }
        public DbSet<CustomerBillingInfo> CustomerBillingInfos { get; set; }
        public DbSet<ClientDatabaseP2PAddressResponseModel> ClientDatabaseP2PAddresses { get; set; }
        public DbSet<Post_MainInsResponseModel> Post_MainIns { get; set; }
        public DbSet<BtsSetupResponseModel> BtsSetups { get; set; }
        public DbSet<tbl_ODFNameSetUpResponseModel> tbl_ODFNameSetups { get; set; }
        public DbSet<tbl_WebServerResponseModel> tbl_WebServers { get; set; }
        public DbSet<tbl_Team_InfoResponseModel> tbl_Team_Infos { get; set; }
        public DbSet<ServiceWisePermissionResponseModel> ServiceWisePermissions { get; set; }
        public DbSet<EmployeeTicketPriorityResponseModel> EmployeeTicketPrioritys { get; set; }
        public DbSet<ODFDetailsTrayIdResponseModel> ODFDetailsTrayIds { get; set; }
        public DbSet<ODFDetailsportIdResponseModel> ODFDetailsPortIds { get; set; }
        public DbSet<VwSplitColorResponseModel> VwSplitColors { get; set; }
        public DbSet<tbl_ODFDetailsEntryResponseModel> tbl_ODFDetailsEntrys { get; set; }
        public DbSet<View_JoinColorResponseModel> View_JoinColors { get; set; }
        public DbSet<Tbl_DarkFiber_CoreResponseModel> Tbl_DarkFiber_Cores { get; set; }
        public DbSet<View_DarkClient_ColorResponseModel> View_DarkClient_Colors { get; set; }
        public DbSet<BlgCycleSetupResponseModel> BlgCycleSetups { get; set; }
        public DbSet<IPTVResponseModel> IPTVs { get; set; }
        public DbSet<PacakgeNameInfoResponseModel> PacakgeNameInfos { get; set; }
        public DbSet<Host_IPInfoResponseModel> Host_IPInfos { get; set; }
        public DbSet<Kh_IpAddressResponseModel> Kh_IpAddress { get; set; }
        public DbSet<tbl_ColorInfoResponseModel> tbl_Colors { get; set; }
        public DbSet<tbl_Splitter_JoincolorEntryRequestModel> tbl_Splitter_JoincolorEntrys { get; set; }
        public DbSet<tbl_ODF_JoincolorEntryRequestModel> tbl_ODF_JoincolorEntries { get; set; }
        public DbSet<tbl_MainSpliterEntryResponseModel> tbl_MainSpliterEntries { get; set; }
        public DbSet<View_SubSplliterResponseModel> View_SubSplliters { get; set; }
        public DbSet<Cli_EmailLogResponseModel> Cli_EmailLogs { get; set; }
        public DbSet<tbl_SubSpliterEntryModel> tbl_SubSpliterEntry { get; set; }
        public DbSet<colorInfoModel> colorInfo { get; set; }
        public DbSet<VwSplitColorNewResponseModel> vwSplitColorNews { get; set; }
        public DbSet<Cli_InstallationCompleteByTeamResponseModel> Cli_InstallationCompleteByTeams { get; set; }
        public DbSet<Cli_InstallationCompleteByTeamMaxIdResponseModel> Cli_InstallationCompleteByTeamMaxIds { get; set; }
        public DbSet<Cli_PendingTicketInfoResponseModel> cli_PendingTickets { get; set; }
        public DbSet<Cli_PendingCountStatusResponseModel> cli_PendingCounts { get; set; }
        public DbSet<slsSalesDetailsTotalResponseModel> slsSalesDetailsTotals { get; set; }
        public DbSet<slsSalesDetailsMaxIdResponseModel> slsSalesDetailsMaxIds { get; set; }
        public DbSet<IpTvPackageInfoResponseModel> IpTvPackageInfos { get; set; }
        public DbSet<Cli_PendingPrioritySetResponseModel> cli_PendingPrioritySets { get; set; }
        public DbSet<SP_CommonResponseModel> RSMConnectionDoneP2MApi_131 { get; set; }
        public DbSet<Cli_PendingServiceNameModel> cli_PendingServiceName { get; set; }
        public DbSet<View_pendinModel> view_pendin { get; set; }
        public DbSet<Cli_MktpendingModel> Cli_Mktpending { get; set; }
        public DbSet<SP_CommonResponseModel> SP_AddCommentMisInstallationTickeAPI { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
