/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	public static class CacheFactory
	{
		private static CacheManagerFactory factory = new CacheManagerFactory(ConfigurationSourceFactory.Create());
		private static object lockObject = new object();
		public static ICacheManager GetCacheManager()
		{
			try
			{
				lock (lockObject)
				{
					return factory.CreateDefault();
				}
			}
			catch (ConfigurationErrorsException configurationException)
			{
				TryLogConfigurationError(configurationException, "default");
				throw;
			}
		}
		public static ICacheManager GetCacheManager(string cacheManagerName)
		{
			try
			{
				lock (lockObject)
				{
					return factory.Create(cacheManagerName);
				}
			}
			catch (ConfigurationErrorsException configurationException)
			{
				TryLogConfigurationError(configurationException, cacheManagerName);
				throw;
			}
		}
		private static void TryLogConfigurationError(ConfigurationErrorsException configurationException, string instanceName)
		{
			try
			{
				DefaultCachingEventLogger eventLogger = EnterpriseLibraryFactory.BuildUp<DefaultCachingEventLogger>();
				if (eventLogger != null)
				{
					eventLogger.LogConfigurationError(instanceName, configurationException);
				}
			}
			catch { }
		}
	}
}
