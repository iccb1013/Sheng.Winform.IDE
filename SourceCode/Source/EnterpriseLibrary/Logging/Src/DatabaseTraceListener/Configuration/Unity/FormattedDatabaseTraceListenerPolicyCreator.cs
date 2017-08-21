/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Unity
{
	public class FormattedDatabaseTraceListenerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			new PolicyBuilder<FormattedDatabaseTraceListener, FormattedDatabaseTraceListenerData>(
				instanceName,
				(FormattedDatabaseTraceListenerData)configurationObject,
				c => new FormattedDatabaseTraceListener(
					Resolve.Reference<Data.Database>(c.DatabaseInstanceName),
					c.WriteLogStoredProcName,
					c.AddCategoryStoredProcName,
					Resolve.OptionalReference<ILogFormatter>(c.Formatter)))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
