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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
	public class CategoryFilterDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<CategoryFilterData>
	{
		public const String CategoryFilterModePropertyName = "categoryFilterMode";
		public const String CategoryFiltersKeyName = "categoryFilters";
		public CategoryFilterDataManageabilityProvider()
		{
			CategoryFilterDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			CategoryFilterData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddDropDownListPartForEnumeration<CategoryFilterMode>(Resources.CategoryFilterFilterModePartName,
				CategoryFilterModePropertyName,
				configurationObject.CategoryFilterMode);
			contentBuilder.AddTextPart(Resources.CategoryFilterCategoriesPartName);
			LoggingSettings configurationSection
				= configurationSource.GetSection(LoggingSettings.SectionName) as LoggingSettings;
			String logFilterCategoriesKeyName
				= elementPolicyKeyName + @"\" + CategoryFiltersKeyName;
			foreach (TraceSourceData category in configurationSection.TraceSources)
			{
				contentBuilder.AddCheckboxPart(category.Name,
					logFilterCategoriesKeyName,
					category.Name,
					configurationObject.CategoryFilters.Contains(category.Name),
					true,
					false);
			}
		}
        protected override string ElementPolicyNameTemplate
		{
			get
			{
				return Resources.FilterPolicyNameTemplate;
			}
		}
        protected override void OverrideWithGroupPolicies(CategoryFilterData configurationObject, IRegistryKey policyKey)
		{
			CategoryFilterMode? categoryFilterModelOverride = policyKey.GetEnumValue<CategoryFilterMode>(CategoryFilterModePropertyName);
			configurationObject.CategoryFilters.Clear();
			using (IRegistryKey categoryFiltersOverrideKey = policyKey.OpenSubKey(CategoryFiltersKeyName))
			{
				if (categoryFiltersOverrideKey != null)
				{
					foreach (String valueName in categoryFiltersOverrideKey.GetValueNames())
					{
						configurationObject.CategoryFilters.Add(new CategoryFilterEntry(valueName));
					}
				}
			}
			configurationObject.CategoryFilterMode = categoryFilterModelOverride.Value;
		}
        protected override void GenerateWmiObjects(CategoryFilterData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CategoryFilterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
