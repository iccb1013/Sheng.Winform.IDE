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
	public class ReplaceHandlerDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<ReplaceHandlerData>
	{
		public const String ExceptionMessagePropertyName = "exceptionMessage";
		public const String ReplaceExceptionTypePropertyName = "replaceExceptionType";
		public ReplaceHandlerDataManageabilityProvider()
		{
			ReplaceHandlerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ReplaceHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			ReplaceHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(String.Format(CultureInfo.CurrentCulture,
													Resources.HandlerPartNameTemplate,
													configurationObject.Name));
			contentBuilder.AddEditTextPart(Resources.ReplaceHandlerExceptionMessagePartName,
				elementPolicyKeyName,
				ExceptionMessagePropertyName,
				configurationObject.ExceptionMessage,
				1024,
				true);
			contentBuilder.AddEditTextPart(Resources.ReplaceHandlerExceptionTypePartName,
				elementPolicyKeyName,
				ReplaceExceptionTypePropertyName,
				configurationObject.ReplaceExceptionType.AssemblyQualifiedName,
				1024,
				true);
		}
        protected override string ElementPolicyNameTemplate
		{
			get { return null; }
		}
        protected override void OverrideWithGroupPolicies(ReplaceHandlerData configurationObject, IRegistryKey policyKey)
		{
			String exceptionMessageOverride = policyKey.GetStringValue(ExceptionMessagePropertyName);
			Type replaceExceptionTypeOverride = policyKey.GetTypeValue(ReplaceExceptionTypePropertyName);
			configurationObject.ExceptionMessage = exceptionMessageOverride;
			configurationObject.ReplaceExceptionType = replaceExceptionTypeOverride;
		}
        protected override void GenerateWmiObjects(ReplaceHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			ReplaceHandlerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
