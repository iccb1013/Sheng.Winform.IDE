/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    [Image(typeof(SpecialTraceSourcesNode))]
    public sealed class SpecialTraceSourcesNode : ConfigurationNode
    {
        public SpecialTraceSourcesNode()
            : base(Resources.SpecialTraceSourcesNodeName)
        {
        }
		[Browsable(false)]
		public override bool SortChildren
		{
			get { return false; }
		}
		[ReadOnly(true)]
		public override string Name
		{
			get { return base.Name; }			
		}        
    }
}
