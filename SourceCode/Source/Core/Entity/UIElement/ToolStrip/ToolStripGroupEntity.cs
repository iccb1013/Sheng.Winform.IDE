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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    public class ToolStripGroupEntity:EntityBase
    {
        public const string Property_PageId = "PageId";
        private string _pageId = String.Empty;
        public string PageId
        {
            get { return _pageId; }
            set { _pageId = value; }
        }
        public ToolStripGroupEntity()
        {
            XmlRootName = "Group";
        }
        public override void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Id = xmlDoc.GetAttributeObject( "Id");
            this.Name = xmlDoc.GetAttributeObject( "Name");
            this.Code = xmlDoc.GetAttributeObject( "Code");
            this.PageId = xmlDoc.GetAttributeObject( "PageId");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = new SEXElement(XmlRootName);
            xmlDoc.AppendAttribute(String.Empty, "Id", this.Id);
            xmlDoc.AppendAttribute(String.Empty, "Name", this.Name);
            xmlDoc.AppendAttribute(String.Empty, "Code", this.Code);
            xmlDoc.AppendAttribute(String.Empty, "PageId", this.PageId);
            return xmlDoc.ToString();
        }
    }
}
