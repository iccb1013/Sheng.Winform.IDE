/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability
{
	public class FaultContractExceptionHandlerDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<FaultContractExceptionHandlerData>
	{
		public const String AttributesPropertyName = "attributes";
		public const String ExceptionMessagePropertyName = "exceptionMessage";
		public const String FaultContractTypePropertyName = "faultContractType";
		public FaultContractExceptionHandlerDataManageabilityProvider()
		{
			FaultContractExceptionHandlerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			FaultContractExceptionHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			FaultContractExceptionHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(String.Format(CultureInfo.CurrentCulture,
													Resources.HandlerPartNameTemplate,
													configurationObject.Name));
			contentBuilder.AddEditTextPart(Resources.FaultContractExceptionHandlerExceptionMessagePartName,
				elementPolicyKeyName,
				ExceptionMessagePropertyName,
				configurationObject.ExceptionMessage,
				512,
				true);
			contentBuilder.AddEditTextPart(Resources.FaultContractExceptionHandlerFaultContractTypePartName,
				elementPolicyKeyName,
				FaultContractTypePropertyName,
				configurationObject.FaultContractType,
				512,
				true);
			contentBuilder.AddEditTextPart(Resources.FaultContractExceptionHandlerAttributesPartName,
				elementPolicyKeyName,
				AttributesPropertyName,
				GenerateAttributesString(configurationObject.Attributes),
				1024,
				false);
		}
        protected override string ElementPolicyNameTemplate
		{
			get { return null; }
		}
        protected override void OverrideWithGroupPolicies(FaultContractExceptionHandlerData configurationObject, IRegistryKey policyKey)
		{
			String attributesOverride = policyKey.GetStringValue(AttributesPropertyName);
			String exceptionMessageOverride = policyKey.GetStringValue(ExceptionMessagePropertyName);
			String faultContractTypeOverride = policyKey.GetStringValue(FaultContractTypePropertyName);
			configurationObject.ExceptionMessage = exceptionMessageOverride;
			configurationObject.FaultContractType = faultContractTypeOverride;
			configurationObject.PropertyMappings.Clear();
			Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
			KeyValuePairParser.ExtractKeyValueEntries(attributesOverride, attributesDictionary);
			foreach (KeyValuePair<String, String> kvp in attributesDictionary)
			{
				configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData(kvp.Key, kvp.Value));
			}
		}
		private static String GenerateAttributesString(NameValueCollection attributes)
		{
			KeyValuePairEncoder encoder = new KeyValuePairEncoder();
			foreach (String key in attributes.AllKeys)
			{
				encoder.AppendKeyValuePair(key, attributes[key]);
			}
			return encoder.GetEncodedKeyValuePairs();
		}
        protected override void GenerateWmiObjects(FaultContractExceptionHandlerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			FaultContractExceptionHandlerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
