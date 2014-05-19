namespace CustomerTracker.Web.Models.ViewModels
{
    public class SocialUserRegisterModel : RegisterModel
    {
        public string Provider { get; set; }

        public string ProviderUserId { get; set; }
    }
}