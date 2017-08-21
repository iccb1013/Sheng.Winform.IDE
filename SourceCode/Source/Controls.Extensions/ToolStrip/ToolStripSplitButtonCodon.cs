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
    public class ToolStripSplitButtonCodon<TView>
        : ToolStripDropDownItemCodonAbstract<TView>, IToolStripDropDownItemCodon where TView : IToolStripItemView
    {
        public OnToolStripItemCodonActionHandler DropDownOpeningAction
        {
            get;
            set;
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image)
            : base(pathPoint,text, image)
        {
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle)
            : base(pathPoint, text, image, displayStyle, null)
        {
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, System.Windows.Forms.ToolStripItemDisplayStyle.Image, action)
        {
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, displayStyle, action)
        {
        }
    }
    public class ToolStripSplitButtonCodon : ToolStripSplitButtonCodon<ToolStripSplitButtonView>
    {
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image)
            : base(pathPoint, text, image)
        {
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle)
            : base(pathPoint, text, image, displayStyle, null)
        {
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, System.Windows.Forms.ToolStripItemDisplayStyle.Image, action)
        {
        }
        public ToolStripSplitButtonCodon(string pathPoint, string text, Image image, System.Windows.Forms.ToolStripItemDisplayStyle displayStyle, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, displayStyle, action)
        {
        }
    }
}
