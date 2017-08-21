/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
	internal class OracleDatabaseAssembler : IDatabaseAssembler
	{
		public Database Assemble(string name, ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource)
		{
			OracleConnectionSettings oracleConnectionSettings = OracleConnectionSettings.GetSettings(configurationSource);
			if (oracleConnectionSettings != null)
			{
				OracleConnectionData oracleConnectionData = oracleConnectionSettings.OracleConnectionsData.Get(name);
				if (oracleConnectionData != null)
				{
					IOraclePackage[] packages = new IOraclePackage[oracleConnectionData.Packages.Count];
					int i = 0;
					foreach (IOraclePackage package in oracleConnectionData.Packages)
					{
						packages[i++] = package;
					}
					return new OracleDatabase(connectionStringSettings.ConnectionString, packages);
				}
			}
			return new OracleDatabase(connectionStringSettings.ConnectionString);
		}
	}
}
