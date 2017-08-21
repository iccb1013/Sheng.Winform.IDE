/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public interface IExceptionHandler
    {
        Exception HandleException(Exception exception, Guid handlingInstanceId);
    }
}
