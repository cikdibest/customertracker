using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Business.SearchBusiness
{
    public class SearchResultModel
    {
        public int SearchTypeId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Summary { get; set; }
    }
}