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
    [ComVisible(false)]
    public class ExtendedFormatTime : ICacheItemExpiration
    {
        private string extendedFormat;
        private DateTime lastUsedTime;       
        public ExtendedFormatTime(string timeFormat)
        {
            if (string.IsNullOrEmpty(timeFormat))
            {
				throw new ArgumentException(Resources.ExceptionNullTimeFormat, "timeFormat");
            }
            ExtendedFormat.Validate(timeFormat);
            this.extendedFormat = timeFormat;
            this.lastUsedTime = DateTime.Now.ToUniversalTime();
        }       
		public string TimeFormat
		{
			get { return extendedFormat; }
		}
        public bool HasExpired()
        {
            DateTime nowDateTime = DateTime.Now.ToUniversalTime();
            ExtendedFormat format = new ExtendedFormat(extendedFormat);
            if (format.IsExpired(lastUsedTime, nowDateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Notify()
        {
        }
        public void Initialize(CacheItem owningCacheItem)
        {
        } 
    }
}
