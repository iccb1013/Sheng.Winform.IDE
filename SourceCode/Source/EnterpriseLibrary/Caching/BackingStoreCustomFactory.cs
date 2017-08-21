/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	public class BackingStoreCustomFactory : AssemblerBasedCustomFactory<IBackingStore, CacheStorageData>
	{
		public static BackingStoreCustomFactory Instance = new BackingStoreCustomFactory();
		protected override CacheStorageData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			CachingConfigurationView view = new CachingConfigurationView(configurationSource);
			return view.GetCacheStorageData(name);
		}
	}
}
