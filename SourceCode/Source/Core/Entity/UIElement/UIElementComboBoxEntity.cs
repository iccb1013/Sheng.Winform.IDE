/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Sheng.SailingEase.Core;
using System.ComponentModel;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [NormalUIElementEntityProvide("FormElementComboBoxEntity", 0x000065)]
    public class UIElementComboBoxEntity : UIElement, IUIElementEditControl
    {
        private bool _allowEmpty = true;
        [DefaultValue(true)]
        [CorePropertyRelator("FormElementComboBoxEntity_AllowEmpty", "PropertyCatalog_Normal", 
            Description = "FormElementComboBoxEntity_AllowEmpty_Description", XmlNodeName = "AllowEmpty")]
        [PropertyBooleanEditorAttribute()]
        public bool AllowEmpty
        {
            get
            {
                return this._allowEmpty;
            }
            set
            {
                this._allowEmpty = value;
            }
        }
        private string _dataItemId = String.Empty;
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("FormElementComboBoxEntity_DataItemId", "PropertyCatalog_Normal", 
            Description = "FormElementComboBoxEntity_DataItemId_Description", XmlNodeName = "DataItemId")]
        [PropertyDataItemChooseEditorAttribute(ShowDataItem = true)]
        public string DataItemId
        {
            get
            {
                return this._dataItemId;
            }
            set
            {
                this._dataItemId = value;
            }
        }
        public override bool DataSourceUseable
        {
            get
            {
                return true;
            }
        }
        public const string Property_DataSourceMode = "DataSourceMode";
        private EnumComboBoxDataSourceMode _dataSourceMode;
        public EnumComboBoxDataSourceMode DataSourceMode
        {
            get
            {
                return this._dataSourceMode;
            }
            set
            {
                this._dataSourceMode = value;
            }
        }
        public const string Property_EnumId = "EnumId";
        private string _enumId = String.Empty;
        public string EnumId
        {
            get
            {
                return this._enumId;
            }
            set
            {
                this._enumId = value;
            }
        }
        public const string Property_DataEntityId = "DataEntityId";
        private string _dataEntityId = String.Empty;
        public string DataEntityId
        {
            get
            {
                return this._dataEntityId;
            }
            set
            {
                this._dataEntityId = value;
            }
        }
        public const string Property_TextDataItemId = "TextDataItemId";
        private string _textDataItemId = String.Empty;
        public string TextDataItemId
        {
            get
            {
                return this._textDataItemId;
            }
            set
            {
                this._textDataItemId = value;
            }
        }
        public const string Property_ValueDataItemId = "ValueDataItemId";
        private string _valueDataItemId = String.Empty;
        public string ValueDataItemId
        {
            get
            {
                return this._valueDataItemId;
            }
            set
            {
                this._valueDataItemId = value;
            }
        }
        private EnumComboBoxStyle _comboBoxStyle = EnumComboBoxStyle.DropDownList;
        public EnumComboBoxStyle ComboBoxStyle
        {
            get
            {
                return this._comboBoxStyle;
            }
            set
            {
                this._comboBoxStyle = value;
            }
        }
        private string _waterText;
        public string WaterText
        {
            get
            {
                return this._waterText;
            }
            set
            {
                this._waterText = value;
            }
        }
        public UIElementComboBoxEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.DataItemId = xmlDoc.GetInnerObject("/DataItem");
            this.AllowEmpty = xmlDoc.GetInnerObject<bool>("/AllowEmpty", true);
            this.DataSourceMode = (EnumComboBoxDataSourceMode)xmlDoc.GetInnerObject<int>("/DataSourceMode", 0);
            this.EnumId = xmlDoc.GetInnerObject("/EnumId");
            this.DataEntityId = xmlDoc.GetInnerObject("/DataEntityId");
            this.TextDataItemId = xmlDoc.GetInnerObject("/TextDataItemId");
            this.ValueDataItemId = xmlDoc.GetInnerObject("/ValueDataItemId");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "DataItem", this.DataItemId);
            xmlDoc.AppendChild(String.Empty, "AllowEmpty", this.AllowEmpty);
            xmlDoc.AppendChild(String.Empty, "DataSourceMode", (int)this.DataSourceMode);
            xmlDoc.AppendChild(String.Empty, "EnumId", this.EnumId);
            xmlDoc.AppendChild(String.Empty, "DataEntityId", this.DataEntityId);
            xmlDoc.AppendChild(String.Empty, "TextDataItemId", this.TextDataItemId);
            xmlDoc.AppendChild(String.Empty, "ValueDataItemId", this.ValueDataItemId);
            return xmlDoc.ToString();
        }
        private static UIElementComboBoxEventTimes _eventTimes;
        public override List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new UIElementComboBoxEventTimes();
                }
                return _eventTimes.Times;
            }
        }
        public override string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new UIElementComboBoxEventTimes();
            }
            return _eventTimes.GetEventName(code);
        }
        public enum EnumComboBoxDataSourceMode
        {
            [LocalizedDescription("EnumComboBoxDataSourceMode_Enum")]
            Enum = 0,
            [LocalizedDescription("EnumComboBoxDataSourceMode_DataEntity")]
            DataEntity = 1
        }
        public enum EnumComboBoxStyle
        {
            [LocalizedDescription("EnumComboBoxStyle_DropDown")]
            DropDown = 1,
            [LocalizedDescription("EnumComboBoxStyle_DropDownList")]
            DropDownList = 2,
        }
    }
}
