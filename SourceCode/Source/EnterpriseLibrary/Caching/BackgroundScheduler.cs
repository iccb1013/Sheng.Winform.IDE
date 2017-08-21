/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class BackgroundScheduler : ICacheScavenger
    {
        private ProducerConsumerQueue inputQueue = new ProducerConsumerQueue();
        private Thread inputQueueThread;
        private ExpirationTask expirer;
        private ScavengerTask scavenger;
        private bool isActive;
        private bool running;
        private CachingInstrumentationProvider instrumentationProvider;
        private object ignoredScavengeRequestsCountLock = new object();
        private int ignoredScavengeRequestsCount;
        public BackgroundScheduler(ExpirationTask expirer, ScavengerTask scavenger, CachingInstrumentationProvider instrumentationProvider)
        {
            this.expirer = expirer;
            this.scavenger = scavenger;
            this.instrumentationProvider = instrumentationProvider;
            this.ignoredScavengeRequestsCount = 0;
            ThreadStart queueReader = new ThreadStart(QueueReader);
            inputQueueThread = new Thread(queueReader);
            inputQueueThread.IsBackground = true;
        }
        public void Start()
        {
            running = true;
            inputQueueThread.Start();
        }
        public void Stop()
        {
            running = false;
            inputQueueThread.Interrupt();
        }
        public bool IsActive
        {
            get { return isActive; }
        }
        public void ExpirationTimeoutExpired(object notUsed)
        {
            inputQueue.Enqueue(new ExpirationTimeoutExpiredMsg(this));
        }
        public void StartScavenging()
        {
            bool scheduleRequest = false;
            lock (ignoredScavengeRequestsCountLock)
            {
                int currentCount = ignoredScavengeRequestsCount;
                scheduleRequest = currentCount == 0;
                ignoredScavengeRequestsCount = (currentCount + 1) % this.scavenger.NumberOfItemsToBeScavenged;
            }
            if (scheduleRequest)
            {
                inputQueue.Enqueue(new StartScavengingMsg(this));
            }
        }
        internal void DoStartScavenging()
        {
            lock (ignoredScavengeRequestsCountLock)
            {
                ignoredScavengeRequestsCount = 0;
            }
            scavenger.DoScavenging();
        }
        internal void DoExpirationTimeoutExpired()
        {
            expirer.DoExpirations();
        }
        private void QueueReader()
        {
            isActive = true;
            while (running)
            {
                IQueueMessage msg = inputQueue.Dequeue() as IQueueMessage;
                try
                {
                    if (msg == null)
                    {
                        continue;
                    }
                    msg.Run();
                }
                catch (ThreadInterruptedException)
                {
                }
                catch (Exception e)
                {
                    instrumentationProvider.FireCacheFailed(Resources.BackgroundSchedulerProducerConsumerQueueFailure, e);
                }
            }
            isActive = false;
        }
    }
}
