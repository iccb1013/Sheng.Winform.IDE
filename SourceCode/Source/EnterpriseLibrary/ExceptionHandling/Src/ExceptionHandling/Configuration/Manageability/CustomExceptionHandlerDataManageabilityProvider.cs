/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Properties;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	public class CustomHandlerDataManageabilityProvider
		: CustomProviderDataManageabilityProvider<CustomHandlerData>
	{
		public new const String ProviderTypePropertyName = CustomProviderDataManageabilityProvider<CustomHandlerData>.ProviderTypePropertyName;
		public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomHandlerData>.AttributesPropertyName;
		public CustomHandlerDataManageabilityProvider()
			: base("")
		{
			CustomExceptionHandlerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			CustomHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			CustomHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(String.Format(CultureInfo.CurrentCulture,
													Resources.HandlerPartNameTemplate,
													configurationObject.Name));
			contentBuilder.AddEditTextPart(Resources.CustomHandlerTypePartName,
				elementPolicyKeyName,
				ProviderTypePropertyName,
				configurationObject.Type.AssemblyQualifiedName,
				1024,
				true);
			contentBuilder.AddEditTextPart(Resources.CustomHandlerAttributesPartName,
				elementPolicyKeyName,
				AttributesPropertyName,
				GenerateAttributesString(configurationObject.Attributes),
				1024,
				false);
		}
        protected override void GenerateWmiObjects(CustomHandlerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomExceptionHandlerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
