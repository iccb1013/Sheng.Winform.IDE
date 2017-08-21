/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Docking
{
    public partial class DockWindow
    {
        private class SplitterControl : SplitterBase
        {
            protected override int SplitterSize
            {
                get { return Measures.SplitterSize; }
            }
            protected override void StartDrag()
            {
                DockWindow window = Parent as DockWindow;
                if (window == null)
                    return;
                window.DockPanel.BeginDrag(window, window.RectangleToScreen(Bounds));
            }
        }
    }
}
