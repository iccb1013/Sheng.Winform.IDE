/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ContainerPolicyCreatorAttribute : Attribute
	{
		public ContainerPolicyCreatorAttribute(Type policyCreatorType)
		{
			Guard.ArgumentNotNull(policyCreatorType, "policyCreatorType");
			if (!typeof (IContainerPolicyCreator).IsAssignableFrom(policyCreatorType))
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture,
						Resources.ExceptionMustImplementIContainerPolicyCreator, policyCreatorType.AssemblyQualifiedName),
					"policyCreatorType");
			}
			if (policyCreatorType.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionMustHaveNoArgsConstructor, policyCreatorType.AssemblyQualifiedName),
					"policyCreatorType");
			}
			this.PolicyCreatorType = policyCreatorType;
		}
		public Type PolicyCreatorType
		{
			get;
			private set;
		}
	}
}
