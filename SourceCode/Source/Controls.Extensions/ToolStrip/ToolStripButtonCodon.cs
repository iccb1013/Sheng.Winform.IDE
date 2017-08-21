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
    public class ToolStripButtonCodon<TView> : ToolStripItemCodonAbstract<TView> where TView : IToolStripItemView
    {
        public ToolStripButtonCodon(string pathPoint, string text, Image image)
            : base(pathPoint, text, image)
        {
        }
        public ToolStripButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle)
            : base(pathPoint, text, image, displayStyle, null)
        {
        }
        public ToolStripButtonCodon(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, System.Windows.Forms.ToolStripItemDisplayStyle.Image, action)
        {
        }
        public ToolStripButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, displayStyle, action)
        {
        }
    }
    public class ToolStripButtonCodon : ToolStripButtonCodon<ToolStripButtonView>
    {
        public ToolStripButtonCodon(string pathPoint, string text, Image image)
            : base(pathPoint, text, image)
        {
        }
        public ToolStripButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle)
            : base(pathPoint, text, image, displayStyle, null)
        {
        }
        public ToolStripButtonCodon(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, System.Windows.Forms.ToolStripItemDisplayStyle.Image, action)
        {
        }
        public ToolStripButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, displayStyle, action)
        {
        }
    }
}
