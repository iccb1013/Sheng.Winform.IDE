/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Unity
{
	public class FaultContractExceptionHandlerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			FaultContractExceptionHandlerData castConfigurationObject = (FaultContractExceptionHandlerData)configurationObject;
			new PolicyBuilder<FaultContractExceptionHandler, FaultContractExceptionHandlerData>(
				NamedTypeBuildKey.Make<FaultContractExceptionHandler>(instanceName),
				castConfigurationObject,
				c => new FaultContractExceptionHandler(
					Type.GetType(castConfigurationObject.FaultContractType),
					castConfigurationObject.ExceptionMessage,
					castConfigurationObject.Attributes))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
