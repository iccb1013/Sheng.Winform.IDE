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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.NavigationComponent
{
    class ToolStripItemEntityDevTypes : ToolStripItemEntityTypesAbstract
    {
        private static InstanceLazy<ToolStripItemEntityDevTypes> _instance =
            new InstanceLazy<ToolStripItemEntityDevTypes>(() => new ToolStripItemEntityDevTypes());
        public static ToolStripItemEntityDevTypes Instance
        {
            get { return _instance.Value; }
        }
        private ToolStripItemEntityDevTypes()
        {
            _collection = new ToolStripItemEntityTypeCollection();
            _collection.Add(typeof(ToolStripButtonEntityDev));
            _collection.Add(typeof(ToolStripLabelEntityDev));
            _collection.Add(typeof(ToolStripSeparatorEntityDev));
            FormElementEntityDevTypes.Instance.AddRange(_collection);
        }
    }
}
