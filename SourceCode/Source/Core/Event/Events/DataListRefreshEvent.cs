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
using System.Data;
using System.Xml.Linq;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("刷新", 0x000075, "根据指定的条件更新列表中的数据集")]
    public class DataListRefreshEvent : EventBase
    {
        private string _dataListId;
        public string DataListId
        {
            get
            {
                return this._dataListId;
            }
            set
            {
                this._dataListId = value;
            }
        }
        private EnumRefreshMode _refreshMode;
        public EnumRefreshMode RefreshMode
        {
            get
            {
                return this._refreshMode;
            }
            set
            {
                this._refreshMode = value;
            }
        }
        private XmlableCollection<WhereItem> _where = new XmlableCollection<WhereItem>("Where");
        public XmlableCollection<WhereItem> Where
        {
            get { return _where; }
            set { _where = value; }
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
        public DataListRefreshEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.RefreshMode = (EnumRefreshMode)xmlDoc.GetInnerObject<int>("/Mode", 0);
            this.DataListId = xmlDoc.GetInnerObject("/DataListId");
            this.SqlRegex = xmlDoc.GetInnerObject("/SqlRegex");
            this.Where.FromXml(xmlDoc.SelectSingleNode("/Where").ToString());
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Mode", (int)this.RefreshMode);
            xmlDoc.AppendChild(String.Empty, "SqlRegex", this.SqlRegex);
            xmlDoc.AppendChild(String.Empty, "DataListId", this.DataListId);
            xmlDoc.AppendInnerXml(this.Where.ToXml());
            return xmlDoc.ToString();
        }
        public enum EnumRefreshMode
        {
            DataEntity = 0,
            SQL = 1
        }
        [Serializable]
        public class WhereItem : IXmlable, IDataItemDataSourceRelationship
        {
            public string DataItem { get; set; }
            public string DataItemName { get; set; }
            public DataSource Source { get; set; }
            public string SourceName { get; set; }
            public EnumMatchType MatchType { get; set; }
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
    }
}
