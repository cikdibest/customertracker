using System.Net.Mail;
using CustomerTracker.Web.Angular.Infrastructure.Tasks;
using System.Linq;
using System.Net;

namespace CustomerTracker.Web.Angular.Utilities
{
    public class MailSenderUtility : IMailSenderUtility
    {
        private readonly IAsyncTaskService _asyncTaskService;

        public MailSenderUtility()
        {
            _asyncTaskService = new AsyncTaskService();
        }

        public void SendEmailAsync(MailMessage message)
        {
            _asyncTaskService.AddSendMailTask(message);
        }

        public void SendEmail(MailMessage message)
        { 
            using (var smtpClient = new SmtpClient()
            {
                Host =ConfigurationHelper.SmtpHost,
                Port = ConfigurationHelper.SmtpPort,
                Credentials = new NetworkCredential(ConfigurationHelper.SmtpUserName, ConfigurationHelper.SmtpPassword),
                EnableSsl = true,
                Timeout = 10000,
            })
            {
                smtpClient.Send(message);
            }
        }
    }

    public interface IMailSenderUtility
    {
        void SendEmailAsync(MailMessage message);

        void SendEmail(MailMessage message);
    }
}