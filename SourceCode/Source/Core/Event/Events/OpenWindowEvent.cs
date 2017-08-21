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
    [EventProvide("�򿪴���", 0x000069,"��ָ���Ĵ��壬�����䴫������")]
    public class OpenWindowEvent : EventBase
    {
        private string _windowId;
        public string WindowId
        {
            get
            {
                return this._windowId;
            }
            set
            {
                this._windowId = value;
            }
        }
        private string _sendDataXml;
        public string SendDataXml
        {
            get
            {
                return this._sendDataXml;
            }
            set
            {
                this._sendDataXml = value;
            }
        }
        private bool _isDiablog;
        public bool IsDialog
        {
            get
            {
                return this._isDiablog;
            }
            set
            {
                this._isDiablog = value;
            }
        }
        private EnumOpenWindowIfOpend _ifOpend;
        public EnumOpenWindowIfOpend IfOpend
        {
            get
            {
                return this._ifOpend;
            }
            set
            {
                this._ifOpend = value;
            }
        }
        public OpenWindowEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.WindowId = xmlDoc.GetInnerObject("/WindowId");
            this.IfOpend = (EnumOpenWindowIfOpend)xmlDoc.GetInnerObject<int>("/IfOpend", 0);
            this.IsDialog = xmlDoc.GetInnerObject<bool>("/IsDialog", false);
            this.SendDataXml = xmlDoc.SelectSingleNode("/SendData").ToString();
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "WindowId", this.WindowId);
            xmlDoc.AppendChild(String.Empty, "IfOpend", (int)this.IfOpend);
            xmlDoc.AppendChild(String.Empty, "IsDialog", this.IsDialog);
            if (this.SendDataXml == null || this.SendDataXml == String.Empty)
            {
                this.SendDataXml = "<SendData/>";
            }
            xmlDoc.AppendInnerXml(this.SendDataXml);
            return xmlDoc.ToString();
        }
        public enum EnumOpenWindowIfOpend
        {
            [LocalizedDescription("EnumOpenFormIfOpend_Activate")]
            Activate = 0,
            [LocalizedDescription("EnumOpenFormIfOpend_OpenNew")]
            OpenNew = 1,
            [LocalizedDescription("EnumOpenFormIfOpend_CloseAndOpenNew")]
            CloseAndOpenNew = 2
        }
    }
}
