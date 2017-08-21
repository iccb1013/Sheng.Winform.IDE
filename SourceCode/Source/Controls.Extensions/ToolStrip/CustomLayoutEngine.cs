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
using System.Windows.Forms.Layout;
using System.Windows.Forms;
using System.Drawing;
namespace Sheng.SailingEase.Controls.Extensions
{
    class CustomLayoutEngine : LayoutEngine
    {
        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            Control parent = container as Control;
            Rectangle parentDisplayRectangle = parent.DisplayRectangle;
            Control[] source = new Control[parent.Controls.Count];
            parent.Controls.CopyTo(source, 0);
            Point nextControlLocation = parentDisplayRectangle.Location;
            foreach (Control c in source)
            {
                if (!c.Visible) continue;
                nextControlLocation.Offset(c.Margin.Left, c.Margin.Top);
                c.Location = nextControlLocation;
                if (c.AutoSize)
                {
                    c.Size = c.GetPreferredSize(parentDisplayRectangle.Size);
                }
                nextControlLocation.Y = parentDisplayRectangle.Y;
                nextControlLocation.X += c.Width + c.Margin.Right + parent.Padding.Horizontal;
            }
            return false;
        }
    }
}
