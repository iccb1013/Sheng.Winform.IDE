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
    class ToolStripItemEntityTypes : ToolStripItemEntityTypesAbstract
    {
        private static InstanceLazy<ToolStripItemEntityTypes> _instance =
            new InstanceLazy<ToolStripItemEntityTypes>(() => new ToolStripItemEntityTypes());
        public static ToolStripItemEntityTypes Instance
        {
            get { return _instance.Value; }
        }
        private ToolStripItemEntityTypes()
        {
            _collection = new ToolStripItemEntityTypeCollection();
            _collection.Add(typeof(ToolStripButtonEntity));
            _collection.Add(typeof(ToolStripLabelEntity));
            _collection.Add(typeof(ToolStripSeparatorEntity));
            FormElementEntityTypes.Instance.AddRange(_collection);
        }
    }
}
