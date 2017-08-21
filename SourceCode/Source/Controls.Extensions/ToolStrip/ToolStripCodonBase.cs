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
namespace Sheng.SailingEase.Controls.Extensions
{
    public abstract class ToolStripCodonBase
    {
        public string PathPoint
        {
            get;
            protected set;
        }
        public Func<ToolStripCodonBase, bool> IsVisible
        {
            get;
            set;
        }
        public Func<ToolStripCodonBase, bool> IsEnabled
        {
            get;
            set;
        }
        public bool Visible
        {
            get
            {
                if (IsVisible == null)
                    return true;
                else
                    return IsVisible(this);
            }
        }
        public bool Enabled
        {
            get
            {
                if (IsEnabled == null)
                    return true;
                else
                    return IsEnabled(this);
            }
        }
        private ToolStripItemCodonCollection _items;
        public ToolStripItemCodonCollection Items
        {
            get
            {
                if (_items == null)
                    _items = new ToolStripItemCodonCollection()
                    {
                        Owner = this
                    };
                return _items;
            }
            set { _items = value; }
        }
        public ToolStripCodonBase(string pathPoint)
        {
            PathPoint = pathPoint;
        }
    }
}
