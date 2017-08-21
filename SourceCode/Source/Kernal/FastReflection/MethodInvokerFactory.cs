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
namespace Sheng.SailingEase.Kernal.FastReflection
{
    public class MethodInvokerFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        public IMethodInvoker Create(MethodInfo key)
        {
            return new MethodInvoker(key);
        }
        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }
    }
}
