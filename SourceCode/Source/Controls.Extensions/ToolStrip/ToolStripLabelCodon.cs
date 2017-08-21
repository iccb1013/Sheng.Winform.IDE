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
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripLabelCodon<TView> : ToolStripItemCodonAbstract<TView> where TView : IToolStripItemView
    {
        public ToolStripLabelCodon(string pathPoint, string text)
            : base(pathPoint, text, ToolStripItemDisplayStyle.Text)
        {
        }
    }
    public class ToolStripLabelCodon : ToolStripLabelCodon<ToolStripLabelView>
    {
        public ToolStripLabelCodon(string pathPoint, string text)
            : base(pathPoint, text)
        {
        }
    }
}
