/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    static class Helper
    {
        public static void ZOrderSync(UIElement entity)
        {
            IFormElementEntityDev entityDev = entity as IFormElementEntityDev;
            Debug.Assert(entityDev != null, "实体对象没有实现 IFormElementEntityDev");
            Control control = entityDev.Component as Control;
            Debug.Assert(control != null, "entityDev.Component 不是 Control");
            if (control.Parent == null)
                return;
            Control.ControlCollection controls = control.Parent.Controls;
            IShellControlDev controlDev;
            foreach (Control c in controls)
            {
                controlDev = c as IShellControlDev;
                Debug.Assert(controlDev != null, "Control 没有实现 IShellControlDev");
                if (controlDev == null)
                    continue;
                ((UIElement)controlDev.Entity).ZOrder = controls.GetChildIndex(c);
            }
        }
    }
}
