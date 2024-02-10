using L3T.Infrastructure.Helpers.Implementation.FieldForce;
using L3T.Infrastructure.Helpers.Interface.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Reflection;

namespace L3T.Infrastructure.Helpers.Implementation.Email
{
    public class MailSenderService : IMailSenderService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<FieldForceService> _logger;

        public MailSenderService(IConfiguration configuration, ILogger<FieldForceService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SingleSendMail(string subject, string body, string from, string toaddress)
        {
            var methodName = "MailSenderService/SingleSendMail";
            List<string> emailaddress = new List<string>();

            emailaddress.Add(toaddress);

            try
            {
                SendMail(subject, body, from, emailaddress, null, null);
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }
        public async Task SendMail(string subject, string body, string fromAddress, List<string> toAddress, List<string> ccAddress, List<string> bccAddress, string displayName = "")
        {
            var methodName = "MailSenderService/SendMail";
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(fromAddress, displayName);
            foreach (string toadd in toAddress)
            {
                if (toadd.Length > 0)
                {
                    mailMessage.To.Add(new MailAddress(toadd));
                }
            }

            if (ccAddress != null)
            {
                foreach (string ccadd in ccAddress)
                {
                    if (ccadd.Length > 0)
                    {
                        mailMessage.CC.Add(ccadd);
                    }
                }
            }

            if (bccAddress != null)
            {
                foreach (string bccadd in bccAddress)
                {
                    if (bccadd.Length > 0)
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

            SmtpClient smtpClient = new SmtpClient("mailx.link3.net");

            //***********************//

            try
            {
                // Send the email
                //smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                throw new Exception(ex.ToString());
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
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }
}
