/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Management;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    public class WmiEventWatcher : IDisposable
    {
        readonly object eventsCollectionLock = new object();
        readonly List<ManagementBaseObject> eventsReceived = new List<ManagementBaseObject>();
        readonly ManagementEventWatcher eventWatcher;
        readonly int numberOfEventsToWatchFor;
        public WmiEventWatcher(int numberOfEventsToWatchFor)
            : this(numberOfEventsToWatchFor, "BaseWmiEvent") {}
        public WmiEventWatcher(int numberOfEventsToWatchFor,
                               string query)
        {
            this.numberOfEventsToWatchFor = numberOfEventsToWatchFor;
            WqlEventQuery eventQuery = new WqlEventQuery(query);
            ManagementScope scope = new ManagementScope(@"\\.\root\EnterpriseLibrary");
            eventWatcher = new ManagementEventWatcher(scope, eventQuery);
            eventWatcher.EventArrived += delegate_EventArrived;
            eventWatcher.Start();
        }
        public List<ManagementBaseObject> EventsReceived
        {
            get { return eventsReceived; }
        }
        public void delegate_EventArrived(object sender,
                                          EventArrivedEventArgs e)
        {
            lock (eventsCollectionLock)
            {
                eventsReceived.Add(e.NewEvent);
            }
        }
        public void Dispose()
        {
            eventWatcher.Stop();
            eventWatcher.Dispose();
        }
        public void WaitForEvents()
        {
            for (int i = 0; i < numberOfEventsToWatchFor * 2; i++)
            {
                Thread.Sleep(100);
                lock (eventsCollectionLock)
                {
                    if (eventsReceived.Count == numberOfEventsToWatchFor) break;
                }
            }
        }
    }
}
