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
    [EventProvide("更新行", 0x000078,"更新符合条件的行中的数据")]
    public class DataListUpdateRowEvent : EventBase
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
        private XmlableCollection<DataItem> _data = new XmlableCollection<DataItem>("Data");
        public XmlableCollection<DataItem> Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public DataListUpdateRowEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.TargetWindow = (EnumTargetWindow)xmlDoc.GetInnerObject<int>("/TargetWindow", 0);
            this.DataList = xmlDoc.GetInnerObject("/DataList");
            this.Data.FromXml(xmlDoc.SelectSingleNode("/Data").ToString());
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "TargetWindow", (int)this.TargetWindow);
            xmlDoc.AppendChild(String.Empty, "DataList", this.DataList);
            xmlDoc.AppendInnerXml(this.Data.ToXml());
            return xmlDoc.ToString();
        }
        [Serializable]
        public class DataItem : IXmlable
        {
            public string DataColumn { get; set; }
            public string DataColumnName { get; set; }
            public string Source { get; set; }
            public string SourceName { get; set; }
            public bool Where { get; set; }
            public void FromXml(string strXml)
            {
                XElement element = XElement.Parse(strXml);
                this.DataColumn = element.Attribute("DataColumn").Value;
                this.Source = element.Attribute("Source").Value;
                this.Where = Convert.ToBoolean(element.Attribute("Where").Value);
            }
            public string ToXml()
            {
                XElement element = new XElement("Item",
                    new XAttribute("DataColumn", this.DataColumn),
                    new XAttribute("Source", this.Source),
                    new XAttribute("Where", this.Where));
                return element.ToString();
            }
        }
    }
}
