/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics.CodeAnalysis;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    public interface IStringResolver
    {
        [SuppressMessage("Microsoft.Design", "CA1024", Justification = "May be computationally expensive.")]
        string GetString();
    }
}
