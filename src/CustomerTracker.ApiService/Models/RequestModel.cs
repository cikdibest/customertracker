using System.Net;

namespace CustomerTracker.ApiService.Models
{
    public class RequestModel
    {
        public string RequestUrl { get; set; }
        public string RequestDate { get; set; }
        public string ResponseStatusCode { get; set; }
    }
}