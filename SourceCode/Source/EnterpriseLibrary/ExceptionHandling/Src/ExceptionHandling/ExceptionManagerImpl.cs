/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class ExceptionManagerImpl : ExceptionManager, IInstrumentationEventProvider
	{
		private readonly IDictionary<string, ExceptionPolicyImpl> exceptionPolicies;
		private readonly DefaultExceptionHandlingInstrumentationProvider instrumentationProvider;
		public ExceptionManagerImpl(IDictionary<string, ExceptionPolicyImpl> exceptionPolicies)
		{
			if (exceptionPolicies == null)
				throw new ArgumentNullException("exceptionPolicies");
			this.exceptionPolicies = exceptionPolicies;
			this.instrumentationProvider = new DefaultExceptionHandlingInstrumentationProvider();
		}
		public override bool HandleException(Exception exceptionToHandle, string policyName)
		{
			if (policyName == null)
				throw new ArgumentNullException("policyName");
			if (exceptionToHandle == null)
				throw new ArgumentNullException("exceptionToHandle");
			ExceptionPolicyImpl exceptionPolicy;
			if (!this.exceptionPolicies.TryGetValue(policyName, out exceptionPolicy))
			{
				string message = string.Format(Resources.ExceptionPolicyNotFound, policyName);
				this.instrumentationProvider.FireExceptionHandlingErrorOccurred(policyName, message);
				throw new ExceptionHandlingException(message);
			}
			return exceptionPolicy.HandleException(exceptionToHandle);
		}
		public override bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
		{
			try
			{
				bool retrowAdviced = HandleException(exceptionToHandle, policyName);
				exceptionToThrow = null;
				return retrowAdviced;
			}
			catch (Exception exception)
			{
				exceptionToThrow = exception;
				return true;
			}
		}
		public override void Process(Action action, string policyName)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				if (HandleException(e, policyName))
				{
					throw;
				}
			}
		}
		object IInstrumentationEventProvider.GetInstrumentationEventProvider()
		{
			return this.instrumentationProvider;
		}
	}
}
