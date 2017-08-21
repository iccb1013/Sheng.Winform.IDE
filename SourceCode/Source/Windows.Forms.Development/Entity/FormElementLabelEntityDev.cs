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
using System.Runtime.Serialization;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [Serializable]
    class FormElementLabelEntityDev : UIElementLabelEntity, IFormElementEntityDev, IWarningable
    {
        public FormElementLabelEntityDev()
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
        }
        [OnDeserialized]
        void AfterDeserialized(StreamingContext ctx)
        {
            base.EventTypesAdapter = EventDevTypes.Instance;
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
            get { return typeof(SELabelExDev); }
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
            FormElementLabelEntityDevChecker.CheckWarning(this);
        }
    }
}
