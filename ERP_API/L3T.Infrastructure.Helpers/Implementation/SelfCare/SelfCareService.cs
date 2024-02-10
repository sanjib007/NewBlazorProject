using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.DataContext.OracleDBContex;
using L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext;
using L3T.Infrastructure.Helpers.Implementation.Cams;
using L3T.Infrastructure.Helpers.Interface.SelfCare;
using L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model;
using L3T.Infrastructure.Helpers.Models.SelfCare.RequestModel;
using L3T.Infrastructure.Helpers.Models.SelfCare.ResponseModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Implementation.SelfCare
{
    public class SelfCareService : ISelfCareService
    {
        private readonly SelfCareWriteDbContext _sc_context;
        private readonly ILogger<SelfCareService> _logger;
        private readonly Wfa2_133DBContext _wfa2_133DBContext;
        private readonly L3T_131DBContext _L3T_131DBContext;
        private readonly WFA2_131DBContext _WFA2_131DBContext;
        private readonly OraDbContext _oracleDB;


        public SelfCareService(SelfCareWriteDbContext sc_context, 
            ILogger<SelfCareService> logger,
            Wfa2_133DBContext wfa2_133DBContext,
            L3T_131DBContext l3t_131DBContext,
            WFA2_131DBContext wFA2_131DBContext, OraDbContext oracleDB)
        {
            _sc_context = sc_context;
            _logger = logger;
            _wfa2_133DBContext = wfa2_133DBContext;
            _L3T_131DBContext = l3t_131DBContext;
            _WFA2_131DBContext = wFA2_131DBContext;
            _oracleDB = oracleDB;
        }

        public async Task<ApiResponse> testMethod(int id)
        {
            var methodName = "SelfCareService/test";
            try
            {
                var aObj = new testObj()
                {
                    Id = 1,
                    Name = "test",
                };
                var response= new ApiResponse()
                {
                    Status = "success",
                    StatusCode = 200,
                    Message = id.ToString(),
                    //Data = "get data"
                    Data = aObj
                };
                await InsertRequestResponse(id, response, methodName, null, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(id, ex, methodName, null, null, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> ServiceCreate(ServiceCreateRequestModel model, string ip, string userId)
        {
            var methodName = "SelfCareService/ServiceCreate";
            try
            {
                decimal price = 0;
                var ServiceCode = "";
                var CustomerID = model.CustomerID;
                var ServiceName = model.ServiceName;
                var ServiceID = model.ServiceID;

                /******* Check if service is enable or not ******/
                string sql = "select ItemID, ItemName, ItemCode, ItemSalesPrice, IsActive from slsItemDetails where IsActive='Y' and ItemCode='" + ServiceID + "'";
                var ProductInfo = await _L3T_131DBContext.GetProductInfo.FromSqlRaw(sql).FirstOrDefaultAsync();

                if (ProductInfo != null)
                {
                    ServiceCode = ProductInfo.ItemCode;
                    ServiceName = ProductInfo.ItemName;
                    price = ProductInfo.ItemSalesPrice;
                }
                else
                {
                    throw new Exception("Sorry this service was not found or inactive");
                }
                /****** END *****/

               
                DateTime curDateTime = DateTime.Now;
                DateTime NextBillingMonth = curDateTime.AddMonths(1); // Default puls one month
                var N_SERVICE_ID = "";
                // if Code 14.005.255 Bongo 6 months
                // if Code 14.005.254 Bongo 3 months
                // if Code 14.005.250 Bongo free 1 month

                if (ServiceCode == "14.005.255")
                {
                    NextBillingMonth = curDateTime.AddMonths(6); // puls 6 month
                    N_SERVICE_ID = "27750663901"; //- Bongo 140Tk 6 Months
                }
                else if (ServiceCode == "14.005.254")
                {
                    NextBillingMonth = curDateTime.AddMonths(3); // puls 3 month
                    N_SERVICE_ID = "27753127101";  // Bongo 75Tk 3 Months
                }
                else  // Free customer
                {
                    N_SERVICE_ID = "21576278801"; // Bongo Free Subscription
                }

                string perFix = CustomerID.Substring(0, 3);
                perFix = perFix.ToLower();

                // For Hydra Customer 
                if (perFix == "l3r")
                {
                    //string oradb = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.0.67)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = hydrast))); User Id=Ais_NET;Password=OxcjtkzYWxx125PnyQ3mqrR0;Connection Timeout=1800; ";

                    var N_USER_ID = "";
                    var N_CUSTOMER_ID = "";

                    var commandText = "SELECT N_SUBJECT_ID as a, N_CUSTOMER_ID as b FROM SI_V_USERS WHERE VC_CODE='" + CustomerID  + "'"; 
                    var dbResult = await _oracleDB.ServiceData.FromSqlRaw(commandText).ToListAsync();
                    if(dbResult !=null)
                    {
                        N_USER_ID = dbResult[0].a.ToString(); ; 
                        N_CUSTOMER_ID = dbResult[0].b.ToString(); ; // For Free service
                    } 
                    else
                    {
                        throw new Exception("Sorry customer not found!");
                    }
                    // Create permission for this customer 
                    var query = @"
                        BEGIN
                          MAIN.INIT(
                            vch_VC_IP            => '123.200.0.67', -- IP that would be used to login in Hydra
                            vch_VC_USER          => 'L3T1293',      -- Employee login
                            vch_VC_PASS          => 'L3T1293@wwf',  -- Employee password
                            num_N_USER_ID        =>  VAR_N_USER_ID,   -- Customer ID
                            vch_VC_AUTH_APP_CODE => 'NETSERV_ARM_ISP', -- Check credentials of different application (Service Provider Console)
                            vch_VC_APP_CODE      => 'NETSERV_ARM_Private_Office', -- Application code (Self-Care Portal, used for session creation and permisions check)
                            vch_VC_CLN_APPID     => 'Link3 Self-Care Portal' -- Description of the application (name, version etc);
                          );
                          COMMIT;
                        END;";
                    query = query.Replace("VAR_N_USER_ID", N_USER_ID);
                    var a = await _oracleDB.Database.ExecuteSqlRawAsync(query);


                    /************ Create service For this customer ************/ 
                    var N_ACCOUNT_ID = "";
                    var N_CONTRACT_ID = "";
                    var N_OBJECT_ID = "";
                    
                    //-- Service ID (27753127101 - Bongo 75Tk 3 Months, 27750663901
                    //-- Service ID (21576278801 - Bongo Free Subscription)

                    query = "SELECT N_ACCOUNT_ID as a, N_ACCOUNT_ID as b FROM AP_V_PERSONAL_ACCOUNTS";
                    dbResult = await _oracleDB.ServiceData.FromSqlRaw(query).ToListAsync();
                    if (dbResult != null)
                    {
                        N_ACCOUNT_ID = dbResult[0].a.ToString();
                    }

                    query = "SELECT N_DOC_ID as a, N_DOC_ID as b FROM AP_V_USER_OFFICE_CONTR";
                    dbResult = await _oracleDB.ServiceData.FromSqlRaw(query).ToListAsync();
                    if (dbResult != null)
                    {
                        N_CONTRACT_ID = dbResult[0].a.ToString();
                    }

                    query = "SELECT N_DEVICE_ID as a, N_DEVICE_ID as b FROM AP_V_USER_OFFICE_DEVICES";
                    dbResult = await _oracleDB.ServiceData.FromSqlRaw(query).ToListAsync();
                    if (dbResult != null)
                    {
                        N_OBJECT_ID = dbResult[0].a.ToString();
                    }

                    if(N_ACCOUNT_ID =="" || N_CONTRACT_ID =="" || N_OBJECT_ID =="")
                    {
                        throw new Exception("Sorry customer's necessery information not found!");
                    }

                    DateTime dtu = DateTime.Now;
                    var TODAY = string.Format("{00:00}", dtu.Year) + "-" + string.Format("{00:00}", dtu.Month) + "-" + dtu.Day.ToString() + " " + string.Format("{00:00}", dtu.Hour) + ":" + string.Format("{00:00}", dtu.Minute + 2) + ":" + string.Format("{00:00}", dtu.Second);
                    var END_DATE = string.Format("{00:00}", dtu.Year) + "-" + string.Format("{00:00}", dtu.Month +1) + "-" + dtu.Day.ToString() + " " + string.Format("{00:00}", dtu.Hour) + ":" + string.Format("{00:00}", dtu.Minute + 2) + ":" + string.Format("{00:00}", dtu.Second);

                    var SQL = "";
                    // Free subscription
                    if ( N_SERVICE_ID == "21576278801")
                    {
                        SQL = @"
                            DECLARE
                                num_N_SUBSCRIPTION_ID NUMBER;
                            BEGIN
                                SI_SUBSCRIPTIONS_PKG.SI_SUBSCRIPTIONS_PUT(
                                num_N_SUBSCRIPTION_ID => num_N_SUBSCRIPTION_ID,
                                num_N_CUSTOMER_ID => VAR_N_CUSTOMER_ID, -- Customer ID
                                num_N_ACCOUNT_ID => VAR_N_ACCOUNT_ID, -- Customer account ID
                                num_N_DOC_ID => VAR_N_CONTRACT_ID, -- Contract ID
                                num_N_OBJECT_ID => VAR_N_OBJECT_ID, -- Customer premises equipment ID
                                num_N_SERVICE_ID => VAR_N_SERVICE_ID, -- Service ID (21576278801 - Bongo Free Subscription)
                                num_N_PAY_DAY => 1, -- Monthly/billing date
                                dt_D_BEGIN => TO_DATE('VAR_D_BEGIN', 'YYYY-MM-DD HH24:MI:SS'), -- Service begin date
                                dt_D_END =>TO_DATE('VAR_D_END', 'YYYY-MM-DD HH24:MI:SS') -- Service end date
                                );
                                COMMIT;
                            END;";
                    } 
                    else
                    {
                        SQL = @"
                            DECLARE
	                            ONE_OFF_N_SUBSCRIPTION_ID NUMBER;
	                            BEGIN
		                            ONE_OFF_N_SUBSCRIPTION_ID := SI_SUBSCRIPTIONS_PKG.ADD_ONE_OFF_SUBSCRIPTION(
			                            num_N_ACCOUNT_ID => VAR_N_ACCOUNT_ID,
			                            num_N_CONTRACT_ID => VAR_N_CONTRACT_ID,
			                            num_N_OBJECT_ID => VAR_N_OBJECT_ID,
			                            num_N_SERVICE_ID => VAR_N_SERVICE_ID,
			                            dt_D_BEGIN => TO_DATE('VAR_D_BEGIN', 'YYYY-MM-DD HH24:MI:SS')
		                            );
	                            COMMIT;
                            END;";
                    }                    

                    SQL = SQL.Replace("VAR_N_ACCOUNT_ID", N_ACCOUNT_ID).Replace("VAR_N_CONTRACT_ID", N_CONTRACT_ID).Replace("VAR_N_OBJECT_ID", N_OBJECT_ID).Replace("VAR_N_SERVICE_ID", N_SERVICE_ID).Replace("VAR_D_BEGIN", TODAY).Replace("VAR_D_END", END_DATE).Replace("VAR_N_CUSTOMER_ID", N_CUSTOMER_ID);

                    var b = await _oracleDB.Database.ExecuteSqlRawAsync(SQL);
                    /****** END *****/

                }
                else
                {
                    // if MIS Customer  
                    int brslno = 1;
                    string UserCode = "SelfCare";
                    var categry = "";
                    var distid = "";

                    var BillingCycleID = 1;
                    var BillingCycleValue = 0;

                    if (CustomerID.Length <10)
                    {
                        throw new Exception("Sorry invalid customer ID!");
                    }

                    /******* Check if customer is exist ******/
                    sql = "select brCliCode, brcategoryID, DistributorID, phone_no, email_id, brSlNo from clientDatabaseMain WHERE brCliCode = '" + CustomerID + "' and brSlNo=1 ";
                    var CustomerInfo = await _WFA2_131DBContext.GetCustomerInfo.FromSqlRaw(sql).FirstOrDefaultAsync();

                    if (CustomerInfo != null)
                    {
                        categry = CustomerInfo.brcategoryID.ToString();
                        distid = CustomerInfo.DistributorID.ToString();
                    }
                    else
                    {
                        throw new Exception("Sorry invalid customer ID!");
                    }
                    /****** END *****/


                    /******* Check if service is already activated ******/
                    sql = "SELECT TOP(1) r.SalesID, r.ServiceCode, r.ServiceNarration, r.NextBillingMonth, r.UsagesStartDate, r.UsagesEndDate, r.BillingServiceRate, s.ClientID FROM blgRevenueFromService as r INNER JOIN slsSalesDetails as s ON r.SalesID = s.SalesID where s.ClientID = '" + CustomerID + "' AND r.ServiceCode = '" + ServiceID + "'";
                    var RevenueSalesInfo = await _L3T_131DBContext.GetRevenueSalesInfo.FromSqlRaw(sql).FirstOrDefaultAsync();

                    if (RevenueSalesInfo != null)
                    {
                        throw new Exception("Sorry you have already this service activated!");
                    }
                    /****** END *****/
                                         
                    if (distid == "")
                    {
                        distid = "0";
                    }
                    string subscriberID = CustomerID; 

                    // resolved sp call
                    string SPsql = "EXEC SP_VasService_Create_Like_Bongo @subscriberID, @categry, @UserCode, @distid, @ServiceCode, @ServiceName, @price, @BillingCycleID, @BillingCycleValue, @NextBillingMonth";

                    List<SqlParameter> parms = new List<SqlParameter>
                    { 
                        // Create parameters    
                        new SqlParameter { ParameterName = "@subscriberID", Value = subscriberID },
                        new SqlParameter { ParameterName = "@categry", Value = categry },
                        new SqlParameter { ParameterName = "@UserCode", Value = UserCode },
                        new SqlParameter { ParameterName = "@distid", Value = distid },

                        new SqlParameter { ParameterName = "@ServiceCode", Value = ServiceCode },
                        new SqlParameter { ParameterName = "@ServiceName", Value = ServiceName },
                        new SqlParameter { ParameterName = "@price", Value = price },
                        new SqlParameter { ParameterName = "@BillingCycleID", Value = BillingCycleID },
                        new SqlParameter { ParameterName = "@BillingCycleValue", Value = BillingCycleValue },
                        new SqlParameter { ParameterName = "@NextBillingMonth", Value = NextBillingMonth },
                    };

                    var result = await _L3T_131DBContext.CreateServiceSP.FromSqlRaw(SPsql, parms.ToArray()).ToListAsync();

                    if(result.Count > 0)
                    {
                        if (result[0].Code == 400) {
                            throw new Exception(result[0].SalesId);
                        }
                    }

                }

                var response = new ApiResponse()
                {
                    Status = "success",
                    StatusCode = 200,
                    Message = "Successfully service created! ",
                    Data = model
                };

                await InsertRequestResponse(model, response, methodName, ip, userId, "");
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message); 
            }
        }


        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            return errormessage;
        }



        private async Task InsertRequestResponse(object request, object response, string methodName, string requestedIP, string userId, string errorLog)
        {
            var errorMethordName = "ForwardTicketService/InsertRequestResponse";
            try
            {
                var reqResModel = new SCRequestResponseModel()
                {
                    CreatedAt = DateTime.Now,
                    Request = JsonConvert.SerializeObject(request),
                    Response = JsonConvert.SerializeObject(response),
                    RequestedIP = requestedIP,
                    MethodName = methodName,
                    UserId = userId,
                    ErrorLog = errorLog
                };
                await _sc_context.SCRequestResponses.AddAsync(reqResModel);
                await _sc_context.SaveChangesAsync();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }


    }
    public class testObj { 
        public int Id { get; set; }
        public string Name { get; set; }
    }



}
