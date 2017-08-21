/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	public static class ExceptionHandlingSettingsWmiMapper
	{
		public static void GenerateWmiObjects(ExceptionHandlingSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
		}
		public static void GenerateExceptionTypeWmiObjects(ExceptionTypeData exceptionType,
			ExceptionPolicyData parentPolicy,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			ExceptionTypeSetting wmiSetting
				= new ExceptionTypeSetting(exceptionType,
					exceptionType.Name,
					exceptionType.Type.AssemblyQualifiedName,
					exceptionType.PostHandlingAction.ToString());
			wmiSetting.Policy = parentPolicy.Name;
			wmiSettings.Add(wmiSetting);
		}
		public static void GenerateExceptionPolicyDataWmiObjects(ExceptionPolicyData policy,
				ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new ExceptionPolicySetting(policy, policy.Name));
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ExceptionPolicySetting),
				typeof(ExceptionTypeSetting));
		}
	}
}
