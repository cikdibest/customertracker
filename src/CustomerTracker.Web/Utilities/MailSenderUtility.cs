using System.Net.Mail;
using CustomerTracker.Web.Infrastructure.Tasks;
using CustomerTracker.Web.Models.Entities;
using System.Linq;
using System.Net;

namespace CustomerTracker.Web.Utilities
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
            //var smtpSetting = (SmtpSetting)Infrastructure.Services.DistributedCacheService.SingletonDistributedCacheService.DistributedCacheService.GetEntry("SmtpSetting");

            //using (var smtpClient = new SmtpClient()
            //{
            //    Host = smtpSetting.Host,
            //    Port = smtpSetting.Port,
            //    Credentials = new NetworkCredential(smtpSetting.UserName, smtpSetting.Password)
            //})
            //{
            //    smtpClient.Send(message);
            //}
        }
    }

    public interface IMailSenderUtility
    {
        void SendEmailAsync(MailMessage message);

        void SendEmail(MailMessage message);
    }
}