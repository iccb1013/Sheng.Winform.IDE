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
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [Serializable]
    class FormElementDataListTextBoxColumnEntityDev : UIElementDataListTextBoxColumnEntity, IFormElementEntityDev, IWarningable
    {
        IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        public new bool Enabled
        {
            get { return base.Enabled; }
        }
        public new string BackColorValue
        {
            get { return base.BackColorValue; }
        }
        public new string ForeColorValue
        {
            get { return base.ForeColorValue; }
        }
        public new int Height
        {
            get { return base.Height; }
        }
        public new UIElementAnchor Anchor
        {
            get { return base.Anchor; }
        }
        public new int Top
        {
            get { return base.Top; }
        }
        public new int Left
        {
            get { return base.Left; }
        }
        public new UIElementFont Font
        {
            get { return base.Font; }
        }
        public override string FullName
        {
            get
            {
                return this.DataList.Name + "." + this.Name;
            }
        }
        public FormElementDataListTextBoxColumnEntityDev()
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
            base.DataRuleTypesAdapter = FormElementDataListColumnDataRuleDevTypes.Instance;
        }
        [OnDeserialized]
        void AfterDeserialized(StreamingContext ctx)
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
            base.DataRuleTypesAdapter = FormElementDataListColumnDataRuleDevTypes.Instance;
        }
        public string GetDataItemCode()
        {
            if (this.IsBind)
            {
                string[] ids = this.DataItemId.Split('.');
                string dataEntityId = ids[0];
                string dataItemId = ids[1];
                return _dataEntityComponentService.GetDataItemEntity(dataItemId,dataEntityId).Code;
            }
            else
            {
                return this.DataPropertyName;
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
            get { throw new NotImplementedException(); }
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
            FormElementDataListTextBoxColumnEntityDevChecker.CheckWarning(this);
        }
    }
}
