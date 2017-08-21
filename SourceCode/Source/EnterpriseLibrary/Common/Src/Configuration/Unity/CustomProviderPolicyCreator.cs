/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public sealed class CustomProviderPolicyCreator<T> : IContainerPolicyCreator
		where T : NameTypeConfigurationElement, IHelperAssistedCustomConfigurationData<T>
	{
		private readonly Type[] constructorTypes = new Type[] { typeof(NameValueCollection) };
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			T castConfigurationObject = (T)configurationObject;
			ConstructorInfo ctor = castConfigurationObject.Type.GetConstructor(
				BindingFlags.Public | BindingFlags.Instance, null, constructorTypes, null);
			if (ctor == null)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionCustomProviderTypeDoesNotHaveTheRequiredConstructor,
						castConfigurationObject.Type.AssemblyQualifiedName),
					"configurationObject");
			}
			SelectedConstructor selectedCtor = new SelectedConstructor(ctor);
			string parameterKey = Guid.NewGuid().ToString();
			selectedCtor.AddParameterKey(parameterKey);
			policyList.Set<IConstructorSelectorPolicy>(
				new FixedConstructorSelectorPolicy(selectedCtor),
				new NamedTypeBuildKey(castConfigurationObject.Type, instanceName));
			policyList.Set<IDependencyResolverPolicy>(
				new ConstantResolverPolicy(new NameValueCollection(castConfigurationObject.Attributes)),
				parameterKey);
		}
	}
}
