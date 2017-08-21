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
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using System.Collections;
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ExplorerTreeView : PadViewBase
    {
        private TabControlController _tabControlController;
        private ITreeContainer _currentTreeContainer;
        private MenuStripTreeContainer _menuTreeContainer = new MenuStripTreeContainer()
        {
            Dock = DockStyle.Fill
        };
        private ToolStripTreeContainer _toolStripTreeContainer = new ToolStripTreeContainer()
        {
            Dock = DockStyle.Fill
        };
        public Type SelectedTabType
        {
            get
            {
                return _tabControlController.SelectedTabType;
            }
        }
        public object SelectedNodeData
        {
            get
            {
                if (_currentTreeContainer.SelectedNode == null)
                    return null;
                else
                    return _currentTreeContainer.SelectedNode.DataBoundItem;
            }
        }
        public Type SelectedNodeType
        {
            get
            {
                if (_currentTreeContainer.SelectedNode == null)
                    return null;
                else
                    return _currentTreeContainer.SelectedNode.Codon.DataBoundType;
            }
        }
        public Type SelectedNodeItemType
        {
            get
            {
                if (_currentTreeContainer.SelectedNode == null)
                    return null;
                else
                    return _currentTreeContainer.SelectedNode.ItemType;
            }
        }
        public IList SelectedNodeItems
        {
            get
            {
                if (_currentTreeContainer.SelectedNode == null)
                    return null;
                else
                    return _currentTreeContainer.SelectedNode.Items;
            }
        }
        public ExplorerTreeView()
        {
            InitializeComponent();
            this.TabText = Language.Current.Explorer_ExplorerTreeView_TabText;
            InitTreeContainer();
            InitTabController();
        }
        private void ExplorerTreeView_Load(object sender, EventArgs e)
        {
            Control selectedTabView = _tabControlController.SelectedView;
            if (selectedTabView != null)
            {
                _currentTreeContainer = selectedTabView as ITreeContainer;
            }
        }
        private void InitTreeContainer()
        {
            _menuTreeContainer.AfterSelect += new OnTreeContainerAfterSelectHandler(treeContainer_AfterSelect);
            _toolStripTreeContainer.AfterSelect += new OnTreeContainerAfterSelectHandler(treeContainer_AfterSelect);
        }
        private void InitTabController()
        {
            _tabControlController = new TabControlController(tabControl);
            _tabControlController.AddTabPage<MenuEntity>(
                Language.Current.Explorer_ExplorerTreeView_Tab_Menu, _menuTreeContainer);
            _tabControlController.AddTabPage<ToolStripPageEntity>(
                Language.Current.Explorer_ExplorerTreeView_Tab_ToolStrip, _toolStripTreeContainer);
            _tabControlController.TabPageChanged +=
                new TabControlController.OnTabPageChangedEventHandler(_tabControlController_TabPageChanged);
        }
        public void Select(object obj)
        {
            _currentTreeContainer.Select(obj);
        }
        public void Expand()
        {
            _currentTreeContainer.Expand();
        }
        void treeContainer_AfterSelect(object sender, TreeContainerEventArgs e)
        {
            if (AfterSelect != null)
            {
                AfterSelectEventArgs args = new AfterSelectEventArgs(_tabControlController.SelectedTabType)
                {
                    DataBoundItem = e.Node.DataBoundItem,
                    DataBoundType = e.Node.Codon.DataBoundType,
                    ItemType = e.Node.ItemType,
                    Items = e.Node.Items
                };
                AfterSelect(this, args);
            }
        }
        void _tabControlController_TabPageChanged(TabControlControllerEventArgs e)
        {
            _currentTreeContainer = e.View as ITreeContainer;
            if (TabPageChanged != null)
            {
                TabPageChangedEventArgs args = new TabPageChangedEventArgs(e.BoundType);
                TabPageChanged(this, args);
            }
        }
        public delegate void OnAfterSelectHandler(object sender, AfterSelectEventArgs e);
        public event OnAfterSelectHandler AfterSelect;
        public delegate void OnTabPageChangedEventHandler(object sender, TabPageChangedEventArgs e);
        public event OnTabPageChangedEventHandler TabPageChanged;
        public class AfterSelectEventArgs : EventArgs
        {
            private Type _tabPageType;
            public Type TabPageType
            {
                get { return _tabPageType; }
            }
            public object DataBoundItem
            {
                get;
                internal set;
            }
            public Type DataBoundType
            {
                get;
                internal set;
            }
            public Type ItemType
            {
                get;
                internal set;
            }
            public IList Items
            {
                get;
                internal set;
            }
            public AfterSelectEventArgs(Type tabPageType)
            {
                _tabPageType = tabPageType;
            }
        }
        public class TabPageChangedEventArgs : EventArgs
        {
            private Type _tabType;
            public Type TabType
            {
                get { return _tabType; }
            }
            public TabPageChangedEventArgs(Type tabType)
            {
                _tabType = tabType;
            }
        }
    }
}
