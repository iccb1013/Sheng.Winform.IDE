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
    public class ToolStripWorkWindowSeparatorCodon<TView> : ToolStripItemCodonAbstract<TView> where TView : IToolStripItemView
    {
        private EnumPaintAlignment _paintAlignment = EnumPaintAlignment.Left;
        public EnumPaintAlignment PaintAlignment
        {
            get { return _paintAlignment; }
            set { _paintAlignment = value; }
        }
        public ToolStripWorkWindowSeparatorCodon(EnumPaintAlignment paintAlignment)
        {
            _paintAlignment = paintAlignment;
        }
        public enum EnumPaintAlignment
        {
            Left,
            Right
        }
    }
    public class ToolStripWorkWindowSeparatorCodon : ToolStripWorkWindowSeparatorCodon<ToolStripWorkWindowSeparatorView>
    {
        public ToolStripWorkWindowSeparatorCodon(EnumPaintAlignment paintAlignment)
            : base(paintAlignment)
        {
        }
    }
}
