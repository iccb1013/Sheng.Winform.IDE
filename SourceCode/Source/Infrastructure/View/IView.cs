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
using System.Drawing;
using Sheng.SailingEase.Controls.Docking;
namespace Sheng.SailingEase.Infrastructure
{
    public interface IView
    {
        string Title { get; }
        bool Single { get; set; }
        object SingleKey { get; set; }
        bool CompareSingleKey(object singleKey);
        bool HideOnClose { get; set; }
        Icon Icon { get; set; }
        void Show(DockPanel dockPanel);
    }
}
