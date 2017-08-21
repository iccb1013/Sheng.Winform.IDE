/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
	public class StorageEncryptionProviderCustomFactory 
		: AssemblerBasedCustomFactory<IStorageEncryptionProvider, StorageEncryptionProviderData>
	{
        public static StorageEncryptionProviderCustomFactory Instance = new StorageEncryptionProviderCustomFactory();
        protected override StorageEncryptionProviderData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			CachingConfigurationView view = new CachingConfigurationView(configurationSource);
			return view.GetStorageEncryptionProviderData(name);
		}
	}
}
