/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    public sealed class DelegateStringResolver : IStringResolver
    {
        public DelegateStringResolver(Func<string> resolverDelegate)
        {
            this.resolverDelegate = resolverDelegate;
        }
        private readonly Func<string> resolverDelegate;
        string IStringResolver.GetString()
        {
            return this.resolverDelegate();
        }
    }
}
