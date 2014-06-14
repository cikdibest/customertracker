using System.Collections.Generic;
using CustomerTracker.ApiService.Models;

namespace CustomerTracker.ApiService
{
    public class CacheData
    {
        private static CacheData _instance;

        public static CacheData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CacheData();

                    _instance.HttpRequestMessages=new List<RequestModel>();
                }
                 
                return _instance;
            }
        }

        public List<RequestModel> HttpRequestMessages;


    }
}