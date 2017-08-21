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
    [EventProvide("���´�������", 0x00006E,"������Ԫ���е����ݸ��µ�����ʵ����")]
    public class UpdateFormDataEvent:EventBase
    {
        private EnumUpdateFormDataMode _updateMode;
        public EnumUpdateFormDataMode UpdateMode
        {
            get
            {
                return this._updateMode;
            }
            set
            {
                this._updateMode = value;
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
        private XmlableCollection<WhereItem> _where = new XmlableCollection<WhereItem>("Where");
        public XmlableCollection<WhereItem> Where
        {
            get { return _where; }
            set { _where = value; }
        }
        private XmlableCollection<UpdateItem> _update = new XmlableCollection<UpdateItem>("Update");
        public XmlableCollection<UpdateItem> Update
        {
            get { return _update; }
            set { _update = value; }
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
        public UpdateFormDataEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.UpdateMode = (EnumUpdateFormDataMode)xmlDoc.GetInnerObject<int>("/Mode", 0);
            this.DataEntityId = xmlDoc.GetInnerObject("/DataEntityId");
            this.SqlRegex = xmlDoc.GetInnerObject("/SqlRegex");
            this.Update.FromXml(xmlDoc.SelectSingleNode("/Update").ToString());
            this.Where.FromXml(xmlDoc.SelectSingleNode("/Where").ToString());
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Mode", (int)this.UpdateMode);
            xmlDoc.AppendChild(String.Empty, "SqlRegex", this.SqlRegex);
            xmlDoc.AppendChild(String.Empty, "DataEntityId", this.DataEntityId);
            xmlDoc.AppendInnerXml(this.Update.ToXml());
            xmlDoc.AppendInnerXml(this.Where.ToXml());
            return xmlDoc.ToString();
        }
        public enum EnumUpdateFormDataMode
        {
            DataEntity = 0,
            SQL = 1
        }
        [Serializable]
        public class WhereItem : IXmlable
        {
            public string DataItem { get; set; }
            public string DataItemName { get; set; }
            public DataSource Source { get; set; }
            public string SourceName { get; set; }
            private EnumMatchType _matchType = EnumMatchType.Equal;
            public EnumMatchType MatchType
            {
                get { return _matchType; }
                set { _matchType = value; }
            }
            public void FromXml(string strXml)
            {
                XElement element = XElement.Parse(strXml);
                this.DataItem = element.Attribute("DataItem").Value;
                this.Source = new DataSource(element.Attribute("Source").Value);
                this.MatchType = (EnumMatchType)Convert.ToInt32(element.Attribute("MatchType").Value);
            }
            public string ToXml()
            {
                XElement element = new XElement("Item",
                    new XAttribute("DataItem", this.DataItem),
                    new XAttribute("Source", this.Source.ToString()),
                    new XAttribute("MatchType", (int)this.MatchType));
                return element.ToString();
            }
        }
        [Serializable]
        public class UpdateItem : IXmlable
        {
            public string DataItem { get; set; }
            public string DataItemName { get; set; }
            public DataSource Source { get; set; }
            public string SourceName { get; set; }
            public void FromXml(string strXml)
            {
                XElement element = XElement.Parse(strXml);
                this.DataItem = element.Attribute("DataItem").Value;
                this.Source = new DataSource(element.Attribute("Source").Value);
            }
            public string ToXml()
            {
                XElement element = new XElement("Item",
                    new XAttribute("DataItem", this.DataItem),
                    new XAttribute("Source", this.Source.ToString()));
                return element.ToString();
            }
        }
    }
}
