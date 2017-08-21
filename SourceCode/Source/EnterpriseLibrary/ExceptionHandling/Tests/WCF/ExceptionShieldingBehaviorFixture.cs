/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class ExceptionShieldingBehaviorFixture
    {
        [TestMethod]
        public void ShouldSetShieldingWithNonIncludeExceptionDetailInFaults()
        {
            Uri serviceUri = new Uri("http://tests:30003");
            ServiceHost host = new ServiceHost(typeof(MockService), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new WSHttpBinding(), serviceUri);
            host.Open();
            try
            {
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Assert.AreEqual(0, dispatcher.ErrorHandlers.Count);
                    Assert.IsFalse(dispatcher.IncludeExceptionDetailInFaults);
                }
                ExceptionShieldingBehavior behavior = new ExceptionShieldingBehavior();
                behavior.ApplyDispatchBehavior(null, host);
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Assert.AreEqual(1, dispatcher.ErrorHandlers.Count);
                    Assert.IsTrue(dispatcher.ErrorHandlers[0].GetType().IsAssignableFrom(typeof(ExceptionShieldingErrorHandler)));
                }
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }
        [TestMethod]
        public void ShouldFilterMexHttpEndpointsAndAddOneInstance()
        {
            FilterMexEndpointsAndAddOneInstance(Uri.UriSchemeHttp);
        }
        [TestMethod]
        public void ShouldFilterMexHttpsEndpointsAndAddOneInstance()
        {
            FilterMexEndpointsAndAddOneInstance(Uri.UriSchemeHttps);
        }
        void FilterMexEndpointsAndAddOneInstance(string mexScheme)
        {
            Uri serviceUri = new Uri(mexScheme + "://tests:30003");
            ServiceHost host = new ServiceHost(typeof(MockServiceWithShielding), serviceUri);
            SecurityMode securityMode = SetMexAndSecurity(host, mexScheme);
            WSHttpBinding mockServiceBinding = new WSHttpBinding(securityMode);
            host.AddServiceEndpoint(typeof(IMockService), mockServiceBinding, serviceUri);
            host.Open();
            try
            {
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Assert.AreEqual(dispatcher.BindingName.Contains(mockServiceBinding.Name) ? 1 : 0, dispatcher.ErrorHandlers.Count);
                    Assert.IsFalse(dispatcher.IncludeExceptionDetailInFaults);
                }
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }
        SecurityMode SetMexAndSecurity(ServiceHost host,
                                       string mexScheme)
        {
            ServiceMetadataBehavior mexHttpBehavior = new ServiceMetadataBehavior();
            Uri mexUri = new Uri("/mex", UriKind.Relative);
            Binding binding;
            SecurityMode securityMode;
            if (mexScheme == Uri.UriSchemeHttp)
            {
                mexHttpBehavior.HttpGetEnabled = true;
                binding = MetadataExchangeBindings.CreateMexHttpBinding();
                securityMode = SecurityMode.Message;
            }
            else
            {
                mexHttpBehavior.HttpsGetEnabled = true;
                binding = MetadataExchangeBindings.CreateMexHttpsBinding();
                securityMode = SecurityMode.TransportWithMessageCredential;
            }
            host.Description.Behaviors.Add(mexHttpBehavior);
            host.AddServiceEndpoint(typeof(IMetadataExchange), binding, mexUri);
            return securityMode;
        }
    }
}
