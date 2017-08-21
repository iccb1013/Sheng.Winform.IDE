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
    [EventProvide("�����������", 0x000064,"�������Ԫ���е�����")]
    public class ClearFormDataEvent:EventBase
    {
        private EnumClearFormDataMode _clearFormDataMode;
        public EnumClearFormDataMode ClearFormDataMode
        {
            get
            {
                return this._clearFormDataMode;
            }
            set
            {
                this._clearFormDataMode = value;
            }
        }
        private string _clearsSetXml;
        public string ClearsSetXml
        {
            get
            {
                return this._clearsSetXml;
            }
            set
            {
                this._clearsSetXml = value;
            }
        }
        public ClearFormDataEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.ClearFormDataMode = (EnumClearFormDataMode)xmlDoc.GetInnerObject<int>("/Mode", 0);
            this.ClearsSetXml = xmlDoc.SelectSingleNode("/Clears").ToString();
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Mode", (int)this.ClearFormDataMode);
            if (this.ClearsSetXml == String.Empty)
            {
                this.ClearsSetXml = "<Clears/>";
            }
            xmlDoc.AppendInnerXml(this.ClearsSetXml);
            return xmlDoc.ToString();
        }
        public enum EnumClearFormDataMode
        {
            [LocalizedDescription("EnumClearFormDataMode_All")]
            All = 0,
            [LocalizedDescription("EnumClearFormDataMode_IgnoreDataList")]
            IgnoreDataList = 1,
            [LocalizedDescription("EnumClearFormDataMode_Appoint")]
            Appoint = 2
        }
    }
}
