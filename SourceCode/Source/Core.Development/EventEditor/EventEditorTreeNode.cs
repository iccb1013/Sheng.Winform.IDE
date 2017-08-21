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
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Core.Development
{
    class EventEditorTreeNode : SENavigationTreeNode
    {
        private EventEditorNode _editorNode;
        public EventEditorNode EditorNode
        {
            get { return _editorNode; }
            private set { _editorNode = value; }
        }
        public EventEditorTreeNode(EventEditorNode editorNode)
            : base(editorNode.Name)
        {
            _editorNode = editorNode;
            this.ImageIndex = editorNode.ImageIndex;
            this.SelectedImageIndex = editorNode.SelectedImageIndex;
            Control panel = editorNode.Panel as Control;
            if (panel != null)
                this.Panel = panel;
            CreateChildNodes();
        }
        private void CreateChildNodes()
        {
            foreach (EventEditorNode node in _editorNode.ChildNodes)
            {
                EventEditorTreeNode childNode = new EventEditorTreeNode(node);
                this.Nodes.Add(childNode);
            }
        }
    }
}
