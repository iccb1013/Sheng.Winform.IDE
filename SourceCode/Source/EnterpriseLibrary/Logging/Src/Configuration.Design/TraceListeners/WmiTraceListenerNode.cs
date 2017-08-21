/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    public class WmiTraceListenerNode : TraceListenerNode
    {       
        public WmiTraceListenerNode()
            : this(new WmiTraceListenerData(Resources.WmiTraceListenerNode))
        {
        }
        public WmiTraceListenerNode(WmiTraceListenerData wmiTraceListenerData)
        {
			if (null == wmiTraceListenerData) throw new ArgumentNullException("wmiTraceListenerData");
			Rename(wmiTraceListenerData.Name);
			TraceOutputOptions = wmiTraceListenerData.TraceOutputOptions;
            Filter = wmiTraceListenerData.Filter;
        }
		public override TraceListenerData TraceListenerData
		{
			get 
			{ 
				WmiTraceListenerData data = new WmiTraceListenerData(Name);
				data.TraceOutputOptions = TraceOutputOptions;
                data.Filter = Filter;
				return data;
			}
		}
    }
}
