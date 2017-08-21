/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity
{
	public class GenericDatabasePolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			ConnectionStringSettings castConfigurationObject = (ConnectionStringSettings)configurationObject;
			new PolicyBuilder<GenericDatabase, ConnectionStringSettings>(
					instanceName,
					castConfigurationObject,
					c => new GenericDatabase(c.ConnectionString, DbProviderFactories.GetFactory(castConfigurationObject.ProviderName)))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
