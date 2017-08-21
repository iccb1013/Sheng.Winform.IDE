/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Web.Services.Protocols;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Serialization;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
        Inherited = false, AllowMultiple = false)]
    public class ExceptionShieldingAttribute : 
        Attribute, IServiceBehavior, IContractBehavior, IErrorHandler
    {
        private string exceptionPolicyName;
        private IServiceBehavior serviceBehavior;
        private IContractBehavior contractBehavior;
        private IErrorHandler errorHandler;
        public ExceptionShieldingAttribute()
            : this(ExceptionShielding.DefaultExceptionPolicy)
        {
        }
        public ExceptionShieldingAttribute(string exceptionPolicyName)
        {
            this.exceptionPolicyName = exceptionPolicyName;
            ExceptionShieldingBehavior behavior = new ExceptionShieldingBehavior(exceptionPolicyName);
            this.contractBehavior = (IContractBehavior)behavior;
            this.serviceBehavior = (IServiceBehavior)behavior;
            this.errorHandler = new ExceptionShieldingErrorHandler(exceptionPolicyName);
        }
        public string ExceptionPolicyName
        {
            get { return exceptionPolicyName; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    value = ExceptionShielding.DefaultExceptionPolicy;
                }
                exceptionPolicyName = value;
            }
        }
        public void Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            this.serviceBehavior.Validate(description, serviceHostBase);
        }
        public void AddBindingParameters(ServiceDescription description,
            ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
            this.serviceBehavior.AddBindingParameters(description, serviceHostBase, endpoints, parameters);
        }
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            this.serviceBehavior.ApplyDispatchBehavior(description, serviceHostBase);
        }
        public void AddBindingParameters(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            BindingParameterCollection bindingParameters)
        {
            this.contractBehavior.AddBindingParameters(contractDescription, endpoint, bindingParameters);
        }
        public void ApplyClientBehavior(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            ClientRuntime clientRuntime)
        {
            this.contractBehavior.ApplyClientBehavior(contractDescription, endpoint, clientRuntime);
        }
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            DispatchRuntime dispatchRuntime)
        {
             this.contractBehavior.ApplyDispatchBehavior(contractDescription, endpoint, dispatchRuntime);
        }
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            this.contractBehavior.Validate(contractDescription, endpoint);
        }
        public bool HandleError(Exception error)
        {
            return this.errorHandler.HandleError(error);
        }
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            this.errorHandler.ProvideFault(error, version, ref fault);
        }
    }
}
