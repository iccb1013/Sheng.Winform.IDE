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
using System.Diagnostics;
namespace Sheng.SailingEase.Controls.Extensions
{
   
    public class TabControlController
    {
        private TabControl _tabControl;
        private Dictionary<Type, TypeBinderTabPage> _tabPages = new Dictionary<Type, TypeBinderTabPage>();
        public Type SelectedTabType
        {
            get
            {
                if (_tabControl.SelectedTab == null)
                    return null;
                else
                {
                    TypeBinderTabPage tabPage = (TypeBinderTabPage)_tabControl.SelectedTab;
                    return tabPage.BoundType;
                }
            }
        }
        public Control SelectedView
        {
            get
            {
                if (_tabControl.SelectedTab == null)
                    return null;
                else
                {
                    TypeBinderTabPage tabPage = (TypeBinderTabPage)_tabControl.SelectedTab;
                    return tabPage.View;
                }
            }
        }
        public TabControlController(TabControl tabControl)
        {
            _tabControl = tabControl;
            Debug.Assert(_tabControl.TabPages.Count == 0, "不能把带有tab页的TabControl放进来");
            _tabControl.TabPages.Clear();
            _tabControl.Selected += new TabControlEventHandler(_tabControl_Selected);
        }
        void _tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.Action == TabControlAction.Selected)
            {
                if (TabPageChanged != null)
                {
                    TypeBinderTabPage tabPage = (TypeBinderTabPage)e.TabPage;
                    TabControlControllerEventArgs args = new TabControlControllerEventArgs(tabPage.BoundType, tabPage.View);
                    TabPageChanged(args);
                }
            }
        }
        public void AddTabPage<T>(string text, Control view)
        {
            Debug.Assert(view != null, "view为null");
            Type tType = typeof(T);
            TypeBinderTabPage tabPage = new TypeBinderTabPage(text, tType, view);
            _tabPages.Add(tType, tabPage);
            _tabControl.TabPages.Add(tabPage);
        }
        public void Select<T>()
        {
            Type tType = typeof(T);
            Debug.Assert(_tabPages.Keys.Contains(tType), "没有与指定类型绑定的tab页");
            if (_tabPages.Keys.Contains(tType))
            {
                _tabControl.SelectedTab = _tabPages[tType];
            }
        }
        public delegate void OnTabPageChangedEventHandler(TabControlControllerEventArgs e);
        public event OnTabPageChangedEventHandler TabPageChanged;
    }
    public class TabControlControllerEventArgs : EventArgs
    {
        private Type _boundType;
        public Type BoundType
        {
            get { return _boundType; }
        }
        private Control _view;
        public Control View
        {
            get { return _view; }
        }
        public TabControlControllerEventArgs(Type boundType, Control view)
        {
            _boundType = boundType;
            _view = view;
        }
    }
}
