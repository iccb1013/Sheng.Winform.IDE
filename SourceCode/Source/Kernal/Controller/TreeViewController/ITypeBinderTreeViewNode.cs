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
using System.Collections;
using System.Windows.Forms;
namespace Sheng.SailingEase.Kernal
{
    public interface ITypeBinderTreeViewNode
    {
        object DataBoundItem { get; set; }
        TypeBinderTreeViewNodeCodon Codon { get; set; }
        Type ItemType { get; }
        IList Items { get; }
        IList OwnerList { get; set; }
        string TextMember { get; }
        string ItemsMember { get; }
        void Update();
        void Update(object obj);
        TreeNode Node { get; }
        ITypeBinderTreeViewNode PrevNode { get; }
        ITypeBinderTreeViewNode NextNode { get; }
        ITypeBinderTreeViewNode Parent { get; }
        void AddChild(ITypeBinderTreeViewNode node);
        void Remove();
    }
}
