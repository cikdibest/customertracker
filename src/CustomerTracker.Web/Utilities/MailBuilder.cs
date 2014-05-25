using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using CustomerTracker.Web.Models.ViewModels;

namespace CustomerTracker.Web.Utilities
{
    public class MailBuilder : IMailBuilder
    {
        private MailMessage BuildMailMessage(MailViewModel mailViewModel)
        {
            var plainView = AlternateView.CreateAlternateViewFromString(mailViewModel.Body, new ContentType("text/html; charset=UTF-8"));

            plainView.TransferEncoding = TransferEncoding.SevenBit;

            var retMessage = new MailMessage
            {
                From = new MailAddress(mailViewModel.FromMail),
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = null, //HACK: clear the default view, we will use the alternate one ! 
                Subject = mailViewModel.Subject,
            };

            retMessage.AlternateViews.Add(plainView);

            retMessage.To.Add(new MailAddress(mailViewModel.ToMail));

            return retMessage;
        }

     
        public MailMessage BuildMailMessageForSendToUserAfterRegistration(SendToUserAfterRegistrationMailViewModel sendToUserAfterRegistrationMailViewModel)
        {

            string subject = "Ankaref müşteri takip üyeliğiniz oluşturuldu";

            string body = string.Format("Sn. {0} <br/> " +
                                        "Göstermiş olduğunuz ilgi ve güvenden dolayı teşekkür ederiz!<br/>" +
                                        "<table><tr><td>Kullanıcı adınız</td><td>:</td><td>{1}</td></tr> <tr><td>Şifreniz</td><td>:</td><td>{2}</td></tr><table>", sendToUserAfterRegistrationMailViewModel.FullName, sendToUserAfterRegistrationMailViewModel.Username, sendToUserAfterRegistrationMailViewModel.Password);

            var mailViewModel = new MailViewModel()
            {
                FromMail = ConfigurationHelper.FromMailAddressForUserRegistration,
                Subject = subject,
                ToMail = sendToUserAfterRegistrationMailViewModel.UserMailAdress,
                Body = body,
            };

            return BuildMailMessage(mailViewModel);
        }
         
    }

    public interface IMailBuilder
    { 
        MailMessage BuildMailMessageForSendToUserAfterRegistration(SendToUserAfterRegistrationMailViewModel sendToUserAfterRegistrationMailViewModel);
    }
}

