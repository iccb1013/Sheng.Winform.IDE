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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(CustomProviderAssembler<ILogFormatter, FormatterData, CustomFormatterData>))]
	[ContainerPolicyCreator(typeof(CustomProviderPolicyCreator<CustomFormatterData>))]
	public class CustomFormatterData
		: FormatterData, IHelperAssistedCustomConfigurationData<CustomFormatterData>
	{
		private readonly CustomProviderDataHelper<CustomFormatterData> helper;
		public CustomFormatterData()
		{
			helper = new CustomProviderDataHelper<CustomFormatterData>(this);
		}
		public CustomFormatterData(string name, Type type)
		{
			helper = new CustomProviderDataHelper<CustomFormatterData>(this);
			Name = name;
			Type = type;
		}
        public CustomFormatterData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomFormatterData>(this);
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
		CustomProviderDataHelper<CustomFormatterData> IHelperAssistedCustomConfigurationData<CustomFormatterData>.Helper
		{
			get { return helper; }
		}
		object IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseGetPropertyValue(ConfigurationProperty property)
		{
			return base[property];
		}
		void IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
		{
			base[property] = value;
		}
		void IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseUnmerge(ConfigurationElement sourceElement,
					ConfigurationElement parentElement,
					ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}
		void IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseReset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}
		bool IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseIsModified()
		{
			return base.IsModified();
		}
	}
}
