using System.ComponentModel;

namespace CustomerTracker.Web.Models.Enums
{
    public enum RemoteConnectionType
    {
        [Description("Teamviewer")]
        TeamViewer = 1,

        [Description("Remote Desktop")]
        RemoteDesktop = 2,

        [Description("Ammyy")]
        Ammyy = 3,

        [Description("Alpemix")]
        Alpemix = 4,

        [Description("Vpn")]
        Vpn = 5,

        [Description("File Server")]
        FileServer = 6,

        [Description("Sql server")]
        SqlServer = 7,

        [Description("Other")]
        Other = 8,
    };
}