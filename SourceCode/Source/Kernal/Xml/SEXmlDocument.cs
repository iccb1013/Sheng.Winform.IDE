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
using System.Diagnostics;
namespace  Sheng.SailingEase.Kernal
{
    public class SEXmlDocument : XmlDocument
    {
        public void AppendChild(string name)
        {
            AppendChild( String.Empty, name, null);
        }
        public void AppendChild(string xPath, string name)
        {
            AppendChild( xPath, name, null);
        }
        public void AppendChild(string xPath, string name, object innerText)
        {
            XmlElement xmlEle = this.CreateElement(name);
            if (innerText != null)
                xmlEle.InnerText = innerText.ToString();
            if (xPath != String.Empty)
            {
                XmlNode xmlNode = this.SelectSingleNode(xPath);
                if (xmlNode != null)
                {
                    xmlNode.AppendChild(xmlEle);
                }
                else
                {
                    Debug.Assert(false, "没有找到指定的节点", xPath);
                }
            }
            else
            {
                this.AppendChild(xmlEle);
            }
        }
        public void AppendChild(string xPath, XmlNode node)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode != null)
            {
                xmlNode.AppendChild(node);
            }
            else
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
            }
        }
        public void AppendInnerXml(string innerXml)
        {
            this.InnerXml += innerXml;
        }
        public void AppendInnerXml(string xPath, string innerXml)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode == null)
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
                return;
            }
            xmlNode.InnerXml += innerXml;
        }
        public void AppendInnerXml(string xPath, string name, string innerXml)
        {
            AppendInnerXml(xPath, name, innerXml, null);
        }
        public void AppendInnerXml(string xPath, string name, string innerXml, string defaultValue)
        {
            XmlElement xmlEle = this.CreateElement(name);
            if (innerXml != null)
                xmlEle.InnerXml = innerXml;
            else
                xmlEle.InnerXml = defaultValue;
            if (xPath != String.Empty)
            {
                XmlNode xmlNode = this.SelectSingleNode(xPath);
                if (xmlNode != null)
                {
                    xmlNode.AppendChild(xmlEle);
                }
                else
                {
                    Debug.Assert(false, "没有找到指定的节点", xPath);
                }
            }
            else
            {
                this.AppendChild(xmlEle);
            }
        }
        public void AppendAttribute(string xPath, string name)
        {
            AppendAttribute(xPath, name, null);
        }
        public void AppendAttribute(string xPath, string name, object value)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode == null)
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
                return;
            }
            XmlAttribute xmlAttr = this.CreateAttribute(name);
            if (value != null)
                xmlAttr.Value = value.ToString();
            xmlNode.Attributes.Append(xmlAttr);
        }
        public T GetInnerObject<T>(string xPath, T defaultValue)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode != null)
                return (T)Convert.ChangeType(xmlNode.InnerText, defaultValue.GetType());
            else
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
                return (T)Convert.ChangeType(defaultValue, defaultValue.GetType());
            }
        }
        public string GetInnerObject(string xPath)
        {
            return GetInnerObject( xPath, String.Empty);
        }
        public T GetAttributeObject<T>(string xPath, string name, T defaultValue)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            Debug.Assert(xmlNode != null, "没有找到指定的节点", xPath);
            XmlAttribute xmlAttr = xmlNode.Attributes[name];
            if (xmlAttr != null)
                return (T)Convert.ChangeType(xmlAttr.Value, defaultValue.GetType());
            else
            {
                Debug.Assert(false, "没有找到指定节点上的属性", xPath + "," + name);
                return (T)Convert.ChangeType(defaultValue, defaultValue.GetType());
            }
        }
        public string GetAttributeObject(string xPath, string name)
        {
            return GetAttributeObject<string>(xPath, name, String.Empty);
        }
        public string GetOuterXml(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode != null)
                return xmlNode.OuterXml;
            else
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
                return String.Empty;
            }
        }
        public string GetInnerXml(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode != null)
                return xmlNode.InnerXml;
            else
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
                return String.Empty;
            }
        }
        public void SetInnerText(string xPath, object value)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            if (xmlNode == null)
            {
                Debug.Assert(false, "没有找到指定的节点", xPath);
                return;
            }
            if (value == null)
                xmlNode.InnerText = String.Empty;
            else
                xmlNode.InnerText = value.ToString();
        }
        public void CreateDefaultDeclaration()
        {
            XmlDeclaration xmldecl = this.CreateXmlDeclaration("1.0", "utf-8", null);
            this.AppendChild(xmldecl);
        }
    }
}
