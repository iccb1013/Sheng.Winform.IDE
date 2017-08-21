/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	[CustomFactory(typeof(ExceptionPolicyCustomFactory))]
	public class ExceptionPolicyImpl : IInstrumentationEventProvider
	{
		private Dictionary<Type, ExceptionPolicyEntry> policyEntries;
		ExceptionHandlingInstrumentationProvider instrumentationProvider;
		public ExceptionPolicyImpl(string policyName, Dictionary<Type, ExceptionPolicyEntry> policyEntries)
		{
			if (policyEntries == null) throw new ArgumentNullException("policyEntries");
			if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "policyEntries");
			this.policyEntries = policyEntries;
			this.instrumentationProvider = new ExceptionHandlingInstrumentationProvider();
			InjectPolicyNameIntoEntries(policyName, policyEntries);
			InjectInstrumentationProviderToEntries(policyEntries);
		}
		public bool HandleException(Exception exceptionToHandle)
		{
			if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandler");
			ExceptionPolicyEntry entry = GetPolicyEntry(exceptionToHandle);
			if (entry == null)
			{
				return true;
			}
			return entry.Handle(exceptionToHandle);
		}
		private ExceptionPolicyEntry GetPolicyEntry(Exception ex)
		{
			Type exceptionType = ex.GetType();
			ExceptionPolicyEntry entry = this.FindExceptionPolicyEntry(exceptionType);
			return entry;
		}
		public ExceptionPolicyEntry GetPolicyEntry(Type exceptionType)
		{
			if (policyEntries.ContainsKey(exceptionType))
			{
				return policyEntries[exceptionType];
			}
			return null;
		}
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}
		private ExceptionPolicyEntry FindExceptionPolicyEntry(Type exceptionType)
		{
			ExceptionPolicyEntry entry = null;
			while (exceptionType != typeof(Object))
			{
				entry = this.GetPolicyEntry(exceptionType);
				if (entry == null)
				{
					exceptionType = exceptionType.BaseType;
				}
				else
				{
					break;
				}
			}
			return entry;
		}
		private void InjectPolicyNameIntoEntries(string policyName, Dictionary<Type, ExceptionPolicyEntry> policyEntries)
		{
			foreach (ExceptionPolicyEntry entry in policyEntries.Values)
			{
				entry.PolicyName = policyName;
			}
		}
		private void InjectInstrumentationProviderToEntries(Dictionary<Type, ExceptionPolicyEntry> policyEntries)
		{
			foreach (ExceptionPolicyEntry entry in policyEntries.Values)
			{
				entry.SetInstrumentationProvider(instrumentationProvider);
			}
		}
	}
}
