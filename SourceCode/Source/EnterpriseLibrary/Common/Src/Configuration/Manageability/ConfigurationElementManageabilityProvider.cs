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
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public abstract class ConfigurationElementManageabilityProvider
	{
		public const String PolicyValueName = ConfigurationSectionManageabilityProvider.PolicyValueName;
		protected ConfigurationElementManageabilityProvider()
		{ }
		public abstract void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource,
			String parentKey);
		public abstract bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationElement configurationObject,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings);
		protected virtual void LogExceptionWhileOverriding(Exception exception)
		{
			ManageabilityExtensionsLogger.LogExceptionWhileOverriding(exception);
		}
	}
}
