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
    public class ToolStripCodon : ToolStripCodonBase
    {
        private ToolStripView _view;
        public ToolStripView View
        {
            get
            {
                if (_view == null)
                    _view = new ToolStripView(this);
                return _view;
            }
            set { _view = value; }
        }
        public ToolStripCodon(string pathPoint)
            :base(pathPoint)
        {
        }
    }
}
