using System.Linq;
using System.Net;
using System.Net.Mail;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Infrastructure.Tasks
{
    public class SendEmailTask : BackgroundTask
    {
        private readonly MailMessage _mailMessage;

        IMailSenderUtility _mailSenderUtility;

        public SendEmailTask(MailMessage mailMessage)
        {
            _mailMessage = mailMessage;

            _mailSenderUtility = new MailSenderUtility();
        }

        protected override void Execute()
        {
            _mailSenderUtility.SendEmail(_mailMessage); 
        }

        protected override void OnError(System.Exception e)
        {
            throw e;
        }
    }

     
}