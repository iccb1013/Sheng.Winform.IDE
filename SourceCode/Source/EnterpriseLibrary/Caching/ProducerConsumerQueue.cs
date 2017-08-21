/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	public class ProducerConsumerQueue
    {
        private object lockObject = new Object();
        private Queue queue = new Queue();
        public int Count
        {
            get { return queue.Count; }
        }
        public object Dequeue()
        {
            lock (lockObject)
            {
                while (queue.Count == 0)
                {
                    if (WaitUntilInterrupted())
                    {
                        return null;
                    }
                }
                return queue.Dequeue();
            }
        }
        public void Enqueue(object queueItem)
        {
            lock (lockObject)
            {
                queue.Enqueue(queueItem);
                Monitor.Pulse(lockObject);
            }
        }
        private bool WaitUntilInterrupted()
        {
            try
            {
                Monitor.Wait(lockObject);
            }
            catch (ThreadInterruptedException)
            {
                return true;
            }
            return false;
        }
    }
}
