/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    public sealed class ResourceStringResolver : IStringResolver
    {
        public ResourceStringResolver(Type resourceType, string resourceName, string fallbackValue)
        {
            this.resourceType = resourceType;
            this.resourceName = resourceName;
            this.fallbackValue = fallbackValue;
        }
        public ResourceStringResolver(string resourceTypeName, string resourceName, string fallbackValue)
            : this(LoadType(resourceTypeName), resourceName, fallbackValue)
        { }
        private static Type LoadType(string resourceTypeName)
        {
            return Type.GetType(resourceTypeName, false);
        }
        private readonly Type resourceType;
        private readonly string resourceName;
        private readonly string fallbackValue;
        string IStringResolver.GetString()
        {
            string value;
            if (!(this.resourceType == null || string.IsNullOrEmpty(this.resourceName)))
            {
                value
                    = ResourceStringLoader.LoadString(
                        this.resourceType.FullName,
                        this.resourceName,
                        this.resourceType.Assembly);
            }
            else
            {
                value = this.fallbackValue;
            }
            return value;
        }
    }
}
