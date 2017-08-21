/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    public class ExceptionShieldingBehavior : IServiceBehavior, IContractBehavior
    {
        private string exceptionPolicyName;
        public ExceptionShieldingBehavior()
            : this(ExceptionShielding.DefaultExceptionPolicy)
        {
        }
        public ExceptionShieldingBehavior(string exceptionPolicyName)
        {
            this.exceptionPolicyName = exceptionPolicyName;
        }
        public void Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
        }
        public void AddBindingParameters(ServiceDescription description,
            ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
        }
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                AddErrorHandler(dispatcher);
            }
        }
        public void AddBindingParameters(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            DispatchRuntime dispatchRuntime)
        {
            AddErrorHandler(dispatchRuntime.ChannelDispatcher);
        }
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
        private void AddErrorHandler(ChannelDispatcher channelDispatcher)
        {
            if (!channelDispatcher.IncludeExceptionDetailInFaults &&
                !ContainsExceptionShieldingErrorHandler(channelDispatcher.ErrorHandlers) &&
                !ContainsMetadataEndpoint(channelDispatcher.Endpoints))
            {
                IErrorHandler errorHandler = new ExceptionShieldingErrorHandler(exceptionPolicyName);
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }
        private bool ContainsExceptionShieldingErrorHandler(Collection<IErrorHandler> handlers)
        {
            foreach (IErrorHandler handler in handlers)
            {
                if (typeof(ExceptionShieldingErrorHandler).IsInstanceOfType(handler))
                {
                    return true;
                }
            }
            return false;
        }
        private bool ContainsMetadataEndpoint(SynchronizedCollection<EndpointDispatcher> endpoints)
        {
            string mexContractName = typeof(IMetadataExchange).Name;
            foreach (EndpointDispatcher endpoint in endpoints)
            {
                if (endpoint.ContractName == mexContractName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
