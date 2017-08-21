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
using System.Xml.Linq;
using System.Xml.XPath;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    public class UIElementDataListColumnDataRules
    {
        [Serializable]
        [UIElementDataListColumnDataRuleProvide("无", 0x000064)]
        public class Normal : UIElementDataListColumnDataRuleAbstract
        {
            public override void FromXml(string strXml)
            {
            }
            public override string ToXml()
            {
                XElement xmlDoc = new XElement("DataRule", 
                        new XAttribute("Type", UIElementDataListColumnDataRuleTypes.Instance.GetProvideAttribute(this).Code));
                return xmlDoc.ToString();
            }
        }
        [Serializable]
        [UIElementDataListColumnDataRuleProvide("关联枚举", 0x000065)]
        public class RelationEnum : UIElementDataListColumnDataRuleAbstract
        {
            private string _enumId = String.Empty;
            public string EnumId
            {
                get { return _enumId; }
                set { _enumId = value; }
            }
            public override void FromXml(string strXml)
            {
                SEXElement xmlDoc = SEXElement.Parse(strXml);
                this.EnumId = xmlDoc.SelectSingleNode("/EnumId").Value;
            }
            public override string ToXml()
            {
                XElement xmlDoc = 
                    new XElement("DataRule", 
                        new XAttribute("Type", UIElementDataListColumnDataRuleTypes.Instance.GetProvideAttribute(this).Code),
                        new XElement("EnumId", this.EnumId));
                return xmlDoc.ToString();
            }
        }
        [Serializable]
        [UIElementDataListColumnDataRuleProvide("关联数据实体", 0x000066)]
        public class RelationDataEntity : UIElementDataListColumnDataRuleAbstract
        {
            private string _dataEntityId = String.Empty;
            public string DataEntityId
            {
                get { return _dataEntityId; }
                set { _dataEntityId = value; }
            }
            private string _valueItemId = String.Empty;
            public string ValueItemId
            {
                get { return _valueItemId; }
                set { _valueItemId = value; }
            }
            private string _displayItemId = String.Empty;
            public string DisplayItemId
            {
                get { return _displayItemId; }
                set { _displayItemId = value; }
            }
            public override void FromXml(string strXml)
            {
                SEXElement xmlDoc = SEXElement.Parse(strXml);
                this.DataEntityId = xmlDoc.SelectSingleNode("/DataEntityId").Value;
                this.ValueItemId = xmlDoc.SelectSingleNode("/ValueItemId").Value;
                this.DisplayItemId = xmlDoc.SelectSingleNode("/DisplayItemId").Value;
            }
            public override string ToXml()
            {
                XElement xmlDoc = new XElement("DataRule", 
                    new XAttribute("Type", UIElementDataListColumnDataRuleTypes.Instance.GetProvideAttribute(this).Code),
                        new XElement("DataEntityId", this.DataEntityId),
                        new XElement("ValueItemId", this.ValueItemId),
                        new XElement("DisplayItemId", this.DisplayItemId));
                return xmlDoc.ToString();
            }
        }
    }
}
