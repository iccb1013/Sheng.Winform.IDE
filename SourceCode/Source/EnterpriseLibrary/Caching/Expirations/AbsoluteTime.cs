/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    [Serializable]    
    public class AbsoluteTime : ICacheItemExpiration
    {
        private DateTime absoluteExpirationTime;        
        public AbsoluteTime(DateTime absoluteTime)
        {
            if (absoluteTime > DateTime.Now)
            {
                this.absoluteExpirationTime = absoluteTime.ToUniversalTime();
            }
            else
            {
                throw new ArgumentOutOfRangeException("absoluteTime", Resources.ExceptionRangeAbsoluteTime);
            }
        }
		public DateTime AbsoluteExpirationTime
		{
			get { return absoluteExpirationTime; }
		}
        public AbsoluteTime(TimeSpan timeFromNow) : this(DateTime.Now + timeFromNow)
        {
        }
        public bool HasExpired() 
        {
            DateTime nowDateTime = DateTime.Now.ToUniversalTime();
            return nowDateTime.Ticks >= this.absoluteExpirationTime.Ticks;
        }
        public void Notify()
        {
        }
        public void Initialize(CacheItem owningCacheItem)
        {
        }        
    }
}
