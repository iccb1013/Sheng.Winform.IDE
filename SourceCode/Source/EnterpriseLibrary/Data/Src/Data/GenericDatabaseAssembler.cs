/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	internal class GenericDatabaseAssembler : IDatabaseAssembler
	{
		public Database Assemble(string name, ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource)
		{
			DbProviderFactory providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
			return new GenericDatabase(connectionStringSettings.ConnectionString, providerFactory);
		}
	}
}
