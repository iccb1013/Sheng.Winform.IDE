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
    [NormalUIElementEntityProvide("FormElementTextBoxEntity", 0x000064)]
    public class UIElementTextBoxEntity : UIElement, IUIElementEditControl
    {
        private bool _allowEmpty = true;
        [DefaultValue(true)]
        [CorePropertyRelator("FormElementTextBoxEntity_AllowEmpty", "PropertyCatalog_Normal", 
            Description = "FormElementTextBoxEntity_AllowEmpty_Description", XmlNodeName = "AllowEmpty")]
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
        [CorePropertyRelator("FormElementTextBoxEntity_DataItemId", "PropertyCatalog_Normal", 
            Description = "FormElementTextBoxEntity_DataItemId_Description", XmlNodeName = "DataItemId")]
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
        private int _maxLength = 0;
        [DefaultValue(0)]
        [CorePropertyRelator("FormElementTextBoxEntity_MaxLength", "PropertyCatalog_Normal",
            Description = "FormElementTextBoxEntity_MaxLength_Description", XmlNodeName = "MaxLength")]
        [PropertyNumericUpDownEditorAttribute(NumericMin = 0, NumericMax = Int32.MaxValue)]
        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }
        private bool _multiLine = false;
        [DefaultValue(false)]
        [CorePropertyRelator("FormElementTextBoxEntity_MultiLine", "PropertyCatalog_Normal",
            Description = "FormElementTextBoxEntity_MultiLine_Description", XmlNodeName = "MultiLine")]
        [PropertyBooleanEditorAttribute()]
        public bool MultiLine
        {
            get
            {
                return this._multiLine;
            }
            set
            {
                this._multiLine = value;
            }
        }
        private bool _readOnly = false;
        [DefaultValue(false)]
        [CorePropertyRelator("FormElementTextBoxEntity_ReadOnly", "PropertyCatalog_Normal", 
            Description = "FormElementTextBoxEntity_ReadOnly_Description", XmlNodeName = "ReadOnly")]
        [PropertyBooleanEditorAttribute()]
        public bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
            }
        }
        public const string Property_Regex = "Regex";
        private string _regex;
        public string Regex
        {
            get
            {
                return this._regex;
            }
            set
            {
                this._regex = value;
            }
        }
        public const string Property_RegexMsg = "RegexMsg";
        private string _regexMsg;
        public string RegexMsg
        {
            get
            {
                return this._regexMsg;
            }
            set
            {
                this._regexMsg = value;
            }
        }
        public UIElementTextBoxEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.DataItemId = xmlDoc.GetInnerObject("/DataItem");
            this.AllowEmpty = xmlDoc.GetInnerObject<bool>("/AllowEmpty", true);
            this.MaxLength = xmlDoc.GetInnerObject<int>("/MaxLength", 0);
            this.MultiLine = xmlDoc.GetInnerObject<bool>("/MultiLine", true);
            this.ReadOnly = xmlDoc.GetInnerObject<bool>("/ReadOnly", true);
            this.Regex = xmlDoc.GetInnerObject("/Regex");
            this.RegexMsg = xmlDoc.GetInnerObject("/RegexMsg");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "DataItem", this.DataItemId);
            xmlDoc.AppendChild(String.Empty, "AllowEmpty", this.AllowEmpty);
            xmlDoc.AppendChild(String.Empty, "MaxLength", this.MaxLength);
            xmlDoc.AppendChild(String.Empty, "MultiLine", this.MultiLine);
            xmlDoc.AppendChild(String.Empty, "ReadOnly", this.ReadOnly);
            xmlDoc.AppendChild(String.Empty, "Regex", this.Regex);
            xmlDoc.AppendChild(String.Empty, "RegexMsg", this.RegexMsg);
            return xmlDoc.ToString();
        }
        private static UIElementTextBoxEventTimes _eventTimes;
        public override List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new UIElementTextBoxEventTimes();
                }
                return _eventTimes.Times;
            }
        }
        public override string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new UIElementTextBoxEventTimes();
            }
            return _eventTimes.GetEventName(code);
        }
    }
}
