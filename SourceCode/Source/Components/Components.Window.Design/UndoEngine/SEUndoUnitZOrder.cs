/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    
    class SEUndoUnitZOrder : SEUndoUnitAbstract
    {
        private CommandID _commandId;
        private MenuCommandService _menuCommandService;
        public override string Name
        {
            get { return "改变控件顺序"; }
        }
        public List<SEUndoUnitZOrderMember> _members = new List<SEUndoUnitZOrderMember>();
        public List<SEUndoUnitZOrderMember> Members
        {
            get { return _members; }
        }
        private EnumSendTo _sendTo;
        public EnumSendTo SendTo
        {
            get { return _sendTo; }
        }
        public SEUndoUnitZOrder(EnumSendTo sendTo, MenuCommandService menuCommandService, CommandID commandID)
        {
            _sendTo = sendTo;
            _menuCommandService = menuCommandService;
            _commandId = commandID;
        }
        public override void Undo(bool action)
        {
           
            SEUndoUnitZOrderMember[] members = null;
            if (_sendTo == EnumSendTo.Forward)
            {
                var a = from c in this.Members orderby c.OldOrder descending select c;
                members = a.ToArray();
            }
            else
            {
                var a = from c in this.Members orderby c.OldOrder ascending select c;
                members = a.ToArray();
            }
            foreach (SEUndoUnitZOrderMember member in members)
            {
                IFormElementEntityDev entityDev = member.Entity as IFormElementEntityDev;
                Debug.Assert(entityDev != null, "实体对象没有实现 IFormElementEntityDev");
                if (entityDev == null)
                    return;
                Control control = entityDev.Component as Control;
                Debug.Assert(control.Parent != null, "control.Parent 为 null");
                control.Parent.Controls.SetChildIndex(control, member.OldOrder);
                Helper.ZOrderSync(member.Entity);
            }
            if (action && Action != null)
            {
                Action(this, SEUndoEngine.Type.Redo);
            }
        }
        public override void Redo(bool action)
        {
           
            SEUndoUnitZOrderMember[] members = null;
            if (_sendTo == EnumSendTo.Forward)
            {
                var a = from c in this.Members orderby c.NewOrder ascending select c;
                members = a.ToArray();
            }
            else
            {
                var a = from c in this.Members orderby c.NewOrder descending select c;
                members = a.ToArray();
            }
            foreach (SEUndoUnitZOrderMember member in members)
            {
                IFormElementEntityDev entityDev = member.Entity as IFormElementEntityDev;
                Debug.Assert(entityDev != null, "实体对象没有实现 IFormElementEntityDev");
                if (entityDev == null)
                    return;
                Control control = entityDev.Component as Control;
                Debug.Assert(control.Parent != null, "control.Parent 为 null");
                control.Parent.Controls.SetChildIndex(control, member.NewOrder);
                Helper.ZOrderSync(member.Entity);
            }
            if (action && Action != null)
            {
                Action(this, SEUndoEngine.Type.Redo);
            }
        }
        public void SetNewOrder(EntityBase entity, int newOrder)
        {
            foreach (SEUndoUnitZOrderMember member in this.Members)
            {
                if (member.Entity == entity)
                {
                    member.NewOrder = newOrder;
                    break;
                }
            }
        }
        public enum EnumSendTo
        {
            Backward,
            Forward
        }
    }
    class SEUndoUnitZOrderMember
    {
        public UIElement Entity
        {
            get;
            set;
        }
        public int OldOrder
        {
            get;
            set;
        }
        public int NewOrder
        {
            get;
            set;
        }
        public SEUndoUnitZOrderMember(UIElement entity, int oldOrder)
        {
            this.Entity = entity;
            this.OldOrder = oldOrder;
        }
    }
}
