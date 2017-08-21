/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity
{
	public class OracleDatabasePolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			ConnectionStringSettings castConfigurationObject = (ConnectionStringSettings)configurationObject;
			IList<IOraclePackage> packages = new IOraclePackage[0];
			OracleConnectionSettings oracleConnectionSettings = OracleConnectionSettings.GetSettings(configurationSource);
			if (oracleConnectionSettings != null)
			{
				OracleConnectionData oracleConnectionData
					= oracleConnectionSettings.OracleConnectionsData.Get(castConfigurationObject.Name);
				if (oracleConnectionData != null)
				{
					packages = new List<IOraclePackage>(from op in oracleConnectionData.Packages select (IOraclePackage)op);
				}
			}
			new PolicyBuilder<OracleDatabase, ConnectionStringSettings>(
				instanceName,
				castConfigurationObject,
				c => new OracleDatabase(c.ConnectionString, packages))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
