/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    [Serializable]    
    public class SlidingTime : ICacheItemExpiration
    {
        private DateTime timeLastUsed;
        private TimeSpan itemSlidingExpiration;
        public SlidingTime(TimeSpan slidingExpiration)
        {
            if (!(slidingExpiration.TotalSeconds >= 1))
            {
                throw new ArgumentOutOfRangeException("slidingExpiration",
                                                      Resources.ExceptionRangeSlidingExpiration);
            }
            this.itemSlidingExpiration = slidingExpiration;
        }
        public SlidingTime(TimeSpan slidingExpiration, DateTime originalTimeStamp) : this(slidingExpiration)
        {
            timeLastUsed = originalTimeStamp;
        }        
        public TimeSpan ItemSlidingExpiration
        {
            get { return itemSlidingExpiration; }
        }
        public DateTime TimeLastUsed
        {
            get { return timeLastUsed; }
        }
        public bool HasExpired()
        {
            bool expired = CheckSlidingExpiration(DateTime.Now,
                                                  this.timeLastUsed,
                                                  this.itemSlidingExpiration);
            return expired;
        }
        public void Notify()
        {
            this.timeLastUsed = DateTime.Now;
        }
        public void Initialize(CacheItem owningCacheItem)
        {
            timeLastUsed = owningCacheItem.LastAccessedTime;
        }
        private static bool CheckSlidingExpiration(DateTime nowDateTime,
                                                   DateTime lastUsed,
                                                   TimeSpan slidingExpiration)
        {
            DateTime tmpNowDateTime = nowDateTime.ToUniversalTime();
            DateTime tmpLastUsed = lastUsed.ToUniversalTime();
            long expirationTicks = tmpLastUsed.Ticks + slidingExpiration.Ticks;
            bool expired = (tmpNowDateTime.Ticks >= expirationTicks) ? true : false;
            return expired;
        }
    }
}
