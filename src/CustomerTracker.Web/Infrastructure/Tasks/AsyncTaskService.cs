using System.Net.Mail;

namespace CustomerTracker.Web.Infrastructure.Tasks
{
    public class AsyncTaskService : IAsyncTaskService
    {
        public void AddSendMailTask(MailMessage mail)
        {
            TaskExecutor.ExcuteLater(new SendEmailTask(mail));
        }

        
    }

    public interface IAsyncTaskService
    {
        void AddSendMailTask(MailMessage mail);
    }
}