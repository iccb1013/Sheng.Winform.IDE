/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	sealed class ExceptionHandlingSettingsBuilder 
	{
		private ExceptionHandlingSettingsNode exceptionHandlingSettingsNode;
		private IConfigurationUIHierarchy hierarchy;
		private ExceptionHandlingSettings exceptionHandlingSettings;
		public ExceptionHandlingSettingsBuilder(IServiceProvider serviceProvider, ExceptionHandlingSettingsNode exceptionHandlingSettingsNode) 
		{
			this.exceptionHandlingSettingsNode = exceptionHandlingSettingsNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}
		public ExceptionHandlingSettings Build()
		{
			exceptionHandlingSettings = new ExceptionHandlingSettings();
			if (!this.exceptionHandlingSettingsNode.RequirePermission)	
				exceptionHandlingSettings.SectionInformation.RequirePermission = this.exceptionHandlingSettingsNode.RequirePermission;
			BuildPolicies();
			return exceptionHandlingSettings;
		}
		private void BuildPolicies()
		{
			IList<ConfigurationNode> policies = hierarchy.FindNodesByType(exceptionHandlingSettingsNode, typeof(ExceptionPolicyNode));
			foreach (ConfigurationNode policyNode in policies)
			{
				exceptionHandlingSettings.ExceptionPolicies.Add(CreateExceptionPolicyData((ExceptionPolicyNode)policyNode));
			}
		}		
		private ExceptionPolicyData CreateExceptionPolicyData(ExceptionPolicyNode policyNode)
		{
			ExceptionPolicyData policyData = new ExceptionPolicyData(policyNode.Name);						
			IList<ConfigurationNode> exceptionTypes = hierarchy.FindNodesByType(policyNode, typeof(ExceptionTypeNode));
			foreach (ConfigurationNode exceptionTypeNode in exceptionTypes)
			{
				policyData.ExceptionTypes.Add(CreateExceptionTypeData((ExceptionTypeNode)exceptionTypeNode));
			}
			return policyData;
		}
		private static ExceptionTypeData CreateExceptionTypeData(ExceptionTypeNode exceptionTypeNode)
		{
			ExceptionTypeData exceptionTypeData = new ExceptionTypeData(exceptionTypeNode.Name, exceptionTypeNode.Type, exceptionTypeNode.PostHandlingAction);			
			foreach (ConfigurationNode exceptionHandlerNode in exceptionTypeNode.Nodes)
			{
				exceptionTypeData.ExceptionHandlers.Add(((ExceptionHandlerNode)exceptionHandlerNode).ExceptionHandlerData);
			}
			return exceptionTypeData;
		}
	}
}
