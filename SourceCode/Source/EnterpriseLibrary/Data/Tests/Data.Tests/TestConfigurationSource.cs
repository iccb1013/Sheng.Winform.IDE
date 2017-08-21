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
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
	public class TestConfigurationSource
	{
		public const string NorthwindDummyUser = "entlib";
		public const string NorthwindDummyPassword = "hdf7&834k(*KA";
		public static DictionaryConfigurationSource CreateConfigurationSource()
		{
			DictionaryConfigurationSource source = new DictionaryConfigurationSource();
			DatabaseSettings settings = new DatabaseSettings();
			settings.DefaultDatabase = "Service_Dflt";
			OracleConnectionSettings oracleConnectionSettings = new OracleConnectionSettings();
			OracleConnectionData data = new OracleConnectionData();
			data.Name = "OracleTest";
			data.Packages.Add(new OraclePackageData("TESTPACKAGE", "TESTPACKAGETOTRANSLATE"));
			oracleConnectionSettings.OracleConnectionsData.Add(data);
			source.Add(DatabaseSettings.SectionName, settings);
			source.Add(OracleConnectionSettings.SectionName, oracleConnectionSettings);
			return source;
		}
	}
}
