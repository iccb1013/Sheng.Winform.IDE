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
using System.Diagnostics;
namespace Sheng.SailingEase.Kernal
{
    public static class PathHelper
    {
        public static string GetFileNameRegexFilter(string filter)
        {
            Debug.Assert(filter != null, "filter为null");
            if (String.IsNullOrEmpty(filter))
                return String.Empty;
            StringBuilder regexFilter = new StringBuilder();
            string[] filterArray = filter.Split(';');
            for (int i = 0; i < filterArray.Length; i++)
            {
                string item = filterArray[i];
                if (item.Length == 0)
                    continue;
                string regex = Regex.Escape(item);
                regex = regex.Replace("\\*", ".*").Replace("\\?", ".");
                if (item.IndexOf('.') > 0)
                    regex += "$";
                regexFilter.Append(regex);
                if (i < filterArray.Length - 1)
                    regexFilter.Append(";");
            }
            return regexFilter.ToString();
        }
    }
}
