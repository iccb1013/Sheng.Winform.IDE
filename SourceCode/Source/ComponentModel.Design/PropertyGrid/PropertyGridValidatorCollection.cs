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
using System.Collections.ObjectModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyGridValidatorCollection : Collection<PropertyGridValidator>
    {
        public PropertyGridValidator GetValidator(object[] objs)
        {
            Type type = ReflectionHelper.GetLowLevelType(objs);
            foreach (PropertyGridValidator validator in this)
            {
                if (validator.Type.Equals(type))
                    return validator;
                if (validator.ActOnSub && ReflectionHelper.IsSubOf(type, validator.Type))
                {
                    return validator;
                }
            }
            return null;
        }
    }
}
