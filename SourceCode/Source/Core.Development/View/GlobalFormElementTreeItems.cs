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
using Sheng.SailingEase.Infrastructure;
using System.Diagnostics;
namespace Sheng.SailingEase.Core.Development
{
    abstract class GlobalFormElementTreeItemBase
    {
        public GlobalFormElementTreeItemBase(EntityBase entity)
        {
            this.Entity = entity;
        }
        private EntityBase _entity;
        public EntityBase Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                this._entity = value;
            }
        }
        public Image Icon
        {
            get
            {
                    return new Bitmap(1, 1);
            }
        }
        public virtual string Name
        {
            get
            {
                return this.Entity.Name;
            }
        }
        public virtual string Code
        {
            get
            {
                return this.Entity.Code;
            }
        }
        public virtual string Remark
        {
            get
            {
                return this.Entity.Remark;
            }
        }
        public abstract string ControlTypeName
        {
            get;
        }
        private GlobalFormElementTreeItemBase _parent;
        public GlobalFormElementTreeItemBase Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
    }
    class FolderItem : GlobalFormElementTreeItemBase
    {
        private WindowFolderEntity _entity;
        public WindowFolderEntity Folder
        {
            get { return _entity; }
        }
        public FolderItem(WindowFolderEntity entity)
            : base(null)
        {
            _entity = entity;
        }
        public override string Name
        {
            get
            {
                return _entity.Name;
            }
        }
        public override string Code
        {
            get
            {
                return String.Empty;
            }
        }
        public override string Remark
        {
            get
            {
                return String.Empty;
            }
        }
        public override string ControlTypeName
        {
            get { return String.Empty; }
        }
    }
    class FormItem : GlobalFormElementTreeItemBase
    {
        public FormItem(EntityBase entity,GlobalFormElementTreeItemBase parent)
            : base(entity)
        {
            this.Parent = parent;
        }
        public override string ControlTypeName
        {
            get { return String.Empty; }
        }
    }
    class FormElementItem : GlobalFormElementTreeItemBase
    {
        UIElement _formElement;
        public FormElementItem(EntityBase entity,GlobalFormElementTreeItemBase parent)
            : base(entity)
        {
            this.Parent = parent;
            _formElement = (UIElement)entity;
        }
        public override string ControlTypeName
        {
            get
            {
                return FormElementEntityDevTypes.Instance.GetName(this._formElement);
            }
        }
    }
}
