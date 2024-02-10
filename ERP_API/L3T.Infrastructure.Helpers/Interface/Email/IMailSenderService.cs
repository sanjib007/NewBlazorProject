using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.Email
{
    public interface IMailSenderService
    {
        Task SingleSendMail(string subject, string body, string from, string toaddress);
        Task SendMail(string subject, string body, string fromAddress, List<string> toAddress, List<string> ccAddress, List<string> bccAddress, string displayName = "");
        Task ExceptionSendMail(string exception, string subject);
        Task SendMailWithNOC(string exception, string subject);
    }
}
