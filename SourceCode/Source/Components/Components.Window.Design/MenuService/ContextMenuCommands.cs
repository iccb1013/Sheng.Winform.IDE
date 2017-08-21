/*
 * 设计器快捷菜单命令
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    #region 抽象基类

    /// <summary>
    /// 所有快捷菜单事件的基类
    /// </summary>
    abstract class AbstractContextMenuCommand
    {
        #region 私有及受保护成员

        protected MenuCommandService _menuCommandService;

        #endregion

        #region 公开属性

        public virtual CommandID CommandID
        {
            get
            {
                return null;
            }
        }

        public abstract string Text
        {
            get;
        }

        /// <summary>
        /// 确定此命令是否可用
        /// </summary>
        /// <returns></returns>
        public virtual bool Enabled
        {
            get
            {
                if (FormHostingContainer.Instance.ActiveHosting == null)
                    return false;

                Debug.Assert(this.CommandID != null, "this.CommandID 为 null");

                if (this.CommandID == null)
                    return false;

                return FormHostingContainer.Instance.ActiveHosting.IsMenuCommandEnabled(this.CommandID);
            }
        }

        #endregion

        #region 公开方法

        public AbstractContextMenuCommand(MenuCommandService menuCommandService)
        {
            _menuCommandService = menuCommandService;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 执行命令
        /// </summary>
        public virtual void Run()
        {
            Debug.Assert(this.CommandID != null, "this.CommandID 为 null");

            if (this.CommandID != null)
                this._menuCommandService.GlobalInvoke(this.CommandID);
        }

        internal virtual void CommandCallBack(object sender, EventArgs e)
        {
            this.Run();
        }

        #endregion
    }

    #endregion

    #region 显示事件

    /// <summary>
    /// 显示事件
    /// 如果选中了多个控件,不可用
    /// </summary>
    class ContextMenuCommandShowEvents : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.ViewCode;
            }
        }

        public ContextMenuCommandShowEvents(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandShowEvents;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (FormHostingContainer.Instance.ActiveHosting == null)
                    return false;

                  //如果多选不可用
                return FormHostingContainer.Instance.ActiveHosting.EnableEvent;
            }
        }

        public override void Run()
        {
            using (FormDesignerEvents formEvents = new FormDesignerEvents())
            {
                formEvents.ShowDialog();
            }
        }
    }

    #endregion

    #region 显示属性

    /// <summary>
    /// 显示属性
    /// </summary>
    class ContextMenuCommandProperties : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.Properties;
            }
        }

        public ContextMenuCommandProperties(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandProperties;
            }
        }

        public override void Run()
        {
            //if (FormDesignerManager.ActiveFormFormDesinger != null)
            //{
            //    FormDesignerManager.ActiveFormFormDesinger.ShowProperties();
            //}

            FormHostingContainer.Instance.ShowProperties();
        }
    }

    #endregion

    #region 全选

    /// <summary>
    /// 全选
    /// </summary>
    class ContextMenuCommandSelectAll : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.SelectAll;
            }
        }

        public ContextMenuCommandSelectAll(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandSelectAll ;
            }
        }
    }

    #endregion

    #region 剪切

    /// <summary>
    /// 剪切
    /// </summary>
    class ContextMenuCommandCut : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.Cut;
            }
        }

        public ContextMenuCommandCut(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandCut;
            }
        }
    }

    #endregion

    #region 粘贴

    /// <summary>
    /// 粘贴
    /// </summary>
    class ContextMenuCommandPaste : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.Paste;
            }
        }

        public ContextMenuCommandPaste(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandPaste;
            }
        }

    }

    #endregion

    #region 复制

    /// <summary>
    /// 复制
    /// </summary>
    class ContextMenuCommandCopy : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.Copy;
            }
        }

        public ContextMenuCommandCopy(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandCopy;
            }
        }

    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除
    /// </summary>
    class ContextMenuCommandDelete : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.Delete;
            }
        }

        public ContextMenuCommandDelete(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandDelete;
            }
        }
    }

    #endregion

    #region 锁定控件

    /// <summary>
    /// 锁定控件
    /// </summary>
    class ContextMenuCommandLockControls : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.LockControls;
            }
        }

        public ContextMenuCommandLockControls(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandLockControls;
            }
        }
    }

    #endregion

    #region TabOrder

    /// <summary>
    /// TabOrder
    /// </summary>
    class ContextMenuCommandTabOrder : AbstractContextMenuCommand
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.TabOrder;
            }
        }

        public ContextMenuCommandTabOrder(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandTabOrder ;
            }
        }
    }

    #endregion

    #region ZOrder 操作基类



    class ContextMenuCommandZOrder : AbstractContextMenuCommand
    {
        //在继承者的构造中赋值
        protected SEUndoUnitZOrder.EnumSendTo _sendTo;

        public ContextMenuCommandZOrder(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {

        }

        public override string Text
        {
            get
            {
                return String.Empty;
            }
        }

        public override void Run()
        {
            //1.获取当前选定的全部对象
            //2.封装可撤销的工作单元，把对象和他们对应的当前ZOrder保存进去,把CommandID保存进去
            //3.Undo操作就设置控件的ZOrder,Redo操作 就调用CommandID

            ISelectionService selectionService = FormHostingContainer.Instance.ActiveHosting.SelectionService;
            ICollection collection = selectionService.GetSelectedComponents();
            List<Control> controls = new List<Control>();
            Control control;
            foreach (IComponent component in collection)
            {
                control = component as Control;
                if (control == null || control.Parent == null)
                    continue;

                controls.Add(control);
            }

            SEUndoUnitZOrder undoUnit = new SEUndoUnitZOrder(_sendTo, _menuCommandService, this.CommandID);
            IShellControlDev controlDev;
            foreach (Control crl in controls)
            {
                controlDev = crl as IShellControlDev;
                undoUnit.Members.Add(new SEUndoUnitZOrderMember((UIElement)controlDev.Entity, crl.Parent.Controls.GetChildIndex(crl)));
            }

            //调用命令
            this._menuCommandService.GlobalInvoke(this.CommandID);

            foreach (Control crl in controls)
            {
                controlDev = crl as IShellControlDev;
                undoUnit.SetNewOrder(controlDev.Entity, crl.Parent.Controls.GetChildIndex(crl));
            }

            FormHostingContainer.Instance.ActiveHosting.AddUndoUnit(undoUnit);
        }
    }

    #endregion

    #region 向后移动

    /// <summary>
    /// 向后移动
    /// </summary>
    class ContextMenuCommandSendBackward : ContextMenuCommandZOrder
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.SendBackward;
            }
        }

        public ContextMenuCommandSendBackward(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {
            _sendTo = SEUndoUnitZOrder.EnumSendTo.Backward;
        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandSendBackward;
            }
        }
    }

    #endregion

    #region 向前移动

    /// <summary>
    /// 向前移动
    /// </summary>
    class ContextMenuCommandBringForward : ContextMenuCommandZOrder
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.BringForward;
            }
        }

        public ContextMenuCommandBringForward(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {
            _sendTo = SEUndoUnitZOrder.EnumSendTo.Forward;
        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandBringForward;
            }
        }

    }

    #endregion

    #region 置于顶层

    /// <summary>
    /// 置于顶层
    /// </summary>
    class ContextMenuCommandBringToFront : ContextMenuCommandZOrder
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.BringToFront;
            }
        }

        public ContextMenuCommandBringToFront(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {
            _sendTo = SEUndoUnitZOrder.EnumSendTo.Forward;
        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandBringToFront;
            }
        }
    }

    #endregion

    #region 置于底层

    /// <summary>
    /// 置于底层
    /// </summary>
    class ContextMenuCommandSendToBack : ContextMenuCommandZOrder
    {
        public override CommandID CommandID
        {
            get
            {
                return StandardCommands.SendToBack;
            }
        }

        public ContextMenuCommandSendToBack(MenuCommandService menuCommandService)
            : base(menuCommandService)
        {
            _sendTo = SEUndoUnitZOrder.EnumSendTo.Backward;
        }

        public override string Text
        {
            get
            {
                return Language.Current.ContextMenuCommandSendToBack;
            }
        }
    }

    #endregion
}
