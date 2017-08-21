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
using System.Linq.Expressions;
namespace Sheng.SailingEase.Kernal
{
    public interface IConstructorInvoker
    {
        object Invoke(params object[] parameters);
    }
    public class ConstructorInvoker : IConstructorInvoker
    {
        private Func<object[], object> m_invoker;
        public ConstructorInfo ConstructorInfo { get; private set; }
        public ConstructorInvoker(ConstructorInfo constructorInfo)
        {
            this.ConstructorInfo = constructorInfo;
            this.m_invoker = InitializeInvoker(constructorInfo);
        }
        private Func<object[], object> InitializeInvoker(ConstructorInfo constructorInfo)
        {
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            var parameterExpressions = new List<Expression>();
            var paramInfos = constructorInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            var instanceCreate = Expression.New(constructorInfo, parameterExpressions);
            var instanceCreateCast = Expression.Convert(instanceCreate, typeof(object));
            var lambda = Expression.Lambda<Func<object[], object>>(instanceCreateCast, parametersParameter);
            return lambda.Compile();
        }
        public object Invoke(params object[] parameters)
        {
            return this.m_invoker(parameters);
        }
        object IConstructorInvoker.Invoke(params object[] parameters)
        {
            return this.Invoke(parameters);
        }
    }
}
