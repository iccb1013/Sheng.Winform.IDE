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
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    interface INavigationNode
    {
        Type Type { get; }
        Type ItemType { get; }
        object DataSource { get; }
        IList Items { get; }
        List<DataGridViewColumn> Columns { get;  }
        Action Action { get; set; }
        ContextMenuStrip ContextMenuStrip { get; set; }
    }
}
