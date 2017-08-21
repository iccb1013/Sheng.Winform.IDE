/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
	public class FormattedDatabaseTraceListenerDataManageabilityProvider
		: TraceListenerDataManageabilityProvider<FormattedDatabaseTraceListenerData>
	{
		public const String AddCategoryStoredProcNamePropertyName = "addCategoryStoredProcName";
		public const String DatabaseInstanceNamePropertyName = "databaseInstanceName";
		public const String WriteLogStoredProcNamePropertyName = "writeLogStoredProcName";
        public new const String FormatterPropertyName =
			TraceListenerDataManageabilityProvider<FormattedDatabaseTraceListenerData>.FormatterPropertyName;
        public new const String TraceOutputOptionsPropertyName =
			TraceListenerDataManageabilityProvider<FormattedDatabaseTraceListenerData>.TraceOutputOptionsPropertyName;
		public FormattedDatabaseTraceListenerDataManageabilityProvider()
		{
			FormattedDatabaseTraceListenerDataWmiMapper.RegisterWmiTypes();
		}
	    protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			FormattedDatabaseTraceListenerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			List<AdmDropDownListItem> connectionStrings = new List<AdmDropDownListItem>();
			ConnectionStringsSection connectionStringsSection
				= (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
			if (connectionStringsSection != null)
			{
				foreach (ConnectionStringSettings connectionString in connectionStringsSection.ConnectionStrings)
				{
					connectionStrings.Add(new AdmDropDownListItem(connectionString.Name, connectionString.Name));
				}
			}
			contentBuilder.AddDropDownListPart(Resources.DatabaseTraceListenerDatabasePartName,
				DatabaseInstanceNamePropertyName,
				connectionStrings,
				configurationObject.DatabaseInstanceName);
			contentBuilder.AddEditTextPart(Resources.DatabaseTraceListenerWriteStoreProcPartName,
				WriteLogStoredProcNamePropertyName,
				configurationObject.WriteLogStoredProcName,
				512,
				true);
			contentBuilder.AddEditTextPart(Resources.DatabaseTraceListenerAddCategoryStoreProcPartName,
				AddCategoryStoredProcNamePropertyName,
				configurationObject.AddCategoryStoredProcName,
				512,
				false);
			AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);
			AddFilterPart(contentBuilder, configurationObject.Filter);
			AddFormattersPart(contentBuilder, configurationObject.Formatter, configurationSource);
		}
	    protected override void OverrideWithGroupPolicies(FormattedDatabaseTraceListenerData configurationObject, IRegistryKey policyKey)
		{
			String addCategoryStoredProcNameOverride = policyKey.GetStringValue(AddCategoryStoredProcNamePropertyName);
			String databaseInstanceNameOverride = policyKey.GetStringValue(DatabaseInstanceNamePropertyName);
			String formatterOverride = GetFormatterPolicyOverride(policyKey);
			TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			String writeLogStoredProcNameOverride = policyKey.GetStringValue(WriteLogStoredProcNamePropertyName);
			configurationObject.AddCategoryStoredProcName = addCategoryStoredProcNameOverride;
			configurationObject.DatabaseInstanceName = databaseInstanceNameOverride;
			configurationObject.Formatter = formatterOverride;
			configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.WriteLogStoredProcName = writeLogStoredProcNameOverride;
		}
	    protected override void GenerateWmiObjects(FormattedDatabaseTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			FormattedDatabaseTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
