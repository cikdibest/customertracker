using System.Web.Mvc;
using CustomerTracker.Web.Models.ViewModels;
using CustomerTracker.Web.Utilities; 

namespace CustomerTracker.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IMailSenderUtility _mailSenderUtility;
        private IMailBuilder _mailBuilder;

        public ContactController()
        {
            _mailSenderUtility = new MailSenderUtility();

            _mailBuilder=new MailBuilder();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactViewModel contactViewModel)
        {
            //var contactMailViewModel = new ContactMailViewModel()
            //{
            //    contactViewModel
            //};

            //var mailMessage = _mailBuilder.BuildMailMessageForContact(contactMailViewModel);

            //_mailSenderUtility.SendEmailAsync(mailMessage);

            return View();
        }
  
    }

}
