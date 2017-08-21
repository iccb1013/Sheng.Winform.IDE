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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core.Development
{
    [Serializable]
     class ValidateFormDataDev : ValidateFormDataEvent, IEventEditorSupport, IWarningable
    {
        [NonSerialized]
        private EventEditorAdapterAbstract _editorAdapater;
        [ObjectCompare(Ignore = true)]
        public EventEditorAdapterAbstract EditorAdapter
        {
            get
            {
                if (_editorAdapater == null)
                    _editorAdapater = new ValidateFormDataDevEditorAdapter(this);
                return _editorAdapater;
            }
        }
        public string WarningSignName
        {
            get { return this.Name; }
        }
        [NonSerialized]
        private WarningSign _warning;
        [ObjectCompare(Ignore = true)]
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
            ValidateFormDataDevChecker.CheckWarning(this);
        }
    }
}
