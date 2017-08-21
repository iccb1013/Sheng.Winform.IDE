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
using System.Xml.Linq;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Modules.ProjectModule
{
    class ProjectSummary : IProjectSummary, IXmlable
    {
        private int _version = 0;
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }
        private string _productVersion = Application.ProductVersion;
        public string ProductVersion
        {
            get { return _productVersion; }
            set { _productVersion = value; }
        }
        private bool _firstRun = false;
        public bool FirstRun
        {
            get { return _firstRun; }
            set { _firstRun = value; }
        }
        public void FromXml(string strXml)
        {
            SEXElement xmlSummary = SEXElement.Parse(strXml);
            _version = xmlSummary.GetAttributeObject<int>("Version", 0);
            _productVersion = xmlSummary.GetInnerObject("/ProductVersion");
            _firstRun = xmlSummary.GetInnerObject<bool>("/FirstRun", false);
        }
        public string ToXml()
        {
            XElement xmlSummary = new SEXElement("Summary",
                new XAttribute("Version", _version),
                new XElement("ProductVersion", _productVersion),
                new XElement("FirstRun", _firstRun));
            return xmlSummary.ToString();
        }
    }
}
