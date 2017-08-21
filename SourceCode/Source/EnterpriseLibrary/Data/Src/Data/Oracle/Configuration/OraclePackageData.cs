/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
    public class OraclePackageData : NamedConfigurationElement, IOraclePackage
    {
		private const string prefixProperty = "prefix";
        public OraclePackageData() : base()
        {
            this.Prefix = string.Empty;
        }
        public OraclePackageData(string name, string prefix) : base(name)
        {
            this.Prefix = prefix;
        }
		[ConfigurationProperty(prefixProperty, IsRequired= true)]
		public string Prefix
		{
			get
			{
				return (string)this[prefixProperty];
			}
			set
			{
				this[prefixProperty] = value;
			}
		}
    }
}
