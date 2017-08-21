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
    class TreeMenuDataEntityFolder : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public TreeMenuDataEntityFolder()
            : base("TreeMenuDataEntityFolder")
        {
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.TreeMenuDataEntityFolder_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
                {
                    IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
                });
        }
    }
}
