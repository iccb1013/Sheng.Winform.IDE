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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
namespace Sheng.SailingEase.RegexTool
{
    struct RegexItem
    {
        public string name;
        public string regexString;
        public string remark;
        public string type;
    }
    static class RegexLib
    {
        static List<RegexItem> regexItems;
        public static List<RegexItem> Items
        {
            get
            {
                if (regexItems == null)
                {
                    LoadRegexItems();
                }
                return regexItems;
            }
        }
        static void LoadRegexItems()
        {
            regexItems = new List<RegexItem>();
            string resourceName = Path.Combine(Application.StartupPath, "RegexItems.xml");
            if (File.Exists(resourceName) == false)
                return;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(resourceName);
                XmlNodeList nodeList = xmlDoc.SelectNodes("RegexItems/Item");
                foreach (XmlNode xmlNode in nodeList)
                {
                    RegexItem regexItem = new RegexItem()
                    {
                        type = xmlNode.Attributes["Type"].Value,
                        name = xmlNode.Attributes["Name"].Value,
                        regexString = xmlNode.Attributes["RegexString"].Value,
                        remark = xmlNode.Attributes["Remark"].Value
                    };
                    regexItems.Add(regexItem);
                }
            }
            catch
            {
                Debug.Assert(false, "载入 RegexItems.xml 失败");
            }
        }
    }
}
