/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    internal class MockMsmqLogDistributor : MsmqLogDistributor
    {
        private bool stopRecv = false;
        private bool isCompleted = true;
        public bool ReceiveMsgCalled = false;
        public bool ExceptionOnGetIsCompleted = false;
        public override bool StopReceiving
        {
            get { return stopRecv; }
            set { stopRecv = value; }
        }
        public override bool IsCompleted
        {
            get
            {
                if (ExceptionOnGetIsCompleted)
                {
                    throw new Exception("simulated exception");
                }
                return isCompleted;
            }
        }
        public void SetIsCompleted(bool val)
        {
            isCompleted = val;
        }
        public MockMsmqLogDistributor(LogWriter logWriter, string msmqPath) :
			base(logWriter, msmqPath, new DistributorEventLogger())
        {
        }
        public override void CheckForMessages()
        {
            ReceiveMsgCalled = true;
        }
    }
}
