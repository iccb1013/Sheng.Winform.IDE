/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    public sealed class AddOracleConnectionElementNodeCommand : AddChildNodeCommand
    {
        public AddOracleConnectionElementNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(OracleConnectionElementNode)) {}
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            OracleConnectionElementNode node = ChildNode as OracleConnectionElementNode;
            Debug.Assert(null != node, "Expected OracleConnectionElementNode");
            node.AddNode(new OraclePackageElementNode());
        }
    }
}
