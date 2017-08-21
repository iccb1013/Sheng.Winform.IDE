/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	public class WrapHandlerDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<WrapHandlerData>
	{
		public const String ExceptionMessagePropertyName = "exceptionMessage";
		public const String WrapExceptionTypePropertyName = "wrapExceptionType";
		public WrapHandlerDataManageabilityProvider()
		{
			WrapHandlerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			WrapHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			WrapHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(String.Format(CultureInfo.CurrentCulture,
													Resources.HandlerPartNameTemplate,
													configurationObject.Name));
			contentBuilder.AddEditTextPart(Resources.WrapHandlerExceptionMessagePartName,
				elementPolicyKeyName,
				ExceptionMessagePropertyName,
				configurationObject.ExceptionMessage,
				1024,
				true);
			contentBuilder.AddEditTextPart(Resources.WrapHandlerExceptionTypePartName,
				elementPolicyKeyName,
				WrapExceptionTypePropertyName,
				configurationObject.WrapExceptionType.AssemblyQualifiedName,
				1024,
				true);
		}
        protected override string ElementPolicyNameTemplate
		{
			get { return null; }
		}
        protected override void OverrideWithGroupPolicies(WrapHandlerData configurationObject, IRegistryKey policyKey)
		{
			String exceptionMessageOverride = policyKey.GetStringValue(ExceptionMessagePropertyName);
			Type wrapExceptionTypeOverride = policyKey.GetTypeValue(WrapExceptionTypePropertyName);
			configurationObject.ExceptionMessage = exceptionMessageOverride;
			configurationObject.WrapExceptionType = wrapExceptionTypeOverride;
		}
        protected override void GenerateWmiObjects(WrapHandlerData configurationObject, ICollection<ConfigurationSetting> wmiSettings)
		{
			WrapHandlerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
