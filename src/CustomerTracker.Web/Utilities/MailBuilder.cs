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

        public MailMessage BuildMailMessageForContact(ContactMailViewModel contactMailViewModel)
        {
            string subject = "Takıtasarım - " + contactMailViewModel.SubjectTypeDescription;

            string body = string.Format("{0} mail adresinden bir {1} isteği aldınız.<br/><hr/>{2}", contactMailViewModel.FromMail, contactMailViewModel.SubjectTypeDescription, contactMailViewModel.Message);

            var mailViewModel = new MailViewModel()
            {
                FromMail = ConfigurationHelper.FromMailAddressForContactRequest,
                Subject = subject,
                ToMail = ConfigurationHelper.ToMailAddressForContactRequest,
                Body = body,
            };

            return BuildMailMessage(mailViewModel);
        }


        public MailMessage BuildMailMessageForSendToUserAfterRegistration(SendToUserAfterRegistrationMailViewModel sendToUserAfterRegistrationMailViewModel)
        {

            string subject = "Takitasarim.info üyeliğiniz oluşturuldu";

            string body = string.Format("Sn. {0} <br/> " +
                                        "Takitasarim.info ' a göstermiş olduğunuz ilgi ve güvenden dolayı teşekkür ederiz!<br/>" +
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
        MailMessage BuildMailMessageForContact(ContactMailViewModel contactMailViewModel);


        MailMessage BuildMailMessageForSendToUserAfterRegistration(SendToUserAfterRegistrationMailViewModel sendToUserAfterRegistrationMailViewModel);
    }
}

