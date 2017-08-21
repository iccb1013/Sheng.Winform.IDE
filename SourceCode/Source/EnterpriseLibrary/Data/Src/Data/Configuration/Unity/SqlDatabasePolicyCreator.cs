/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity
{
	public class SqlDatabasePolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList, 
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			ConnectionStringSettings castConfigurationObject = (ConnectionStringSettings)configurationObject;
			new PolicyBuilder<SqlDatabase, ConnectionStringSettings>(
				instanceName,
				castConfigurationObject,
				c => new SqlDatabase(c.ConnectionString))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
