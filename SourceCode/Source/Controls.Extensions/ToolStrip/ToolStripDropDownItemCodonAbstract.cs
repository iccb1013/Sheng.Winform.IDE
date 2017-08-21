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
using System.Drawing;
namespace Sheng.SailingEase.Controls.Extensions
{
    public abstract class ToolStripDropDownItemCodonAbstract<TView> :
        ToolStripItemCodonAbstract<TView>, IToolStripDropDownItemCodon where TView : IToolStripItemView
    {
        private ToolStripItemCodonCollection _items;
        public ToolStripItemCodonCollection Items
        {
            get
            {
                if (_items == null)
                    _items = new ToolStripItemCodonCollection()
                    {
                        Owner = this.Owner
                    };
                return _items;
            }
            set { _items = value; }
        }
        public override ToolStripCodonBase Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                foreach (IToolStripItemCodon item in this.Items)
                {
                    item.Owner = value;
                }
            }
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint)
            : base(pathPoint)
        {
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, action)
        {
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint, string text, ToolStripItemDisplayStyle displayStyle)
            : base(pathPoint, text, displayStyle)
        {
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint, string text)
            : base(pathPoint, text)
        {
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint, string text, Image image)
            : base(pathPoint, text, image)
        {
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, action)
        {
        }
        public ToolStripDropDownItemCodonAbstract(string pathPoint, string text, Image image, ToolStripItemDisplayStyle displayStyle, OnToolStripItemCodonActionHandler action)
            : base(pathPoint, text, image, displayStyle, action)
        {
        }
    }
}
