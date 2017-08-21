/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity
{
	public class IsolatedStorageBackingStorePolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			IsolatedStorageCacheStorageData castConfigurationObject = (IsolatedStorageCacheStorageData)configurationObject;
			new PolicyBuilder<IsolatedStorageBackingStore, IsolatedStorageCacheStorageData>(
					instanceName,
					castConfigurationObject,
					c => new IsolatedStorageBackingStore(
						c.PartitionName,
						Resolve.OptionalReference<IStorageEncryptionProvider>(c.StorageEncryption)))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
