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
using System.Collections;
namespace Sheng.SailingEase.Kernal
{
    public interface ITypeBinderDataGridViewTypeCodon
    {
        Type DataBoundType { get; }
        bool ActOnSubClass { get; }
        Type ItemType { get; }
        string ItemsMember { get; }
        ContextMenuStrip ContextMenuStrip { get; }
        List<DataGridViewColumn> Columns { get; }
        bool Compatible(object obj);
        bool Compatible(Type type);
        bool UpwardCompatible(object obj);
        bool UpwardCompatible(Type type);
        IBindingListEx InitializeBindingList();
        IBindingListEx InitializeBindingList(IList list);
    }
}
