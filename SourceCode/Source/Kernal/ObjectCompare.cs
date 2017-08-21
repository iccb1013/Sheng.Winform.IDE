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
using System.Reflection;
namespace Sheng.SailingEase.Kernal
{
    public class ObjectCompare
    {
        public static List<ObjectCompareResult> Compare(object sourceObject, object compareObject)
        {
            Type sourceType = sourceObject.GetType();
            Type compareType = compareObject.GetType();
            if (sourceType.Equals(compareType) == false)
            {
                throw new ArgumentException();
            }
            List<ObjectCompareResult> result = new List<ObjectCompareResult>();
            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            object sourceValue, compareValue;
            foreach (PropertyInfo property in sourceProperties)
            {
                if (property.CanRead == false || property.CanWrite == false)
                    continue;
                object [] attrs = property.GetCustomAttributes(typeof(ObjectCompareAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    ObjectCompareAttribute attr = (ObjectCompareAttribute)attrs[0];
                    if (attr.Ignore)
                        continue;
                }
                sourceValue = property.GetValue(sourceObject, null);
                compareValue = property.GetValue(compareObject, null);
                if (sourceValue != null)
                {
                    if (sourceValue.Equals(compareValue) == false)
                    {
                        result.Add(new ObjectCompareResult(property.Name, sourceValue, compareValue));
                    }
                }
                else
                {
                    if (compareValue != null)
                    {
                        if (compareValue.Equals(sourceValue) == false)
                        {
                            result.Add(new ObjectCompareResult(property.Name, sourceValue, compareValue));
                        }
                    }
                }
            }
            return result;
        }
    }
    public class ObjectCompareResult
    {
        public string MemberName
        {
            get;
            private set;
        }
        public object SourceValue
        {
            get;
            private set;
        }
        public object CompareValue
        {
            get;
            private set;
        }
        public ObjectCompareResult(string memberName, object sourceValue, object compareValue)
        {
            this.MemberName = memberName;
            this.SourceValue = sourceValue;
            this.CompareValue = compareValue;
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ObjectCompareAttribute : Attribute
    {
        private bool _ignore = false;
        public bool Ignore
        {
            get { return _ignore; }
            set { _ignore = value; }
        }
    }
}
