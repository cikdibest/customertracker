using System.ComponentModel;

namespace CustomerTracker.Web.Models.Enums
{
    public enum SubjectType
    {
        [Description("İstek")]
        Request = 1,

        [Description("Öneri")]
        Suggestion = 2,

        [Description("Şikayet")]
        Complaint = 3,

        [Description("Bilgi Alma")]
        Debriefing = 4,

    }
}