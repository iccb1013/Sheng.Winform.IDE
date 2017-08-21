/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    internal class ExpirationTimeoutExpiredMsg : IQueueMessage
    {
        private BackgroundScheduler callback;
        public ExpirationTimeoutExpiredMsg(BackgroundScheduler callback)
        {
            this.callback = callback;
        }
        public void Run()
        {
            callback.DoExpirationTimeoutExpired();
        }
    }
}
