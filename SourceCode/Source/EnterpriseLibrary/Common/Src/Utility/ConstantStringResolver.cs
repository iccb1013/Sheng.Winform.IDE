/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    public sealed class ConstantStringResolver : IStringResolver
    {
        public ConstantStringResolver(string value)
        {
            this.value = value;
        }
        private readonly string value;
        string IStringResolver.GetString()
        {
            return this.value;
        }
    }
}
