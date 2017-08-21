/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	[DatabaseAssembler(typeof(GenericDatabaseAssembler))]
	[ContainerPolicyCreator(typeof(GenericDatabasePolicyCreator))]
	public class GenericDatabase : Database
	{
		public GenericDatabase(string connectionString, DbProviderFactory dbProviderFactory
		)
			: base(connectionString, dbProviderFactory)
		{
		}
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
			throw new NotSupportedException(Resources.ExceptionParameterDiscoveryNotSupportedOnGenericDatabase);
		}
	}
}
