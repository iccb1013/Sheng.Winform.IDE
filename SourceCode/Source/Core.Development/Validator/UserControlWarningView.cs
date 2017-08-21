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
using Sheng.SailingEase.Controls.ObjectListView;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(true)]
    partial class UserControlWarningView : UserControlViewBase
    {
        public UserControlWarningView()
        {
            InitializeComponent();
            imageList.Images.Add(IconsLibrary.EmptyIcon); 
            imageList.Images.Add(IconsLibrary.Error2); 
            InitializeTreeListView();
        }
        public void ShowWarning(WarningSign warningSign)
        {
            if (warningSign == null)
            {
                this.treeListView.Roots = new ArrayList();
                return;
            }
            ArrayList roots = new ArrayList() { warningSign };
            this.treeListView.Roots = roots;
            this.treeListView.ExpandAll();
        }
        public void ShowWarning(WarningSignCollection warningSignCollection)
        {
            if (warningSignCollection == null)
            {
                this.treeListView.Roots = new ArrayList();
                return;
            }
            ArrayList roots = new ArrayList(warningSignCollection);
            this.treeListView.Roots = roots;
            this.treeListView.ExpandAll();
        }
        public void ExpandAll()
        {
            this.treeListView.ExpandAll();
        }
        private void InitializeTreeListView()
        {
            this.treeListView.CanExpandGetter = delegate(object x)
            {
                WarningSign warningSign = x as WarningSign;
                if (x == null)
                    return false;
                return (warningSign.Warnings.Count > 0);
            };
            this.treeListView.ChildrenGetter = delegate(object x)
            {
                WarningSign warningSign = x as WarningSign;
                if (warningSign == null)
                    return new ArrayList();
                return new ArrayList(warningSign.Warnings);
            };
            this.treeListView.CheckBoxes = false;
            TreeListView.TreeRenderer treeRenderer = this.treeListView.TreeColumnRenderer as TreeListView.TreeRenderer;
            treeRenderer.IsShowLines = false;
            this.treeColumnIcon.ImageGetter = delegate(object x)
            {
                WarningSign warningSign = x as WarningSign;
                if (String.IsNullOrEmpty(warningSign.Message))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            };
            this.treeColumnName.AspectGetter = delegate(object x)
            {
                WarningSign warningSign = x as WarningSign;
                if (String.IsNullOrEmpty(warningSign.Message))
                {
                    return warningSign.Name;
                }
                else
                {
                    return warningSign.Message;
                }
            };
        }
    }
}
