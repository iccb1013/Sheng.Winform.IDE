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
    public class WindowFolderEntity
    {
        const string XmlRootName = "Folder";
        private string _id = Guid.NewGuid().ToString();
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public const string Property_Name = "Name";
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _parent = String.Empty;
        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public WindowFolderEntity()
        {
        }
        public void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Id = xmlDoc.GetAttributeObject( "Id");
            this.Name = xmlDoc.GetAttributeObject( "Name");
            this.Parent = xmlDoc.GetAttributeObject( "Parent");
        }
        public string ToXml()
        {
            SEXElement xmlDoc = new SEXElement(XmlRootName);
            xmlDoc.AppendAttribute(String.Empty, "Id", this.Id);
            xmlDoc.AppendAttribute(String.Empty, "Name", this.Name);
            xmlDoc.AppendAttribute(String.Empty, "Parent", this.Parent);
            return xmlDoc.ToString();
        }
    }
}
