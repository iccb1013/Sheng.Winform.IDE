/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;
namespace Data.SqlCe.Tests.VSTS
{
	public class TestConfigurationSource
	{
		public static DictionaryConfigurationSource CreateConfigurationSource()
		{
			DictionaryConfigurationSource source = new DictionaryConfigurationSource();
			DatabaseSettings settings = new DatabaseSettings();
			settings.DefaultDatabase = "SqlCeTestConnection";
			ConnectionStringsSection section = new ConnectionStringsSection();
            section.ConnectionStrings.Add(new ConnectionStringSettings("SqlCeTestConnection", "Data Source='testdb.sdf'", "System.Data.SqlServerCe.3.5"));
			source.Add(DatabaseSettings.SectionName, settings);
			source.Add("connectionStrings", section);
			return source;
		}
	}
}
