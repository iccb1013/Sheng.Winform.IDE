/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    [Image(typeof(ProviderMappingsNode))]
    public sealed class ProviderMappingsNode : ConfigurationNode
    {	
		public ProviderMappingsNode()
			: base(Resources.ProviderMappingsNodeName)
		{
		}
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }        
    }
}
