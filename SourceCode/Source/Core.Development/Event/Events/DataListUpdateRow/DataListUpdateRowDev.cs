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
    class DataListUpdateRowDev : DataListUpdateRowEvent, IEventEditorSupport, ICallEntityMethod, IWarningable
    {
        [NonSerialized]
        private EventEditorAdapterAbstract _editorAdapater;
        [ObjectCompare(Ignore = true)]
        public EventEditorAdapterAbstract EditorAdapter
        {
            get
            {
                if (_editorAdapater == null)
                    _editorAdapater = new DataListUpdateRowDevEditorAdapter(this);
                return _editorAdapater;
            }
        }
        public CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm CallEntityMethodObjectForm
        {
            set
            {
                DataListUpdateRowDevEditorAdapter adapter = _editorAdapater as DataListUpdateRowDevEditorAdapter;
                switch (value)
                {
                    case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current:
                        adapter.ParameterPanels.Data.TargetWindow = EnumTargetWindow.Current;
                        break;
                    case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller:
                        adapter.ParameterPanels.Data.TargetWindow = EnumTargetWindow.Caller;
                        break;
                }
            }
        }
        public object CallEntityMethodFormElement
        {
            set
            {
                DataListUpdateRowDevEditorAdapter adapter = _editorAdapater as DataListUpdateRowDevEditorAdapter;
                UIElement formElement = value as UIElement;
                if (formElement != null)
                    adapter.ParameterPanels.Data.DataListId = formElement.Id;
                else
                    adapter.ParameterPanels.Data.DataListId = value.ToString();
            }
        }
        public static EnumEventDataSource AllowDataDataSourceType
        {
            get
            {
                return EnumEventDataSource.FormElement;
            }
        }
        public static UIElementEntityTypeCollection AllowFormElementControlType
        {
            get
            {
                UIElementEntityTypeCollection collection = new UIElementEntityTypeCollection();
                collection.Add(typeof(UIElementTextBoxEntity));
                collection.Add(typeof(UIElementComboBoxEntity));
                collection.Add(typeof(UIElementDataListTextBoxColumnEntity));
                return collection;
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
            DataListUpdateRowDevChecker.CheckWarning(this);
        }
    }
}
