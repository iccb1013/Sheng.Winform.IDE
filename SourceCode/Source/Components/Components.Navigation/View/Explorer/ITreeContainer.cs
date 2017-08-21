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
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    public delegate void OnTreeContainerAfterSelectHandler(object sender, TreeContainerEventArgs e);
    interface ITreeContainer
    {
        ITypeBinderTreeViewNode SelectedNode { get; }
        void Select(object obj);
        void Expand();
        event OnTreeContainerAfterSelectHandler AfterSelect;
    }
    public class TreeContainerEventArgs
    {
        private ITypeBinderTreeViewNode _node;
        public ITypeBinderTreeViewNode Node
        {
            get { return _node; }
        }
        public TreeContainerEventArgs(ITypeBinderTreeViewNode node)
        {
            _node = node;
        }
    }
}
