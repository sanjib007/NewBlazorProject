using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.Common;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.SocketModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tik4net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace L3T.Infrastructure.Helpers.Implementation.Cams
{
    public class SocketService : ISocketService
    {
        private readonly CamsDataWriteContext _camsContext;
        private readonly ILogger<CamsService> _logger;
        private readonly ISystemService _systemService;
        private readonly IMailSenderService _mailrepo;
        private readonly IHubContext<ChartHub> _hub;
        private readonly TimerManager _timer;

        public SocketService(
            CamsDataWriteContext camsContext,
            ILogger<CamsService> logger,
            ISystemService systemService,
            IMailSenderService mailrepo,
            IHubContext<ChartHub> hub, 
            TimerManager timer
        )
        {
            _camsContext = camsContext;
            _logger = logger;
            _systemService = systemService;
            _mailrepo = mailrepo;
            _hub = hub;
            _timer = timer;
        }



        public async Task<ApiResponse> GetLiveDataFormMikrotikRouter(GetUserInfoFromMikrotikRequestModel requestModel)
        {
            var methordName = "CamsService/GetUserInfoFromMikrotikRouter";
            try
            {
                // using ()
                //{

                //ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api;
                //connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);


                //var singleInfo = new MikrotikInterFaceDataModel();

                if (!_timer.IsTimerStarted)
                    {
                        _timer.PrepareTimer(async () => _hub.Clients.All.SendAsync("TransferChartData", DataManager.GetData(requestModel))); //getDataFromMikrotik(connection, requestModel.CustomerIp, requestModel.RouterIp)

                    }

                    var apiResponse = new ApiResponse()
                    {
                        Message = "get data",
                        Status = "success",
                        StatusCode = 200
                    };
                    await InsertRequestResponse(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                    return apiResponse;
               // }
                //_timer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferChartData", DataManager.GetData()));
                //return null;
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
                await InsertRequestResponse(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return response;
            }
        }


        private async Task<ChartDtoInfo> getDataFromMikrotik(ITikConnection connection, string CustomerIp, string RouterIp)
        {
            var chartData = new ChartDtoInfo();
            var response = await getUserByIpMethod(connection, CustomerIp);
            if (response.Count() == 0)
            {
                throw new Exception("User not found.");
            }
            else
            {
                var singleInfo = new MikrotikInterFaceDataModel();
                singleInfo = await ParseModel(response.FirstOrDefault().Words, RouterIp);
                chartData = new ChartDtoInfo()
                {
                    RxValue = Convert.ToInt32(singleInfo.rxbyte),
                    TxValue = Convert.ToInt32(singleInfo.txbyte),
                    TimeLabel = DateTime.Now.ToString("HH:mm:ss")
                };
            }
            return chartData;
        }


    private async Task<IEnumerable<ITikReSentence>> getUserByIpMethod(ITikConnection connection, string userName)
        {
            var getInfo = connection.CreateCommandAndParameters("/interface/print", "name", userName);
            var response = getInfo.ExecuteList();
            return response;
        }

        private async Task<MikrotikInterFaceDataModel> ParseModel(IReadOnlyDictionary<string, string> words, string routerIp)
        {
            var newData = new MikrotikInterFaceDataModel();
            foreach (var info in words)
            {
                //row.Add(info.Key, info.Value);
                if (info.Key == ".id")
                {
                    newData.Id = info.Value;
                }
                else if (info.Key == "name")
                {
                    newData.name = info.Value;
                }
                else if (info.Key == "default-name")
                {
                    newData.defaultname = info.Value;
                }
                else if (info.Key == "type")
                {
                    newData.type = info.Value;
                }
                else if (info.Key == "mtu")
                {
                    newData.mtu = info.Value;
                }
                else if (info.Key == "actual-mtu")
                {
                    newData.actualmtu = info.Value;
                }
                else if (info.Key == "l2mtu")
                {
                    newData.l2mtu = info.Value;
                }
                else if (info.Key == "max-l2mtu")
                {
                    newData.maxl2mtu = info.Value;
                }
                else if (info.Key == "mac-address")
                {
                    newData.macaddress = info.Value;
                }
                else if (info.Key == "last-link-down-time")
                {
                    newData.lastlinkdowntime = info.Value;
                }
                else if (info.Key == "last-link-up-time")
                {
                    newData.lastlinkuptime = info.Value;
                }
                else if (info.Key == "rx-byte")
                {
                    newData.rxbyte = info.Value;
                }
                else if (info.Key == "tx-byte")
                {
                    newData.txbyte = info.Value;
                }
                else if (info.Key == "rx-packet")
                {
                    newData.rxpacket = info.Value;
                }
                else if (info.Key == "tx-packet")
                {
                    newData.txpacket = info.Value;
                }
                else if (info.Key == "rx-drop")
                {
                    newData.rxdrop = info.Value;
                }
                else if (info.Key == "txdrop")
                {
                    newData.txdrop = info.Value;
                }
                else if (info.Key == "tx-queue-drop")
                {
                    newData.txqueuedrop = info.Value;
                }
                else if (info.Key == "rx-error")
                {
                    newData.rxerror = info.Value;
                }
                else if (info.Key == "tx-error")
                {
                    newData.txerror = info.Value;
                }
                else if (info.Key == "fp-rx-byte")
                {
                    newData.fprxbyte = info.Value;
                }
                else if (info.Key == "fp-tx-byte")
                {
                    newData.fptxbyte = info.Value;
                }
                else if (info.Key == "fp-rx-packet")
                {
                    newData.fprxpacket = info.Value;
                }
                else if (info.Key == "fp-tx-packet")
                {
                    newData.fptxpacket = info.Value;
                }
                else if (info.Key == "running")
                {
                    newData.running = info.Value;
                }
                else if (info.Key == "disabled")
                {
                    newData.disabled = info.Value;
                }
            }
            newData.RouterIp = routerIp;
            newData.CreatedAt = DateTime.Now;

            return newData;
        }


        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }

        private async Task InsertRequestResponse(object request, object response, string cusIp, string routerIp, string methordName, string userId, string subId = null)
        {
            var errorMethordName = "CamsService/InsertRequestResponse";
            try
            {
                var reqResModel = new MikrotikRequestResponse()
                {
                    Request = JsonConvert.SerializeObject(request),
                    Response = JsonConvert.SerializeObject(response),
                    MethordName = methordName,
                    CustomerIp = cusIp,
                    RouterIp = routerIp,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    SubId = subId
                };
                await _camsContext.MikrotikRequestResponses.AddAsync(reqResModel);
                await _camsContext.SaveChangesAsync();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }






    }

    public static class DataManager
    {
        public static ChartDtoInfo GetData(GetUserInfoFromMikrotikRequestModel requestModel)
        {
            //var r = new Random();
            //return new List<ChartDto>()
            //{
            //    new ChartDto { RxValue = r.Next(1, 40), TxValue = r.Next(1, 40), TimeLabel = DateTime.Now.ToString("HH:mm:ss") },
            //    new ChartDto { RxValue = r.Next(1, 40), TxValue = r.Next(1, 40), TimeLabel = DateTime.Now.ToString("HH:mm:ss") },
            //    new ChartDto { RxValue = r.Next(1, 40), TxValue = r.Next(1, 40), TimeLabel = DateTime.Now.ToString("HH:mm:ss") },
            //    new ChartDto { RxValue = r.Next(1, 40), TxValue = r.Next(1, 40), TimeLabel = DateTime.Now.ToString("HH:mm:ss") }
            //};
            using(ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);

                var chartData = new ChartDtoInfo();
                var getInfo = connection.CreateCommandAndParameters("/interface/print", "name", requestModel.CustomerIp);
                var response = getInfo.ExecuteList();
                //connection.Close();
                if (response.Count() == 0)
                {
                    throw new Exception("User not found.");
                }
                else
                {
                    var newData = new MikrotikInterFaceDataModel();

                    foreach (var info in response.FirstOrDefault().Words)
                    {
                        //row.Add(info.Key, info.Value);
                        if (info.Key == ".id")
                        {
                            newData.Id = info.Value;
                        }
                        else if (info.Key == "rx-byte")
                        {
                            newData.rxbyte = info.Value;
                        }
                        else if (info.Key == "tx-byte")
                        {
                            newData.txbyte = info.Value;
                        }
                        else if (info.Key == "rx-packet")
                        {
                            newData.rxpacket = info.Value;
                        }
                        else if (info.Key == "tx-packet")
                        {
                            newData.txpacket = info.Value;
                        }
                        else if (info.Key == "rx-drop")
                        {
                            newData.rxdrop = info.Value;
                        }
                        else if (info.Key == "txdrop")
                        {
                            newData.txdrop = info.Value;
                        }
                        else if (info.Key == "tx-queue-drop")
                        {
                            newData.txqueuedrop = info.Value;
                        }
                        else if (info.Key == "rx-error")
                        {
                            newData.rxerror = info.Value;
                        }
                        else if (info.Key == "tx-error")
                        {
                            newData.txerror = info.Value;
                        }
                        else if (info.Key == "fp-rx-byte")
                        {
                            newData.fprxbyte = info.Value;
                        }
                        else if (info.Key == "fp-tx-byte")
                        {
                            newData.fptxbyte = info.Value;
                        }
                        else if (info.Key == "fp-rx-packet")
                        {
                            newData.fprxpacket = info.Value;
                        }
                        else if (info.Key == "fp-tx-packet")
                        {
                            newData.fptxpacket = info.Value;
                        }
                    }

                    chartData = new ChartDtoInfo()
                    {
                        RxValue = ((((Convert.ToDecimal(newData.rxbyte)) / 1000) / 1000) / 8),
                        TxValue = (((Convert.ToDecimal(newData.txbyte) / 1000) / 1000) / 8),
                        TimeLabel = DateTime.Now.ToString("HH:mm:ss")
                    };
                    return chartData;
                }
            
            }            
        }
    }

    public class ChartDto
    {
        public string TimeLabel { get; set; }
        public int RxValue { get; set; }
        public int TxValue { get; set; }
    }
}
