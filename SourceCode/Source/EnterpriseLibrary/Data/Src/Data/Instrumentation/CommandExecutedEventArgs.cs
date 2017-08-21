/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    public class CommandExecutedEventArgs : EventArgs
    {
        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
        }
        public CommandExecutedEventArgs(DateTime startTime)
        {
            this.startTime = startTime;
        }
    }
}
