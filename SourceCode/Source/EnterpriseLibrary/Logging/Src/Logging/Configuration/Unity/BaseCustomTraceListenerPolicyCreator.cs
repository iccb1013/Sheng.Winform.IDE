/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	public class BaseCustomTraceListenerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			BasicCustomTraceListenerData castConfigurationObject = (BasicCustomTraceListenerData)configurationObject;
			string listenerName = castConfigurationObject.Name;
			Type listenerType = castConfigurationObject.Type;
			string initData = castConfigurationObject.InitData;
			TraceOptions traceOutputOptions = castConfigurationObject.TraceOutputOptions;
			NameValueCollection attributes = castConfigurationObject.Attributes;	
			TraceFilter filter = castConfigurationObject.Filter != SourceLevels.All ? new EventTypeFilter(castConfigurationObject.Filter) : null;
			string formatterName = castConfigurationObject is CustomTraceListenerData
				? ((CustomTraceListenerData)castConfigurationObject).Formatter
				: null;
			policyList.Set<IBuildPlanPolicy>(
				new DelegateBuildPlanPolicy(
					context =>
					{
						TraceListener traceListener
							= SystemDiagnosticsTraceListenerCreationHelper.CreateSystemDiagnosticsTraceListener(
								listenerName,
								listenerType,
								initData,
								attributes);
						traceListener.Name = listenerName;
						traceListener.TraceOutputOptions = traceOutputOptions;
						traceListener.Filter = filter;
						CustomTraceListener customTraceListener = traceListener as CustomTraceListener;
						if (customTraceListener != null && !string.IsNullOrEmpty(formatterName))
						{
							IBuilderContext formatterContext = context.CloneForNewBuild(NamedTypeBuildKey.Make<ILogFormatter>(formatterName), null);
							customTraceListener.Formatter = (ILogFormatter)formatterContext.Strategies.ExecuteBuildUp(formatterContext);
						}
						return traceListener;
					}),
				new NamedTypeBuildKey(listenerType, instanceName));
		}
	}
}
