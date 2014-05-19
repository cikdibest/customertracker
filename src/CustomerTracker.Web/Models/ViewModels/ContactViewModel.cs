using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Models.ViewModels
{
    public class ContactViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public SubjectType? SubjectType { get; set; }

        public string Message { get; set; }

        public string FullName { get { return this.FirstName + " " + this.LastName; } }
    }
}