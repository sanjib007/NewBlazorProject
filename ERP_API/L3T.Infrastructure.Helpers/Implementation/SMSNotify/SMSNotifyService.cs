using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;
using L3T.Infrastructure.Helpers.Interface.Common;
using L3T.Infrastructure.Helpers.Interface.SMSNotify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;
using L3T.Infrastructure.Helpers.Models.SmsNotification;
using System.Globalization;
using L3T.Infrastructure.Helpers.Models.SmsNotification.ReqponseModels;
using L3T.Infrastructure.Helpers.Interface.Email;

namespace L3T.Infrastructure.Helpers.Implementation.SMSNotify;

public class SMSNotifyService : ISMSNotifyService
{
    private readonly SMSNotifyWriteDBContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SMSNotifyService> _logger;
    private readonly IMailSenderService _mailrepo;
    private readonly SMSNotifyReadDBContext _readDbContext;
    private readonly SMSNotify131DBContext _read131db;

    public SMSNotifyService(
        SMSNotifyWriteDBContext context, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration, 
        ILogger<SMSNotifyService> logger,
        IMailSenderService mailrepo, 
        SMSNotifyReadDBContext readDbContext,
        SMSNotify131DBContext read131db)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _mailrepo = mailrepo;
        _readDbContext = readDbContext; ;
        _read131db = read131db;
    }
    public async Task<ApiResponse> GetSMSNotification()
    {
        var methordName = "GetSMSNotification";
        var msisdn = _configuration.GetValue<string>("BanglalinkConfig:Msisdn");
        var userId = _configuration.GetValue<string>("BanglalinkConfig:UserID");
        var password = _configuration.GetValue<string>("BanglalinkConfig:Password");
        var getLastItem = _context.SmsNotification.Where(x=> x.Reciver == msisdn).OrderByDescending(x=> x.ID).FirstOrDefault();
        if (getLastItem == null)
        {
            getLastItem = new SmsNotification();
            getLastItem.MessageId = 1;
        }

        var url =
            $"https://vas.banglalink.net/ems_feedback_data_pull/pull_sms.php?msisdn={msisdn}&userID={userId}&passwd={password}&messageID={getLastItem.MessageId}";

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            var smsResponse = await httpClient.GetAsync(url);
            var responseString = await smsResponse.Content.ReadAsStringAsync();
            await InsertRequestResponse(url, responseString, methordName);
            if (smsResponse.IsSuccessStatusCode)
            {
                if(responseString!= "[]")
                {
                    var findStr = responseString.Contains("messageID:");
                    if (findStr)
                    {
                        var stringParse = responseString.Replace("messageID:", "messageID\":\"");
                        var stringParse1 = stringParse.Replace(":{\"sender\"", ",\"Data\":{\"sender\"");
                        var stringParse2 = stringParse1.Replace("{\"messageID\"", "{\"SMSObj\" : [{\"messageID\"");
                        var stringParse3 = stringParse2.Replace("},\"messageID\"", "}},{\"messageID\"");
                        var fullStr = stringParse3 + "]}";
                        var smsData = JsonConvert.DeserializeObject<Root>(fullStr);
                        var smsInfo = new List<SmsNotification>();

                        foreach (var aInfo in smsData.SMSObj)
                        {
                            var newData = new SmsNotification()
                            {
                                MessageId = Convert.ToInt64(aInfo.messageID),
                                Sender = aInfo.Data.sender,
                                Reciver = aInfo.Data.receiver,
                                SMS = aInfo.Data.message,
                                Rcv_Date_Bl = aInfo.Data.received_date,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                Status = "0",
                                TtyCount = 0
                            };
                            var item = await DataProcessControl(newData);
                            smsInfo.Add(item);
                        }
                        await _context.SmsNotification.AddRangeAsync(smsInfo);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new ApiResponse()
                        {
                            Message = responseString,
                            Status = "Error",
                            StatusCode = 400
                        };
                    }                   
                }

                return new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data"
                };
            }
            return new ApiResponse()
            {
                Message = "Request Fail.",
                Status = "Error",
                StatusCode = 400
            };
        }
        catch (Exception ex)
        {
            await errorMethord(ex, methordName);
            var response = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
            await InsertRequestResponse(url, ex.Message, methordName);
            return response;
        }
    }

    private async Task<SmsNotification> DataProcessControl(SmsNotification item)
    {
        try
        {
            var phoneNumber = item.Sender;
            if (item.Sender.Length == 13)
            {
                phoneNumber = item.Sender.Substring(2, 11);
            }

            var clientData = new ClientDatabaseMain();
            clientData = await _read131db.ClientDatabaseMain.FromSqlRaw($"select top 1 ROW_NUMBER() OVER(ORDER BY MqID) AS num_row, MqID, phone_no,[brCliCode] from [WFA2].[dbo].[clientDatabaseMain] where  phone_no ='{item.Sender}' OR phone_no ='{phoneNumber}' order by MqID desc").FirstOrDefaultAsync();

            if (clientData == null)
            {
                item.frm = item.Sender;
                item.rcv_date = item.Rcv_Date_Bl.Substring(0, 10).Replace("-", ":");
                item.rcv_time = item.Rcv_Date_Bl.Substring(11, 8);
                item.msg = item.SMS;
                item.sys_time = DateTime.Now.ToString();
                item.called = "SMS";
                item.address_by = "User01";
                item.date_convert = DateTime.ParseExact(item.Rcv_Date_Bl.Substring(0, 10), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                item.stat = 1;
            }
            else
            {
                item.frm = item.Sender;
                item.rcv_date = item.Rcv_Date_Bl.Substring(0, 10).Replace("-", ":");
                item.rcv_time = item.Rcv_Date_Bl.Substring(11, 8);
                item.msg = item.SMS;
                item.Cust_ID = string.IsNullOrEmpty(clientData.MqID) ? clientData.brCliCode : clientData.MqID;
                item.sys_time = DateTime.Now.ToString();
                item.called = "SMS";
                item.address_by = "User01";
                item.date_convert = DateTime.ParseExact(item.Rcv_Date_Bl.Substring(0, 10), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                item.stat = 1;
            }
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error : " + ex.Message);
            return item;
        }
    }

    private async Task<string> errorMethord(Exception ex, string info)
    {
        string errormessage = "Error : " + ex.Message.ToString();
        _logger.LogInformation("Exception " + errormessage);
        //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
        return errormessage;
    }

    private async Task InsertRequestResponse(string request, string response, string methordName)
    {
        var reqResModel = new SmsNotifyRequestResponse()
        {
            Request = request,
            Response = response,
            MethordName = methordName,
            CreatedAt = DateTime.Now
        };
        var data = await _context.SmsNotifyRequestResponse.AddAsync(reqResModel);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Inserted Data : "+ JsonConvert.SerializeObject(reqResModel));
    }
    
    public async Task<ApiResponse> SmsPushInMySqlDB()
    {
        try
        {
            var getLastItem = await _context.SmsNotification.Where(x => x.Status == "0" && x.TtyCount < 10).ToListAsync();
            if (getLastItem == null)
            {
                throw new Exception("data is null");
            }
            var newData = new raw_data();
            foreach (var item in getLastItem)
            {
                try
                {
                    newData = new raw_data()
                    {
                        frm = item.Sender,
                        rcv_date = item.rcv_date,
                        rcv_time = item.rcv_time,
                        msg = item.msg,
                        sys_time = item.sys_time,
                        called = item.called,
                        address_by = item.address_by,
                        date_convert = item.date_convert,
                        stat = item.stat
                    };
                    await _readDbContext.raw_data.AddAsync(newData);
                    if (await _readDbContext.SaveChangesAsync() > 0)
                    {
                        item.Status = "1";
                        _context.SmsNotification.Update(item);
                        _context.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    item.TtyCount = item.TtyCount + 1;
                    _context.SmsNotification.Update(item);
                    _context.SaveChanges();
                    _logger.LogInformation("Error Message : " + ex.Message);

                    continue;
                }
            }

            return new ApiResponse() { Message = "get", Status = "success", StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error : " + ex.Message);
            return new ApiResponse() { Message = ex.Message, Status = "error", StatusCode = 400 };
        }
    }
    

}