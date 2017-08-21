/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    internal class MockMsmqListener : MsmqListener
    {
        public bool ExceptionOnStart = false;
        public bool ExceptionOnStop = false;
        public bool StopReturnsFalse = false;
        public bool StartCalled = false;
        public bool StopCalled = false;
		public MockMsmqListener(DistributorService logDistributor, int timerInterval, string msmqPath)
            : base(logDistributor, timerInterval, msmqPath)
        {
        }
        public override void StartListener()
        {
            StartCalled = true;
            if (ExceptionOnStart)
            {
                throw new Exception("simulated exception");
            }
        }
        public override bool StopListener()
        {
            StopCalled = true;
            if (ExceptionOnStop)
            {
                throw new Exception("simulated exception");
            }
            if (StopReturnsFalse)
            {
                return false;
            }
            return true;
        }
    }
}
