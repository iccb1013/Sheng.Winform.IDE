/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	public class TraceListenerConstructorArgumentMatchingPolicyCreator : ConstructorArgumentMatchingPolicyCreator
	{
		public TraceListenerConstructorArgumentMatchingPolicyCreator(Type targetType)
			: base(CheckTraceListenerType(targetType))
		{ }
		private static Type CheckTraceListenerType(Type targetType)
		{
			Guard.ArgumentNotNull(targetType, "targetType");
			return targetType;
		}
		protected override bool MatchArgumentAndPropertyTypes(ParameterInfo parameterInfo, PropertyInfo propertyInfo)
		{
			return base.MatchArgumentAndPropertyTypes(parameterInfo, propertyInfo)
				|| (parameterInfo.ParameterType == typeof(ILogFormatter)
					&& propertyInfo.PropertyType == typeof(string));
		}
		protected override IDependencyResolverPolicy CreateDependencyResolverPolicy(ParameterInfo parameterInfo, object value)
		{
			if (parameterInfo.ParameterType == typeof(ILogFormatter))
			{
				if (value != null)
				{
					if (value is string)
					{
						string stringValue = (string)value;
						if (!string.IsNullOrEmpty(stringValue))
						{
							return new ReferenceResolverPolicy(NamedTypeBuildKey.Make<ILogFormatter>(stringValue));
						}
						else
						{
							value = null;
						}
					}
					else
					{
						Debug.Fail("This shouldn't happen. When matching the parameter and the property type compatibility should have been ensured.");
					}
				}
				else
				{
				}
			}
			return base.CreateDependencyResolverPolicy(parameterInfo, value);
		}
	}
}
