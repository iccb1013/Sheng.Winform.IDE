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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core.Development
{
    public static class StringParserLogic
    {
        public static string FormElementVisibleString(UIElement element)
        {
            string str = Language.Current.DataSource_FormElement + ".{0} {1}({2})";
            return String.Format(str, FormElementEntityDevTypes.Instance.GetName(element), element.FullName, element.Code);
        }
        public static string FormElementString(UIElement element)
        {
            string str = "FormElement.{0}";
            return String.Format(str, element.Id);
        }
        public static string EnumSystemDataSourceVisibleString(EnumSystemDataSource enumSystemDataSource)
        {
            string str = Language.Current.DataSource_System + ".{0}";
            str = String.Format(str,
            EnumDescConverter.Get(typeof(EnumSystemDataSource)).Select("Value='" + (int)enumSystemDataSource + "'")[0]["Text"].ToString());
            return str;
        }
        public static string EnumSystemDataSourceString(EnumSystemDataSource enumSystemDataSource)
        {
            string str = "System.{0}";
            str = String.Format(str, (int)enumSystemDataSource);
            return str;
        }
       
        public static string DataSourceVisibleString(WindowEntity formEntity, string str, out bool warning)
        {
            warning = false;
            if (str == null || str == String.Empty)
            {
                warning = true;
                return String.Empty;
            }
            if (!str.StartsWith("FormElement.") &&
                !str.StartsWith("System."))
            {
                warning = true;
                throw new ArgumentException();
            }
            string visibleStr;
            string[] strArray = str.Split('.');
            switch (strArray[0])
            {
                case "FormElement":
                    UIElement formElement =
                       formEntity.FindFormElementById(strArray[1]);
                    if (formElement == null)
                    {
                        warning = true;
                        return String.Empty;
                    }
                    visibleStr = FormElementVisibleString(formElement);
                    break;
                case "System":
                    EnumSystemDataSource enumSystemDataSource =
                        (EnumSystemDataSource)Enum.Parse(typeof(EnumSystemDataSource), strArray[1]);
                    visibleStr = EnumSystemDataSourceVisibleString(enumSystemDataSource);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return visibleStr;
        }
        public static string DataSourceVisibleString(object dataSourceObject)
        {
            if (!(dataSourceObject is UIElement) && !(dataSourceObject is EnumSystemDataSource))
            {
                throw new ArgumentException();
            }
            if (dataSourceObject is UIElement)
            {
                return FormElementVisibleString(dataSourceObject as UIElement);
            }
            else if (dataSourceObject is EnumSystemDataSource)
            {
                return EnumSystemDataSourceVisibleString((EnumSystemDataSource)dataSourceObject);
            }
            return String.Empty;
        }
        public static string DataSourceString(object dataSourceObject)
        {
            if (!(dataSourceObject is UIElement) && !(dataSourceObject is EnumSystemDataSource))
            {
                throw new ArgumentException();
            }
            if (dataSourceObject is UIElement)
            {
                return FormElementString(dataSourceObject as UIElement);
            }
            else if (dataSourceObject is EnumSystemDataSource)
            {
                return EnumSystemDataSourceString((EnumSystemDataSource)dataSourceObject);
            }
            return String.Empty;
        }
    }
}
