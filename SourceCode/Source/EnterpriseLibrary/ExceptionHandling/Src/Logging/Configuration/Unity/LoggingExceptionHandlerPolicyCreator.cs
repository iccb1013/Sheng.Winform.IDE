/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Unity
{
	public class LoggingExceptionHandlerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			LoggingExceptionHandlerData castConfigurationObject = (LoggingExceptionHandlerData)configurationObject;
			new PolicyBuilder<LoggingExceptionHandler, LoggingExceptionHandlerData>(
				NamedTypeBuildKey.Make<LoggingExceptionHandler>(instanceName),
				castConfigurationObject,
				c => new LoggingExceptionHandler(
						castConfigurationObject.LogCategory,
						castConfigurationObject.EventId,
						castConfigurationObject.Severity,
						castConfigurationObject.Title,
						castConfigurationObject.Priority,
						castConfigurationObject.FormatterType,
						Resolve.Reference<LogWriter>(null)))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
