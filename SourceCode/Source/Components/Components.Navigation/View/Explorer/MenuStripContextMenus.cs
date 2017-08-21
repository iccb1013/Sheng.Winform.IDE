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
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    class GridMenuMenuEntity : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MooveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }
        public GridMenuMenuEntity()
            : base("GridMenuMenuEntity")
        {
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_GridViewMenuMenuEntity_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_GridViewMenuMenuEntity_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_GridViewMenuMenuEntity_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new ToolStripMenuItemCodon("Up", Language.Current.Explorer_GridViewMenuMenuEntity_Up,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MooveUpCommand != null) MooveUpCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MooveUpCommand == null) return false; else return MooveUpCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Down", Language.Current.Explorer_GridViewMenuMenuEntity_Down,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveDownCommand != null) MoveDownCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveDownCommand == null) return false; else return MoveDownCommand.CanExcute(); }
            });
        }
    }
    class TreeMenuMenuEntity : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }
        public TreeMenuMenuEntity()
            : base("TreeMenuMenuEntity")
        {
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_TreeMenuMenuEntity_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_TreeMenuMenuEntity_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_TreeMenuMenuEntity_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripSeparatorCodon());
            this.Items.Add(new ToolStripMenuItemCodon("MoveBeforeCommand", Language.Current.Explorer_TreeMenuMenuEntity_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfterCommand", Language.Current.Explorer_TreeMenuMenuEntity_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
        }
    }
    class TreeMenuMenuEntityFolder : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public TreeMenuMenuEntityFolder()
            : base("TreeMenuMenuEntityFolder")
        {
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_TreeMenuMenuEntityFolder_Add,
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
