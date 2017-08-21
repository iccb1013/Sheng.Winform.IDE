/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    public abstract class ExceptionManager
    {
        public abstract bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow);
        public abstract bool HandleException(Exception exceptionToHandle, string policyName);
        public abstract void Process(Action action, string policyName);
    }
}
