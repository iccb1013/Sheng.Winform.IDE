/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    public abstract class DataEvent: BaseWmiEvent
    {
		private string instanceName;
		protected DataEvent(string instanceName)
		{
			this.instanceName = instanceName;
		}
		public string InstanceName
		{
			get { return instanceName; }
		}
	}
}
