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
namespace Sheng.SailingEase.Core
{
    public static class CommonOperater
    {
        public static string CombineFieldAndValue(string field, string value, EnumMatchType matchType)
        {
            string result = String.Empty;
            result += "[" + field + "] ";
            switch (matchType)
            {
                case EnumMatchType.Equal:
                    result += " = ";
                    result += value;
                    break;
                case EnumMatchType.NotEqual:
                    result += " <> ";
                    result += value;
                    break;
                case EnumMatchType.Like:
                    result += " LIKE ";
                    result += " '%' + " + value + " + '%' ";
                    break;
                case EnumMatchType.Large:
                    result += " > ";
                    result += value;
                    break;
                case EnumMatchType.LargeEqual:
                    result += " >= ";
                    result += value;
                    break;
                case EnumMatchType.Less:
                    result += " < ";
                    result += value;
                    break;
                case EnumMatchType.LessEqual:
                    result += " <= ";
                    result += value;
                    break;
            }
            return result;
        }
    }
}
