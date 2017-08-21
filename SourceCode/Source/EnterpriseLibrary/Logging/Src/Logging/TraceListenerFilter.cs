/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class TraceListenerFilter
	{
		private Dictionary<TraceListener, object> viewedTraceListeners;
		public TraceListenerFilter()
		{
			viewedTraceListeners = new Dictionary<TraceListener, object>();
		}
		public IEnumerable<TraceListener> GetAvailableTraceListeners(IList traceListeners)
		{
			IList<TraceListener> filteredTraceListeners = new List<TraceListener>(traceListeners.Count);
			foreach (TraceListener listener in traceListeners)
			{
				if (!viewedTraceListeners.ContainsKey(listener))
				{
					viewedTraceListeners.Add(listener, listener);
					filteredTraceListeners.Add(listener);
				}
			}
			return filteredTraceListeners;
		}
	}
}
