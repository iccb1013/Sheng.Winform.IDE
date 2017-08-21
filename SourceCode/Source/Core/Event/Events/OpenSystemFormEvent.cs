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
    [EventProvide("打开系统窗体", 0x000070,"打开预置的系统窗体")]
    public class OpenSystemFormEvent : EventBase
    {
        private EnumSystemForm _form;
        public EnumSystemForm Form
        {
            get
            {
                return this._form;
            }
            set
            {
                this._form = value;
            }
        }
        public OpenSystemFormEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Form = (EnumSystemForm)xmlDoc.GetInnerObject<int>("/Form", 0);
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Form", (int)this.Form);
            return xmlDoc.ToString();
        }
        public enum EnumSystemForm
        {
            [LocalizedDescription("EnumSystemForm_About")]
            About = 0,
            [LocalizedDescription("EnumSystemForm_User")]
            User = 1,
            [LocalizedDescription("EnumSystemForm_UserGroup")]
            UserGroup = 2
        }
    }
}
