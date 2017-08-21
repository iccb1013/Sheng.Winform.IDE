/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	[ManagementEntity]
	public class IsolatedStorageCacheStorageSetting : CacheStorageSetting
	{
		private string partitionName;
		private string storageEncryption;
		public IsolatedStorageCacheStorageSetting(string name, string partitionName, string storageEncryption)
			: base(name)
		{
			this.partitionName = partitionName;
			this.storageEncryption = storageEncryption;
		}
		[ManagementProbe]
		public string PartitionName
		{
			get { return partitionName; }
			set { partitionName = value; }
		}
		[ManagementProbe]
		public string StorageEncryption
		{
			get { return storageEncryption; }
			set { storageEncryption = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<IsolatedStorageCacheStorageSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<IsolatedStorageCacheStorageSetting>();
		}
		[ManagementBind]
		public static IsolatedStorageCacheStorageSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<IsolatedStorageCacheStorageSetting>(ApplicationName, SectionName, Name);
		}
	}
}
