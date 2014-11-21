using System;
using System.Linq;
using CustomerTracker.Web.Angular.Models;
using CustomerTracker.Web.Angular.Utilities;

namespace CustomerTracker.Web.Angular.Infrastructure.Services.DistributedCacheService
{
    public class CachingService
    {
        public static void StartCaching()
        {
            //using (Infrastructure.Repository.IUnitOfWork unitOfWork = new Repository.UnitOfWork())
            //{ 
            //    var pageViews = unitOfWork.GetRepository<PageView>().SelectAll().ToList();

            //    SingletonDistributedCacheService.DistributedCacheService.AddEntry("AllPageViews", pageViews);


             
            //}
        }
    }
}