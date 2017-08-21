/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public sealed class ExceptionPolicyEntry
	{
		private PostHandlingAction postHandlingAction;
		private ICollection<IExceptionHandler> handlers;
		private string policyName = string.Empty;
		private ExceptionHandlingInstrumentationProvider instrumentationProvider;
		public ExceptionPolicyEntry(PostHandlingAction postHandlingAction, ICollection<IExceptionHandler> handlers)
		{
			if (handlers == null) throw new ArgumentNullException("handlers");
			this.postHandlingAction = postHandlingAction;
			this.handlers = handlers;
		}
		internal string PolicyName
		{
			set { policyName = value; }
		}
		private ExceptionHandlingInstrumentationProvider InstrumentationProvider
		{
			get { return instrumentationProvider; }
			set { instrumentationProvider = value; }
		}
		public bool Handle(Exception exceptionToHandle)
		{
			if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandler");
			Guid handlingInstanceID = Guid.NewGuid();
			Exception chainException = ExecuteHandlerChain(exceptionToHandle, handlingInstanceID);
			if (InstrumentationProvider != null) InstrumentationProvider.FireExceptionHandledEvent();
			return RethrowRecommended(chainException, exceptionToHandle);
		}
		private Exception IntentionalRethrow(Exception chainException, Exception originalException)
		{
			if (chainException != null)
			{
				throw chainException;
			}
			Exception wrappedException = new ExceptionHandlingException(Resources.ExceptionNullException);
			if (InstrumentationProvider != null)
			{
				InstrumentationProvider.FireExceptionHandlingErrorOccurred(ExceptionUtility.FormatExceptionHandlingExceptionMessage(policyName, wrappedException, chainException, originalException));
			}
			return wrappedException;
		}
		private bool RethrowRecommended(Exception chainException, Exception originalException)
		{
			if (postHandlingAction == PostHandlingAction.None) return false;
			if (postHandlingAction == PostHandlingAction.ThrowNewException)
			{
				throw IntentionalRethrow(chainException, originalException);
			}
			return true;
		}
		private Exception ExecuteHandlerChain(Exception ex, Guid handlingInstanceID)
		{
			string lastHandlerName = String.Empty;
			Exception originalException = ex;
			try
			{
				foreach (IExceptionHandler handler in handlers)
				{
					lastHandlerName = handler.GetType().Name;
					ex = handler.HandleException(ex, handlingInstanceID);
					if (InstrumentationProvider != null) InstrumentationProvider.FireExceptionHandlerExecutedEvent();
				}
			}
			catch (Exception handlingException)
			{
				if (InstrumentationProvider != null)
				{
					InstrumentationProvider.FireExceptionHandlingErrorOccurred(
						ExceptionUtility.FormatExceptionHandlingExceptionMessage(
							policyName,
							new ExceptionHandlingException(string.Format(Resources.Culture, Resources.UnableToHandleException, lastHandlerName), handlingException),
							ex,
							originalException
						));
				}
				throw new ExceptionHandlingException(string.Format(Resources.Culture, Resources.UnableToHandleException, lastHandlerName));
			}
			return ex;
		}
		public void SetInstrumentationProvider(ExceptionHandlingInstrumentationProvider provider)
		{
			this.InstrumentationProvider = provider;
		}
	}
}
