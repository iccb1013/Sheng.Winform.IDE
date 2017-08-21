/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	[Image(typeof(OracleConnectionElementNode))]
	public sealed class OracleConnectionElementNode : ConfigurationNode
    {
		public OracleConnectionElementNode()
			: base(Resources.OraclePackagesNodeName)
		{
		}
		[ReadOnly(true)]
		public override string Name
		{
			get
			{
				return base.Name;
			}			
		}		
	}
}
