/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Timer = System.Timers.Timer;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
	public class MsmqListener
	{
		public int QueueListenerRetries = 30;
		private const int DefaultQueueTimerInterval = 20000;
		private const int QueueListenerRetrySleepTime = 1000;
		private static int queueTimerInterval;
		private System.Timers.Timer queueTimer = null;
		private DistributorEventLogger eventLogger = null;
		private DistributorService distributorService;
		private MsmqLogDistributor logDistributor;
		public MsmqListener(DistributorService distributorService, int timerInterval, string msmqPath)
		{
			this.distributorService = distributorService;
			this.QueueTimerInterval = timerInterval;
			this.eventLogger = distributorService.EventLogger;
			this.logDistributor = new MsmqLogDistributor(EnterpriseLibraryFactory.BuildUp<LogWriter>(), msmqPath, this.eventLogger);
		}
		public int QueueTimerInterval
		{
			get
			{
				if (queueTimerInterval == 0)
				{
					return DefaultQueueTimerInterval;
				}
				else
				{
					return queueTimerInterval;
				}
			}
			set
			{
				if (value == 0)
				{
					queueTimerInterval = DefaultQueueTimerInterval;
				}
				else
				{
					queueTimerInterval = value;
				}
			}
		}
		public virtual void StartListener()
		{
			try
			{
				this.eventLogger.AddMessage(Resources.ListenerStartingMessage, Resources.ListenerStarting);
				this.logDistributor.StopReceiving = false;
				if (this.queueTimer == null)
				{
					this.queueTimer = new Timer();
					this.queueTimer.Elapsed += new ElapsedEventHandler(OnQueueTimedEvent);
				}
				this.queueTimer.Interval = this.QueueTimerInterval;
				this.queueTimer.Enabled = true;
				this.eventLogger.AddMessage(Resources.ListenerStartCompleteMessage, string.Format(Resources.Culture, Resources.ListenerStartComplete, this.QueueTimerInterval));
			}
			catch (Exception e)
			{
				this.eventLogger.AddMessage(Resources.ListenerStartErrorMessage, Resources.ListenerStartError);
				this.eventLogger.AddMessage(Resources.Exception, e.Message);
				throw;
			}
		}
		public virtual bool StopListener()
		{
			try
			{
				if (this.queueTimer != null)
				{
					this.eventLogger.AddMessage(Resources.ListenerStopStartedMessage, Resources.ListenerStopStarted);
					this.queueTimer.Enabled = false;
					this.logDistributor.StopReceiving = true;
					if (WaitUntilListenerStopped())
					{
						return true;
					}
					this.queueTimer.Enabled = true;
					this.logDistributor.StopReceiving = false;
					return false;
				}
				return true;
			}
			catch (Exception e)
			{
				this.eventLogger.AddMessage(Resources.ListenerStopErrorMessage, Resources.ListenerStopError);
				this.eventLogger.AddMessage(Resources.Exception, e.Message);
				throw;
			}
		}
		private bool WaitUntilListenerStopped()
		{
			int timeOut = 0;
			while (timeOut < QueueListenerRetries)
			{
				if (this.logDistributor.IsCompleted)
				{
					this.eventLogger.AddMessage(Resources.ListenerStopCompletedMessage,
												string.Format(Resources.Culture, Resources.ListenerStopCompleted, timeOut.ToString(CultureInfo.InvariantCulture)));
					return true;
				}
				Thread.Sleep(QueueListenerRetrySleepTime);
				++timeOut;
			}
			this.eventLogger.AddMessage(Resources.StopListenerWarningMessage,
										string.Format(Resources.Culture, Resources.ListenerCannotStop, timeOut.ToString(CultureInfo.InvariantCulture)));
			return false;
		}
		public void SetMsmqLogDistributor(MsmqLogDistributor logDistributor)
		{
			this.logDistributor = logDistributor;
		}
		private void OnQueueTimedEvent(object sender, ElapsedEventArgs args)
		{
			try
			{
				if (this.logDistributor.IsCompleted &&
					(this.distributorService.Status == ServiceStatus.OK))
				{
					this.logDistributor.CheckForMessages();
				}
			}
			catch (Exception e)
			{
				this.eventLogger.LogServiceFailure(
					Resources.QueueTimedEventError,
					e,
					TraceEventType.Error);
				this.distributorService.Status = ServiceStatus.Shutdown;
			}
		}
	}
}
