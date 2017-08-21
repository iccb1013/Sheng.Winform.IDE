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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
	public class OracleConnectionData : NamedConfigurationElement
	{
		private const string packagesProperty = "packages";
		public OracleConnectionData()
		{
		}
		[ConfigurationProperty(packagesProperty, IsRequired = true)]
		public NamedElementCollection<OraclePackageData> Packages
		{
			get
			{
				return (NamedElementCollection<OraclePackageData>)base[packagesProperty];
			}
		}
	}
}
