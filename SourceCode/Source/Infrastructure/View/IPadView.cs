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
using Sheng.SailingEase.Controls.Docking;
using System.Drawing;
namespace Sheng.SailingEase.Infrastructure
{
    public interface IPadView : IView
    {
        PadAreas PadAreas { get; set; }
    }
    public enum PadAreas
    {
        Float = 1,
        DockLeft = 2,
        DockRight = 4,
        DockTop = 8,
        DockBottom = 16
    }
}
