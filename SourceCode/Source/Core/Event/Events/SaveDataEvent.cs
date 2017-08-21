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
using System.Xml.Linq;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("��������", 0x00006C,"�����ݱ��浽����ʵ����")]
    public class SaveDataEvent:EventBase
    {
        private EnumSaveDataMode _saveMode;
        public EnumSaveDataMode SaveMode
        {
            get
            {
                return this._saveMode;
            }
            set
            {
                this._saveMode = value;
            }
        }
        private string _dataEntityId;
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
        private XmlableCollection<SaveItem> _save = new XmlableCollection<SaveItem>("Save");
        public XmlableCollection<SaveItem> Save
        {
            get { return _save; }
            set { _save = value; }
        }
        private string _sqlRegex;
        public string SqlRegex
        {
            get
            {
                return this._sqlRegex;
            }
            set
            {
                this._sqlRegex = value;
            }
        }
        public SaveDataEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.DataEntityId = xmlDoc.GetInnerObject("/DataEntityId");
            this.Save.FromXml(xmlDoc.SelectSingleNode("/Save").ToString());
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "DataEntityId", this.DataEntityId);
            xmlDoc.AppendInnerXml(this.Save.ToXml());
            return xmlDoc.ToString();
        }
        public enum EnumSaveDataMode
        {
            DataEntity = 0,
            SQL = 1
        }
        [Serializable]
        public class SaveItem : IXmlable
        {
            public string DataItem { get; set; }
            public string DataItemName { get; set; }
            public string Source { get; set; }
            public string SourceName { get; set; }
            public void FromXml(string strXml)
            {
                XElement element = XElement.Parse(strXml);
                this.DataItem = element.Attribute("DataItem").Value;
                this.Source = element.Attribute("Source").Value;
            }
            public string ToXml()
            {
                XElement element = new XElement("Item",
                    new XAttribute("DataItem", this.DataItem),
                    new XAttribute("Source", this.Source));
                return element.ToString();
            }
        }
    }
}
