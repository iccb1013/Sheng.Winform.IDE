/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	sealed class ExceptionHandlingNodeMapRegistrar : NodeMapRegistrar
	{
		public ExceptionHandlingNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		public override void Register()
		{
			AddMultipleNodeMap(Resources.DefaultCustomHandlerNodeName,
				typeof(CustomHandlerNode),
				typeof(CustomHandlerData));
			AddMultipleNodeMap(Resources.DefaultWrapHandlerNodeName,
				typeof(WrapHandlerNode),
				typeof(WrapHandlerData));
			AddMultipleNodeMap(Resources.DefaultReplaceHandlerNodeName,
				typeof(ReplaceHandlerNode),
				typeof(ReplaceHandlerData));
		}        
	}
}
