/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.WindowComponent.View
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
