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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
	public class OracleConnectionSettings : SerializableConfigurationSection
	{
		private const string oracleConnectionDataCollectionProperty = "";
		public const string SectionName = "oracleConnectionSettings";
		public OracleConnectionSettings()
		{
		}
		public static OracleConnectionSettings GetSettings(IConfigurationSource configurationSource)
		{
			return configurationSource.GetSection(SectionName) as OracleConnectionSettings;
		}
		[ConfigurationProperty(oracleConnectionDataCollectionProperty, IsRequired=false, IsDefaultCollection=true)]
		public NamedElementCollection<OracleConnectionData> OracleConnectionsData
		{
			get { return (NamedElementCollection<OracleConnectionData>)base[oracleConnectionDataCollectionProperty]; }
		}
	}
}
