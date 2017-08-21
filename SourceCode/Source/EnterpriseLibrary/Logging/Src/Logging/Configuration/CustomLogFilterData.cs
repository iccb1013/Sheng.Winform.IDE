/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(CustomProviderAssembler<ILogFilter, LogFilterData, CustomLogFilterData>))]
	public class CustomLogFilterData
		: LogFilterData, IHelperAssistedCustomConfigurationData<CustomLogFilterData>
	{
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
		private readonly CustomProviderDataHelper<CustomLogFilterData> helper;
		public CustomLogFilterData()
		{
			helper = new CustomProviderDataHelper<CustomLogFilterData>(this);
		}
		public CustomLogFilterData(string name, Type type)
            :this(name, typeConverter.ConvertToString(type))
		{
		}
        public CustomLogFilterData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomLogFilterData>(this);
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
		CustomProviderDataHelper<CustomLogFilterData> IHelperAssistedCustomConfigurationData<CustomLogFilterData>.Helper
		{
			get { return helper; }
		}
		object IHelperAssistedCustomConfigurationData<CustomLogFilterData>.BaseGetPropertyValue(ConfigurationProperty property)
		{
			return base[property];
		}
		void IHelperAssistedCustomConfigurationData<CustomLogFilterData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
		{
			base[property] = value;
		}
		void IHelperAssistedCustomConfigurationData<CustomLogFilterData>.BaseUnmerge(ConfigurationElement sourceElement,
					ConfigurationElement parentElement,
					ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}
		void IHelperAssistedCustomConfigurationData<CustomLogFilterData>.BaseReset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}
		bool IHelperAssistedCustomConfigurationData<CustomLogFilterData>.BaseIsModified()
		{
			return base.IsModified();
		}
	}
}
