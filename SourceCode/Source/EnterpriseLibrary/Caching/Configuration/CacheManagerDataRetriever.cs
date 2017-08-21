/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	public class CacheManagerDataRetriever : IConfigurationNameMapper
	{
		public string MapName(string name, IConfigurationSource configurationSource)
		{
			if (name == null)
			{
				CachingConfigurationView view = new CachingConfigurationView(configurationSource);
				return view.DefaultCacheManager;
			}
			return name;
		}
	}
}
