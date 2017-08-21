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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Controls.Docking;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    public partial class ExplorerView : WorkbenchViewBase
    {
        public const string SINGLEKEY = "WindowExplorerView";
        private ExplorerTreeView _treeView;
        private ExplorerPropertyView _propertyView;
        private ExplorerGridView _gridView;
        public ExplorerView()
        {
            InitializeComponent();
            this.Single = true;
            this.SingleKey = SINGLEKEY;
            this.HideOnClose = true;
            this.Icon = DrawingTool.ImageToIcon(IconsLibrary.Form);
            this.TabText = Language.Current.ExplorerView_TabText;
            if (DesignMode)
                return;
            _treeView = new ExplorerTreeView();
            _treeView.PadAreas = PadAreas.DockLeft;
            _treeView.HideOnClose = true;
            _treeView.AfterSelect += new ExplorerTreeView.OnAfterSelectHandler(_treeView_AfterSelect);
            _treeView.NodeAdded += new ExplorerTreeView.OnNodeAddedHandler(_treeView_NodeAdded);
            _propertyView = new ExplorerPropertyView();
            _propertyView.PadAreas = PadAreas.DockLeft;
            _propertyView.HideOnClose = true;
            _gridView = new ExplorerGridView();
            _gridView.DockAreas = DockAreas.Document;
            _gridView.GridDoubleClick += new ExplorerGridContainer.OnGridDoubleClickHandler(_gridView_GridDoubleClick);
            _gridView.GridSelectedItemChanged += new ExplorerGridContainer.OnGridSelectedItemChangedHandler(_gridView_GridSelectedItemChanged);
        }
        private void ExplorerView_Load(object sender, EventArgs e)
        {
            _treeView.Show(this.dockPanel);
            _propertyView.Show(this.dockPanel);
            _propertyView.DockTo(_treeView.DockHandler.Pane, DockStyle.Bottom, 0);
            _gridView.Show(this.dockPanel);
        }
        void _treeView_AfterSelect(object sender, ExplorerTreeView.AfterSelectEventArgs e)
        {
            DataBind(e.DataBoundItem, e.Items, e.ItemType);
        }
        void _treeView_NodeAdded(object sender, ExplorerTreeView.NodeAddedEventArgs e)
        {
            DataBind(e.DataBoundItem, e.Items, e.ItemType);
        }
        void _gridView_GridDoubleClick(object sender, ExplorerGridContainer.GridDoubleClickEventArgs e)
        {
            if (e.Data == null)
                return;
            _treeView.Select(e.Data);
            _treeView.Expand();
        }
        void _gridView_GridSelectedItemChanged(object sender, ExplorerGridContainer.GridSelectedItemChangedEventArgs e)
        {
            List<object> selectedObjects = e.Data;
            if (selectedObjects.Count == 1)
            {
                IEntityIndex entityIndex = selectedObjects[0] as IEntityIndex;
                _propertyView.SetSelectedObject(entityIndex.Entity);
            }
            else
            {
                _propertyView.SetSelectedObject(null);
            }
        }
        private void DataBind(object data, IList items, Type itemType)
        {
            if (data == null)
            {
                _gridView.Clear();
            }
            else
            {
                _gridView.DataBind(typeof(IEntityIndex), items, itemType, data);
            }
        }
        private List<ToolStripCodon> _toolStripList;
        public override List<ToolStripCodon> ToolStripList
        {
            get
            {
                if (_toolStripList == null)
                {
                    _toolStripList = new List<ToolStripCodon>();
                    _toolStripList.Add(_gridView.ToolStrip);
                }
                return _toolStripList;
            }
        }
    }
}
