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
namespace Sheng.SailingEase.Controls
{
    public class SENavigationTreeView : TreeView
    {
        private Panel _emptyPanel = new Panel();
        public Panel EmptyPanel
        {
            get { return this._emptyPanel; }
            set { this._emptyPanel = value; }
        }
        public SENavigationTreeNode SelectedNavigationNode
        {
            get { return this.SelectedNode as SENavigationTreeNode; }
        }
        public bool IsAvailabilityNavigationPanel
        {
            get
            {
                if (SelectedNavigationNode != null && SelectedNavigationNode.Panel != null)
                    return true;
                else
                    return false;
            }
        }
        public Control AvailabilityNavigationPanel
        {
            get
            {
                if (SelectedNavigationNode != null && SelectedNavigationNode.Panel != null)
                    return SelectedNavigationNode.Panel;
                else
                    return this.EmptyPanel;
            }
        }
        private bool _autoDockFill = true;
        public bool AutoDockFill
        {
            get { return this._autoDockFill; }
            set { this._autoDockFill = value; }
        }
        public SENavigationTreeView()
        {
            this.HideSelection = false;
        }
        public SENavigationTreeNode AddNode(string name)
        {
            return AddNode(null, name, null, -1, null);
        }
        public SENavigationTreeNode AddNode(string path, string name)
        {
            return AddNode(path, name, null, -1, null);
        }
        public SENavigationTreeNode AddNode(string path, string name, string text)
        {
            return AddNode(path, name, text, -1, null);
        }
        public SENavigationTreeNode AddNode(string name, Control panel)
        {
            return AddNode(null, name, null, -1, panel);
        }
        public SENavigationTreeNode AddNode(string name, string text, Control panel)
        {
            return AddNode(null, name, text, -1, panel);
        }
        public SENavigationTreeNode AddNode(string path, string name, string text, Control panel)
        {
            return AddNode(path, name, text, -1, panel);
        }
        public SENavigationTreeNode AddNode(string path, string name, string text, int imageIndex, Control panel)
        {
            SENavigationTreeNode node = new SENavigationTreeNode();
            if (name != null)
                node.Name = name;
            if (text != null)
                node.Text = text;
            else
                node.Text = name;
            if (panel != null)
                node.Panel = panel;
            if (AutoDockFill)
                node.Panel.Dock = DockStyle.Fill;
            if (path == null || path == String.Empty)
            {
                this.Nodes.Add(node);
            }
            else
            {
                SENavigationTreeNode targetNode = GetNode(path);
                if (targetNode == null)
                {
                    Debug.Assert(false, "没有找到路径 " + path);
                    throw new Exception();
                }
                targetNode.Nodes.Add(node);
            }
            return node;
        }
        public SENavigationTreeNode GetNode(string path)
        {
            return GetNode(path, null);
        }
        public SENavigationTreeNode GetNode(string path, TreeNodeCollection nodes)
        {
            TreeNodeCollection targetNodes;
            if (nodes != null)
                targetNodes = nodes;
            else
                targetNodes = this.Nodes;
            if (targetNodes == null)
                return null;
            string[] paths = path.Split('\\');
            TreeNode[] findTreeNodes;
            for (int i = 0; i < paths.Length; i++)
            {
                findTreeNodes = targetNodes.Find(paths[i], false);
                if (findTreeNodes.Length == 0)
                {
                    return null;
                }
                if (i == paths.Length - 1)
                {
                    return findTreeNodes[0] as SENavigationTreeNode;
                }
                else
                {
                    targetNodes = findTreeNodes[0].Nodes;
                }
            }
            return null;
        }
        public SENavigationTreeNode SetPanel(string path, Control panel)
        {
            SENavigationTreeNode node = GetNode(path);
            if (node == null)
            {
                Debug.Assert(false, "没有找到路径 " + path);
                throw new Exception();
            }
            node.Panel = panel;
            if (AutoDockFill)
                node.Panel.Dock = DockStyle.Fill;
            return node;
        }
        public delegate void OnAfterSelectNavigationNodeHandler(SENavigationTreeView treeView, SENavigationTreeNode node);
        public event OnAfterSelectNavigationNodeHandler OnAfterSelectNavigationNode;
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            if (OnAfterSelectNavigationNode != null)
            {
                OnAfterSelectNavigationNode(this, e.Node as SENavigationTreeNode);
            }
        }
    }
}
