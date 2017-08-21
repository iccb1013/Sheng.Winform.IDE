/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage
{
	public abstract class ConfigurationChangeWatcher : IConfigurationChangeWatcher
	{
		private static readonly object configurationChangedKey = new object();
		private static int defaultPollDelayInMilliseconds = 15000;
		private int pollDelayInMilliseconds = defaultPollDelayInMilliseconds;
		private Thread pollingThread;
		private EventHandlerList eventHandlers = new EventHandlerList();
		private DateTime lastWriteTime;
		private PollingStatus pollingStatus;
		private object lockObj = new object();
		public static void SetDefaultPollDelayInMilliseconds(int newDefaultPollDelayInMilliseconds)
		{
			defaultPollDelayInMilliseconds = newDefaultPollDelayInMilliseconds;
		}
        public static void ResetDefaultPollDelay()
		{
			defaultPollDelayInMilliseconds = 15000;
		}
		public void SetPollDelayInMilliseconds(int newDelayInMilliseconds)
		{
			pollDelayInMilliseconds = newDelayInMilliseconds;
		}
		public ConfigurationChangeWatcher()
		{
		}
		~ConfigurationChangeWatcher()
		{
			Disposing(false);
		}
		public event ConfigurationChangedEventHandler ConfigurationChanged
		{
			add { eventHandlers.AddHandler(configurationChangedKey, value); }
			remove { eventHandlers.RemoveHandler(configurationChangedKey, value); }
		}
		public abstract string SectionName
		{
			get;
		}
		public void StartWatching()
		{
			lock (lockObj)
			{
				if (pollingThread == null)
				{
					pollingStatus = new PollingStatus(true);
					pollingThread = new Thread(new ParameterizedThreadStart(Poller));
					pollingThread.IsBackground = true;
					pollingThread.Name = this.BuildThreadName();
					pollingThread.Start(pollingStatus);
				}
			}
		}
		public void StopWatching()
		{
			lock (lockObj)
			{
				if (pollingThread != null)
				{
					pollingStatus.Polling = false;
					pollingStatus = null;
					pollingThread = null;
				}
			}
		}
		public void Dispose()
		{
			Disposing(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Disposing(bool isDisposing)
		{
			if (isDisposing)
			{
				eventHandlers.Dispose();
				StopWatching();
			}
		}
		protected virtual void OnConfigurationChanged()
		{
			ConfigurationChangedEventHandler callbacks = (ConfigurationChangedEventHandler)eventHandlers[configurationChangedKey];
			ConfigurationChangedEventArgs eventData = this.BuildEventData();
			try
			{
				if (callbacks != null)
				{
					foreach (ConfigurationChangedEventHandler callback in callbacks.GetInvocationList())
					{
						if (callback != null)
						{
							callback(this, eventData);
						}
					}
				}
			}
			catch (Exception e)
			{
				LogException(e);
			}
		}
		private void LogException(Exception e)
		{
			try
			{
				EventLog.WriteEntry(GetEventSourceName(), Resources.ExceptionEventRaisingFailed + GetType().FullName + " :" + e.Message, EventLogEntryType.Error);
			}
			catch
			{
			}
		}
		protected abstract DateTime GetCurrentLastWriteTime();
		protected abstract string BuildThreadName();
		protected abstract ConfigurationChangedEventArgs BuildEventData();
		protected abstract string GetEventSourceName();
		private void Poller(object parameter)
		{
			lastWriteTime = DateTime.MinValue;
			DateTime currentLastWriteTime = DateTime.MinValue;
			PollingStatus pollingStatus = (PollingStatus)parameter;
			while (pollingStatus.Polling)
			{
				currentLastWriteTime = GetCurrentLastWriteTime();
				if (currentLastWriteTime != DateTime.MinValue)
				{
					if (lastWriteTime.Equals(DateTime.MinValue))
					{
						lastWriteTime = currentLastWriteTime;
					}
					else
					{
						if (lastWriteTime.Equals(currentLastWriteTime) == false)
						{
							lastWriteTime = currentLastWriteTime;
							OnConfigurationChanged();
						}
					}
				}
				Thread.Sleep(pollDelayInMilliseconds);		
			}
		}
		private class PollingStatus
		{
			private bool polling;
			public PollingStatus(bool polling)
			{
				this.polling = polling;
			}
			public bool Polling
			{
				get { return polling; }
				set { polling = value; }
			}
		}
	}
}
