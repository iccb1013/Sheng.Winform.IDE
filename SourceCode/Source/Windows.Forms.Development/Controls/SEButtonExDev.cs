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
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using System.Diagnostics;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(Button))]
    [Description(ConstantLanguage.SEButtonExDev_Description)]
    [RuntimeControlToolboxItem(ConstantLanguage.SEButtonExDev_Name, ConstantLanguage.SEButtonExDev_Catalog, typeof(SEButtonExDev))]
    [RuntimeControlDesignSupportAttribute(typeof(FormElementButtonEntityDev), "Button")]
    class SEButtonExDev : Button, IShellControlDev
    {
        EntityBase _entity;
        [NonSerialized]
        UIElement _formElement;
        private bool _viewUpdating = false;
        public bool ViewUpdating
        {
            get { return this._viewUpdating; }
            set { this._viewUpdating = value; }
        }
        public void UpdateView()
        {
            this.ViewUpdating = true;
            ShellControlHelper.PropertyDescriptorUpdate(this, this._entity);
            this.ViewUpdating = false;
        }
        public void UpdateEntity()
        {
            ShellControlHelper.UpdateEntity(this._formElement, this);
        }
        public string GetCode()
        {
            return this._entity.Code;
        }
        public string GetName()
        {
            return this._entity.Name;
        }
        public string GetControlTypeName()
        {
            return FormElementEntityDevTypes.Instance.GetName( this._formElement);
        }
        public void ClearEntity()
        {
            this._entity = null;
            this._formElement = null;
        }
        public string GetText()
        {
            return this._formElement.Text;
        }
        public EventCollection GetEvents()
        {
            return this._formElement.Events;
        }
        public EntityBase Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                ShellControlHelper.ReplaceControlEntity(_formElement, (UIElement)value);
                this._entity = value;
                this._formElement = (UIElement)value;
                IFormElementEntityDev entityDev = value as IFormElementEntityDev;
                if (entityDev != null)
                {
                    entityDev.Component = this;
                }                
            }
        }
        public void InitializationEntity(EntityBase entity)
        {
            FormElementButtonEntityDev buttonEntity = entity as FormElementButtonEntityDev;
            if (buttonEntity == null)
            {
                Debug.Assert(false, "entity 为空或类型不对");
                return;
            }
            buttonEntity.Text = buttonEntity.Code;
        }
    }
}
