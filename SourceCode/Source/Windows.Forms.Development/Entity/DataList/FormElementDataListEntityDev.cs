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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [Serializable]
    class FormElementDataListEntityDev : UIElementDataListEntity, ICallEntityMethodSupport, IFormElementEntityDev, IWarningable
    {
        public FormElementDataListEntityDev()
            : base()
        {
            this.Width = 240;
            this.Height = 150;
            base.ColumnEntityTypesAdapter = FormElementDataListColumnEntityDevTypes.Instance;
            base.EventTypesAdapter = EventDevTypes.Instance;
        }
        [OnDeserialized]
        void AfterDeserialized(StreamingContext ctx)
        {
            base.ColumnEntityTypesAdapter = FormElementDataListColumnEntityDevTypes.Instance;
            base.EventTypesAdapter = EventDevTypes.Instance;
        }
        public UIElementDataListColumnEntityAbstract GetDataColumnDev(string id)
        {
            return this.DataColumns.GetEntityById(id);
        }
        public EventTypeCollection GetAvailabilityEntityMethod(CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm objectForm)
        {
            EventTypeCollection availabilityEvent = new EventTypeCollection();
            switch (objectForm)
            {
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current:
                    availabilityEvent.Add(typeof(DataListRefreshEvent));
                    availabilityEvent.Add(typeof(DataListAddRowEvent));
                    availabilityEvent.Add(typeof(DataListUpdateRowEvent));
                    availabilityEvent.Add(typeof(DataListDeleteRowEvent));
                    break;
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller:
                    availabilityEvent.Add(typeof(DataListAddRowEvent));
                    availabilityEvent.Add(typeof(DataListUpdateRowEvent));
                    availabilityEvent.Add(typeof(DataListDeleteRowEvent));
                    break;
            }
            return availabilityEvent;
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
            get { return typeof(SEDataGridViewDev); }
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
            FormElementDataListEntityDevChecker.CheckWarning(this);
        }
        public class AddColumnCommand : CommandAbstract
        {
            private FormElementDataListEntityDev _entity;
            public UIElementDataListColumnEntityAbstract Column { get; set; }
            public AddColumnCommand(FormElementDataListEntityDev entity)
            {
                this._entity = entity;
            }
            public override void Execute()
            {
                this._entity.DataColumns.Add(this.Column);
                SEUndoUnitCollectionEdit undoUnit = new SEUndoUnitCollectionEdit(Language.Current.FormElementDataListEntityDev_Column)
                {
                    List = this._entity.DataColumns,
                    EditType = CollectionEditType.Add,
                    Value = this.Column,
                    Index = this._entity.DataColumns.IndexOf(this.Column),
                };
                this.UndoUnit = undoUnit;
            }
        }
    }
}
