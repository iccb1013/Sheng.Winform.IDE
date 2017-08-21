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
    [EventProvide("�򿪳���", 0x00006D,"���ָ��·���ϵĳ���")]
    public class StartProcessEvent:EventBase
    {
        private string _path;
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }
        public StartProcessEvent()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Path = xmlDoc.GetInnerObject("/Path");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild(String.Empty, "Path", this.Path);
            return xmlDoc.ToString();
        }
    }
}
