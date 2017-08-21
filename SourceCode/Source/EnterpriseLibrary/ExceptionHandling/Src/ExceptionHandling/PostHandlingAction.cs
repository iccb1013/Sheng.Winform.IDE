/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    public enum PostHandlingAction
    {
        None = 0,
        NotifyRethrow = 1,
        ThrowNewException = 2
    }
}
