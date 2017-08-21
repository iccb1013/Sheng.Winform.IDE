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
using System.Data;
using System.Reflection;
using System.ComponentModel;
namespace Sheng.SailingEase.Kernal
{
    
    public class EnumDescConverter
    {
        private static Dictionary<Type, DataTable> _cache = new Dictionary<Type, DataTable>();
        public static DataTable Get(Type enumType)
        {
            if (enumType.IsEnum == false)
            {    
                throw new ArgumentException();
            }
            if (_cache.Keys.Contains(enumType)  == true)
            {
                return _cache[enumType];
            }
            SEDataTable dt = new SEDataTable(
                    new string[] { "Text", "Value", "Object","DefaultValue" },
                    new Type[] { typeof(string), typeof(string), typeof(object),typeof (object) });
            FieldInfo[] fiieldInfos = enumType.GetFields();
            string name = String.Empty;
            object defaultValue = null;
            foreach (FieldInfo fi in fiieldInfos)
            {
                if (fi.FieldType.IsEnum == false)
                    continue;
                name = fi.Name;
                object memberValue = enumType.InvokeMember(fi.Name, BindingFlags.GetField, null, null, null);
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    DescriptionAttribute attr = attrs[0] as DescriptionAttribute;
                    name = attr.Description;
                }
                attrs = fi.GetCustomAttributes(typeof(DefaultValueAttribute), false);
                if (attrs.Length > 0)
                {
                    DefaultValueAttribute attr = attrs[0] as DefaultValueAttribute;
                    defaultValue = attr.Value;
                }
                dt.AddRow(name, (int)memberValue, memberValue, defaultValue);
            }
            _cache.Add(enumType, dt);
            return dt;
        }
    }
}
