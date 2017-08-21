/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity
{
    public class ReplaceHandlerPolicyCreator : IContainerPolicyCreator
    {
        void IContainerPolicyCreator.CreatePolicies(
            IPolicyList policyList,
            string instanceName,
            ConfigurationElement configurationObject,
            IConfigurationSource configurationSource)
        {
            ReplaceHandlerData castConfigurationObject = (ReplaceHandlerData)configurationObject;
            new PolicyBuilder<ReplaceHandler, ReplaceHandlerData>(
                NamedTypeBuildKey.Make<ReplaceHandler>(instanceName),
                castConfigurationObject,
                c => new ReplaceHandler(
                    new ResourceStringResolver(
                        c.ExceptionMessageResourceType,
                        c.ExceptionMessageResourceName,
                        c.ExceptionMessage),
                    c.ReplaceExceptionType))
                .AddPoliciesToPolicyList(policyList);
        }
    }
}
