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
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripMenuItemCodon<TView>
        : ToolStripDropDownItemCodonAbstract<TView>, IToolStripDropDownItemCodon where TView : IToolStripItemView
    {
        public ToolStripMenuItemCodon(string pathPoint, string text)
            : base(pathPoint,text)
        {
            this.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
        }
        public ToolStripMenuItemCodon(string pathPoint, string text, Image image)
            : base(pathPoint, text, image, System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText, null)
        {
        }
        public ToolStripMenuItemCodon(string pathPoint, string text, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, null, System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText, action)
        {
        }
        public ToolStripMenuItemCodon(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText, action)
        {
        }
    }
    public class ToolStripMenuItemCodon : ToolStripMenuItemCodon<ToolStripMenuItemView>
    {
        public ToolStripMenuItemCodon(string pathPoint, string text)
            : base(pathPoint, text)
        {
            this.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
        }
        public ToolStripMenuItemCodon(string pathPoint, string text, Image image)
            : base(pathPoint, text, image)
        {
        }
        public ToolStripMenuItemCodon(string pathPoint, string text, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, action)
        {
        }
        public ToolStripMenuItemCodon(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, action)
        {
        }
    }
}
