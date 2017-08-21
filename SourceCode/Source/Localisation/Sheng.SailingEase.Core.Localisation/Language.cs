/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
namespace Sheng.SailingEase.Core.Localisation
{
    public class Language
    {
        private static ILanguage _Language;
        private Language()
        {
        }
        public static ILanguage Current
        {
            get
            {
                if ((_Language == null))
                {
                    _Language = new Chinese__Simplified_();
                }
                return _Language;
            }
            set
            {
                _Language = value;
            }
        }
        public static string GetString(string name)
        {
            return Current.ResourceManager.GetString(name);
        }
    }
}
