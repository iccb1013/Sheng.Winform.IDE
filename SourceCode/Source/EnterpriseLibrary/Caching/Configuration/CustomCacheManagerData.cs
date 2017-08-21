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
	[Assembler(typeof(CustomProviderAssembler<ICacheManager, CacheManagerDataBase, CustomCacheManagerData>))]
	[ContainerPolicyCreator(typeof(CustomProviderPolicyCreator<CustomCacheManagerData>))]
	public class CustomCacheManagerData
		: CacheManagerDataBase, IHelperAssistedCustomConfigurationData<CustomCacheManagerData>
	{
		private readonly CustomProviderDataHelper<CustomCacheManagerData> helper;
		public CustomCacheManagerData()
		{
			helper = new CustomProviderDataHelper<CustomCacheManagerData>(this);
		}
		public CustomCacheManagerData(string name, Type type)
		{
			helper = new CustomProviderDataHelper<CustomCacheManagerData>(this);
			Name = name;
			Type = type;
		}
		public CustomCacheManagerData(string name, string typeName)
		{
			helper = new CustomProviderDataHelper<CustomCacheManagerData>(this);
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
		CustomProviderDataHelper<CustomCacheManagerData> IHelperAssistedCustomConfigurationData<CustomCacheManagerData>.Helper
		{
			get { return helper; }
		}
		object IHelperAssistedCustomConfigurationData<CustomCacheManagerData>.BaseGetPropertyValue(ConfigurationProperty property)
		{
			return base[property];
		}
		void IHelperAssistedCustomConfigurationData<CustomCacheManagerData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
		{
			base[property] = value;
		}
		void IHelperAssistedCustomConfigurationData<CustomCacheManagerData>.BaseUnmerge(ConfigurationElement sourceElement,
					ConfigurationElement parentElement,
					ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}
		void IHelperAssistedCustomConfigurationData<CustomCacheManagerData>.BaseReset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}
		bool IHelperAssistedCustomConfigurationData<CustomCacheManagerData>.BaseIsModified()
		{
			return base.IsModified();
		}
	}
}
