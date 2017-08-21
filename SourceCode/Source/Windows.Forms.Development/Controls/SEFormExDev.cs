/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class SEFormExDev : Form, IShellControlDev, IWindowDesignerRootComponent
    {
        private EntityBase _entity;
        private WindowEntity _windowEntity;
        public SEFormExDev()
        {
        }
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
            _windowEntity.Size = this.Size;
            _windowEntity.ClientHeight = this.ClientSize.Height;
            _windowEntity.ClientWidth = this.ClientSize.Width;
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
            return this._windowEntity.ControlTypeName;
        }
        public void ClearEntity()
        {
            this._entity = null;
            this._windowEntity = null;
        }
        public string GetText()
        {
            return this._windowEntity.Text;
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EntityBase Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                this._entity = value;
                this._windowEntity = (WindowEntity)value;
                IFormElementEntityDev entityDev = value as IFormElementEntityDev;
                if (entityDev != null)
                {
                    entityDev.Component = this;
                }
            }
        }
        public EventCollection GetEvents()
        {
            return this._windowEntity.Events;
        }
        public void InitializationEntity(EntityBase entity)
        {
        }
    }
}
