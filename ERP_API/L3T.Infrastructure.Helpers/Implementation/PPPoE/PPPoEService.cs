using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.DataContext.PPPoEDBContext;
using L3T.Infrastructure.Helpers.Interface.PPPoE;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.PPPoE;
using L3T.Infrastructure.Helpers.Models.SmsNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using tik4net;
using tik4net.Objects.User;

namespace L3T.Infrastructure.Helpers.Implementation.PPPoE
{
    public class PPPoEService: IPPPoEService
    {
        private readonly ILogger<PPPoEService> _logger;
        private readonly PPPoEDBContext _pppoEDBContext;
        private readonly DMARadiusDBContext _dmaradiusContext;

        public PPPoEService(
            ILogger<PPPoEService> logger, 
            PPPoEDBContext pppoEDBContext,  
            DMARadiusDBContext dmaradiusContext)
        {
            _logger = logger;
            _pppoEDBContext = pppoEDBContext;
            _dmaradiusContext = dmaradiusContext;
        }


        public async Task<ApiResponse> GetRadcheck(string username)
        {
            var methordName = "PPPoEService/GetRadcheck";
            try
            {
                //var clientData = await _dmaradiusContext.radcheck.FromSqlRaw($"SELECT * FROM `radcheck` WHERE `username` = '"+ username + "'").ToListAsync();
                var clientData = await _dmaradiusContext.radcheck.ToListAsync();

                if (clientData.Count < 0)
                {
                    throw new Exception("Data is not fuond.");
                }


                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = clientData
                };
                await InsertRequestResponse(username, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);                
                await InsertRequestResponse(username, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Nas Create
        public async Task<ApiResponse> NasCreate(string router_name, string secret, string router_ip)
        {
            var methordName = "PPPoEService/NasCreate";
            try
            {
                var message = "";
                var nasData = _dmaradiusContext.nas.FromSqlRaw($"SELECT * FROM `nas` WHERE `nasname` = '" + router_ip + "'").FirstOrDefault();

                if (nasData == null)
                {
                    var newNasData = new nas();
                    newNasData.nasname = router_ip;
                    newNasData.shortname = router_name;
                    newNasData.secret = secret;
                    newNasData.type = 0;

                    _dmaradiusContext.nas.Add(newNasData);
                    _dmaradiusContext.SaveChanges();
                    message = "Nas successfully created!";
                }
                else
                {
                    nasData.shortname = router_name;
                    nasData.secret = secret;

                    _dmaradiusContext.nas.Update(nasData);
                    _dmaradiusContext.SaveChanges();
                    message = "Nas successfully updated!";
                }

                var apiResponse = new ApiResponse()
                {
                    Message = message,
                    Status = "success",
                    StatusCode = 200,
                    Data = "success"
                };
                await InsertRequestResponse(router_name, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                await InsertRequestResponse(router_name, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }

        // Expiry Update
        public async Task<ApiResponse> ExpiryUpdate(string client_id, DateTime expiry_date)
        {
            var methordName = "PPPoEService/ExpiryUpdate";
            try
            {
                var todaysDate = DateTime.Today;

                if (expiry_date < todaysDate)
                {
                    throw new Exception("Sorry invalid expiry date!");
                }

                var rm_users_info = _dmaradiusContext.rm_users.FromSqlRaw($"SELECT * FROM `rm_users` WHERE `username` = '" + client_id + "'").FirstOrDefault();
                if (rm_users_info == null)
                {
                    throw new Exception("Sorry invalid client ID!");
                }
                else
                {
                    // Customer expiry update here 
                    rm_users_info.expiration = expiry_date;

                    _dmaradiusContext.rm_users.Update(rm_users_info);
                    _dmaradiusContext.SaveChanges();
                }

                var apiResponse = new ApiResponse()
                {
                    Message = "Client expiry date successfully updated!",
                    Status = "success",
                    StatusCode = 200,
                    Data = "success"
                };
                await InsertRequestResponse(client_id, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                await InsertRequestResponse(client_id, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }

        // Get Customer Information
        public async Task<ApiResponse> GetCustomerInfo(string client_id)
        {
            var methordName = "PPPoEService/GetCustomerInfo";
            try
            {
                var todaysDate = DateTime.Today;

                var rm_users_info = _dmaradiusContext.rm_users.FromSqlRaw($"SELECT * FROM `rm_users` WHERE `username` = '" + client_id + "'").FirstOrDefault();
                if (rm_users_info == null)
                {
                    throw new Exception("Sorry invalid client ID!");
                }

                int package_id = rm_users_info.srvid;
                var package_info = _dmaradiusContext.rm_services.FromSqlRaw($"SELECT * FROM `rm_services` WHERE `srvid` = '" + package_id + "'").FirstOrDefault();
                if (package_info == null)
                {
                    throw new Exception("Sorry package not found!");
                }

                var C_Info = new CustomerInformation()
                {  
                    CustomerID = client_id,
                    Package = package_info.srvname,
                    ExpiryDate = rm_users_info.expiration,
                    IP = rm_users_info.staticipcpe
                }; 

                var apiResponse = new ApiResponse()
                {
                    Message = "Customer successfully found!",
                    Status = "success",
                    StatusCode = 200,
                    Data = C_Info
                };
                await InsertRequestResponse(client_id, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                await InsertRequestResponse(client_id, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }


        // Package Update
        public async Task<ApiResponse> PackageUpdate(string client_id, string new_package)
        { 
            var methordName = "PPPoEService/PackageUpdate";
            try
            {
                int package_id = 0;
                var PackageData = _dmaradiusContext.rm_services.FromSqlRaw($"SELECT * FROM `rm_services` WHERE `srvname` = '" + new_package + "'").FirstOrDefault();

                if (PackageData == null)
                {
                    throw new Exception("Sorry invalid package name!");
                }
                else
                {
                    package_id = Int32.Parse(PackageData.srvid.ToString());
                }

                var rm_users_info = _dmaradiusContext.rm_users.FromSqlRaw($"SELECT * FROM `rm_users` WHERE `username` = '" + client_id + "'").FirstOrDefault();
                if (rm_users_info == null)
                {
                    throw new Exception("Sorry invalid client ID!");
                }

                // update rm_users, new entry rm_changesrv table
                if (rm_users_info != null && package_id >0 )
                {
                    // Customer expiry update here 
                    rm_users_info.srvid = package_id;

                    _dmaradiusContext.rm_users.Update(rm_users_info);
                    _dmaradiusContext.SaveChanges();

                    DateTime today = DateTime.Now;
                    var newServiceEntry = new rm_changesrv()
                    {
                        username = client_id,
                        newsrvid = package_id,
                        newsrvname = new_package,
                        scheduledate = today,
                        requestdate = today,
                        status = 1,
                        requested = "misapi"
                    };
                    _dmaradiusContext.rm_changesrv.Add(newServiceEntry);
                    _dmaradiusContext.SaveChanges();
                }

                var apiResponse = new ApiResponse()
                {
                    Message = "Client package successfully updated!",
                    Status = "success",
                    StatusCode = 200,
                    Data = "success"
                };
                await InsertRequestResponse(client_id, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                await InsertRequestResponse(client_id, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }
        
        // IpPoolCreate
        public async Task<ApiResponse> IpPoolCreate(string pool_name, string first_ip, string last_ip)
        {
            int pool_id = 0;
            var methordName = "PPPoEService/IpPoolCreate";
            try
            { 
                // Table rm_ippools, radippool, 
                var message = "";
                var IpPoolData = _dmaradiusContext.rm_ippools.FromSqlRaw($"SELECT * FROM `rm_ippools` WHERE `name` = '" + pool_name + "'").FirstOrDefault();

                if (IpPoolData == null)
                {
                    var newIpPoolData = new rm_ippools();
                    newIpPoolData.type = 1;
                    newIpPoolData.name = pool_name;
                    newIpPoolData.fromip = first_ip;
                    newIpPoolData.toip = last_ip;

                    _dmaradiusContext.rm_ippools.Add(newIpPoolData);
                    _dmaradiusContext.SaveChanges();
                    message = "Nas successfully created!";

                    //SELECT MAX(id)+1 as id FROM `rm_ippools` WHERE 1
                    var LastIpPoolData = _dmaradiusContext.rm_ippools.FromSqlRaw($"SELECT MAX(id) as id FROM `rm_ippools` WHERE 1").FirstOrDefault();

                    if (LastIpPoolData != null)
                    {
                        pool_id = LastIpPoolData.id;
                    }
                }
                else
                {
                    IpPoolData.fromip = first_ip;
                    IpPoolData.toip = last_ip;

                    _dmaradiusContext.rm_ippools.Update(IpPoolData);
                    _dmaradiusContext.SaveChanges();
                    message = "IP Pool successfully updated!";

                    pool_id = IpPoolData.id;
                }

                string ip;
                var startIpArray = first_ip.Split('.');
                var endIpArray = last_ip.Split('.');
                if (endIpArray[3] != null && startIpArray[3] != endIpArray[3])
                {
                    var newRadipPoolData = new radippool();
                    int end = Int32.Parse(endIpArray[3]);
                    int start = Int32.Parse(startIpArray[3]);
                    //var newIpPoolDataData = new nas();
                    for (int i = start; i < end; i++)
                    {
                        ip = startIpArray[0] + "." + startIpArray[1] + "." + startIpArray[2] + "." + i;
                        // add pool_id + ip
                        newRadipPoolData.pool_name = pool_id;
                        newRadipPoolData.framedipaddress = ip; 
                    }
                    // insert into radippool pool_id + ip
                    _dmaradiusContext.radippool.Add(newRadipPoolData);
                }



                var apiResponse = new ApiResponse()
                {
                    Message = message,
                    Status = "success",
                    StatusCode = 200,
                    Data = "success"
                };
                await InsertRequestResponse(pool_name, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                await InsertRequestResponse(pool_name, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //CreateUser
        public async Task<ApiResponse> CreateUser(string username, string package, string password, string ip)
        {
            var methordName = "PPPoEService/CreateUser";
            try
            {
                //var clientData = await _dmaradiusContext.radcheck.FromSqlRaw($"SELECT * FROM `radcheck` WHERE `username` = '"+ username + "'").ToListAsync();
                //var clientData = _dmaradiusContext.radcheck.FromSqlRaw($"SELECT * FROM `radcheck` WHERE `username` = '"+ username + "' and attribute='Cleartext-Password'").FirstOrDefault();

                var package_id = "";
                var PackageData = _dmaradiusContext.rm_services.FromSqlRaw($"SELECT * FROM `rm_services` WHERE `srvname` = '" + package + "'").FirstOrDefault();

                if (PackageData == null)
                {
                    throw new Exception("Sorry invalid package name!");
                }
                else
                {
                    package_id = PackageData.srvid.ToString();
                }

                var raw_password = password;

                using (var md5Hash = MD5.Create())
                {
                    // Byte array representation of source string
                    var sourceBytes = Encoding.UTF8.GetBytes(password);

                    // Generate hash value(Byte Array) for input data
                    var hashBytes = md5Hash.ComputeHash(sourceBytes);

                    // Convert hash byte array to string
                    password = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                }

                /******* Insert into User Table *******/
                var rm_users_info = _dmaradiusContext.rm_users.FromSqlRaw($"SELECT * FROM `rm_users` WHERE `username` = '" + username + "'").FirstOrDefault();

                if (rm_users_info == null)
                {
                    var rm_usersData = new rm_users();
                    rm_usersData.username = username;
                    rm_usersData.password = password;
                    rm_usersData.srvid = Int32.Parse(package_id);
                    rm_usersData.staticipcpe = ip;
                    rm_usersData.ipmodecpe = 2;
                    rm_usersData.expiration = DateTime.Now;
                    rm_usersData.createdby = "misapi";
                    rm_usersData.owner = "misapi";
                    rm_usersData.createdon = DateTime.Now;

                    _dmaradiusContext.rm_users.Add(rm_usersData);
                    _dmaradiusContext.SaveChanges();
                }
                else
                {
                    rm_users_info.password = password;
                    rm_users_info.staticipcpe = ip;
                    rm_users_info.srvid = Int32.Parse(package_id);

                    _dmaradiusContext.rm_users.Update(rm_users_info);
                    _dmaradiusContext.SaveChanges();
                }
                /***** End *****/

                var rm_service_info = _dmaradiusContext.rm_changesrv.FromSqlRaw($"SELECT * FROM `rm_changesrv` WHERE `username` = '" + username + "'").FirstOrDefault();

                if (rm_service_info == null)
                {
                    var changesrvData = new rm_changesrv();
                    changesrvData.username = username;
                    changesrvData.newsrvid = Int32.Parse(package_id);
                    changesrvData.newsrvname = package;
                    changesrvData.scheduledate = DateTime.Now; 
                    changesrvData.requestdate = DateTime.Now;
                    changesrvData.status = 1;
                    changesrvData.requested = "misapi";

                    _dmaradiusContext.rm_changesrv.Add(changesrvData);
                    _dmaradiusContext.SaveChanges();
                }
                else
                {
                    rm_service_info.newsrvname = package;
                    rm_service_info.newsrvid = Int32.Parse(package_id); 

                    _dmaradiusContext.rm_changesrv.Update(rm_service_info);
                    _dmaradiusContext.SaveChanges();
                }



                var clientData = _dmaradiusContext.radcheck.FromSqlRaw($"SELECT * FROM `radcheck` WHERE `username` = '"+ username + "' and attribute='Cleartext-Password'").FirstOrDefault();

                if (clientData == null)
                {
                    var radcheckdata = new radcheck();
                    radcheckdata.username = username;
                    radcheckdata.attribute = "Cleartext-Password";
                    radcheckdata.op = ":=";
                    radcheckdata.value = raw_password;

                    _dmaradiusContext.radcheck.Add(radcheckdata);
                    _dmaradiusContext.SaveChanges();
                } 
                else
                {
                    clientData.value = raw_password;

                    _dmaradiusContext.radcheck.Update(clientData);
                    _dmaradiusContext.SaveChanges();
                }

                var clientUseData = _dmaradiusContext.radcheck.FromSqlRaw($"SELECT * FROM `radcheck` WHERE `username` = '" + username + "' and attribute='Simultaneous-Use'").FirstOrDefault();
                if (clientUseData == null)
                {
                    var radcheckUsedata = new radcheck();
                    radcheckUsedata.username = username;
                    radcheckUsedata.attribute = "Simultaneous-Use";
                    radcheckUsedata.op = ":=";
                    radcheckUsedata.value = "1";

                    _dmaradiusContext.radcheck.Add(radcheckUsedata);
                    _dmaradiusContext.SaveChanges();
                }

 
                var apiResponse = new ApiResponse()
                {
                    Message = "User successfully created!",
                    Status = "success",
                    StatusCode = 200,
                    Data = "success"
                };
                await InsertRequestResponse(username, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                await InsertRequestResponse(username, ex, null, null, methordName, null);
                throw new Exception($"Error: {ex.Message}");
            }
        }


        private async Task InsertRequestResponse(object request, object response, string cusIp, string routerIp, string methordName, string userId, string subId = null)
        {
            var errorMethordName = "CamsService/InsertRequestResponseSync";
            try
            {
                var reqResModel = new PPPoERequestResponseModel()
                {
                    Request = JsonConvert.SerializeObject(request),
                    Response = System.Convert.ToString(JsonConvert.SerializeObject(response)),
                    MethordName = methordName,
                    CustomerIp = cusIp,
                    RouterIp = routerIp,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    SubId = subId
                };
                _pppoEDBContext.PPPoERequestResponse.Add(reqResModel);
                _pppoEDBContext.SaveChanges();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }

        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }

    public class CustomerInformation
    {
        public string CustomerID { get; set; }
        public string Package { get; set; }
        public string IP { get; set; }
        public DateTime ExpiryDate { get; set; } 
    }
}
