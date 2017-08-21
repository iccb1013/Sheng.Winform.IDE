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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    class GridViewMenuDataItemEntity : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public GridViewMenuDataItemEntity()
            : base("GridViewMenuDataItemEntity")
        {
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.GridViewMenuDataItemEntity_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
                {
                    IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
                });
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.GridViewMenuDataItemEntity_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
                {
                    IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
                });
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.GridViewMenuDataItemEntity_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
                {
                    IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
                });
        }
    }
}
