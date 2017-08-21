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
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
	[Assembler(typeof(CustomProviderAssembler<IExceptionHandler, ExceptionHandlerData, CustomHandlerData>))]
	[ContainerPolicyCreator(typeof(CustomProviderPolicyCreator<CustomHandlerData>))]
	public class CustomHandlerData
		: ExceptionHandlerData, IHelperAssistedCustomConfigurationData<CustomHandlerData>
	{
		private readonly CustomProviderDataHelper<CustomHandlerData> helper;
		public CustomHandlerData()
		{
			helper = new CustomProviderDataHelper<CustomHandlerData>(this);
		}
		public CustomHandlerData(string name, Type type)
		{
			helper = new CustomProviderDataHelper<CustomHandlerData>(this);
			Name = name;
			Type = type;
		}
        public CustomHandlerData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomHandlerData>(this);
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
		CustomProviderDataHelper<CustomHandlerData> IHelperAssistedCustomConfigurationData<CustomHandlerData>.Helper
		{
			get { return helper; }
		}
		object IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseGetPropertyValue(ConfigurationProperty property)
		{
			return base[property];
		}
		void IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
		{
			base[property] = value;
		}
		void IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseUnmerge(ConfigurationElement sourceElement,
					ConfigurationElement parentElement,
					ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}
		void IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseReset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}
		bool IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseIsModified()
		{
			return base.IsModified();
		}
	}
}
