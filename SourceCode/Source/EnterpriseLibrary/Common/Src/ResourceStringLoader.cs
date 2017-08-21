/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Resources;
namespace Microsoft.Practices.EnterpriseLibrary.Common
{
    public static class ResourceStringLoader
    {
        public static string LoadString(string baseName, string resourceName)
        {
            return LoadString(baseName, resourceName, Assembly.GetCallingAssembly());
        }
        public static string LoadString(string baseName, string resourceName, Assembly asm)
        {
            if (string.IsNullOrEmpty(baseName)) throw new ArgumentNullException("baseName");
            if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName");
            string value = null;
            if (null != asm) value = SearchForResource(asm, baseName, resourceName);
            if (null == value) value = LoadAssemblyString(Assembly.GetExecutingAssembly(), baseName, resourceName);
            if (null == value) return string.Empty;
            return value;
        }
        private static string SearchForResource(Assembly asm, string baseName, string resourceName)
        {
            string[] resources = asm.GetManifestResourceNames();
            foreach (string resource in resources)
            {
                const string token = ".resources";
                string resourceToUse = (string)resource.Clone();
                if (resource.EndsWith(token))
                {
                    resourceToUse = resource.Replace(token, string.Empty);
                }
                string result = LoadAssemblyString(asm, resourceToUse, resourceName);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }
            return null;
        }
        private static string LoadAssemblyString(Assembly asm, string baseName, string resourceName)
        {
            try
            {
                ResourceManager rm = new ResourceManager(baseName, asm);
                return rm.GetString(resourceName);
            }
            catch (MissingManifestResourceException)
            {
            }
            return null;
        }
    }
}
