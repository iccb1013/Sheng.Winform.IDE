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
using Sheng.SailingEase.ComponentModel.Design.Localisation;
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public class SEUndoUnitStandard : SEUndoUnitAbstract
    {
        private string _name;
        public override  string Name
        {
            get
            {
                if (String.IsNullOrEmpty(this._name))
                {
                    return String.Format(Language.Current.SEUndoUnit_Set, this.Members.Members);
                }
                else
                {
                    return this._name;
                }
            }
        }
        private SEUndoMemberCollection _members;
        public SEUndoMemberCollection Members
        {
            get
            {
                if (_members == null)
                    _members = new SEUndoMemberCollection();
                return this._members;
            }
            set { this._members = value; }
        }
        private object _value;
        public object Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        public SEUndoUnitStandard()
        {
        }
        public SEUndoUnitStandard(string name)
        {
            _name = name;
        }
        public override void Undo(bool action)
        {
            foreach (SEUndoMember member in this.Members)
            {
                member.SetMember(Value, SEUndoMember.EnumMemberValue.OldValue);
            }
            if (action && Action != null)
            {
                Action(this, SEUndoEngine.Type.Undo);
            }
        }
        public override void Redo(bool action)
        {
            foreach (SEUndoMember member in this.Members)
            {
                member.SetMember(Value, SEUndoMember.EnumMemberValue.NewValue);
            }
            if (action && Action != null)
            {
                Action(this,SEUndoEngine.Type.Redo);
            }
        }
    }
}
