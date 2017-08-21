/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
	public class TextFormatterDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<TextFormatterData>
	{
		public const String TemplatePropertyName = "template";
		public TextFormatterDataManageabilityProvider()
		{
			TextFormatterDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			TextFormatterData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddEditTextPart(Resources.TextFormatterTemplatePartName,
				TemplatePropertyName,
				"",
				1024,
				true);
			contentBuilder.AddTextPart(Resources.TextFormatterEscapeInstructions_1);
			contentBuilder.AddTextPart(Resources.TextFormatterEscapeInstructions_2);
		}
        protected override string ElementPolicyNameTemplate
		{
			get
			{
				return Resources.FormatterPolicyNameTemplate;
			}
		}
        protected override void OverrideWithGroupPolicies(TextFormatterData configurationObject, IRegistryKey policyKey)
		{
			String templateOverride = UnescapeString(policyKey.GetStringValue(TemplatePropertyName));
			configurationObject.Template = templateOverride;
		}
        protected override void GenerateWmiObjects(TextFormatterData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			TextFormatterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
		public static string EscapeString(string text)
		{
			string result = text.Replace("\\", @"\\");
			result = result.Replace("\n", @"\n");
			result = result.Replace("\r", @"\r");
			return result;
		}
		public static string UnescapeString(string text)
		{
			string result = text.Replace(@"\r", "\r");
			result = result.Replace(@"\n", "\n");
			result = result.Replace(@"\\", "\\");
			return result;
		}
	}
}
