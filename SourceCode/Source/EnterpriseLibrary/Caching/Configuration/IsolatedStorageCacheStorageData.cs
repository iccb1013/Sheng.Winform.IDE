/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	[Assembler(typeof(IsolatedStorageBackingStoreAssembler))]
	[ContainerPolicyCreator(typeof(IsolatedStorageBackingStorePolicyCreator))]
	public class IsolatedStorageCacheStorageData : CacheStorageData
    {
		private const string partitionNameProperty = "partitionName";
        public IsolatedStorageCacheStorageData() 
        {
        }        
        public IsolatedStorageCacheStorageData(string name, string storageEncryption, string partitionName) : base(name, typeof(IsolatedStorageBackingStore), storageEncryption)
        {
            this.PartitionName = partitionName;
        }
        [ConfigurationProperty(partitionNameProperty, IsRequired= true)]
        public string PartitionName
        {
            get { return (string)base[partitionNameProperty]; }
            set { base[partitionNameProperty] = value; }
        }
    }
    public class IsolatedStorageBackingStoreAssembler : IAssembler<IBackingStore, CacheStorageData>
	{
        public IBackingStore Assemble(IBuilderContext context, CacheStorageData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			IsolatedStorageCacheStorageData castedObjectConfiguration
				= (IsolatedStorageCacheStorageData)objectConfiguration;
			IStorageEncryptionProvider encryptionProvider
				= GetStorageEncryptionProvider(context, castedObjectConfiguration.StorageEncryption, configurationSource, reflectionCache);
			IBackingStore createdObject
				= new IsolatedStorageBackingStore(
					castedObjectConfiguration.PartitionName,
					encryptionProvider);
			return createdObject;
		}
		private IStorageEncryptionProvider GetStorageEncryptionProvider(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return StorageEncryptionProviderCustomFactory.Instance.Create(context, name, configurationSource, reflectionCache);
			}
			return null;
		}
	}
}
