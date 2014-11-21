namespace CustomerTracker.Web.Angular.Infrastructure.Services.DistributedCacheService
{
    public static class SingletonDistributedCacheService
    {
        private static DistributedCacheService _distributedCacheService;

        static SingletonDistributedCacheService()
        {
            _distributedCacheService = new DistributedCacheService(new DateTimeProvider());
        }

       public static DistributedCacheService DistributedCacheService
        {
            get { return _distributedCacheService; }
        }

    }
}