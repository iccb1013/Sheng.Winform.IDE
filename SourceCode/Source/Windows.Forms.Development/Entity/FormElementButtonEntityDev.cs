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
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [Serializable]
    class FormElementButtonEntityDev : UIElementButtonEntity, IFormElementEntityDev, IWarningable
    {
        public FormElementButtonEntityDev()
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
                        _eventProvide.Add(typeof(ExitEvent));
                        _eventProvide.Add(typeof(OpenWindowEvent));
                        _eventProvide.Add(typeof(StartProcessEvent));
                        _eventProvide.Add(typeof(LoadDataToFormEvent));
                        _eventProvide.Add(typeof(NewGuidEvent));
                        _eventProvide.Add(typeof(SaveDataEvent));
                        _eventProvide.Add(typeof(UpdateFormDataEvent));
                        _eventProvide.Add(typeof(OpenSystemFormEvent));
                        _eventProvide.Add(typeof(ClearFormDataEvent));
                        _eventProvide.Add(typeof(CloseFormEvent));
                        _eventProvide.Add(typeof(ValidateFormDataEvent));
                        _eventProvide.Add(typeof(CallAddInEvent));
                        _eventProvide.Add(typeof(ReturnDataToCallerFormEvent));
                        _eventProvide.Add(typeof(DeleteDataEvent));
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
            get { return typeof(SEButtonExDev); }
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
            FormElementButtonEntityDevChecker.CheckWarning(this);
        }
    }
}
