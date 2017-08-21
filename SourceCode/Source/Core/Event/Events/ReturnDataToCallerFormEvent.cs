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
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("向调用者窗体返回数据", 0x000073,"向调用者窗体传递数据")]
    public class ReturnDataToCallerFormEvent : EventBase
    {
        private string _returnSetXml;
        public string ReturnSetXml
        {
            get
            {
                return this._returnSetXml;
            }
            set
            {
                this._returnSetXml = value;
            }
        }
        public ReturnDataToCallerFormEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.ReturnSetXml = xmlDoc.SelectSingleNode("/ReturnSet").ToString();
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            if (this.ReturnSetXml == String.Empty)
            {
                this.ReturnSetXml = "<ReturnSet/>";
            }
            xmlDoc.AppendInnerXml(this.ReturnSetXml);
            return xmlDoc.ToString();
        }
    }
}
