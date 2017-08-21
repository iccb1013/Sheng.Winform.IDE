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
    [EventProvide("调用外接程序", 0x000072, "调用指定的外部程序集")]
    public class CallAddInEvent : EventBase
    {
        private string _addInFullName;
        public string AddInFullName
        {
            get
            {
                return this._addInFullName;
            }
            set
            {
                this._addInFullName = value;
            }
        }
        public CallAddInEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.AddInFullName = xmlDoc.GetInnerObject("/AddInFullName");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "AddInFullName", this.AddInFullName);
            return xmlDoc.ToString();
        }
    }
}
