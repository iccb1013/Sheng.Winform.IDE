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
namespace Sheng.SailingEase.Core
{
   
    public class ToolStripItemEntityTypesFactory
    {
        private ToolStripItemEntityTypeCollection _collection = new ToolStripItemEntityTypeCollection();
        private static InstanceLazy<ToolStripItemEntityTypesFactory> _instance =
          new InstanceLazy<ToolStripItemEntityTypesFactory>(() => new ToolStripItemEntityTypesFactory());
        public static ToolStripItemEntityTypesFactory Instance
        {
            get { return _instance.Value; }
        }
        private ToolStripItemEntityTypesFactory()
        {
        }
        public void Register(Type type)
        {
            _collection.Add(type);
        }
        public void Register(Type[] types)
        {
            _collection.AddRange(types);
        }
        public ToolStripItemAbstract CreateInstance(int code)
        {
            return _collection.CreateInstance(code);
        }
        public ToolStripItemAbstract CreateInstance(ToolStripItemEntityProvideAttribute attribute)
        {
            return _collection.CreateInstance(attribute);
        }
        public ToolStripItemEntityProvideAttribute GetProvideAttribute(int code)
        {
            return _collection.GetProvideAttribute(code);
        }
        public ToolStripItemEntityProvideAttribute GetProvideAttribute(ToolStripItemAbstract entity)
        {
            return _collection.GetProvideAttribute(entity);
        }
        public ToolStripItemEntityProvideAttribute GetProvideAttribute(Type type)
        {
            return _collection.GetProvideAttribute(type);
        }
        public List<ToolStripItemEntityProvideAttribute> GetProvideAttributeList()
        {
            return _collection.GetProvideAttributeList();
        }
        public string GetName(ToolStripItemAbstract entity)
        {
            return _collection.GetName(entity);
        }
    }
}
