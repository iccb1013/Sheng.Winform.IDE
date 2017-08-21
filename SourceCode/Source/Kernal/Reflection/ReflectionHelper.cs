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
namespace Sheng.SailingEase.Kernal
{
    public static class ReflectionHelper
    {
        public static bool IsSubOf(Type type, Type targetType)
        {
            if (type.Equals(targetType))
                return false;
            if (targetType.IsInterface)
            {
                Type interfaceType = type.GetInterface(targetType.Name,false);
                if (interfaceType != null)
                    return true;
            }
            else
            {
                if (type.IsSubclassOf(targetType))
                    return true;
            }
            return false;
        }
        public static Type GetHighLevelType(object[] objs)
        {
            int objsLength = objs.Length;
            if (objsLength == 0)
                return null;
            if (objsLength == 1)
                return objs[0].GetType();
            Type[] types = new Type[objsLength];
            for (int i = 0; i < objsLength; i++)
            {
                types[i] = objs[i].GetType();
            }
            Type highLevelType = objs[0].GetType();
            for (int i = 0; i < objs.Length; i++)
            {
                Type type = objs[i].GetType();
                if (IsSubOf(highLevelType, type))
                    highLevelType = type;
            }
            return highLevelType;
        }
        public static Type GetLowLevelType(object[] objs)
        {
            int objsLength = objs.Length;
            if (objsLength == 0)
                return null;
            if (objsLength == 1)
                return objs[0].GetType();
            Type[] types = new Type[objsLength];
            for (int i = 0; i < objsLength; i++)
            {
                types[i] = objs[i].GetType();
            }
            Type lowLevelType = objs[0].GetType();
            for (int i = 0; i < objs.Length; i++)
            {
                Type type = objs[i].GetType();
                if (IsSubOf(type, lowLevelType))
                    lowLevelType = type;
            }
            return lowLevelType;
        }
    }
}
