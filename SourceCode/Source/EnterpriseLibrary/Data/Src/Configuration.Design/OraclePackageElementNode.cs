/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	[Image(typeof(OraclePackageElementNode))]
	public sealed class OraclePackageElementNode : ConfigurationNode
	{
		private string prefix;
		public OraclePackageElementNode()
			: this(new OraclePackageData(Resources.OracleConnectionElementNodeName, string.Empty))
		{
		}
		public OraclePackageElementNode(OraclePackageData oraclePackageElement)
			: base(null == oraclePackageElement ? string.Empty : oraclePackageElement.Name)
		{
			if (null == oraclePackageElement) throw new ArgumentNullException("oraclePackageElement");
			this.prefix = oraclePackageElement.Prefix;
		}
		[Required]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("PrefixDescription", typeof(Resources))]
		public string Prefix
		{
			get { return prefix; }
			set { prefix = value; }
		}
		[Browsable(false)]
		public OraclePackageData OraclePackageElement
		{
			get { return new OraclePackageData(Name, prefix); }
		}
	}
}
