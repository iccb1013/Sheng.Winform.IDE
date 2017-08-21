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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("��������", 0x00006A,"���մ򿪴���ʱ���͸����������")]
    public class ReceiveDataEvent:EventBase
    {
        private string _receiveDataXml;
        public string ReceiveDataXml
        {
            get
            {
                return this._receiveDataXml;
            }
            set
            {
                this._receiveDataXml = value;
            }
        }
        public ReceiveDataEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.ReceiveDataXml = xmlDoc.SelectSingleNode("/ReceiveData").ToString();
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            if (this.ReceiveDataXml == String.Empty)
            {
                this.ReceiveDataXml = "<ReceiveData/>";
            }
            xmlDoc.AppendInnerXml(this.ReceiveDataXml);
            return xmlDoc.ToString();
        }
    }
}
