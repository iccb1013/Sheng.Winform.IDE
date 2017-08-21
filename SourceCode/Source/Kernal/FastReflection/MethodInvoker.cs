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
    public interface IMethodInvoker
    {
        object Invoke(object instance, params object[] parameters);
    }
    public class MethodInvoker : IMethodInvoker
    {
        private Func<object, object[], object> m_invoker;
        public MethodInfo MethodInfo { get; private set; }
        public MethodInvoker(MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
            this.m_invoker = CreateInvokeDelegate(methodInfo);
        }
        public object Invoke(object instance, params object[] parameters)
        {
            return this.m_invoker(instance, parameters);
        }
        private static Func<object, object[], object> CreateInvokeDelegate(MethodInfo methodInfo)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            var parameterExpressions = new List<Expression>();
            var paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            var instanceCast = methodInfo.IsStatic ? null :
                Expression.Convert(instanceParameter, methodInfo.ReflectedType);
            var methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);
            if (methodCall.Type == typeof(void))
            {
                var lambda = Expression.Lambda<Action<object, object[]>>(
                        methodCall, instanceParameter, parametersParameter);
                Action<object, object[]> execute = lambda.Compile();
                return (instance, parameters) =>
                {
                    execute(instance, parameters);
                    return null;
                };
            }
            else
            {
                var castMethodCall = Expression.Convert(methodCall, typeof(object));
                var lambda =Expression.Lambda<Func<object, object[], object>>(
                    castMethodCall, instanceParameter, parametersParameter);
                return lambda.Compile();
            }
        }
        object IMethodInvoker.Invoke(object instance, params object[] parameters)
        {
            return this.Invoke(instance, parameters);
        }
    }
}
