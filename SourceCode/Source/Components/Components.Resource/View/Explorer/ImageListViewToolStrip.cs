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
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Components.ResourceComponent.Localisation;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    class ImageListViewToolStrip : ToolStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ImageListViewToolStrip()
            : base("ImageListViewToolStrip")
        {
            this.Items.Add(new ToolStripButtonCodon("Add", Language.Current.Explorer_ImageListViewContextMenuStrip_Add, IconsLibrary.New2,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_ImageListViewContextMenuStrip_Delete, IconsLibrary.Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.RemoveCommand != null) RemoveCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (RemoveCommand == null) return false; else return RemoveCommand.CanExcute(); }
            });
        }
    }
}
