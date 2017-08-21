/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    public class MyCustomLogEntry : LogEntry
    {
        private string myName;
        public MyCustomLogEntry()
        {
            myName = "MyCustomLogEntry";
        }
        public string MyName
        {
            get { return myName; }
            set { myName = value; }
        }
    }
}
