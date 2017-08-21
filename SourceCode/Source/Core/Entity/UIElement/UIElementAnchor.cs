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
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public class UIElementAnchor
    {
        public bool Top = true;
        public bool Left = true;
        public bool Right = false;
        public bool Bottom = false;
        public override string ToString()
        {
            string strXml = String.Empty;
            if (Top)
            {
                strXml += "Top,";
            }
            if (Right)
            {
                strXml += "Right,";
            }
            if (Bottom)
            {
                strXml += "Bottom,";
            }
            if (Left)
            {
                strXml += "Left,";
            }
            return strXml.TrimEnd(',');
        }
        public string GetXml()
        {
            string strXml = String.Empty;
            strXml += "<Top>" + Top.ToString() + "</Top>";
            strXml += "<Left>" + Left.ToString() + "</Left>";
            strXml += "<Right>" + Right.ToString() + "</Right>";
            strXml += "<Bottom>" + Bottom.ToString() + "</Bottom>";
            return strXml;
        }
        public static explicit operator UIElementAnchor(string strAnchor)
        {
            UIElementAnchor temp = new UIElementAnchor()
            {
                Top = false,
                Left = false,
                Right = false,
                Bottom = false
            };
            string[] strAnchorArray = strAnchor.ToString().Split(',');
            foreach (string str in strAnchorArray)
            {
                switch (str)
                {
                    case "Top":
                        temp.Top = true;
                        break;
                    case "Right":
                        temp.Right = true;
                        break;
                    case "Bottom":
                        temp.Bottom = true;
                        break;
                    case "Left":
                        temp.Left = true;
                        break;
                }
            }
            return temp;
        }
        public override bool Equals(object obj)
        {
            if (obj is UIElementAnchor)
            {
                UIElementAnchor temp = (UIElementAnchor)obj;
                return this.ToString().Equals(temp.ToString());
            }
            else if (obj is string)
            {
                UIElementAnchor temp = (UIElementAnchor)(string)obj;
                return this.ToString().Equals(temp.ToString());
            }
            else
            {
                return false;
            }
        }
    }
}
