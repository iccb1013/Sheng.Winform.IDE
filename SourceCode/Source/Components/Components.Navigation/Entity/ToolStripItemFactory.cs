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
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Components.NavigationComponent
{
    class ToolStripItemFactory
    {
        public static ToolStripItemAbstract GetToolStripItemDevByXml(string strXml)
        {
            ToolStripItemAbstract toolStripItem = null;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXml);
            toolStripItem =
                (ToolStripItemAbstract)FormElementEntityDevTypes.Instance.CreateInstance(Convert.ToInt32(xmlDoc.SelectSingleNode("Element/ControlType").InnerText));
            toolStripItem.FromXml(strXml);
            return toolStripItem;
        }
    }
}
