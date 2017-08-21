/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class AddConnectionStringSettingsNodeCommand : AddChildNodeCommand
	{
		public AddConnectionStringSettingsNodeCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(ConnectionStringSettingsNode))
		{
		}
		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			ConnectionStringSettingsNode node = ChildNode as ConnectionStringSettingsNode;
			Debug.Assert(null != node, "Expected ConnectionStringSettingsNode");
		}
	}
}
