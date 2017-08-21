/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	partial class ConfigurationSetting
	{
		public void Publish()
		{
			System.Management.Instrumentation.Instrumentation.Publish(this);
		}
		public void Revoke()
		{
			System.Management.Instrumentation.Instrumentation.Revoke(this);
		}
		public event EventHandler<EventArgs> Changed
		{
			add { ;}
			remove { ;}
		}
	}
}
