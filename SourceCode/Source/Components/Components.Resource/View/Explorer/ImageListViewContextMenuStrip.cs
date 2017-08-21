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
using Sheng.SailingEase.Components.ResourceComponent.Localisation;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    class ImageListViewContextMenuStrip : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ImageListViewContextMenuStrip()
            : base("ImageListViewContextMenuStrip")
        {
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_ImageListViewContextMenuStrip_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_ImageListViewContextMenuStrip_Delete,
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
