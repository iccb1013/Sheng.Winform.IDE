/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [Serializable]
    class FormElementComboBoxEntityDev : UIElementComboBoxEntity, IFormElementEntityDev, IWarningable
    {
        public FormElementComboBoxEntityDev()
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
        }
        [OnDeserialized]
        void AfterDeserialized(StreamingContext ctx)
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
        }
        private object objLock = new object();
        private static EventTypeCollection _eventProvide;
        public override EventTypeCollection EventProvide
        {
            get
            {
                if (_eventProvide == null)
                    lock (objLock)
                    {
                        _eventProvide = new EventTypeCollection();
                        _eventProvide.Add(typeof(OpenWindowEvent));
                        _eventProvide.Add(typeof(StartProcessEvent));
                        _eventProvide.Add(typeof(LoadDataToFormEvent));
                        _eventProvide.Add(typeof(NewGuidEvent));
                        _eventProvide.Add(typeof(ClearFormDataEvent));
                        _eventProvide.Add(typeof(CloseFormEvent));
                        _eventProvide.Add(typeof(CallAddInEvent));
                        _eventProvide.Add(typeof(ReturnDataToCallerFormEvent));
                        _eventProvide.Add(typeof(CallUIElementMethodEvent));
                    }
                return _eventProvide;
            }
        }
        [NonSerialized]
        private System.ComponentModel.IComponent _component;
        public System.ComponentModel.IComponent Component
        {
            get { return this._component; }
            set { this._component = value; }
        }
        public Type DesignerControlType
        {
            get { return typeof(SEComboBoxExDev); }
        }
        public string WarningSignName
        {
            get { return this.Name; }
        }
        [NonSerialized]
        private WarningSign _warning;
        public WarningSign Warning
        {
            get
            {
                if (_warning == null)
                    _warning = new WarningSign(this);
                return _warning;
            }
            set { _warning = value; }
        }
        public void CheckWarning()
        {
            FormElementComboBoxEntityDevChecker.CheckWarning(this);
        }
        public class ModifyDataRuleCommand : CommandAbstract
        {
            private FormElementComboBoxEntityDev _entity;
            public EnumComboBoxDataSourceMode DataSourceMode { get; set; }
            public string DataEntityId { get; set; }
            public string TextDataItemId { get; set; }
            public string ValueDataItemId { get; set; }
            public string EnumId { get; set; }
            public ModifyDataRuleCommand(FormElementComboBoxEntityDev entity)
            {
                this._entity = entity;
            }
            public override void Execute()
            {
                SEUndoUnitStandard undoUnit = new SEUndoUnitStandard(
                    String.Format(Language.Current.SEUndoUnit_Set, Language.Current.FormElementComboBoxEntityDev_DataRule));
                undoUnit.Value = this._entity;
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_DataSourceMode, this._entity.DataSourceMode, this.DataSourceMode);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_DataEntityId, this._entity.DataEntityId, this.DataEntityId);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_TextDataItemId, this._entity.TextDataItemId, this.TextDataItemId);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_ValueDataItemId, this._entity.ValueDataItemId, this.ValueDataItemId);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_EnumId, this._entity.EnumId, this.EnumId);
                this.UndoUnit = undoUnit;
                this._entity.DataSourceMode = this.DataSourceMode;
                this._entity.DataEntityId = this.DataEntityId;
                this._entity.TextDataItemId = this.TextDataItemId;
                this._entity.ValueDataItemId = this.ValueDataItemId;
                this._entity.EnumId = this.EnumId;
            }
        }
        public class ClearDataRuleCommand : CommandAbstract
        {
            private FormElementComboBoxEntityDev _entity;
            public ClearDataRuleCommand(FormElementComboBoxEntityDev entity)
            {
                this._entity = entity;
            }
            public override void Execute()
            {
                SEUndoUnitStandard undoUnit = new SEUndoUnitStandard(
                    String.Format(Language.Current.SEUndoUnit_Set, Language.Current.FormElementComboBoxEntityDev_DataRule));
                undoUnit.Value = this._entity;
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_DataSourceMode, this._entity.DataSourceMode, UIElementComboBoxEntity.EnumComboBoxDataSourceMode.Enum);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_DataEntityId, this._entity.DataEntityId, String.Empty);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_TextDataItemId, this._entity.TextDataItemId, String.Empty);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_ValueDataItemId, this._entity.ValueDataItemId, String.Empty);
                undoUnit.Members.Add(UIElementComboBoxEntity.Property_EnumId, this._entity.EnumId, String.Empty);
                this.UndoUnit = undoUnit;
                this._entity.DataSourceMode = UIElementComboBoxEntity.EnumComboBoxDataSourceMode.Enum;
                this._entity.DataEntityId = String.Empty;
                this._entity.TextDataItemId = String.Empty;
                this._entity.ValueDataItemId = String.Empty;
                this._entity.EnumId = String.Empty;
            }
        }
    }
}
