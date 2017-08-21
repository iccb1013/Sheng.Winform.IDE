/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    internal class SanityCheck : MarshalByRefObject
    {
        private const int SanityInterval = 5000;
        private static string Header = ExceptionFormatter.Header;
        private Timer timer;
		private DistributorEventLogger eventLogger;
        private DistributorService distributorService;
        public SanityCheck(DistributorService distributorService)
        {
			this.eventLogger = distributorService.EventLogger;
            this.distributorService = distributorService;
        }
        public void StartCheckTimer()
        {
            this.timer = new Timer();
            this.timer.Elapsed += new ElapsedEventHandler(OnSanityTimedEvent);
            this.timer.Interval = SanityInterval;
            this.timer.Enabled = true;
        }
        public void StopService()
        {
            try
            {
                ServiceController myController =
                    new ServiceController(this.distributorService.ApplicationName);
                myController.Stop();
            }
            catch (Exception e)
            {
                string errorMessage = string.Format(
                    Resources.Culture,
                    Resources.ServiceControllerStopException, 
                    this.distributorService.ApplicationName);
 				this.eventLogger.LogServiceFailure(
					errorMessage,
					e,
					TraceEventType.Error);
                throw new LoggingException(errorMessage, e);
            }            
        }
        private void OnSanityTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (this.distributorService.Status == ServiceStatus.Shutdown)
            {
                try
                {
                    ShutdownQueueListener();
                }
                catch (Exception err)
                {
                    string errorMessage = string.Format(Resources.Culture, Resources.ServiceUnableToShutdown, this.distributorService.ApplicationName);
					this.eventLogger.LogServiceFailure(
						errorMessage,
						err,
						TraceEventType.Error);
                    this.distributorService.Status = ServiceStatus.PendingShutdown;
                }
            }
        }
        private void ShutdownQueueListener()
        {
            bool result = this.distributorService.QueueListener.StopListener();
            if (result)
            {
                StopService();
            }
            else
            {
				this.eventLogger.LogServiceFailure(
					string.Format(Resources.Culture, Resources.ServiceControllerStopException, this.distributorService.ApplicationName),
					null,
					TraceEventType.Error);
			}
        }
    }
}
