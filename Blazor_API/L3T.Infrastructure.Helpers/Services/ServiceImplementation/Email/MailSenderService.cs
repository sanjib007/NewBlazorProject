using L3T.Infrastructure.Helpers.Services.ServiceInterface.Email;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Email
{
    public class MailSenderService : IMailSenderService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<MailSenderService> _logger;
        private readonly ICRRequestResponseService _cRRequestResponseService;

        public MailSenderService(
            IConfiguration configuration, 
            ILogger<MailSenderService> logger,
            ICRRequestResponseService cRRequestResponseService)
        {
            _configuration = configuration;
            _logger = logger;
            _cRRequestResponseService = cRRequestResponseService;
        }

        public async Task SingleSendMail(string subject, string body, string from, string toaddress)
        {
            var methodName = "MailSenderService/SingleSendMail";
            List<string> emailaddress = new List<string>();

            try
            {
                emailaddress.Add(toaddress);
                SendMail(subject, body, from, emailaddress, null, null);
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await _cRRequestResponseService.CreateResponseRequest(null, ex, null, methodName, null, ex.Message.ToString());
            }
        }
        public async Task SendMail(string subject, string body, string fromAddress, List<string> toAddress, List<string> ccAddress, List<string> bccAddress, string displayName = "")
        {
            toAddress.Clear();
            ccAddress.Clear();
            ccAddress.Add("loton1984@gmail.com");
            toAddress.Add("sanjib.dhar@link3.net");
            var methodName = "MailSenderService/SendMail";
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(fromAddress, displayName);
            foreach (string toadd in toAddress)
            {
                if (!string.IsNullOrEmpty(toadd))
                {
                    mailMessage.To.Add(new MailAddress(toadd));
                }
            }

            if (ccAddress != null)
            {
                foreach (string ccadd in ccAddress)
                {
                    if (!string.IsNullOrEmpty(ccadd))
                    {
                        mailMessage.CC.Add(ccadd);
                    }
                }
            }

            if (bccAddress != null)
            {
                foreach (string bccadd in bccAddress)
                {
                    if (!string.IsNullOrEmpty(bccadd))
                    {
                        mailMessage.Bcc.Add(bccadd);
                    }
                }
            }

            //Set additional options
            mailMessage.Priority = MailPriority.High;
            //Text/HTML
            mailMessage.IsBodyHtml = true;

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            //SmtpClient smtpClient = new SmtpClient("mailx.link3.net");

            SmtpClient smtpClient = new SmtpClient(_configuration.GetValue<string>("MailConfig:smtpserver"));

            //***********************//

            try
            {
                // Send the email
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await _cRRequestResponseService.CreateResponseRequest(null, ex, null, methodName, null, ex.Message.ToString());
            }
        }

        public async Task ExceptionSendMail(string exception, string subject)
        {
            string from = _configuration["ApplicationSettings:FromAddress"];

            string to = _configuration["ApplicationSettings:RndAddress"];
            List<string> toaddress = new List<string>();

            toaddress.Add(to);

            string body = "<p>Hi,<br/></p>";
            body = body + "<p>Error detail: </p>" + exception;

            SendMail(subject, body, from, toaddress, null, null);
        }

        public async Task SendMailWithNOC(string exception, string subject)
        {
            string from = _configuration["ApplicationSettings:FromAddress"];

            string cc = _configuration["ApplicationSettings:RndAddress"];
            string to = _configuration["ApplicationSettings:NOCAddress"];


            List<string> toaddress = new List<string>();
            toaddress.Add(to);
            List<string> ccaddress = new List<string>();
            ccaddress.Add(cc);

            string body = "<p>Hi,<br/></p>";
            body = body + "<p>Error detail: </p>" + exception;

            SendMail(subject, body, from, toaddress, ccaddress, null);
        }

        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = @$"Exception {DateTime.Now} {ex.Message}";
            _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }
}
