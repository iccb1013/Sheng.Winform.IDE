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
using System.Xml.Linq;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("删除行", 0x000079, "根据指定的条件，删除符合条件的行")]
    public class DataListDeleteRowEvent : EventBase
    {
        private EnumTargetWindow _targetWindow = EnumTargetWindow.Current;
        public EnumTargetWindow TargetWindow
        {
            get
            {
                return this._targetWindow;
            }
            set
            {
                this._targetWindow = value;
            }
        }
        private string _dataList;
        public string DataList
        {
            get
            {
                return this._dataList;
            }
            set
            {
                this._dataList = value;
            }
        }
        private XmlableCollection<WhereItem> _where = new XmlableCollection<WhereItem>("Where");
        public XmlableCollection<WhereItem> Where
        {
            get { return _where; }
            set { _where = value; }
        }
        public DataListDeleteRowEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.TargetWindow = (EnumTargetWindow)xmlDoc.GetInnerObject<int>("/TargetWindow", 0);
            this.DataList = xmlDoc.GetInnerObject("/DataList");
            this.Where.FromXml(xmlDoc.SelectSingleNode("/Where").ToString());
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "TargetWindow", (int)this.TargetWindow);
            xmlDoc.AppendChild(String.Empty, "DataList", this.DataList);
            xmlDoc.AppendInnerXml(String.Empty, this.Where.ToXml());
            return xmlDoc.ToString();
        }
        [Serializable]
        public class WhereItem : IXmlable
        {
            public string DataColumn { get; set; }
            public string DataColumnName { get; set; }
            public string Source { get; set; }
            public string SourceName { get; set; }
            public void FromXml(string strXml)
            {
                XElement element = XElement.Parse(strXml);
                this.DataColumn = element.Attribute("DataColumn").Value;
                this.Source = element.Attribute("Source").Value;
            }
            public string ToXml()
            {
                XElement element = new XElement("Item",
                    new XAttribute("DataColumn", this.DataColumn),
                    new XAttribute("Source", this.Source));
                return element.ToString();
            }
        }
    }
}
