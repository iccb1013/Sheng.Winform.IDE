/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    public class DistributorServiceTestFacade : DistributorService
    {
        public const string MockServiceName = "Enterprise Library Logging Distributor Service";
        public DistributorServiceTestFacade()
        {
            this.ServiceName = MockServiceName;
        }
        private ServiceStatus status;
        public override ServiceStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        new public void OnContinue()
        {
            base.OnContinue();
        }
        public void Initialize()
        {
            base.InitializeComponent();
        }
        public void OnStart()
        {
            base.OnStart(null);
        }
        new public void OnStop()
        {
            base.OnStop();
        }
        new public void OnPause()
        {
            base.OnPause();
        }
        protected override MsmqListener CreateListener(DistributorService distributorService, int timerInterval, string msmqPath)
        {
            if (this.QueueListener != null)
            {
                return this.QueueListener;
            }
            return new MockMsmqListener(distributorService, timerInterval, msmqPath);
        }
    }
}
