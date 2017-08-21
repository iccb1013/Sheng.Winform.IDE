/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
	[CustomFactory(typeof(MockTraceListenerClientCustomFactory))]
	public class MockTraceListenerClient
	{
		public TraceListener traceListener;
	}
	public class MockTraceListenerClientCustomFactory : ICustomFactory
	{
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			MockTraceListenerClient createdObject = new MockTraceListenerClient();
			createdObject.traceListener = TraceListenerCustomFactory.Instance.Create(context, name, configurationSource, reflectionCache);
			return createdObject;
		}
	}
}
