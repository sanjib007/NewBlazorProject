using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Interface.ThirdParty;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels.RSM;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using System.Security.Cryptography;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Reflection;
using tik4net.Objects.User;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{
    public class RsmInstallationTicketService : IRsmInstallationTicketService
    {
        private readonly RsmDbContext _rsmDBContext;
        private readonly L3TDbContext _l3tDBContext;
        private readonly ILogger<RsmInstallationTicketService> _logger;
        private readonly IMailSenderService _mailSenderService;
        private readonly IRSMComplainTicketService _rSMComplainTicketService;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private readonly IOtherService _otherService;
        private readonly MisDBContext _misDBContext;
        private readonly IConfiguration _configuration;

        public RsmInstallationTicketService(
            RsmDbContext rsmDBContext,
            L3TDbContext l3tDBContext,
            ILogger<RsmInstallationTicketService> logger,
            IMailSenderService mailSenderService,
            IRSMComplainTicketService rSMComplainTicketService,
            FFWriteDBContext ffWriteDBContext,
            IOtherService otherService,
            MisDBContext misDBContext,
            IConfiguration configuration
        )
        {
            _rsmDBContext = rsmDBContext;
            _l3tDBContext = l3tDBContext;
            _logger = logger;
            _mailSenderService = mailSenderService;
            _rSMComplainTicketService = rSMComplainTicketService;
            _ffWriteDBContext = ffWriteDBContext;
            _otherService = otherService;
            _misDBContext = misDBContext;
            _configuration = configuration;
        }

        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }

        private async Task InsertRequestResponse(object request, object response, string methodName, string requestedIP, string userId, string errorLog)
        {
            var errorMethordName = "RsmInstallationTicketService/InsertRequestResponse";
            try
            {
                var reqResModel = new FFRequestResponseModel()
                {
                    CreatedAt = DateTime.Now,
                    Request = JsonConvert.SerializeObject(request),
                    Response = JsonConvert.SerializeObject(response),
                    RequestedIP = requestedIP,
                    MethodName = methodName,
                    UserId = userId,
                    ErrorLog = errorLog
                };
                await _ffWriteDBContext.fFRequestResponseModels.AddAsync(reqResModel);
                await _ffWriteDBContext.SaveChangesAsync();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }

        public async Task<ApiResponse> GetSalesPersonsInfo(ClaimsPrincipal user, string ip, string employeeId)
        {
            var methodName = "RsmInstallationTicketService/GetSalesPersonsInfo";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT a.*,a.i_seller+' : '+b.user_name as SalesPerson FROM clientDatabaseMain a left outer join tbl_user_info b "
                    + " on a.i_seller=b.userid WHERE MqID = '" + employeeId + "' ";            //'L3R193283'
                var result = await _rsmDBContext.ClientDatabaseMainModel.FromSqlRaw(strSql).FirstOrDefaultAsync();
                if (result != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Get Data successfully",
                        Data = result
                    };
                    return response;
                }
                else
                {
                    throw new Exception("No data found!");
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetInstallationInfoData(string userId, string ip, string ticketId)
        {
            var methodName = "RsmInstallationTicketService/GetInstallationInfoData";
            var response = new ApiResponse();
            try
            {
                response = await GetInstallationInfoModelData(ticketId, userId, ip);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(response, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> AddColorData(string userId, string ip, AddColorRequestModel model)
        {
            var methodName = "RsmInstallationTicketService/AddColorData";
            var response = new ApiResponse();
            IDbContextTransaction transaction = _rsmDBContext.Database.BeginTransaction();
            IDbContextTransaction transaction2 = _misDBContext.Database.BeginTransaction();

            try
            {
                if(string.IsNullOrEmpty(model.StartPoint))
                {
                    throw new Exception("Mising start  point");
                }

                if (string.IsNullOrEmpty(model.CableID))
                {
                    throw new Exception("Mising Cable ID");
                }

                if (string.IsNullOrEmpty(model.StartMeter))
                {
                    throw new Exception("Mising start meter");
                }

                if (string.IsNullOrEmpty(model.EndMeter))
                {
                    throw new Exception("Mising end meter");
                }

                if (string.IsNullOrEmpty(model.EndPoint))
                {
                    throw new Exception("Mising end point");
                }

                if (model.Position==null)
                {
                    throw new Exception("Mising Position");
                }

                string colorEntryInfoQuery = @"SELECT * FROM tbl_Splitter_JoincolorEntry WHERE (SplitterName = '" + model.SplitterName + "') AND (CustomerID = '" +
                model.CustomerID + "') AND (CustomerSl = '" + model.CustomerSl + "') AND (Position = '" + model.Position + "')";
                var colorEntryInfoData = await _rsmDBContext.ColorEntryModels.FromSqlRaw(colorEntryInfoQuery).FirstOrDefaultAsync();

                if(colorEntryInfoData != null)
                {
                    throw new Exception("Duplicate Position");
                }
                int startMeter = Convert.ToInt32(model.StartMeter);
                int endMeter = Convert.ToInt32(model.EndMeter);
                int length = 0;
                if (startMeter > endMeter)
                {
                    length = startMeter - endMeter;
                }
                else length = endMeter - startMeter;

                string colorEntryQuery = @"INSERT INTO tbl_Splitter_JoincolorEntry (BtsID, OltName, PON, Port, SplitterName, CustomerID
                , StartPoint, CableType, TubeColor, CoreColor, CableID, StartMeter, EndMeter, Length, EndPoint, Remarks, EntryUserID, EntryDate, 
                UpdateUserID, UpdateDate, Position, CustomerSl) VALUES ('" +model.BtsID + "','" + model.OltName + "', '" + model.PON + "','" +
                model.Port + "','" + model.SplitterName + "','" +
                model.CustomerID + "','" + model.StartPoint.Replace("'", "''") + "','" +
                model.CableType + "', '" + model.TubeColor + "', '" +
                model.CoreColor + "','" + model.CableID + "','" + model.StartMeter + "','" +
                model.EndMeter + "', '" + length.ToString() + "', '" +
                model.EndPoint.Replace("'", "''") + "', '" +
                model.Remarks.Replace("'", "''") + "','" +
                userId + "', getdate(),'" +
                userId + "', getdate(),'" +
                model.Position + "','" + model.CustomerSl + "')";
                var colorEntryData = await _rsmDBContext.Database.ExecuteSqlRawAsync(colorEntryQuery);

                string colorEntryQuery2 = @"INSERT INTO tbl_Splitter_JoincolorEntry (BtsID, OltName, PON, Port, SplitterName, CustomerID
                , StartPoint, CableType, TubeColor, CoreColor, CableID, StartMeter, EndMeter, Length, EndPoint, Remarks, EntryUserID, EntryDate, 
                UpdateUserID, UpdateDate, Position, CustomerSl) VALUES ('" + model.BtsID + "','" + model.OltName + "', '" + model.PON + "','" +
               model.Port + "','" + model.SplitterName + "','" +
               model.CustomerID + "','" + model.StartPoint.Replace("'", "''") + "','" +
               model.CableType + "', '" + model.TubeColor + "', '" +
               model.CoreColor + "','" + model.CableID + "','" + model.StartMeter + "','" +
               model.EndMeter + "', '" + length.ToString() + "', '" +
               model.EndPoint.Replace("'", "''") + "', '" +
               model.Remarks.Replace("'", "''") + "','" +
               userId + "', getdate(),'" +
               userId + "', getdate(),'" +
               model.Position + "','" + model.CustomerSl + "')";
                var colorEntryData2 = await _misDBContext.Database.ExecuteSqlRawAsync(colorEntryQuery2);

                if (colorEntryData == 1 && colorEntryData2 == 1)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "get Data",
                        Data = null
                    };
                    transaction.Commit();
                    transaction2.Commit();
                    await _misDBContext.SaveChangesAsync();
                    await _rsmDBContext.SaveChangesAsync();
                    await InsertRequestResponse(model, response, methodName, ip, userId, null);
                    return response;
                }
                else throw new Exception("Faield to add data");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction2.Rollback();
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> UpdateColorData(string userId, string ip, UpdateColorRequestModel model)
        {
            var methodName = "RsmInstallationTicketService/UpdateColorData";
            var response = new ApiResponse();
            IDbContextTransaction transaction = _rsmDBContext.Database.BeginTransaction();
            IDbContextTransaction transaction2 = _misDBContext.Database.BeginTransaction();

            try
            {
                if (string.IsNullOrEmpty(model.StartPoint))
                {
                    throw new Exception("Mising start  point");
                }

                if (string.IsNullOrEmpty(model.CableID))
                {
                    throw new Exception("Mising Cable ID");
                }

                if (string.IsNullOrEmpty(model.StartMeter))
                {
                    throw new Exception("Mising start meter");
                }

                if (string.IsNullOrEmpty(model.EndMeter))
                {
                    throw new Exception("Mising end meter");
                }

                if (string.IsNullOrEmpty(model.EndPoint))
                {
                    throw new Exception("Mising end point");
                }

                string colorEntryInfoQuery = @"SELECT * FROM tbl_Splitter_JoincolorEntry WHERE (SplitterName = '" + model.SplitterName + "') AND (CustomerID = '" +
                model.CustomerID + "') AND (CustomerSl = '" + model.CustomerSl + "') AND (Position = '" + model.Position + "')";
                var colorEntryInfoData = await _rsmDBContext.ColorEntryModels.FromSqlRaw(colorEntryInfoQuery).FirstOrDefaultAsync();

                if (colorEntryInfoData != null)
                {
                    throw new Exception("Duplicate Position");
                }
                int startMeter = Convert.ToInt32(model.StartMeter);
                int endMeter = Convert.ToInt32(model.EndMeter);
                int length = 0;
                if (startMeter > endMeter)
                {
                    length = startMeter - endMeter;
                }
                else length = endMeter - startMeter;

                string colorUpdateQuery = @"UPDATE tbl_Splitter_JoincolorEntry SET  StartPoint = '" +
                model.StartPoint.Replace("'", "''") + "', CableType = '" +
                model.CableType + "', TubeColor = '" +
                model.TubeColor + "', CoreColor = '" +
                model.CoreColor + "', CableID = '" +
                model.CableID + "', StartMeter = '" +
                model.StartMeter + "', EndMeter = '" +
                model.EndMeter + "', Length = '" + length.ToString() + "', EndPoint = '" +
                model.EndPoint.Replace("'", "''") + "', Remarks = '" +
                model.Remarks.Replace("'", "''") + "', UpdateUserID = '" +
                userId + "', UpdateDate = getdate(), Position = '" +
                model.Position + "' WHERE autoid = '" + model.autoid + "'";
                var colorUpdateData = await _rsmDBContext.Database.ExecuteSqlRawAsync(colorUpdateQuery);

                string colorUpdateQuery2 = @"UPDATE tbl_Splitter_JoincolorEntry SET  StartPoint = '" +
                model.StartPoint.Replace("'", "''") + "', CableType = '" +
                model.CableType + "', TubeColor = '" +
                model.TubeColor + "', CoreColor = '" +
                model.CoreColor + "', CableID = '" +
                model.CableID + "', StartMeter = '" +
                model.StartMeter + "', EndMeter = '" +
                model.EndMeter + "', Length = '" + length.ToString() + "', EndPoint = '" +
                model.EndPoint.Replace("'", "''") + "', Remarks = '" +
                model.Remarks.Replace("'", "''") + "', UpdateUserID = '" +
                userId + "', UpdateDate = getdate(), Position = '" +
                model.Position + "' WHERE autoid = '" + model.autoid + "'";
                var colorUpdateData2 = await _rsmDBContext.Database.ExecuteSqlRawAsync(colorUpdateQuery2);

                if (colorUpdateData == 1 && colorUpdateData2==1)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "get Data",
                        Data = null
                    };
                    transaction.Commit();
                    transaction2.Commit();
                    await _misDBContext.SaveChangesAsync();
                    await _rsmDBContext.SaveChangesAsync();
                    await InsertRequestResponse(model, response, methodName, ip, userId, null);
                    return response;
                }
                else throw new Exception("Faield to Update data");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction2.Rollback();
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> AddCommentData(string userId, string ip, RsmInstallationAddCommentsRequestModel model)
        {
            var methodName = "RsmInstallationTicketService/AddCommentData";
            var response = new ApiResponse();
            try
            {
                DateTime dateTimeData = DateTime.Now;
                string dateData = dateTimeData.ToString("yyyy-MM-dd");
                string timeData = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
                string dateTimeText = dateData + " " + timeData;

                if (string.IsNullOrEmpty(model.Comments))
                {
                    throw new Exception("Please Input Comments");
                }

                if (model.PendingReasonId == null)
                {
                    throw new Exception("Please select Pending Reason");
                }

                string sql = "EXEC InstallationAddComments @ticketNo,@subscriberCode,@pendingReasonId,@comments,@userId,@dateTimeText";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ticketNo", Value = model.TicketNo },
                    new SqlParameter { ParameterName = "@subscriberCode", Value = model.SubscriberCode },
                    new SqlParameter { ParameterName = "@pendingReasonId", Value = model.PendingReasonId },
                    new SqlParameter { ParameterName = "@comments", Value = model.Comments },
                    new SqlParameter { ParameterName = "@userId", Value = userId },
                    new SqlParameter { ParameterName = "@dateTimeText", Value = dateTimeText },
                };

                var result = await _rsmDBContext.AddComments.FromSqlRaw(sql, parms.ToArray()).AsNoTracking().ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = result
                };
                await InsertRequestResponse(model, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }




        public async Task<ApiResponse> RouterModelData(string userId, string ip, int brandId)
        {
            var methodName = "RsmInstallationTicketService/RouterModelData";
            var response = new ApiResponse();
            try
            {
                string routerModelInfoQuery = "select * from RSM_RouterModel where RouterBrandID = " + brandId + "";
                var routerModelInfoData = await _rsmDBContext.RouterModels.FromSqlRaw(routerModelInfoQuery).ToListAsync();

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = routerModelInfoData
                };
                await InsertRequestResponse(brandId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(brandId, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> SheduleDataUpdate(string userId, string ip, InstallationSheduleUpdateRequestModel model)
        {
            var methodName = "RsmInstallationTicketService/SheduleDataUpdate";
            var response = new ApiResponse();
            using (IDbContextTransaction transaction = _rsmDBContext.Database.BeginTransaction())
            {
                try
                {
                    if(model.ScheduleDate == null)
                    {
                        throw new Exception("Input missing Schedule date");
                    }
                    if (model.ScheduleDate < DateTime.Now.Date)
                    {
                        throw new Exception("Invalid Schedule date");
                    }

                    string updateCliMktPendingQuery = @"update cli_mktpending set ScheduleDate=CAST('" + model.ScheduleDate + "' AS date)," +
                        " Scheduleby='" + userId + "' where RefNo='" + model.TicketId + "'";
                    int updateCliMktPendingResult = await _rsmDBContext.Database.ExecuteSqlRawAsync(updateCliMktPendingQuery);

                    string strSqlRsmInsertUserTaskHistory = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + model.TicketId + "','" + userId + "','RSM','RSM SOF Schedule update','"
                                      + model.Remarks.Replace("'", "''") + "','Schedule Update')";
                    int getRsmInsertUserTaskHistoryResult = await _rsmDBContext.Database.ExecuteSqlRawAsync(strSqlRsmInsertUserTaskHistory);

                    await _rsmDBContext.SaveChangesAsync();
                    transaction.Commit();
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data Updated Successfully",
                        Data = null
                    };
                    await InsertRequestResponse(model, response, methodName, ip, userId, null);
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await errorMethord(ex, methodName);
                    await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                    await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                    throw new Exception(ex.Message);
                }
            }

        }



        public async Task<ApiResponse> GetAddColorInfoData(string ticketId, string userId, string requestedIp)
        {
            var methodName = "RsmInstallationTicketService/GetAddColorInfoData";
            try
            {
                string subscriberInfoQuery = "select * from RMS_ServiceDetails where RefNO='" + ticketId + "'";
                var subscriberInfoData = await _rsmDBContext.Subscribers.FromSqlRaw(subscriberInfoQuery).FirstOrDefaultAsync();
                if (subscriberInfoData != null)
                {
                    string strSql = " SELECT a.*,a.i_seller+' : '+b.user_name as SalesPerson FROM clientDatabaseMain a left outer join tbl_user_info b "
                           + " on a.i_seller=b.userid WHERE MqID = '" + subscriberInfoData.SubscriberID + "' ";
                    var clientData = await _rsmDBContext.ClientDatabaseMainModel.FromSqlRaw(strSql).FirstOrDefaultAsync();

                    string spliterInfoQueryForFiber = @"select * from tbl_SubSpliterEntry where CustomerCode='" + clientData.brCliCode
                        + "' and CustomerSl='" + clientData.brSlNo + "'";
                    var spliterInfoDataForFiber = await _rsmDBContext.SpliterEntryModels.FromSqlRaw(spliterInfoQueryForFiber).FirstOrDefaultAsync();
                  
                    string colorInfoQuery = @"select * from View_SpltColorInfo where CustomerID='" + clientData.brCliCode + "' and CustomerSl='" + clientData.brSlNo + "'";
                    var colorInfoData = await _rsmDBContext.ViewColors.FromSqlRaw(colorInfoQuery).ToListAsync();

                    var addColorInfoModel = await GetAddColorInfoData();
                    
                    if(spliterInfoDataForFiber != null)
                        addColorInfoModel.SplitterLocation = spliterInfoDataForFiber.SpliterLocation;
                    
                    if(colorInfoData != null)
                        addColorInfoModel.ViewColors = colorInfoData;


                    var response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Get All Data",
                        Data = addColorInfoModel
                    };
                    await InsertRequestResponse(ticketId, response, methodName, requestedIp, userId, null);
                    return response;
                }
                else
                {
                    throw new Exception("Data Not Found");
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, requestedIp, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<AddColorInfoResponseModel> GetAddColorInfoData()
        {
            var methodName = "RsmInstallationTicketService/GetAddColorInfoData";
            try
            {
                var response = new AddColorInfoResponseModel();

                string tubeColorInfoQuery = "SELECT ColorID, ColorName FROM tbl_ColorInfo ORDER BY ColorName";
                var tubeColorInfoData = await _rsmDBContext.TubeColors.FromSqlRaw(tubeColorInfoQuery).ToListAsync();
                var cableTypes = new List<CableTypeResponseModel>()
                {
                    new CableTypeResponseModel()
                    {
                        Value= "96F",
                        Name = "96F"
                    },
                    new CableTypeResponseModel()
                    {
                        Value= "48F",
                        Name = "48F"
                    },
                    new CableTypeResponseModel()
                    {
                        Value= "24F",
                        Name = "24F"
                    },
                    new CableTypeResponseModel()
                    {
                        Value= "12F",
                        Name = "12F"
                    },
                    new CableTypeResponseModel()
                    {
                        Value= "6F",
                        Name = "6F"
                    },
                     new CableTypeResponseModel()
                    {
                        Value= "4F",
                        Name = "4F"
                    },
                      new CableTypeResponseModel()
                    {
                        Value= "2F",
                        Name = "2F"
                    }
                };
                response.CableTypes= cableTypes;
                response.TubeColor = tubeColorInfoData;
                response.CoreColor = tubeColorInfoData;

                await InsertRequestResponse(null, response, methodName, null, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, null, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetInstallationInfoModelData(string ticketId, string userId, string requestedIp)
        {
            var methodName = "RsmInstallationTicketService/GetInstallationInfoModelData";
            try
            {
                var instalationInfoModel = new RsmInstallationInfoResponseModel();
                var response = new ApiResponse();
                string subscriberInfoQuery = "select * from RMS_ServiceDetails where RefNO='" + ticketId + "'";
                var subscriberInfoData = await _rsmDBContext.Subscribers.FromSqlRaw(subscriberInfoQuery).FirstOrDefaultAsync();
                if (subscriberInfoData != null)
                {
                    string strSql = " SELECT a.*,a.i_seller+' : '+b.user_name as SalesPerson FROM clientDatabaseMain a left outer join tbl_user_info b "
                           + " on a.i_seller=b.userid WHERE MqID = '" + subscriberInfoData.SubscriberID + "' ";
                    var clientData = await _rsmDBContext.ClientDatabaseMainModel.FromSqlRaw(strSql).FirstOrDefaultAsync();

                    string spliterInfoQueryForFiber = @"select * from tbl_SubSpliterEntry where CustomerCode='" + clientData.brCliCode
                        + "' and CustomerSl='" + clientData.brSlNo + "'";
                    var spliterInfoDataForFiber = await _rsmDBContext.SpliterEntryModels.FromSqlRaw(spliterInfoQueryForFiber).FirstOrDefaultAsync();

                    string spliterInfoQueryForFoNoc = @"select * from  tbl_SubSpliterEntry WHERE CustomerCode ='" +
                        clientData.brCliCode + "' AND CustomerSl ='" + clientData.brSlNo + "' AND Shifted = 'No'";
                    var spliterInfoDataForFoNoc = await _rsmDBContext.SpliterEntryModels.FromSqlRaw(spliterInfoQueryForFoNoc).FirstOrDefaultAsync();

                    var fiberInfrastructure = false;
                    string clientTechnicalInfoQuery = @"select * from ClientTechnicalInfo where ClientCode='" + subscriberInfoData.SubscriberID + "'";
                    var clientTechnicalInfoData = await _rsmDBContext.ClientTechnicalInfos.FromSqlRaw(clientTechnicalInfoQuery).FirstOrDefaultAsync();
                    if (spliterInfoDataForFoNoc != null)
                        fiberInfrastructure = true;
                    var hydraInfo = await _rSMComplainTicketService.GetTechnicalInfoFromHydra(subscriberInfoData.SubscriberID, userId, requestedIp);
                    var hyDraData = new ShowTechnicalInformationFromHydraModel();
                    if (hydraInfo.Data != null)
                    {
                        hyDraData = (ShowTechnicalInformationFromHydraModel)hydraInfo.Data;
                    }
                    string btsNameQuery = @"select item_desc from clientDatabaseItemDet where brCliCode='" + clientData.brCliCode + "' and brSlNo='" + clientData.brSlNo + "' and itm_type='BTS'";
                    var btsNameData = await _rsmDBContext.GetBtsSelectedName.FromSqlRaw(btsNameQuery).FirstOrDefaultAsync();
                    string btsName = null;
                    if (btsNameData != null)
                        btsName = btsNameData.item_desc;

                    string colorInfoQuery = @"select * from View_SpltColorInfo where CustomerID='" + clientData.brCliCode + "' and CustomerSl='" + clientData.brSlNo + "'";
                    var colorInfoData = await _rsmDBContext.ColorSplitModel.FromSqlRaw(colorInfoQuery).ToListAsync();

                    instalationInfoModel = await GetInstallationClientInfoData(subscriberInfoData.SubscriberID, ticketId, instalationInfoModel, clientData, userId);

                    instalationInfoModel.CableNetwork = await GetCableNetworkInfoData();
                    instalationInfoModel.FiberMediaDetails = await GetFiberMediaInfoData(clientData.brCliCode, clientData.brSlNo, spliterInfoDataForFiber,
                        clientTechnicalInfoData.f_Laser, hyDraData.OLT);
                    instalationInfoModel.ConnectivityDetails = await GetConnectivityDetailsInfoData(subscriberInfoData.SubscriberID, colorInfoData);
                    instalationInfoModel.FoNoc = await GetFoNOCInfoData(clientTechnicalInfoData, spliterInfoDataForFoNoc, fiberInfrastructure, btsName, hyDraData, instalationInfoModel.SalesPersonInfo.SubscriberCode);
                    instalationInfoModel.AddComments = await GetAddCommentsInfoData(ticketId);
                    instalationInfoModel.UpdateHistrory = await GetUpdateInfoData(ticketId);

                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Get All Data",
                        Data = instalationInfoModel
                    };
                    await InsertRequestResponse(ticketId, response, methodName, requestedIp, userId, null);
                    return response;
                }
                else
                {
                    throw new Exception("Data Not Found");
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, requestedIp, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RsmInstallationInfoResponseModel> GetInstallationClientInfoData(string subscriberId, string ticketId,
            RsmInstallationInfoResponseModel model, Rsm_ClientDatabaseMainResponseModel clientData, string userId)
        {
            var methodName = "RsmInstallationTicketService/GetInstallationClientInfoData";
            var allReqest = new
            {
                subscriberId = subscriberId,
                ticketId = ticketId,
                model = model,
                clientData = clientData
            };
            try
            {
                string serviceInfoQuery = "select * from ClientTechnicalInfo where ClientCode='" + subscriberId + "'";
                var serviceInfo = await _rsmDBContext.getServiceName.FromSqlRaw(serviceInfoQuery).FirstOrDefaultAsync();

                string newFormateAddressQuery = @"select b.DistrictName,c.UpazilaName,d.AreaName,a.PostCode,
                        a.HouseName,a.FlatNo,a.RoadNo,a.section,
                        a.landmark,a.houseno,a.RoadName_No from clientDatabaseMain a 
                        inner join tbl_SubscriberRMSDistrict b on a.Districtid=b.ID
                        inner join DistrictAndUpazila c on c.AutoID=a.UpazilaThanaID
                        inner join DistrictThanaArea d on d.ID=a.AreaID
                        inner join tbl_SubscriberTitle e on e.ID=a.SalutionID
                        inner join RSM_MaritialStatus f on f.ID=a.MarriedID
                        inner join tbl_SubscriberGender g on g.ID=a.GenderID
                        inner join SupportOffice h on h.SupportOfficeID=a.brSupportOfficeId
                        inner join tbl_SubscriberIDProof i on i.ID=a.ProofTypeID
                        inner join tbl_SubscriberOccupation j on j.ID=a.OccupationID
                        where a.mqid='" + subscriberId + "'";
                var newFormateAddressData = await _rsmDBContext.NewFormatAddresses.FromSqlRaw(newFormateAddressQuery).FirstOrDefaultAsync();
                var salesPersonInfo = new SalesPersonInfoResponseModel();
                var personalDetails = new PersonalDetailsResponseModel();
                var contactDetails = new ContactDetailsResponseModel();
                var connectivityAddress = new ConnectivityAddressResponseModel();

                string teamQuery = @"select distinct Service from view_pendin where userid='" + userId + "'" +
                    " and RefNo='" + ticketId + "' and Status='INI'";
                var teamData = await _rsmDBContext.ServiceList.FromSqlRaw(teamQuery).ToListAsync();
                connectivityAddress.TeamList = new List<TeamResponseModel>();
                if(teamData.Count > 0)
                {
                    foreach(var service in teamData)
                    {
                        var team = new TeamResponseModel();
                        team.Value = service.Service;
                        team.Text = service.Service;
                        connectivityAddress.TeamList.Add(team);
                    }
                }
                else
                {
                    string teamListInfoQuery = @"select *  from tbl_team_mem_permission a inner join Cli_Pending b on a.Team_Name=b.Service
			               where a.Emp_id='" + userId + "' and  b.RefNo='" + ticketId + "' and b.Status='INI'";
                    var teamListInfoData = await _rsmDBContext.TeamNameList.FromSqlRaw(teamListInfoQuery).ToListAsync();
                    if (teamListInfoData != null)
                    {
                        foreach (var teamName in teamListInfoData)
                        {
                            var team = new TeamResponseModel();
                            team.Value = teamName.Team_Name;
                            team.Text = teamName.Team_Name;
                            connectivityAddress.TeamList.Add(team);
                        }
                    }

                }

                if (clientData != null)
                {
                    salesPersonInfo.brCliCode = clientData.brCliCode;
                    salesPersonInfo.brSlNo = clientData.brSlNo;
                    salesPersonInfo.SalesPerson = clientData.SalesPerson;
                    salesPersonInfo.SupportOffice = clientData.brSupportOffice;
                    salesPersonInfo.BillDeliveryOption = clientData.BillDeliverd;
                    salesPersonInfo.SAFNumber = clientData.SAF;
                    salesPersonInfo.Reference = clientData.ClientRefarence;
                    salesPersonInfo.TKINo = ticketId;
                    salesPersonInfo.SubscriberCode = subscriberId;
                    salesPersonInfo.SubscriberName = clientData.HeadOfficeName;
                    salesPersonInfo.HydraID = clientData.HydraID.ToString();


                    personalDetails.Title = clientData.Salution;
                    personalDetails.FirstName = clientData.FirstName;
                    personalDetails.MidleName = clientData.MiddleName;
                    personalDetails.LastName = clientData.LastName;
                    personalDetails.CompanyName = clientData.brcompanyname;
                    personalDetails.Nationality = clientData.Nationality;
                    personalDetails.IDProof = clientData.ProofID;
                    personalDetails.IDProofNo = clientData.ProofID_No;
                    personalDetails.Occupation = clientData.Occupation;
                    personalDetails.DateofBirth = clientData.DateOfBirth;
                    personalDetails.FirstName = clientData.FatherName;
                    personalDetails.MotherName = clientData.MotherName;
                    personalDetails.SpouseName = clientData.SpouseName;
                    personalDetails.Gender = clientData.GenderType;

                    contactDetails.ContactName = clientData.contact_det;
                    contactDetails.HomePhone = clientData.HomePhone;
                    contactDetails.WorkPhone = clientData.WorkPhone;
                    contactDetails.RegisteredMobile = clientData.phone_no;
                    contactDetails.Fax = clientData.fax_no;
                    contactDetails.RegisteredEmail = clientData.email_id;
                    contactDetails.AltEmail = clientData.AltMobile;
                    contactDetails.Designation = clientData.Contact_Designation;
                    contactDetails.ComplementaryConnection = clientData.Complementary;
                    contactDetails.EmployeeID = clientData.EmployeeID;

                    connectivityAddress.HouseName = clientData.HouseName;
                    connectivityAddress.Area = clientData.Area;
                    connectivityAddress.RoadName = clientData.RoadName_No;
                    connectivityAddress.Sector = clientData.Sector;
                    connectivityAddress.Section = clientData.Section;
                    connectivityAddress.Block = clientData.Block;
                    connectivityAddress.Upazila = clientData.Upazila;
                    connectivityAddress.PoliceStation = clientData.PoliceStation;
                    connectivityAddress.PostCode = clientData.PostCode;
                    connectivityAddress.District = clientData.District;
                    connectivityAddress.Services = serviceInfo.ServicesList;

                }
                model.SalesPersonInfo = salesPersonInfo;
                model.PersonalDetails = personalDetails;
                model.ContactDetails = contactDetails;
                model.ConnectivityAddress = connectivityAddress;
                model.NewFormatAddress = newFormateAddressData;

                await InsertRequestResponse(allReqest, model, methodName, null, userId, null);

                return model;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(allReqest, ex, methodName, null, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<CableNetworkResponseModel> GetCableNetworkInfoData()
        {
            var model = new CableNetworkResponseModel();

            var cableNetworkTypeQuery = "select CablePathID,CablePathName from tbl_BackboneCablePathTypeSetup";
            var cableNetworkTypeData = await _rsmDBContext.CableNetworkTypes.FromSqlRaw(cableNetworkTypeQuery).ToListAsync();

            string nTTNNameQuery = "select NTTNID,NTTNName from tbl_BackboneNTTNSetup";
            var nTTNNameData = await _rsmDBContext.NttnNames.FromSqlRaw(nTTNNameQuery).ToListAsync();

            string typeofLinkQuery = "select Typeofp2mlinkID,Typeofp2mlink from tbl_Typeofp2mlinkSetup";
            var typeofLinkData = await _rsmDBContext.TypeOfLinks.FromSqlRaw(typeofLinkQuery).ToListAsync();

            model.CableNetworkTypeList = cableNetworkTypeData;
            model.NttnNameList = nTTNNameData;
            model.TypeOfLinkList = typeofLinkData;

            return model;

        }

        public async Task<FiberMediaDetailsP2MResponseModel> GetFiberMediaInfoData(string brCliCode, int? brSlNo, Rsm_tbl_SubSpliterEntryResponseModel? subSplitter, string laser, string oltName)
        {
            var model = new FiberMediaDetailsP2MResponseModel();

            string btsQuery = "select BtsSetupID,BtsSetupName from BtsSetup order by BtsSetupName";
            var btsData = await _rsmDBContext.BtsNames.FromSqlRaw(btsQuery).ToListAsync();

            string btsSelectedQuery = "select item_id from clientDatabaseItemDet where brCliCode='"
                + brCliCode + "' and brSlNo='" + brSlNo + "' and itm_type='BTS'";
            var btsSelectedIdData = await _rsmDBContext.GetBtsSelectedId.FromSqlRaw(btsSelectedQuery).FirstOrDefaultAsync();

            model.BtsList = btsData;
            if (btsSelectedIdData != null)
                model.selectedId = btsSelectedIdData.item_id;
            if (oltName != null)
                model.OltName = oltName;
            if (subSplitter != null)
            {
                model.Splitter = subSplitter.SpliterLocation + ":" + subSplitter.EncloserNo + ":" + subSplitter.SpliterCapacity;
                model.OltBrand = subSplitter.OLTBrand;
                model.Pon = subSplitter.PON.ToString();
                model.Port = subSplitter.Port.ToString();
                model.OltName = subSplitter.OltName;
                model.Portcapfb = subSplitter.PortCapacity.ToString();
                model.Remarks = subSplitter.Remarks.ToString();
            }
            if (laser != null)
                model.Laser = laser;

            return model;

        }

        public async Task<ConnectivityDetailsResponseModel> GetConnectivityDetailsInfoData(string subscriberId, List<VwSplitColorNewResponseModel>? colorInfo)
        {
            string sharedInfoQuery = @"select * from RSM_SharedCustomerDetails where CustomerID='" + subscriberId + "'";
            var sharedInfoData = await _rsmDBContext.SharedCustomerDetails.FromSqlRaw(sharedInfoQuery).FirstOrDefaultAsync();



            var model = new ConnectivityDetailsResponseModel();
            model.IsSharedVisible = false;
            var cableNoList = new List<CableNoResponseMode>();
            for (int i = 1; i <= 64; i++)
            {
                var cableNo = new CableNoResponseMode()
                {
                    Value = i,
                    Name = i.ToString()
                };
                cableNoList.Add(cableNo);
            }

            model.CableNo = cableNoList;
            if (sharedInfoData != null)
            {
                model.ShardOnuMac = sharedInfoData.ONUMac;
                model.ShardOnuPort = sharedInfoData.ONUPort;
                model.IsSharedVisible = true;
            }
            if (colorInfo != null)
            {
                string joincolor = "";
                string rm = "";
                foreach (var color in colorInfo)
                {

                    if (color.Remarks != "")
                    {
                        rm = "; {RM:" + color.Remarks.ToString() + "}";
                    }
                    else
                    {
                        rm = "";
                    }
                    joincolor += "[{SP: " + color.StartPoint + "};{Cable " + color.CableType + " (TC:" +
                            color.TubeColorName + ";CC:" + color.CoreColorName + ")};{CID:" +
                            color.CableID + ")" + "(SM:" + color.StartMeter + " - EM:" + color.EndMeter + " = " +
                            color.Length + "m)};" + " {EP:" + color.EndPoint + "}" +
                            rm + "]+";
                }
                model.Color = joincolor;
            }

            return model;

        }

        public async Task<FoNocResponseModel> GetFoNOCInfoData(Rsm_ClientTechnicalInfoResponseModel? clientInfo, Rsm_tbl_SubSpliterEntryResponseModel?
            subSplitter, bool fiberInfrastructure, string? btsName, ShowTechnicalInformationFromHydraModel tecnicalInfo, string subcriberCode)
        {
            var model = new FoNocResponseModel();


            string routerBrandQuery = "select ID,RouterBrand from RSM_RouterBrand";
            var routerBrandData = await _rsmDBContext.RouterBrands.FromSqlRaw(routerBrandQuery).ToListAsync();

            string routerRebotSettingQuery = "select ID,RebootType from RSM_RouterRebootype";
            var routerRebotSettingData = await _rsmDBContext.RouterRebotSettings.FromSqlRaw(routerRebotSettingQuery).ToListAsync();

            string routerRebootTimeQuery = "select ID,RouterRebootTime from RSM_RouterRebootTime";
            var routerRebootTimeData = await _rsmDBContext.RouterRebotTimes.FromSqlRaw(routerRebootTimeQuery).ToListAsync();



            model.RouterBrands = routerBrandData;
            model.RouterRebotSettings = routerRebotSettingData;
            model.RouterRebotTimes = routerRebootTimeData;
            model.FiberInfrastractureIsVisible = fiberInfrastructure;
            if (clientInfo != null)
            {
                model.OnuMac = clientInfo.f_mac;
                model.OnuPort = clientInfo.f_onu == "CDATA" ? "" : clientInfo.f_onu;
                model.OnuId = clientInfo.f_onuNo;
                model.FiberInfrastracture = new FiberInfrastractureResponseModel();
                if (clientInfo.FiberComments != null)
                    model.FiberInfrastracture.Remarks = clientInfo.FiberComments;
            }
            if (btsName != null)
                model.FiberInfrastracture.BtsName = btsName;
            if (subSplitter != null)
            {
                model.FiberInfrastracture.OltBrand = subSplitter.OLTBrand;
                model.FiberInfrastracture.OltName = subSplitter.OltName;
                model.FiberInfrastracture.SplitterLocation = subSplitter.SpliterLocation + ", EID:" + subSplitter.EncloserNo + ", Capacity-1:" + subSplitter.SpliterCapacity;
            }
            if (tecnicalInfo != null)
            {
                model.FiberInfrastracture.OltIp = tecnicalInfo.OLT_IP;
                model.FiberInfrastracture.BRAS = tecnicalInfo.BRAS;
                model.FiberInfrastracture.BrasIp = tecnicalInfo.BRAS_IP;
                model.FiberInfrastracture.Pon = tecnicalInfo.PON;
                model.FiberInfrastracture.Port = tecnicalInfo.PORT;
                model.FiberInfrastracture.Laser = clientInfo.f_Laser;
                model.FiberInfrastracture.Ip = tecnicalInfo.IP;
                model.FiberInfrastracture.Gateway = tecnicalInfo.Gateway;
                model.FiberInfrastracture.SubnetMask = tecnicalInfo.Subnet_Mask;
                model.FiberInfrastracture.IpV6 = clientInfo.IPV6;
                model.FiberInfrastracture.Vlan = tecnicalInfo.VLAN;
                model.CustomerMac = tecnicalInfo.Customer_MAC;
            }

            string routerSettingQuery = "SELECT *  FROM RSM_RouterModelSetting WHERE CustomerID='" + subcriberCode + "'";
            var routerSetting = await _rsmDBContext.RSM_RouterModelSetting.FromSqlRaw(routerSettingQuery).FirstOrDefaultAsync();
            if(routerSetting != null)
            {
                model.RouterBrandsSelectedValue = routerSetting.RouterBrandID;
                model.RouteModelSelectedValue = routerSetting.RouterModelID;
                model.RouterRebotSettingsSelectedValue = routerSetting.RouterModelSettingID;
                model.RouterRebootTimesSelectedValue = routerSetting.RouterRebootTimeID;
            }

            return model;

        }



        public async Task<ApiResponse> AddFonocInfo(RSMfonocAddRequestModel model, string UserName, string ip)
        {
            var methodName = "RsmInstallationTicketService/AddFonocInfo";
            
            try
            {
                if(model.CustomerMac =="" || model.OnuMac == "" || model.OnuId == "" || model.OnuPort == "")
                {
                    throw new Exception();
                }

                var checkResult = CheckHydraData(model.SubscriberCode, model.CustomerMac);
                if (checkResult == true)
                {
                    throw new Exception("This MAC has already exist another customer");
                }
                if (model.ddteamSelectedValue != "")
                {
                   
                    string inv = @"select * from L3T.dbo.InTr_Trn_Hdr where Trn_Hdr_Pcode='" + model.SubscriberCode + "'";
                    var results = await _l3tDBContext.InTrTrnHdrModel.FromSqlRaw(inv).ToListAsync();
                    // ForwardTicketApi.FromSqlRaw(inv).ToListAsync();
                    if (results.Count == 0)
                    {
                        throw new Exception("Inventory Product not issue.");
                    }
                }

                string uuu = "Done :" + model.Remarks;
                var lastComments = uuu.Replace("'", "''");


                string gethydrayRes = "";
                var hydrayData = await _otherService.GetHydraData(model.RefNo, ip);
                if (hydrayData.StatusCode == 200)
                {
                    gethydrayRes = hydrayData.Data.ToString();
                }
                string rtvn = "[" + gethydrayRes + "]" + " Done: " + model.Remarks;
                var HydraValue = rtvn.Replace("'", "''");



                string sqlStd = "EXEC RSM_InstallationFonocAddApi @devicemac, @customermac, @onuport, " +
                     "@onunumber,@subscribercode, @CommentsDate, @LastComments, @UserId, @RefNo, @Service, " +
                     "@HydraValue , @DdteamSelectedItem ";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameters
                    new SqlParameter { ParameterName = "@devicemac", Value = model.OnuMac },
                    new SqlParameter { ParameterName = "@customermac", Value = model.CustomerMac },
                    new SqlParameter { ParameterName = "@onuport", Value = model.OnuPort },
                    new SqlParameter { ParameterName = "@onunumber", Value = model.OnuId },
                    new SqlParameter { ParameterName = "@subscribercode", Value = model.SubscriberCode },
                    new SqlParameter { ParameterName = "@CommentsDate", Value = DateTime.Now},
                    new SqlParameter { ParameterName = "@LastComments", Value = lastComments },
                    new SqlParameter { ParameterName = "@UserId", Value = UserName },
                    new SqlParameter { ParameterName = "@RefNo", Value = model.RefNo },
                    new SqlParameter { ParameterName = "@Service", Value = model.ddteamSelectedValue },
                    new SqlParameter { ParameterName = "@HydraValue", Value = HydraValue },
                    new SqlParameter { ParameterName = "@DdteamSelectedItem", Value = model.ddteamSelectedValue },
                };

                var result = await _rsmDBContext.ResolvedTicketForApi.FromSqlRaw(sqlStd, parms.ToArray()).ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Get Data successfully",
                    Data = model
                };
                await InsertRequestResponse(model, response, methodName, ip, UserName, null);
                return response;
            }

            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, UserName, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
           

        }


        public async Task<ApiResponse> UpdateFonocInfo(RSMfonocUpdateRequestModel model, string UserName, string ip)
        {
            var methodName = "RsmInstallationTicketService/UpdateFonocInfo";
            var response = new ApiResponse();

            try
            {
                if (model.CustomerMac == "" || model.OnuMac == "" || model.OnuId == "" || model.OnuPort == "")
                {
                    throw new Exception("Can not be a blank required field");
                }
                var checkResult = CheckHydraData(model.SubscriberCode, model.CustomerMac);
                if (checkResult == true)
                {
                    throw new Exception("This MAC has already exist another customer");
                }
                if (model.ddteamSelectedValue != "")
                {

                    string inv = @"select * from L3T.dbo.InTr_Trn_Hdr where Trn_Hdr_Pcode='" + model.SubscriberCode + "'";
                    var results = await _l3tDBContext.InTrTrnHdrModel.FromSqlRaw(inv).ToListAsync();
                    // ForwardTicketApi.FromSqlRaw(inv).ToListAsync();
                    if (results.Count == 0)
                    {
                        throw new Exception("Inventory Product not issue.");
                    }
                }
                string gethydrayRes = "";
                var hydrayData = await _otherService.GetHydraData(model.RefNo, ip);
                if (hydrayData.StatusCode == 200)
                {
                    gethydrayRes = hydrayData.Data.ToString();
                }
                string rtvn = "[" + gethydrayRes + "]" + " Update: " + model.Remarks;
                var HydraValue = rtvn.Replace("'", "''");

                string uuu = "Update :" + model.Remarks;
                var lastComments = uuu.Replace("'", "''");

             


                string sqlStd = "EXEC RSM_InstallationFonocUpdateApi @devicemac, @customermac, @onuport, " +
                     "@onunumber,@subscribercode, @CommentsDate, @LastComments, @UserId, @RefNo, @Service, " +
                     "@HydraValue";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameters
                    new SqlParameter { ParameterName = "@devicemac", Value = model.OnuMac },
                    new SqlParameter { ParameterName = "@customermac", Value = model.CustomerMac },
                    new SqlParameter { ParameterName = "@onuport", Value = model.OnuPort },
                    new SqlParameter { ParameterName = "@onunumber", Value = model.OnuId },
                    new SqlParameter { ParameterName = "@subscribercode", Value = model.SubscriberCode },
                    new SqlParameter { ParameterName = "@CommentsDate", Value = DateTime.Now},
                    new SqlParameter { ParameterName = "@LastComments", Value = lastComments },
                    new SqlParameter { ParameterName = "@UserId", Value = UserName },
                    new SqlParameter { ParameterName = "@RefNo", Value = model.RefNo },
                    new SqlParameter { ParameterName = "@Service", Value = model.ddteamSelectedValue },
                    new SqlParameter { ParameterName = "@HydraValue", Value = HydraValue },
                    
                };

                var result = await _rsmDBContext.ResolvedTicketForApi.FromSqlRaw(sqlStd, parms.ToArray()).ToListAsync();
                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Get Data successfully",
                    Data = model
                };
                await InsertRequestResponse(model, response, methodName, ip, UserName, null);
                return response;
            }

            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, UserName, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }


        }



        public async Task<ApiResponse> UpdateFonocRouterInfo(RSMfonocRouterUpdateRequestModel model, string UserName, string ip)
        {
            var methodName = "RsmInstallationTicketService/UpdateFonocRouterInfo";
            var response = new ApiResponse();
            try
            {
                if (model.RouterBrandId < -1  || model.RouterModelId < -1 || model.RouterModelId < -1 )
                {
                    throw new Exception("Can not be a blank required field");
                }

                //string sqlStd = "EXEC RSM_InstallationFonocRouterUpdateApi @subscribercode, @RebootSettingSelectedId, @RebootTime, " +
                //     "@ModelSelectedId,@UserId, @RouterBrandSelectedId";

                //List<SqlParameter> parms = new List<SqlParameter>
                //{
                //    // Create parameters
                //    new SqlParameter { ParameterName = "@subscribercode", Value = model.SubscriberCode },
                //    new SqlParameter { ParameterName = "@RebootSettingSelectedId", Value = model.RouterRebootSettingId },
                //    new SqlParameter { ParameterName = "@RebootTimeId", Value = model.RouterRebootTime },
                //    new SqlParameter { ParameterName = "@ModelSelectedId", Value = model.RouterModelId },
                //    new SqlParameter { ParameterName = "@UserId", Value = UserName},
                //    new SqlParameter { ParameterName = "@RouterBrandSelectedId", Value = model.RouterBrandId },

                //};

                //var result = await _rsmDBContext.ResolvedTicketForApi.FromSqlRaw(sqlStd, parms.ToArray()).ToListAsync();

                string sqlStd = "EXEC RSM_InstallationFonocRouterUpdateApi '"+ model.SubscriberCode + "', '"+ model.RouterRebootSettingId + "', '"+ model.RouterRebootTimeId + "', " +
                     "'"+ model.RouterModelId + "', '"+ UserName + "', '"+ model.RouterBrandId + "'";

                var result = await _rsmDBContext.ResolvedTicketForApi.FromSqlRaw(sqlStd).ToListAsync();

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Get Data successfully",
                    Data = model
                };
                await InsertRequestResponse(model, response, methodName, ip, UserName, null);
                return response;


            }

            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, UserName, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }



        public async Task<ApiResponse> InstallationFileUpload(RSMInstallationFileUploadRequestModel model, string UserName, string ip)
        {
            var methodName = "RsmInstallationTicketService/InstallationFileUpload";
            var response = new ApiResponse();
            try
            {
                if (model.RefNo == "" ||model.SubscriberId == "" || model.FileType == "")
                {
                    throw new Exception("Can not be a blank required field");
                }
                string path = _configuration.GetValue<string>("FilePath:PathRsm");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension and info
                FileInfo fileInfo = new FileInfo(model.FileDetails.FileName);
                string fileName = DateTime.Now.ToString("yyyymmddThhmmssTZD") + "_" + model.FileDetails.FileName;
                string fileExtension = fileInfo.Extension;
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.FileDetails.CopyTo(stream);
                }

                //string sqlStd = "EXEC RSM_InstallationFileUploadApi @RefNo,@SubscriberCode,@FileType, @FileName, " +
                //     "@SavedFilename,@UserId";

                //List<SqlParameter> parms = new List<SqlParameter>
                //{
                //    // Create parameters
                //    new SqlParameter { ParameterName = "@RefNo", Value = model.RefNo },
                //    new SqlParameter { ParameterName = "@SubscriberCode", Value = model.SubscriberId },
                //    new SqlParameter { ParameterName = "@FileType", Value = model.FileType },
                //    new SqlParameter { ParameterName = "@FileName", Value = fileInfo },
                //    new SqlParameter { ParameterName = "@SavedFilename", Value = fileName},
                //    new SqlParameter { ParameterName = "@UserId", Value = UserName },

                //};

                //var result = await _rsmDBContext.ResolvedTicketForApi.FromSqlRaw(sqlStd, parms.ToArray()).ToListAsync();

                string sqlStd = "EXEC RSM_InstallationFileUploadApi '"+ model.RefNo + "','"+ model.SubscriberId + "','"+ model.FileType + "', '"+ fileInfo + "', " +
                     "'"+ fileName + "','"+ UserName + "'";

              
                var result = await _rsmDBContext.ResolvedTicketForApi.FromSqlRaw(sqlStd).ToListAsync();


                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Get Data successfully"
                };
                await InsertRequestResponse(model, response, methodName, ip, UserName, null);
                return response;
                

            }

            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(model, ex, methodName, ip, UserName, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }


        }


        public async Task<AddCommentsResponseModel> GetAddCommentsInfoData(string ticketId)
        {
            var model = new AddCommentsResponseModel();

            string pendingReasonQuery = "select ID,PendingReson from RSM_ReasonPendingInstallation where status='1' order by PendingReson";
            var pendingReasonData = await _rsmDBContext.PendingReasons.FromSqlRaw(pendingReasonQuery).ToListAsync();

            string pendingReasonSelectedQuery = "select PendingReason from RMS_ServiceDetails  where RefNO='" + ticketId + "' and PendingReason<>'12'";
            var pendingReasonSelectedData = await _rsmDBContext.PendingReasonId.FromSqlRaw(pendingReasonSelectedQuery).FirstOrDefaultAsync();
            model.pendingReasons = pendingReasonData;
            if (pendingReasonSelectedData != null)
                model.SelectedId = pendingReasonSelectedData.PendingReason;

            return model;

        }

        public async Task<List<UpdateHistroryResponseModel>> GetUpdateInfoData(string ticketId)
        {
            var model = new List<UpdateHistroryResponseModel>();

            string updateHistoryQuery = @"select a.AutoID as ID,(isnull(a.EntryBy, 'Hydra') + ':' + isnull(b.EmpName, 'System')) as [UpdateUser],
                    isnull(b.Designation + '/' + b.Dept + '/' + b.Sect + '/' + b.ContactNumber, '') as [DesgAndDeptAndSectAndCell],
                    a.ReceivedDate as UpdateDate,a.HydraValue as Remarks from RSM_InstallationUpdateTicekt a left outer join lnk.dbo.Emp_Details b on
                    a.entryby = b.EmpID  where a.TicketID='" + ticketId + "' and a.EntryBy<>'SentToHydra' order by a.AutoID desc";

            var updateHistoryData = await _rsmDBContext.UpdateHistrories.FromSqlRaw(updateHistoryQuery).ToListAsync();
            model = updateHistoryData;

            return model;

        }


        public async Task<ApiResponse> GetP2mHomeSCRIDInfo(string userId, string ip, string prefixText)
        {
            var methodName = "RsmInstallationTicketService/GetP2mHomeSCRIDInfo";
            var response = new ApiResponse();
            var getdataNewModelList = new List<tbl_BackboneFiber_HomeLinkResponseModel>();
            try
            {
                prefixText = "%" + prefixText + "%";

                string queryStr = "SELECT Amountper, Autoid, BTSID, BillingDistancetype, BillingType, Capacity, ConnectionType, ConnectionTypeMC, DateofInstallation, " +
                             "Entryby, Entrydate, FromLDPId, FromLocation, Lastmilemc, LinkDistancepercore, LinkTypeId, NoOfCore, OTC, " +
                             "Remarks, SCRID, Status, TerminateDate, ToLDPId, ToLocation, TotalAmount, UsingStatus " +
                             "FROM tbl_BackboneFiber_HomeLink WHERE  (SCRID LIKE  '" + prefixText + "') ";

                var geDtata = await _rsmDBContext.tbl_BackboneFiber_HomeLink.FromSqlRaw(queryStr).ToListAsync();
                
                foreach (var dr in geDtata)
                {
                    var model = new tbl_BackboneFiber_HomeLinkResponseModel()
                    { 
                        Amountper = dr.Amountper,
                        Autoid = dr.Autoid,
                        BTSID = dr.BTSID,
                        BillingDistancetype = dr.BillingDistancetype,
                        BillingType = dr.BillingType,
                        Capacity = dr.Capacity,
                        ConnectionType = dr.ConnectionType,
                        ConnectionTypeMC = dr.ConnectionTypeMC,
                        DateofInstallation = dr.DateofInstallation,
                        Entryby = dr.Entryby,
                        Entrydate = dr.Entrydate,
                        FromLDPId = dr.FromLDPId,
                        FromLocation = dr.FromLocation,
                        Lastmilemc = dr.Lastmilemc,
                        LinkDistancepercore = dr.LinkDistancepercore,
                        LinkTypeId = dr.LinkTypeId,
                        NoOfCore = dr.NoOfCore,
                        OTC = dr.OTC,
                        Remarks = dr.Remarks,
                        SCRID = dr.SCRID,
                        Status = dr.Status,
                        TerminateDate = dr.TerminateDate,
                        ToLDPId = dr.ToLDPId,
                        ToLocation = dr.ToLocation,
                        TotalAmount = dr.TotalAmount,
                        UsingStatus = dr.UsingStatus

                    };

                    getdataNewModelList.Add(model);
                }

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully",
                    Data = getdataNewModelList
                };
                await InsertRequestResponse(prefixText, response, methodName, ip, userId, null);
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(response, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }



        public async Task<ApiResponse> GetSummitLinkIDInfo(string userId, string ip, string prefixText)
        {
            var methodName = "RsmInstallationTicketService/GetSummitLinkIDInfo";
            var response = new ApiResponse();
            var getdataNewModelList = new List<tbl_BackboneSummitLinkResponseModel>();
            try
            {
                prefixText = "%" + prefixText + "%";

                string queryStr = "SELECT Amountper, Autoid, BillingDistancetype, BillingType, Capacity, ConnectionType, DateofInstallation, Entryby, Entrydate, " +
                             "FromLocation, FromPOCId, LinkDistancepercore, LinkID, LinkTypeId, NoOfCore, " +
                             "OTC, Remarks, Status, TerminateDate, ToLocation, ToPOCId, TotalAmount, UsingStatus " +
                             "FROM tbl_BackboneSummitLink WHERE  (LinkID LIKE  '" + prefixText + "') AND (Status = 'Active') ";
               

                var geDtata = await _rsmDBContext.tbl_BackboneSummitLink.FromSqlRaw(queryStr).ToListAsync();

                foreach (var dr in geDtata)
                {
                    var model = new tbl_BackboneSummitLinkResponseModel()
                    {
                        Amountper = dr.Amountper,
                        Autoid = dr.Autoid,
                        BillingDistancetype = dr.BillingDistancetype,
                        BillingType = dr.BillingType,
                        Capacity = dr.Capacity,
                        ConnectionType = dr.ConnectionType,
                        DateofInstallation = dr.DateofInstallation,
                        Entryby = dr.Entryby,
                        Entrydate = dr.Entrydate,
                        FromLocation = dr.FromLocation,
                        FromPOCId = dr.FromPOCId,
                        LinkDistancepercore = dr.LinkDistancepercore,
                        LinkID = dr.LinkID,
                        LinkTypeId = dr.LinkTypeId,
                        NoOfCore = dr.NoOfCore,
                        OTC = dr.OTC,
                        Remarks = dr.Remarks,
                        Status = dr.Status,
                        TerminateDate = dr.TerminateDate,
                        ToLocation = dr.ToLocation,
                        ToPOCId = dr.ToPOCId,
                        TotalAmount = dr.TotalAmount,
                        UsingStatus = dr.UsingStatus

                    };

                    getdataNewModelList.Add(model);
                }

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully",
                    Data = getdataNewModelList
                };
                await InsertRequestResponse(prefixText, response, methodName, ip, userId, null);
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(prefixText, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetPendingInstallation(string userId, string ip)
        {
            var methodName = "RsmInstallationTicketService/GetPendingInstallation";
            var response = new ApiResponse();
            try
            {

                string Team_Name = "", TeamID = "", insteam = "", techpen ="";

                string tmqstr = "select distinct Team_id,Team_Name from tbl_team_mem_permission where Emp_ID='" + userId + "'";

                var tmqRest = await _rsmDBContext.tbl_teamMemPermission.FromSqlRaw(tmqstr).ToListAsync();
                if(tmqRest.Count <= 0)
                {
                    throw new Exception("Team Permission is not assign.");
                }
                foreach (var tmaRest in tmqRest)
                {
                    if (TeamID == "")
                    {
                        TeamID = "'" + tmaRest.Team_id.ToString() + "'";
                    }
                    else
                    {
                        TeamID = TeamID + ",'" + tmaRest.Team_id.ToString() + "'";
                    }

                    if (Team_Name == "")
                    {
                        Team_Name = "'" + tmaRest.Team_Name.ToString() + "'";
                    }
                    else
                    {
                        Team_Name = Team_Name + ",'" + tmaRest.Team_Name.ToString() + "'";
                    }
                }


                string Suppoffqstr = "select distinct c.SupportOfficeName from RSM_SupportOfficeWiseID a inner join tbl_team_mem_permission b on a.Team_ID = b.Team_id " +
                                  " inner join SupportOffice c on c.SupportOfficeID = a.Support_OfficeID " +
                                  " where a.UserID = '" + userId + "'";

                var SuppoffqRst = await _rsmDBContext.tbl_SupportOffice.FromSqlRaw(Suppoffqstr).ToListAsync();

                foreach (var SuppaRest in SuppoffqRst)
                {
                    if (insteam == "")
                    {
                        insteam = "'" + SuppaRest.SupportOfficeName.ToString().Replace("'", "''") + "'";
                    }
                    else
                    {
                        insteam = insteam + ",'" + SuppaRest.SupportOfficeName.ToString().Replace("'", "''") + "'";
                    }
                }

                var inConditionAdd = string.Empty;
                if (!string.IsNullOrEmpty(Team_Name))
                {
                    inConditionAdd = "and b.Service in(" + Team_Name + ")";
                }

                if (insteam != "")
                {
                     techpen = @"SELECT a.RefNO,c.MqID, c.HeadOfficeName AS CustomerName, c.phone_no, a.EntryDate AS InitiateDate, b.StartDate,d.userid + ':' + d.user_name AS SalesPerson, b.Service, c.Area," +
                        " c.brSupportOffice, b.Status AS Status,Lc.LastComments,mc.ScheduleDate  FROM dbo.RMS_ServiceDetails AS a INNER JOIN " +
                        " dbo.Cli_Pending AS b ON a.RefNO = b.Refno INNER JOIN dbo.clientDatabaseMain AS c ON a.SubscriberID = c.MqID left outer join " +
                        " dbo.tbl_user_info AS d ON d.userid = a.SalesPersonID left outer join RSM_InstallationLastComments Lc on lc.RefNo=a.RefNO " +
                        " left outer join cli_mktpending mc on mc.Refno=a.RefNO WHERE (b.Status = 'INI') and b.Service in(" + insteam + ")" +
                        " GROUP BY a.RefNO, c.HeadOfficeName, c.phone_no, a.EntryDate, b.StartDate, d.userid + ':' + d.user_name, b.Service, c.Area, c.brSupportOffice, b.Status,c.MqID,Lc.LastComments,mc.ScheduleDate order by b.StartDate desc";
                }
                else
                {
                   techpen = " SELECT a.RefNO,c.MqID, c.HeadOfficeName AS CustomerName, c.phone_no, a.EntryDate AS InitiateDate,b.StartDate, d.userid + ':' + d.user_name AS SalesPerson, b.Service, c.Area," +
                        " c.brSupportOffice, b.Status AS Status,Lc.LastComments,mc.ScheduleDate FROM dbo.RMS_ServiceDetails AS a INNER JOIN dbo.Cli_Pending AS b ON a.RefNO = b.Refno INNER JOIN " +
                        " dbo.clientDatabaseMain AS c ON a.SubscriberID = c.MqID left outer join dbo.tbl_user_info AS d ON d.userid = a.SalesPersonID left outer join RSM_InstallationLastComments Lc on lc.RefNo=a.RefNO " +
                        " left outer join cli_mktpending mc on mc.Refno=a.RefNO WHERE (b.Status = 'INI')  " + inConditionAdd +
                        " GROUP BY a.RefNO, c.HeadOfficeName, c.phone_no, a.EntryDate,b.StartDate, d.userid + ':' + d.user_name, b.Service, c.Area, c.brSupportOffice, b.Status,c.MqID,Lc.LastComments,mc.ScheduleDate  order by b.StartDate desc ";

                }


                var geData = await _rsmDBContext.PendingInstallationInfo.FromSqlRaw(techpen).ToListAsync();


                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully",
                    Data = geData
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private bool CheckHydraData(string SubscriberCode, string CustomerMac)
        {
            string oradb = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.0.67)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = hydrast))); User Id=Ais_NET;Password=OxcjtkzYWxx125PnyQ3mqrR0;Connection Timeout=1800; ";

            OracleConnection con = new OracleConnection(oradb);
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = @"SELECT S.VC_CODE, CU.N_OWNER_ID, SI.VC_CODE MAC FROM SI_ADDRESSES SI INNER JOIN EP_EQUIPMENT_ADDRESSES EP ON EP.N_ADDRESS_ID = SI.N_ADDRESS_ID AND SI.C_ACTIVE='Y' AND SI.N_ADDR_TYPE_ID=4006 AND EP.C_ACTIVE='Y'
                            INNER JOIN EP_CUSTOMER_EQUIPMENT CU ON CU.N_EQUIPMENT_ID= EP.N_EQUIPMENT_ID AND CU.C_ACTIVE='Y' AND (NVL(EP.D_END,SYSDATE)>=SYSDATE)
                            INNER JOIN SI_SUBJECTS S ON S.N_SUBJECT_ID = CU.N_OWNER_ID
                            where S.VC_CODE<>'" + SubscriberCode + "' and SI.VC_CODE='" + CustomerMac.Replace(":", "-") + "'";
            cmd.Connection = con;
            con.Open();
            DataTable dtCustomers = new DataTable("Payments");
            using (OracleDataReader sdr = cmd.ExecuteReader())
            {
                dtCustomers.Load(sdr);
            }
            con.Close();

            if (dtCustomers.Rows.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public async Task<ApiResponse> NetworkConnectionDoneP2M(NetworkConnectionRequestModel model, string userId, string ip)
        {
            var methodName = "RSMInstallationTicketService/NetworkConnectionDoneP2M";

            try
            {
                bool bls = false;

                if (model.CableNetworkText.Contains("Select") || string.IsNullOrEmpty(model.CableNetworkText))
                {
                    throw new Exception("select Cable Network Type");
                }
                if (model.CableNetworkValue == 1 || model.TypeOfLinkText == "Link3 Own")
                {
                    if (string.IsNullOrEmpty(model.SplitterName))
                    {
                        throw new Exception("Spliter should not blank");
                    }
                    if (string.IsNullOrEmpty(model.FiberLaser))
                    {
                        throw new Exception("Please confirm LASER at Client end by a good power meter");
                    }
                    if (model.CableNo.Contains("Select"))
                    {
                        throw new Exception("select cable no");
                    }


                }
                if (model.CableNetworkValue == 2)
                {
                    if (model.NttnNameText.Contains("Select") || string.IsNullOrEmpty(model.NttnNameText))
                    {
                        throw new Exception("Select NTTN Name");
                    }
                    if (model.TypeOfLinkText.Contains("Select"))
                    {
                        throw new Exception("select type of p2m link");
                    }
                }

                if (model.CheckSharedLink == true)
                {
                    if (string.IsNullOrEmpty(model.ShareOnuPort) || string.IsNullOrEmpty(model.shareOnuMac))
                    {
                        throw new Exception("Input missing ONU MAC/ONU PORT");
                    }
                }

                if (model.ScrIdP2MVisible == true)
                {
                    if (model.OnuOwnerShip.Contains("Select") || string.IsNullOrEmpty(model.OnuOwnerShip))
                    {
                        throw new Exception("Select Missing Ownership");
                    }
                }

                string invSql = @"select * from InTr_Trn_Hdr where Trn_Hdr_Pcode='" + model.SubscriberCode + "'";
                var invResults = await _l3tDBContext.InTrTrnHdrModel.FromSqlRaw(invSql).ToListAsync();
                if (invResults.Count == 0)
                {
                    throw new Exception("Inventory Product not issue.");
                }

                string sqlFile = @"SELECT * FROM clientDatabaseFile WHERE SubscriberID='" + model.SubscriberCode + "'";
                var sqlFileData = await _rsmDBContext.Tbl_ClientDatabaseFile.FromSqlRaw(sqlFile).ToListAsync();
                if(sqlFileData.Count == 0)
                {
                    throw new Exception("File upload missing.");
                }

                string cusname = model.SubscriberCode + ":" + model.SubscriberName + ":" + model.Area;

                string splitter_query = @"select * from tbl_Splitter_JoincolorEntry where CustomerID='" +
                        model.BrCliCode + "' and CustomerSl='" + model.BrSlNo + "'";
                var splitter_data = await _rsmDBContext.Tbl_Splitter_JoincolorEntry.FromSqlRaw(splitter_query).ToListAsync();
                var splitter_data_count = splitter_data.Count;
                if (model.CableNetworkValue == 1 || model.TypeOfLinkText == "Link3 Own")
                {
                    if (splitter_data_count == 0)
                    {
                        throw new Exception("Please enter color detail");
                    }
                }
                if (model.TeamName.Contains("Select"))
                {
                    throw new Exception("Please Select Team");
                }

                string gethydrayRes = "";
                var hydrayData = await _otherService.GetHydraData(model.TicketNo, ip);
                if (hydrayData.StatusCode == 200)
                {
                    gethydrayRes = hydrayData.Data.ToString();
                }
                string rtvn = "[" + gethydrayRes + "] " + model.Remarks;
                var HydraValue = rtvn.Replace("'", "''");

                string[] ar = model.SplitterName.Split(':');
                string splitloc = ar[0].Trim();
                int splitcap = Convert.ToInt32(ar[2].Trim());
                string EncloserNo = ar[1].Trim();



                string sp_sqlstr = "EXEC RSMConnectionDoneP2MApi '" + model.TicketNo + "','" + model.UserId + "','" + model.SubscriberCode + "','" + model.SubscriberName + "','" + model.TeamName + "','" + model.CableNetworkText + "',"+
                                    " '"+ model.CableNetworkValue +"','"+ model.SplitterName +"','"+ model.CableNo +"','"+ model.NttnNameValue +"','"+ model.NttnNameText +"','"+ model.TypeOfLinkValue +"', "+
                                    "'"+ model.TypeOfLinkText +"','"+ model.CheckSharedLink +"','"+ model.ScrIdP2MVisible +"','"+ model.ScrIdP2MText +"','"+ model.OnuOwnerShip +"','"+ model.LinkIdP2MText +"', "+
                                    "'"+ model.Area +"','"+ model.BrCliCode +"','"+ model.BrSlNo +"','"+ model.Remarks.Replace("'", "''") + "','"+ model.BahonCoreId +"','"+ model.Bahonlinkid +"','"+ model.BtsNameText +"', "+
                                    "'"+ model.BtsNameValue +"','"+ model.FiberPon +"','"+ model.FiberPort +"','"+ model.FiberOltBrand +"','"+ model.FiberOltName +"','"+ model.FiberLaser +"', "+
                                    "'"+ model.PortCapFB +"','"+ model.LinkPath +"','"+ model.shareOnuMac +"','"+ model.ShareOnuPort +"','"+ model.ShareOnuCustomerId +"','"+ cusname.Replace("'", "''") + "', "+
                                    "'"+ splitloc +"','"+ splitcap +"','"+ EncloserNo +"','"+ model.SummitLinkId +"','"+ model.Latitude +"','"+ model.Longitude +"', "+
                                    "'" + splitter_data_count +"','"+ HydraValue +"'";

                var result = await _rsmDBContext.RSMConnectionDoneP2MApi.FromSqlRaw(sp_sqlstr).AsNoTracking().ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }


                string sp_sqlstr_131 = "EXEC RSMConnectionDoneP2MApi_131 '" + model.TicketNo + "','" + model.UserId + "','" + model.SubscriberCode + "','" + model.CableNetworkValue + "','" + model.SplitterName + "'," +
                                        "'"+ model.CableNo +"','"+ model.NttnNameValue +"','"+ model.NttnNameText +"','"+ model.TypeOfLinkValue +"','"+ model.ScrIdP2MText +"'," +
                                        "'"+ model.BrCliCode +"','"+ model.BrSlNo +"','"+ model.Remarks.Replace("'", "''") + "','"+ model.BtsNameText +"','"+ model.BtsNameValue +"'," +
                                        "'"+ model.FiberPon +"','"+ model.FiberPort +"','"+ model.FiberOltBrand +"','"+ model.FiberOltName +"','"+ model.FiberLaser +"'," +
                                        "'"+ model.PortCapFB +"','"+ model.LinkPath +"','"+ cusname.Replace("'", "''") + "','"+ splitloc +"','"+ splitcap +"'," +
                                        "'"+ EncloserNo +"','"+ model.SummitLinkId +"','"+ model.Latitude +"','"+ model.Longitude +"'";

                var result_131 = await _misDBContext.RSMConnectionDoneP2MApi_131.FromSqlRaw(sp_sqlstr_131).AsNoTracking().ToListAsync();
                if (result_131.Count > 0)
                {
                    if (result_131[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result_131[0].SuccessOrErrorMessage);
                    }
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data"
                };
                await InsertRequestResponse(model, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> NetworkConnectionUpdateP2M(NetworkConnectionRequestModel model, string userId, string ip)
        {
            var methodName = "RSMInstallationTicketService/NetworkConnectionUpdateP2M";

            try
            {

                if (string.IsNullOrEmpty(model.SplitterName))
                {
                    throw new Exception("Splitter name should not blank");
                }
                if (model.CableNo.Contains("Select"))
                {
                    throw new Exception("select cable no");
                }

                if (model.CheckSharedLink == true)
                {
                    if (string.IsNullOrEmpty(model.ShareOnuPort) || string.IsNullOrEmpty(model.shareOnuMac))
                    {
                        throw new Exception("Input missing ONU MAC/ONU PORT");
                    }
                }

                if (model.ScrIdP2MVisible == true)
                {
                    if (model.OnuOwnerShip.Contains("Select"))
                    {
                        throw new Exception("Select Missing Ownership");
                    }
                }
                string[] ar = model.SplitterName.Split(':');
                string splitloc = ar[0].Trim();
                int splitcap = Convert.ToInt32(ar[2].Trim());
                string EncloserNo = ar[1].Trim();

                string cusname = model.SubscriberCode + ":" + model.SubscriberName + ":" + model.Area;

                string view_SubSplliter_query = @"select count(*) TotalRow from View_SubSplliter where BtsID='" + model.BtsNameValue + "' and OltName='" +
                    model.FiberOltName + "' and PON='" + model.FiberPon + "' and Port='" + model.FiberPort + "' and EncloserNo='" + EncloserNo + "'";
                var view_subSplliterData = await _rsmDBContext.View_SubSplliter.FromSqlRaw(view_SubSplliter_query).FirstOrDefaultAsync();

                if (view_subSplliterData.TotalRow > splitcap)
                {
                    throw new Exception("Already complete splitter capacity");
                }

                string gethydrayRes = "";
                var hydrayData = await _otherService.GetHydraData(model.TicketNo, ip);
                if (hydrayData.StatusCode == 200)
                {
                    gethydrayRes = hydrayData.Data.ToString();
                }
                string rtvn = "[" + gethydrayRes + "] " + model.Remarks;
                var HydraValue = rtvn.Replace("'", "''");


                string sp_sqlstr = "EXEC RSMConnectionUpdateP2MApi '" + model.TicketNo + "','" + model.UserId + "','" + model.SubscriberCode + "','" + model.SubscriberName + "','" + model.SplitterName + "',"+
                                        "'"+ model.CableNo +"','"+ model.CheckSharedLink +"','"+ model.ScrIdP2MVisible +"','"+ model.BrCliCode +"','"+ model.BrSlNo +"',"+
                                        "'"+ model.Remarks.Replace("'", "''") + "','"+ model.BtsNameText+"','" + model.BtsNameValue + "','" + model.FiberPon + "','" + model.FiberPort + "',"+
                                        "'"+ model.FiberOltBrand +"','"+ model.FiberOltName +"','"+ model.FiberLaser +"','"+ model.PortCapFB +"','"+ model.LinkPath +"',"+
                                        "'"+ model.shareOnuMac +"','"+ model.ShareOnuPort +"','"+ model.ShareOnuCustomerId +"','"+ cusname.Replace("'", "''") + "','"+ splitloc +"',"+
                                        "'"+ splitcap +"','"+ EncloserNo +"','"+ model.Latitude +"','"+ model.Longitude +"','"+ HydraValue +"'";

                var result = await _rsmDBContext.RSMConnectionDoneP2MApi.FromSqlRaw(sp_sqlstr).AsNoTracking().ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data"
                };
                await InsertRequestResponse(model, response, methodName, ip, userId, null);
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }


    }
}
