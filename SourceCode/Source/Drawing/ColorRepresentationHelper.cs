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
using System.Drawing;
using System.Reflection;
namespace Sheng.SailingEase.Drawing
{
    public enum ChooseColorType
    {
        Custom = 0,
        Define = 1,
        System = 2
    }
    public class ColorRepresentationHelper
    {
        public static Color GetColorByValue(string colorValueString)
        {
            if (colorValueString == null || colorValueString == String.Empty)
            {
                return Color.Empty;
            }
            string[] strArray = colorValueString.Split('.');
            ChooseColorType type =
                (ChooseColorType)Convert.ToInt32(strArray[0]);
            Color color = Color.Empty;
            switch (type)
            {
                case ChooseColorType.Custom:
                    color = Color.FromArgb(Convert.ToInt32(strArray[2]));
                    break;
                case ChooseColorType.Define:
                    color = Color.FromArgb(Convert.ToInt32(strArray[2]));
                    break;
                case ChooseColorType.System:
                    Type typeSystemColors = typeof(System.Drawing.SystemColors);
                    PropertyInfo p = typeSystemColors.GetProperty(strArray[1]);
                    color = (Color)p.GetValue(typeSystemColors, null);
                    break;
            }
            return color;
        }
    }
}
