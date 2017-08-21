using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.ComponentModel;

namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    //工具栏项右键菜单

    #region GridMenuToolStripPage

    /// <summary>
    /// 列表中呈现枚举列表时的右键菜单
    /// </summary>
    class GridMenuToolStripPage : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }

        public GridMenuToolStripPage()
            : base("GridMenuToolStripPage")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_GridMenuToolStripPage_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            //编辑
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_GridMenuToolStripPage_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            //删除
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_GridMenuToolStripPage_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            //------------
            this.Items.Add(new ToolStripSeparatorCodon());
            //上移
            this.Items.Add(new ToolStripMenuItemCodon("MoveBefore", Language.Current.Explorer_GridMenuToolStripPage_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            //下移
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfter", Language.Current.Explorer_GridMenuToolStripPage_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
        }
    }

    #endregion

    #region GridMenuToolStripGroup

    /// <summary>
    /// 列表中呈现枚举列表时的右键菜单
    /// </summary>
    class GridMenuToolStripGroup : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }

        public GridMenuToolStripGroup()
            : base("GridMenuToolStripGroup")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_GridMenuToolStripGroup_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            //编辑
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_GridMenuToolStripGroup_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            //删除
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_GridMenuToolStripGroup_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            //------------
            this.Items.Add(new ToolStripSeparatorCodon());
            //上移
            this.Items.Add(new ToolStripMenuItemCodon("MoveBefore", Language.Current.Explorer_GridMenuToolStripGroup_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            //下移
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfter", Language.Current.Explorer_GridMenuToolStripGroup_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
        }
    }

    #endregion

    #region GridMenuToolStripItem

    /// <summary>
    /// 列表中呈现枚举列表时的右键菜单
    /// </summary>
    class GridMenuToolStripItem : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }

        public GridMenuToolStripItem()
            : base("GridMenuToolStripItem")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_GridMenuToolStripItem_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            //编辑
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_GridMenuToolStripItem_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            //删除
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_GridMenuToolStripItem_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            //------------
            this.Items.Add(new ToolStripSeparatorCodon());
            //上移
            this.Items.Add(new ToolStripMenuItemCodon("MoveBefore", Language.Current.Explorer_GridMenuToolStripItem_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            //下移
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfter", Language.Current.Explorer_GridMenuToolStripItem_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
            ////------------
            //this.Items.Add(new ToolStripSeparatorCodon());
            ////预览
            //this.Items.Add(new ToolStripMenuItemCodon("Preview", Language.Current.Explorer_GridMenuToolStripItem_Preview,
            //    delegate(object sender, ToolStripItemCodonEventArgs args)
            //    {
            //        if (this.PreviewCommand != null) PreviewCommand.Excute();
            //    })
            //{
            //    IsEnabled = (e) => { if (PreviewCommand == null) return false; else return PreviewCommand.CanExcute(); }
            //});
        }
    }

    #endregion

    

    #region TreeMenuToolStripItemFolder

    class TreeMenuToolStripItemFolder : ContextMenuStripCodon
    {
        /// <summary>
        /// 添加页
        /// </summary>
        public ICommand AddCommand { get; set; }

        public TreeMenuToolStripItemFolder()
            : base("TreeMenuToolStripItemFolder")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_TreeMenuToolStripItemFolder_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
        }
    }

    #endregion

    #region TreeMenuToolStripPage

    class TreeMenuToolStripPage : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }

        public TreeMenuToolStripPage()
            : base("TreeMenuToolStripPage")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_TreeMenuToolStripPage_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            //编辑
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_TreeMenuToolStripPage_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            //删除
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_TreeMenuToolStripPage_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            //------------
            this.Items.Add(new ToolStripSeparatorCodon());
            //上移
            this.Items.Add(new ToolStripMenuItemCodon("MoveBeforeCommand", Language.Current.Explorer_TreeMenuToolStripPage_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            //下移
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfterCommand", Language.Current.Explorer_TreeMenuToolStripPage_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
        }
    }

    #endregion

    #region TreeMenuToolStripGroup

    class TreeMenuToolStripGroup : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }

        public TreeMenuToolStripGroup()
            : base("TreeMenuToolStripGroup")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_TreeMenuToolStripGroup_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            //编辑
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_TreeMenuToolStripGroup_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            //删除
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_TreeMenuToolStripGroup_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            //------------
            this.Items.Add(new ToolStripSeparatorCodon());
            //上移
            this.Items.Add(new ToolStripMenuItemCodon("MoveBeforeCommand", Language.Current.Explorer_TreeMenuToolStripGroup_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            //下移
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfterCommand", Language.Current.Explorer_TreeMenuToolStripGroup_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
        }
    }

    #endregion

    #region TreeMenuToolStripItem

    class TreeMenuToolStripItem : ContextMenuStripCodon
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveBeforeCommand { get; set; }
        public ICommand MoveAfterCommand { get; set; }

        public TreeMenuToolStripItem()
            : base("TreeMenuToolStripItem")
        {
            //添加
            this.Items.Add(new ToolStripMenuItemCodon("Add", Language.Current.Explorer_TreeMenuToolStripItem_Add,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddCommand != null) AddCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (AddCommand == null) return false; else return AddCommand.CanExcute(); }
            });
            //编辑
            this.Items.Add(new ToolStripMenuItemCodon("Edit", Language.Current.Explorer_TreeMenuToolStripItem_Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditCommand != null) EditCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (EditCommand == null) return false; else return EditCommand.CanExcute(); }
            });
            //删除
            this.Items.Add(new ToolStripMenuItemCodon("Delete", Language.Current.Explorer_TreeMenuToolStripItem_Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteCommand != null) DeleteCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (DeleteCommand == null) return false; else return DeleteCommand.CanExcute(); }
            });
            //------------
            this.Items.Add(new ToolStripSeparatorCodon());
            //上移
            this.Items.Add(new ToolStripMenuItemCodon("MoveBeforeCommand", Language.Current.Explorer_TreeMenuToolStripItem_MoveBefore,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveBeforeCommand != null) MoveBeforeCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveBeforeCommand == null) return false; else return MoveBeforeCommand.CanExcute(); }
            });
            //下移
            this.Items.Add(new ToolStripMenuItemCodon("MoveAfterCommand", Language.Current.Explorer_TreeMenuToolStripItem_MoveAfter,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.MoveAfterCommand != null) MoveAfterCommand.Excute();
                })
            {
                IsEnabled = (e) => { if (MoveAfterCommand == null) return false; else return MoveAfterCommand.CanExcute(); }
            });
        }
    }

    #endregion
}
