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
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class SEUndoEngineFormDesigner : SEUndoEngine
    {
        private IDesignSurfaceHosting _designSurfaceHosting;
        public SEUndoEngineFormDesigner(IDesignSurfaceHosting formFormDesinger)
        {
            _designSurfaceHosting = formFormDesinger;
        }
        public void UpdateView(EntityBase entity)
        {
            _designSurfaceHosting.UpdateView(entity);
        }
        public void UpdatePropertyGrid(bool updateSelectedObject)
        {
            _designSurfaceHosting.UpdatePropertyGrid(updateSelectedObject);
        }
        public void CreateControl(UIElement element)
        {
            _designSurfaceHosting.CreateControl(element);
        }
        public void DestroyControl(UIElement element)
        {
            _designSurfaceHosting.DestroyControl(element);
        }
    }
}
