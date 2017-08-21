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
    [EventProvide("验证窗体数据", 0x000071,"验证窗体元素中的数据的有效性")]
    public class ValidateFormDataEvent : EventBase
    {
        private EnumValidateFormDataMode _validateMode;
        public EnumValidateFormDataMode ValidateMode
        {
            get
            {
                return this._validateMode;
            }
            set
            {
                this._validateMode = value;
            }
        }
        private string _validateSetXml;
        public string ValidateSetXml
        {
            get
            {
                return this._validateSetXml;
            }
            set
            {
                this._validateSetXml = value;
            }
        }
        public ValidateFormDataEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.ValidateMode = (EnumValidateFormDataMode)xmlDoc.GetInnerObject<int>("/Mode", 0);
            this.ValidateSetXml = xmlDoc.SelectSingleNode("/ValidateSet").ToString();
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Mode", (int)this.ValidateMode);
            if (this.ValidateSetXml == String.Empty)
            {
                this.ValidateSetXml = "<ValidateSet/>";
            }
            xmlDoc.AppendInnerXml(this.ValidateSetXml);
            return xmlDoc.ToString();
        }
        public enum EnumValidateFormDataMode
        {
            [LocalizedDescription("EnumValidateFormDataMode_All")]
            All = 0,
            [LocalizedDescription("EnumValidateFormDataMode_Appoint")]
            Appoint = 1
        }
    }
}
