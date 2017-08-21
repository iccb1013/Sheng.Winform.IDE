/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
	public class DistributorService : ServiceBase
	{
		private bool initializeComponentsCalled = false;
		internal const string DefaultApplicationName = "Enterprise Library Logging Distributor Service";
		private static string NameTag = Properties.Resources.DistributorServiceNameTag;
		private DistributorEventLogger eventLogger;
		private string applicationName;
		private ServiceStatus status;
		private MsmqListener queueListener;
		public DistributorService()
		{
			base.CanStop = true;
			base.CanPauseAndContinue = true;
			base.CanStop = true;
			base.AutoLog = false;
		}
		private static void Main()
		{
			ServiceBase[] servicesToRun = new ServiceBase[] { new DistributorService() };
			ServiceBase.Run(servicesToRun);
		}
		public virtual ServiceStatus Status
		{
			get { return this.status; }
			set { this.status = value; }
		}
		public string ApplicationName
		{
			get { return this.applicationName; }
			set { this.applicationName = value; }
		}
		public DistributorEventLogger EventLogger
		{
			get { return this.eventLogger; }
		}
		public MsmqListener QueueListener
		{
			get { return this.queueListener; }
			set { this.queueListener = value; }
		}
		public void InitializeComponent()
		{
			try
			{
				this.ApplicationName = DefaultApplicationName;
				this.eventLogger = new DistributorEventLogger();
				this.eventLogger.AddMessage(Resources.InitializeComponentStartedMessage, Resources.InitializeComponentStarted);
				this.status = ServiceStatus.OK;
				IConfigurationSource configurationSource = GetConfigurationSource();
				MsmqDistributorSettings distributorSettings = MsmqDistributorSettings.GetSettings(configurationSource);
				if (distributorSettings == null)
				{
					throw new ConfigurationErrorsException(string.Format(
							Resources.Culture,
							Resources.ExceptionCouldNotFindConfigurationSection,
							MsmqDistributorSettings.SectionName));
				}
				this.queueListener = CreateListener(this, distributorSettings.QueueTimerInterval, distributorSettings.MsmqPath);
				this.ApplicationName = this.ServiceName;
				this.ApplicationName = distributorSettings.ServiceName;
				this.eventLogger.AddMessage(NameTag, this.ApplicationName);
				this.eventLogger.ApplicationName = this.ApplicationName;
				this.eventLogger.AddMessage(Resources.InitializeComponentCompletedMessage, Resources.InitializeComponentCompleted);
			}
			catch (LoggingException loggingException)
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceStartError, this.ApplicationName),
					loggingException,
					TraceEventType.Error);
				throw;
			}
			catch (Exception ex)
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceStartError, this.ApplicationName),
					ex,
					TraceEventType.Error);
				throw new LoggingException(Resources.ErrorInitializingService, ex);
			}
		}
		protected virtual MsmqListener CreateListener(DistributorService distributorService, int timerInterval, string msmqPath)
		{
			return new MsmqListener(distributorService, timerInterval, msmqPath);
		}
		protected override void OnStart(string[] args)
		{
			if (!initializeComponentsCalled)
			{
				InitializeComponent();
			}
			try
			{
				SanityCheck sanityCheck = new SanityCheck(this);
				sanityCheck.StartCheckTimer();
				if (this.Status == ServiceStatus.OK)
				{
					StartMsmqListener();
					this.eventLogger.LogServiceStarted();
				}
			}
			catch (Exception e)
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceStartError, this.ApplicationName),
					e,
					TraceEventType.Error);
				this.Status = ServiceStatus.Shutdown;
			}
		}
		protected override void OnStop()
		{
			try
			{
				StopMsmqListener();
			}
			catch (Exception e)
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceStopError, this.ApplicationName),
					e,
					TraceEventType.Error);
				this.Status = ServiceStatus.Shutdown;
			}
			GC.Collect();
		}
		protected override void OnPause()
		{
			try
			{
				if (this.queueListener.StopListener())
				{
					this.eventLogger.LogServicePaused();
				}
				else
				{
					this.eventLogger.LogServiceFailure(
						string.Format(Resources.Culture, Resources.ServicePauseWarning, this.ApplicationName),
						null,
						TraceEventType.Warning);
				}
			}
			catch (Exception e)
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServicePauseError, this.ApplicationName),
					e,
					TraceEventType.Error);
				this.Status = ServiceStatus.Shutdown;
			}
		}
		protected override void OnContinue()
		{
			try
			{
				this.queueListener.StartListener();
				this.eventLogger.LogServiceResumed();
			}
			catch (Exception e)
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceResumeError, this.ApplicationName),
					e,
					TraceEventType.Error);
				this.Status = ServiceStatus.Shutdown;
			}
		}
		private void StartMsmqListener()
		{
			try
			{
				this.eventLogger.AddMessage(Resources.InitializeStartupSequenceStartedMessage, Resources.ValidationStarted);
				this.queueListener.StartListener();
				this.eventLogger.AddMessage(Resources.InitializeStartupSequenceFinishedMessage, Resources.ValidationComplete);
			}
			catch
			{
				this.eventLogger.AddMessage(Resources.InitializeStartupSequenceErrorMessage, Resources.ValidationError);
				this.Status = ServiceStatus.Shutdown;
				throw;
			}
		}
		private void StopMsmqListener()
		{
			if (this.queueListener.StopListener())
			{
				this.eventLogger.LogServiceStopped();
			}
			else
			{
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceStopWarning, this.ApplicationName),
					null,
					TraceEventType.Warning);
			}
		}
		private static IConfigurationSource GetConfigurationSource()
		{
			return ConfigurationSourceFactory.Create();
		}
	}
}
