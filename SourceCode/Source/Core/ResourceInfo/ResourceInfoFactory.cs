/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Kernal;
using System.IO;
using System.Reflection;
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    public class ResourceInfoFactory
    {
        Dictionary<ResourceContentAttribute, Type> _attributes = new Dictionary<ResourceContentAttribute, Type>();
        private static InstanceLazy<ResourceInfoFactory> _instance =
          new InstanceLazy<ResourceInfoFactory>(() => new ResourceInfoFactory());
        public static ResourceInfoFactory Instance
        {
            get { return _instance.Value; }
        }
        private ResourceInfoFactory()
        {
            LoadProvides();
        }
        private void LoadProvides()
        {
            List<AttributeAndTypeRelation> attributeAndTypeRelations = 
                ReflectionAttributeHelper.GetAttributeAndTypeRelation<ResourceContentAttribute>(
                Assembly.GetExecutingAssembly(), false);
            foreach (var item in attributeAndTypeRelations)
            {
                _attributes.Add((ResourceContentAttribute)item.Attribute, item.Type);
            }
        }
        private Type GetResourceInfoType(string fileName)
        {
            foreach (var item in _attributes)
            {
                if (item.Key.CanHandle(fileName))
                    return item.Value;
            }
            Debug.Assert(false, "没找到能够处理指定文件的 ResourceContentAttribute");
            return null;
        }
        public ResourceInfo GetResourceInfo(string fileName, Stream stream)
        {
            return GetResourceInfo<ResourceInfo>(fileName, stream);
        }
        public T GetResourceInfo<T>(string fileName, Stream stream) where T : ResourceInfo
        {
            Type resourceInfoType = GetResourceInfoType(fileName);
            Debug.Assert(resourceInfoType != null);
            if (resourceInfoType == null)
                throw new ArgumentOutOfRangeException();
            T resourceInfo = (T)Activator.CreateInstance(resourceInfoType);
            resourceInfo.Name = fileName;
            resourceInfo.ResourceStream = stream;
            return resourceInfo;
        }
    }
}
