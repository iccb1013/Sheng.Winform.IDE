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
    public class SENavigationTreeNode : TreeNode
    {
        public SENavigationTreeNode():base()
        {
        }
        public SENavigationTreeNode(string name)
            : base(name)
        {
        }
        public SENavigationTreeNode(string name,Control panel)
            : this(name)
        {
            this.Panel = panel;
        }
        private Control _panel;
        public Control Panel
        {
            get
            {
                return this._panel;
            }
            set
            {
                this._panel = value;
            }
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
    }
}
