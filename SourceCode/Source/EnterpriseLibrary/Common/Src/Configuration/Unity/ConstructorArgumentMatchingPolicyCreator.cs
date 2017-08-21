/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public class ConstructorArgumentMatchingPolicyCreator : IContainerPolicyCreator
	{
		private Type targetType;
		public ConstructorArgumentMatchingPolicyCreator(Type targetType)
		{
			Guard.ArgumentNotNull(targetType, "targetType");
			this.targetType = targetType;
		}
		[SuppressMessage("Microsoft.Design", "CA1033",
			Justification = "The class defines virtual methods subclasses are expected to override if needed.")]
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			IEnumerable<ConstructorInfo> ctors
				= from ctor in this.targetType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
				  orderby ctor.GetParameters().Length descending
				  select ctor;
			foreach (ConstructorInfo ctor in ctors)
			{
				if (TryToMatchAndAddPolicies(ctor, configurationObject, policyList, instanceName))
				{
					return;
				}
			}
			throw new ArgumentException(
				string.Format(
					CultureInfo.CurrentCulture, 
					Resources.ExceptionUnableToMatchConstructorToConfigurationObject,
					configurationObject.GetType().AssemblyQualifiedName,
					this.targetType.AssemblyQualifiedName));
		}
		private bool TryToMatchAndAddPolicies(
			ConstructorInfo ctor,
			ConfigurationElement configurationObject,
			IPolicyList policyList,
			string instanceName)
		{
			Type configurationObjectType = configurationObject.GetType();
			ParameterInfo[] constructorParameters = ctor.GetParameters();
			List<PropertyInfo> matchingProperties = new List<PropertyInfo>();
			foreach (ParameterInfo parameter in constructorParameters)
			{
				PropertyInfo matchingProperty
					= configurationObjectType.GetProperty(
						parameter.Name,
						BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
				if (!(matchingProperty != null
					&& matchingProperty.CanRead
					&& matchingProperty.GetIndexParameters().Length == 0))
				{
					return false;
				}
				if (!MatchArgumentAndPropertyTypes(parameter, matchingProperty))
				{
					return false;
				}
				matchingProperties.Add(matchingProperty);
			}
			SelectedConstructor selectedConstructor = new SelectedConstructor(ctor);
			for (int i = 0; i < constructorParameters.Length; i++)
			{
				ParameterInfo parameterInfo = constructorParameters[i];
				PropertyInfo propertyInfo = matchingProperties[i];
				string parameterKey = Guid.NewGuid().ToString();
				object value = propertyInfo.GetValue(configurationObject, null);
				policyList.Set<IDependencyResolverPolicy>(CreateDependencyResolverPolicy(parameterInfo, value), parameterKey);
				selectedConstructor.AddParameterKey(parameterKey);
			}
			policyList.Set<IConstructorSelectorPolicy>(
				new FixedConstructorSelectorPolicy(selectedConstructor),
				new NamedTypeBuildKey(targetType, instanceName));
			return true;
		}
		protected virtual bool MatchArgumentAndPropertyTypes(ParameterInfo parameterInfo, PropertyInfo propertyInfo)
		{
			return parameterInfo.ParameterType.IsAssignableFrom(propertyInfo.PropertyType);
		}
		protected virtual IDependencyResolverPolicy CreateDependencyResolverPolicy(ParameterInfo parameterInfo, object value)
		{
			return new ConstantResolverPolicy(value);
		}
	}
}
