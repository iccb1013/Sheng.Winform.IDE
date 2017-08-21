/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Kernal;
using System.Collections;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class ExplorerGridView : WorkbenchViewBase
    {
        private ExplorerGridContainer _gridContainer;
        public ToolStripCodon ToolStrip
        {
            get { return _gridContainer.ToolStrip; }
        }
        public ExplorerGridView()
        {
            InitializeComponent();
            _gridContainer = new ExplorerGridContainer();
            _gridContainer.Dock = DockStyle.Fill;
            _gridContainer.GridDoubleClick += (sender, e) =>
            {
                if (GridDoubleClick != null)
                    GridDoubleClick(this, e);
            };
            _gridContainer.GridSelectedItemChanged += (sender, e) =>
            {
                if (GridSelectedItemChanged != null)
                    GridSelectedItemChanged(this, e);
            };
            this.Controls.Add(_gridContainer);
        }
        public void Clear()
        {
            _gridContainer.Clear();
        }
        public void DataBind(Type currentType, IList list, Type listType, object contextData)
        {
            _gridContainer.DataBind(currentType, list, listType, contextData);
        }
        public event ExplorerGridContainer.OnGridDoubleClickHandler GridDoubleClick;
        public event ExplorerGridContainer.OnGridSelectedItemChangedHandler GridSelectedItemChanged;
    }
}
