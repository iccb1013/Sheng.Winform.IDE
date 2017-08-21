/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Sheng.SailingEase.Controls.PopupControl
{
    [Flags]
    public enum PopupAnimations : int
    {
        None = 0,
        LeftToRight = 0x00001,
        RightToLeft = 0x00002,
        TopToBottom = 0x00004,
        BottomToTop = 0x00008,
        Center = 0x00010,
        Slide = 0x40000,
        Blend = 0x80000,
        Roll = 0x100000,
        SystemDefault = 0x200000,
    }
}
