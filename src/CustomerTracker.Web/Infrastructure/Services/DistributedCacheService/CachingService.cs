using System;
using System.Linq;
using CustomerTracker.Web.Models;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Infrastructure.Services.DistributedCacheService
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