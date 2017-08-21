/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public sealed class ExpirationPollTimer : IDisposable
    {
        private Timer pollTimer;
        public void StartPolling(TimerCallback callbackMethod, int pollCycleInMilliseconds)
        {
            if (callbackMethod == null)
            {
                throw new ArgumentNullException("callbackMethod");
            }
            if (pollCycleInMilliseconds <= 0)
            {
                throw new ArgumentException(Resources.InvalidExpirationPollCycleTime, "pollCycleInMilliseconds");
            }
            pollTimer = new Timer(callbackMethod, null, pollCycleInMilliseconds, pollCycleInMilliseconds);
        }
        public void StopPolling()
        {
            if (pollTimer == null)
            {
                throw new InvalidOperationException(Resources.InvalidPollingStopOperation);
            }
            pollTimer.Dispose();
            pollTimer = null;
        }
		void IDisposable.Dispose()
		{
			if (pollTimer != null) pollTimer.Dispose();
		}		
	}
}
