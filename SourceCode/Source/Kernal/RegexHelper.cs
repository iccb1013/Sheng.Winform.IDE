/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
.	匹配除换行符以外的任意字符
\w	匹配字母或数字或下划线或汉字
\s	匹配任意的空白符
\d	匹配数字
\b	匹配单词的开始或结束
^	匹配字符串的开始
$	匹配字符串的结束
 *	重复零次或更多次
+	重复一次或更多次
?	重复零次或一次
{n}	重复n次
{n,}	重复n次或更多次
{n,m}	重复n到m次
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Sheng.SailingEase.Kernal
{
    public static class RegexHelper
    {
        public static bool IsMatch(string input, string regexString)
        {
            string error;
            return IsMatch(input, regexString, RegexOptions.None, out error);
        }
        public static bool IsMatch(string input, string regexString, out string error)
        {
            return IsMatch(input, regexString, RegexOptions.None, out error);
        }
        public static bool IsMatch(string input, string regexString, RegexOptions options)
        {
            string error;
            return IsMatch(input, regexString, options, out error);
        }
        public static bool IsMatch(string input, string regexString, RegexOptions options, out string error)
        {
            error = null;
            try
            {
                Regex regex = new Regex(regexString, options);
                if (regex.IsMatch(input))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        public static string Replace(string input, string regexString, string replaceString)
        {
            return Replace(input, regexString, replaceString, RegexOptions.None);
        }
        public static string Replace(string input, string regexString, string replaceString, out string error)
        {
            return Replace(input, regexString, replaceString, RegexOptions.None, out error);
        }
        public static string Replace(string input, string regexString, string replaceString, RegexOptions options)
        {
            string error;
            return Replace(input, regexString, replaceString, options, out error);
        }
        public static string Replace(string input, string regexString, string replaceString, RegexOptions options, out string error)
        {
            error = null;
            try
            {
                Regex replaceRegex = new Regex(regexString, options);
                return replaceRegex.Replace(input, replaceString);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return input;
            }
        }
        public static string[] Split(string input, string regexString)
        {
            return Split(input, regexString, RegexOptions.None);
        }
        public static string[] Split(string input, string regexString, out string error)
        {
            return Split(input, regexString, RegexOptions.None, out error);
        }
        public static string[] Split(string input, string regexString, RegexOptions options)
        {
            string error;
            return Split(input, regexString, options, out error);
        }
        public static string[] Split(string input, string regexString, RegexOptions options, out string error)
        {
            error = null;
            try
            {
                Regex splitRegex = new Regex(regexString, options);
                return splitRegex.Split(input);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return new string[] { };
            }
        }
        public static string[] Matches(string input, string regexString)
        {
            return Matches(input, regexString, RegexOptions.None);
        }
        public static string[] Matches(string input, string regexString, out string error)
        {
            return Matches(input, regexString, RegexOptions.None, out error);
        }
        public static string[] Matches(string input, string regexString, RegexOptions options)
        {
            string error;
            return Matches(input, regexString, options, out error);
        }
        public static string[] Matches(string input, string regexString, RegexOptions options, out string error)
        {
            error = null;
            try
            {
                Regex matchesRegex = new Regex(regexString, options);
                MatchCollection matchesFound = matchesRegex.Matches(input);
                string[] result = new string[matchesFound.Count];
                for (int i = 0; i < matchesFound.Count; i++)
                {
                    result[i] = matchesFound[i].Value;
                }
                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return new string[] { };
            }
        }
        public static string[] Groups(string input, string regexString)
        {
            return Groups(input, regexString, RegexOptions.None);
        }
        public static string[] Groups(string input, string regexString, out string error)
        {
            return Groups(input, regexString, RegexOptions.None, out error);
        }
        public static string[] Groups(string input, string regexString, RegexOptions options)
        {
            string error;
            return Groups(input, regexString, options, out error);
        }
        public static string[] Groups(string input, string regexString, RegexOptions options, out string error)
        {
            error = null;
            try
            {
                Regex matchesRegex = new Regex(regexString, options);
                MatchCollection matchesFound = matchesRegex.Matches(input);
                GroupCollection matchGroups;
                List<string> resultList = new List<string>();
                foreach (Match matchMade in matchesFound)
                {
                    matchGroups = matchMade.Groups;
                    foreach (Group matchGroup in matchGroups)
                    {
                        resultList.Add(matchGroup.Value);
                    }
                }
                string[] result = new string[resultList.Count];
                resultList.CopyTo(result);
                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return new string[] { };
            }
        }
        public static bool IsNumeric(string str)
        {
            return IsMatch(str, "^[0-9]+$");
        }
    }
}
