/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	[Assembler(typeof(TypeInstantiationAssembler<IBackingStore, CacheStorageData>))]
	public class CacheStorageData : NameTypeConfigurationElement
	{
		private const string encryptionProviderNameProperty = "encryptionProviderName";
		public CacheStorageData()
		{
		}
		public CacheStorageData(string name, Type type)
			: this(name, type, string.Empty)
		{
		}
		public CacheStorageData(string name, Type type, string storageEncryption)
			: base(name, type)
		{
			this.StorageEncryption = storageEncryption;
		}
		[ConfigurationProperty(encryptionProviderNameProperty, IsRequired = false)]
		public string StorageEncryption
		{
			get { return (string)base[encryptionProviderNameProperty]; }
			set { base[encryptionProviderNameProperty] = value; }
		}
	}
}
