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
using System.Diagnostics;
namespace Sheng.SailingEase.Kernal
{
    public class FieldAccessorPool : FastReflectionPool<string,IFieldAccessor>
    {
        protected override IFieldAccessor Create(Type type, string key)
        {
            if (type == null || String.IsNullOrEmpty(key))
            {
                Debug.Assert(false, "type 或 key 为空");
                throw new ArgumentNullException();
            }
            FieldInfo fieldInfo = type.GetField(key);
            if (fieldInfo == null)
            {
                Debug.Assert(false, "没有指定的FieldInfo:" + key);
                throw new MissingFieldException(key);
            }
            return new FieldAccessor(fieldInfo);
        }
    }
}
