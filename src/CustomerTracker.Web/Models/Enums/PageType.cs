using System.ComponentModel;

namespace CustomerTracker.Web.Models.Enums
{
    public enum PageType
    {
        [Description("Anasayfa")]
        MainPage = 1,
         
        [Description("Hakkımızda")]
        About = 2,

        [Description("Garanti Şartları")]
        GuaranteeCondition = 3,

        [Description("Kurumsal Destek")]
        InstitutionalSupport = 4,

        [Description("Teknik Servis")]
        TechnicalService = 5,

        [Description("İletişim")]
        Contact = 6,

        [Description("Yazılım Hizmeti")]
        SoftwareSupport=7
    }
}