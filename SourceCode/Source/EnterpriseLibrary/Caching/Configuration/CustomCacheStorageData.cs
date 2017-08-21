/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	[Assembler(typeof(CustomProviderAssembler<IBackingStore, CacheStorageData, CustomCacheStorageData>))]
	[ContainerPolicyCreator(typeof(CustomProviderPolicyCreator<CustomCacheStorageData>))]
	public class CustomCacheStorageData
		: CacheStorageData, IHelperAssistedCustomConfigurationData<CustomCacheStorageData>
	{
		private readonly CustomProviderDataHelper<CustomCacheStorageData> helper;
		public CustomCacheStorageData()
		{
			helper = new CustomProviderDataHelper<CustomCacheStorageData>(this);
		}
		public CustomCacheStorageData(string name, Type type)
		{
			helper = new CustomProviderDataHelper<CustomCacheStorageData>(this);
			Name = name;
			Type = type;
		}
        public CustomCacheStorageData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomCacheStorageData>(this);
            Name = name;
            TypeName = typeName;
        }
		public void SetAttributeValue(string key, string value)
		{
			helper.HandleSetAttributeValue(key, value);
		}
		public NameValueCollection Attributes
		{
			get { return helper.Attributes; }
		}
		protected override ConfigurationPropertyCollection Properties
		{
			get { return helper.Properties; }
		}
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			helper.HandleUnmerge(sourceElement, parentElement, saveMode);
		}
		protected override void Reset(ConfigurationElement parentElement)
		{
			helper.HandleReset(parentElement);
		}
		protected override bool IsModified()
		{
			return helper.HandleIsModified();
		}
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			return helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
		}
		CustomProviderDataHelper<CustomCacheStorageData> IHelperAssistedCustomConfigurationData<CustomCacheStorageData>.Helper
		{
			get { return helper; }
		}
		object IHelperAssistedCustomConfigurationData<CustomCacheStorageData>.BaseGetPropertyValue(ConfigurationProperty property)
		{
			return base[property];
		}
		void IHelperAssistedCustomConfigurationData<CustomCacheStorageData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
		{
			base[property] = value;
		}
		void IHelperAssistedCustomConfigurationData<CustomCacheStorageData>.BaseUnmerge(ConfigurationElement sourceElement,
					ConfigurationElement parentElement,
					ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}
		void IHelperAssistedCustomConfigurationData<CustomCacheStorageData>.BaseReset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}
		bool IHelperAssistedCustomConfigurationData<CustomCacheStorageData>.BaseIsModified()
		{
			return base.IsModified();
		}
	}
}
