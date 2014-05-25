using System.ComponentModel;

namespace CustomerTracker.Web.Models.Enums
{
    public enum RemoteConnectionType
    {
        [Description("Teamviewer")]
        TeamViewer = 1,

        [Description("Uzak Masaüstü Bağlantısı")]
        RemoteDesktop = 2,

        [Description("Ammyy")]
        Ammyy = 3,

        [Description("Alpemix")]
        Alpemix = 4,

        [Description("Vpn")]
        Vpn = 5
    };
}