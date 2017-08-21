/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	public class CacheManagerFactory : LocatorNameTypeFactoryBase<ICacheManager>
	{
		protected CacheManagerFactory()
			: base()
		{
		}
		public CacheManagerFactory(IConfigurationSource configurationSource)
			: base(configurationSource)
		{ }
	}
}
