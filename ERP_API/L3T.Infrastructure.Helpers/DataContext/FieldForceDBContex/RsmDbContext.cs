using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex
{

    public class RsmDbContext : DbContext
    {
        public RsmDbContext(DbContextOptions<RsmDbContext> options) : base(options)
        {
        }

        public DbSet<Rsm_ClientDatabaseMainResponseModel> ClientDatabaseMainModel { get; set; }
        public DbSet<Rsm_tbl_SubSpliterEntryResponseModel> SpliterEntryModels { get; set; }
        public DbSet<tbl_Splitter_JoincolorEntryRequestModel> ColorEntryModels { get; set; }        
        public DbSet<GetServiceNameResponseModel> getServiceName { get; set; }
        public DbSet<ServiceResponseModel> ServiceList { get; set; }
        public DbSet<GetTeamNameResponseModel> TeamNameList { get; set; }
        public DbSet<SubscriberResponseModel> Subscribers { get; set; }
        public DbSet<TubeColorResponseModel> TubeColors { get; set; }
        public DbSet<RouterModelResponseModel> RouterModels { get; set; }
        public DbSet<NewFormatAddressResponseModel> NewFormatAddresses { get; set; }
        public DbSet<CableNetworkTypeResponseModel> CableNetworkTypes { get; set; }
        public DbSet<NttnResponseModel> NttnNames { get; set; }
        public DbSet<TypeOfLinkResponseModel> TypeOfLinks { get; set; }
        public DbSet<BtsResponseModel> BtsNames { get; set; }
        public DbSet<BtsSelectedItemResponseModel> GetBtsSelectedId { get; set; }
        public DbSet<GetBtsName> GetBtsSelectedName { get; set; }
        public DbSet<RouterBrandResponseModel> RouterBrands { get; set; }
        public DbSet<RouterRebotTimeResponseModel> RouterRebotTimes { get; set; }
        public DbSet<PendingReasonResponseModel> PendingReasons { get; set; }
        public DbSet<GetPendingReasonIdResponseModel> PendingReasonId { get; set; }
        public DbSet<UpdateHistroryResponseModel> UpdateHistrories { get; set; }
        public DbSet<RouterRebotSettingResponseModel> RouterRebotSettings { get; set; }
        public DbSet<Rsm_EntryOperatorInfoOfTicketResponseModel> EntryOperatorInfosOfTicket { get; set; }
        public DbSet<Rsm_ClientDatabaseItemDetailsResponseModel> ClientDatabaseItemDetails { get; set; }
        public DbSet<Rsm_ClientTechnicalInfoResponseModel> ClientTechnicalInfos { get; set; }
        public DbSet<Rsm_tbl_SubSpliterEntryResponseModel> SubSpliterEntrys { get; set; }
        public DbSet<Rsm_tbl_team_mem_permissionResponseModel> Team_Mem_Permissions { get; set; }
        public DbSet<RSM_SharedCustomerDetailsResponseModel> SharedCustomerDetails { get; set; }
        public DbSet<Rsm_ClientNewAddressInfoResponseModel> ClientNewAddressInfos { get; set; }
        public DbSet<ChecklistResponseModel> GetChecklist { get; set; }
        public DbSet<RouterTypeResponseModel> GetRouterType { get; set; }
        public DbSet<ControllerOwnerResponseModel> GetControllerOwner { get; set; }
        public DbSet<SingleApResponseModel> GetSingleAp { get; set; }
        public DbSet<MultipleApResponseModel> GetMultipleAp { get; set; }
        public DbSet<ChannelWidth20MHzResponseModel> GetChannelWidth20MHz { get; set; }
        public DbSet<GhzEnabledResponseModel> GetGhzEnabled { get; set; }
        public DbSet<ChannelWidthAutoResponseModel> GetChannelWidthAuto { get; set; }
        public DbSet<Channelbetween149_161ResponseModel> GetChannelbetween149_161 { get; set; }
        public DbSet<RsmChecklistDetailsModel> GetRsmChecklistInfos { get; set; }

        public DbSet<RSMComplainTicketResponseModel> RSMComplainTicketResponse { get; set; }
        public DbSet<GetInformationRSMComplainTicketResponseModel> GetInformationRSMComplainTicket { get; set; }
        public DbSet<ComplainLogDetailsModel> ComplainLogDetails { get; set; }
        public DbSet<RsmComplainGenerateTicketSupportType> GetGenerateTicketSupportType { get; set; }
        public DbSet<PostReplayResponse> GetPostReplayResponse { get; set; }
        public DbSet<InstallationAddCommentsResponseModel> AddComments { get; set; }
        public DbSet<RSM_HydraScriptModel> RSM_HydraScript { get; set; }
        public DbSet<VwSplitColorNewResponseModel> ColorSplitModel { get; set; }
        public DbSet<ViewColorResponseModel> ViewColors { get; set; }
        public DbSet<tbl_HelpDeskCategoryModel> tbl_HelpDeskCategory { get; set; }
        public DbSet<RSM_Complain_DetailsModel> RSM_Complain_Details { get; set; }
        public DbSet<RSM_Complain_Details_ViewModel> RSM_Complain_Details_Ticket { get; set; }
        public DbSet<tbl_CasueOfDelayModel> tbl_CauseOfDelay { get; set; }
        public DbSet<GetTicketCategoryAndNeatureModel> GetTicketCategoryAndNeature { get; set; }
        public DbSet<Rsm_tickettypeModel> Rsm_tickettype { get; set; }
        public DbSet<RSM_NatureSetupModel> RSM_NatureSetup { get; set; }
        public DbSet<RSM_SupportOfficeWiseIDModel> RSM_SupportOfficeWiseID { get; set; }
        public DbSet<RSM_TaskAssignModel> RSM_TaskAssign { get; set; }
        public DbSet<RSM_TaskPendingTeamModel> RSM_TaskPendingTeam { get; set; }
        public DbSet<tbl_team_mem_permissionModel> tbl_team_mem_permission { get; set; }
        public DbSet<SP_RSMCloseTicketApiModel> SP_RSMCloseTicketApi { get; set; }
        public DbSet<tbl_BingeServiceDetailsModel> tbl_BingeServiceDetails { get; set; }
        public DbSet<tbl_BackboneFiber_HomeLinkResponseModel> tbl_BackboneFiber_HomeLink { get; set; }
        public DbSet<tbl_BackboneSummitLinkResponseModel> tbl_BackboneSummitLink { get; set; }

        public DbSet<tbl_teamMemPermissionResponseModel> tbl_teamMemPermission { get; set; }
        public DbSet<tbl_SupportOfficeResponseModel> tbl_SupportOffice { get; set; }
        public DbSet<PendingInstallationInfoResponseModel> PendingInstallationInfo { get; set; }

        public DbSet<SP_CommonResponseModel> ResolvedTicketForApi { get; set; }
        public DbSet<RSM_PendingTaskTypeModel> RSM_PendingTaskType { get; set; }
        public DbSet<Tbl_ClientDatabaseFile> Tbl_ClientDatabaseFile { get; set; }
        public DbSet<Tbl_Splitter_JoincolorEntryModel> Tbl_Splitter_JoincolorEntry { get; set; }
        public DbSet<SP_CommonResponseModel> RSMConnectionDoneP2MApi { get; set; }
        public DbSet<View_SubSplliterModel> View_SubSplliter { get; set; }
        public DbSet<RSM_RouterModelSettingModel> RSM_RouterModelSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
