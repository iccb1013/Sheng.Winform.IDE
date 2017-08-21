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
using System.Xml;
using System.Runtime.Serialization;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [Serializable]
    class FormElementTextBoxEntityDev : UIElementTextBoxEntity, IFormElementEntityDev, IWarningable
    {
        public FormElementTextBoxEntityDev()
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
                        _eventProvide.Add(typeof(CallAddInEvent));
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
            get { return typeof(SETextBoxExDev); }
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
            FormElementTextBoxEntityDevChecker.CheckWarning(this);
        }
        public class ModifyDataRuleCommand : CommandAbstract
        {
            private FormElementTextBoxEntityDev _entity;
            public string Regex { get; set; }
            public string RegexMsg { get; set; }
            public ModifyDataRuleCommand(FormElementTextBoxEntityDev entity)
            {
                this._entity = entity;
            }
            public override void Execute()
            {
                SEUndoUnitStandard undoUnit = new SEUndoUnitStandard(
                    String.Format(Language.Current.SEUndoUnit_Set, Language.Current.FormElementTextboxEntityDev_DataRule));
                undoUnit.Value = this._entity;
                undoUnit.Members.Add(UIElementTextBoxEntity.Property_Regex, this._entity.Regex, this.Regex);
                undoUnit.Members.Add(UIElementTextBoxEntity.Property_RegexMsg, this._entity.RegexMsg, this.RegexMsg);
                this.UndoUnit = undoUnit;
                this._entity.Regex = this.Regex;
                this._entity.RegexMsg = this.RegexMsg;
            }
        }
    }
}
