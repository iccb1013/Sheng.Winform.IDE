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
    [EventProvide("����Ψһ��ʶ", 0x000068,"�����µ�Ψһ��ʶ�������ָ���Ĵ���Ԫ����")]
    public class NewGuidEvent:EventBase
    {
        private string _setFormElementId;
        public string SetFormElementId
        {
            get
            {
                return this._setFormElementId;
            }
            set
            {
                this._setFormElementId = value;
            }
        }
        public NewGuidEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.SetFormElementId = xmlDoc.GetInnerObject("/SetFormElement");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "SetFormElement", this.SetFormElementId);
            return xmlDoc.ToString();
        }
    }
}
