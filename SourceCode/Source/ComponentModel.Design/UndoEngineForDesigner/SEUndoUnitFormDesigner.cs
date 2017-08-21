using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.ComponentModel.Design.Localisation;

namespace Sheng.SailingEase.ComponentModel.Design
{
    //放在这里是因为  Windows.Forms.Development 要用
    /// <summary>
    /// 为窗体设计器提供撤销/重做功能
    /// </summary>
    public class SEUndoUnitFormDesigner : SEUndoUnitAbstract
    {
        #region 私有成员

        /// <summary>
        /// 窗体设计器专用的撤销重做引擎（从UndoEngine继承）
        /// </summary>
        private SEUndoEngineFormDesigner UndoEngineFormDesinger
        {
            get
            {
                return this.UndoEngine as SEUndoEngineFormDesigner;
            }
        }

        #endregion

        #region 公开属性

        private string _name = String.Empty;
        public override string Name
        {
            get
            {
                string name = String.Format(Language.Current.SEUndoUnit_Name_SetProperty, this._name);

                switch (this.Type)
                {
                    case UndoUnitType.ComponentChanged:
                        if (this.Members.Count > 0)
                        {
                            switch (this.Members[0].MemberName)
                            {
                                case "Location":
                                case "Left":
                                case "Top":
                                    name = Language.Current.SEUndoUnit_Name_MoveControl; 
                                    break;
                                case "Size":
                                case "Width":
                                case "Height":
                                    name = Language.Current.SEUndoUnit_Name_ResizeControl;
                                    break;
                            }
                        }
                        break;
                    case UndoUnitType.ComponentAdded:
                        name = Language.Current.SEUndoUnit_Name_AddControl;
                        break;
                    case UndoUnitType.ComponentRemoved:
                        name = Language.Current.SEUndoUnit_Name_RemoveControl;
                        break;
                }

                return name;
            }
        }

        private UndoUnitType _type;
        /// <summary>
        /// 可撤销的工作单元的类型
        /// </summary>
        public UndoUnitType Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

        
        private IComponent _components;
        /// <summary>
        /// 设计器组件，仅用于ComponentAdded情况时拿Entity
        /// </summary>
        public IComponent Components
        {
            get { return this._components; }
            set { this._components = value; }
        }

        private EntityBase _entity;
        /// <summary>
        /// 实体对象
        /// 用于UpdateView，以及创建，销毁对象等操作
        /// </summary>
        public EntityBase Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;

                if (this.Value == null)
                    this.Value = value;
            }
        }

        /// <summary>
        /// 此工作单元直接作用的对象
        /// 一般即是EntityBase实体对象，但如果是实体对象的子对象时，这里存储的就是子对象的引用
        /// 子对象可能不派生自EntityBase
        /// 典型的情况如字体
        /// </summary>
        public object Value
        {
            get;
            set;
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

        /// <summary>
        /// 此工作单元是否涉及组件的添加或删除 
        /// </summary>
        public bool ComponentChanged
        {
            get
            {
                if (_type == UndoUnitType.ComponentAdded || _type == UndoUnitType.ComponentRemoved)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region 构造

        public SEUndoUnitFormDesigner(string name)
        {
            _name = name;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 撤销
        /// </summary>
        public override void Undo(bool action)
        {
            switch (Type)
            {
                case UndoUnitType.ComponentChanged:
                    ComponentChange(SEUndoMember.EnumMemberValue.OldValue);
                    break;
                case UndoUnitType.ComponentAdded:
                    ComponentRemove();
                    break;
                case UndoUnitType.ComponentRemoved:
                    ComponentAdd();
                    break;
            }

            if (action && Action != null)
            {
                Action(this,SEUndoEngine.Type.Undo);
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public override void Redo(bool action)
        {
           

            switch (Type)
            {
                case UndoUnitType.ComponentChanged:
                    ComponentChange(SEUndoMember.EnumMemberValue.NewValue);
                    break;
                case UndoUnitType.ComponentAdded:
                    ComponentAdd();
                    break;
                case UndoUnitType.ComponentRemoved:
                    ComponentRemove();
                    break;
            }

            if (action && Action != null)
            {
                Action(this,SEUndoEngine.Type.Redo);
            }
        }

        public override string ToString()
        {
            //return base.ToString();
            StringBuilder str = new StringBuilder(this._name);

            str.Append(" - ");

            foreach (SEUndoMember member in this.Members)
            {
                str.Append(member.ToString());
            }

            return str.ToString();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="values"></param>
        private void ComponentChange(SEUndoMember.EnumMemberValue enumMemberValue)
        {
            //foreach (object obj in this.Entitys)
            //{
                foreach (SEUndoMember member in this.Members)
                {
                    member.SetMember(this.Value, enumMemberValue);
                }
            //}

           
            UndoEngineFormDesinger.UpdateView(this.Entity);
            //UndoEngineFormDesinger.FormFormDesinger.UpdatePropertyGrid(false);
            UndoEngineFormDesinger.UpdatePropertyGrid(false);
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        private void ComponentAdd()
        {
            

            UndoEngineFormDesinger.CreateControl(this.Entity as UIElement);
            //}
        }

        /// <summary>
        /// 删除组件
        /// </summary>
        private void ComponentRemove()
        {
            //foreach (FormElement obj in this.Entitys)
            //{
                //IShellControlDev shellControl = obj as IShellControlDev;
                //if (shellControl == null)
                //    continue;

                UndoEngineFormDesinger.DestroyControl(this.Entity as UIElement);
            //}
        }

        #endregion

        #region UndoUnitType Enum

        /// <summary>
        /// 可撤销的工作单元的类型
        /// </summary>
        public enum UndoUnitType
        {
            /// <summary>
            /// 添加组件
            /// </summary>
            ComponentAdded,
            /// <summary>
            /// 更改组件
            /// </summary>
            ComponentChanged,
            /// <summary>
            /// 删除组件
            /// </summary>
            ComponentRemoved
        }

        #endregion
    }
}
