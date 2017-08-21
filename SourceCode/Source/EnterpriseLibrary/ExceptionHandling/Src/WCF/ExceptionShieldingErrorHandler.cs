//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.ServiceModel;
using System.Web.Services.Protocols;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    /// <summary>
    /// The error handler class that implements the exception shielding logic.
    /// </summary>
    public class ExceptionShieldingErrorHandler : IErrorHandler
    {
        #region Constructors and Fields

        private string exceptionPolicyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingErrorHandler"/> class with
        /// the <see cref="ExceptionShielding.DefaultExceptionPolicy"/> value.
        /// </summary>
        public ExceptionShieldingErrorHandler()
            : this(ExceptionShielding.DefaultExceptionPolicy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingErrorHandler"/> class.
        /// </summary>
        /// <param name="exceptionPolicyName">Name of the exception policy.</param>
        public ExceptionShieldingErrorHandler(string exceptionPolicyName)
        {
            this.exceptionPolicyName = exceptionPolicyName;
        }

        /// <summary>
        /// Gets or sets the name of the exception policy.
        /// </summary>
        /// <value>The name of the exception policy.</value>
        public string ExceptionPolicyName
        {
            get { return exceptionPolicyName; }
        }

        #endregion

        #region IErrorHandler Members

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            // Typical use of this method:
            // Implement the HandleError method to ensure error-related behaviors 
            // (error logging, assuring a fail fast, shutting down the application, and so on). 
            // If any implementation of HandleError returns true, subsequent implementations are
            // not called. If there are no implementations or no implementation returns true, it 
            // is processed according to the ServiceBehaviorAttribute.IncludeExceptionDetailInFaults 
            // property value.

            // Since we did all the exception handling and shielding in ProvideFault method,
            // we just return true.
            return true;
        }

        /// <summary>
        /// Provides the fault.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="version">The version.</param>
        /// <param name="fault">The fault.</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // Will create a default Message in case is null
            EnsureMessage(ref fault, version);

            try
            {
                // Execute the EHAB policy pipeline
                ExceptionPolicy.HandleException(error, exceptionPolicyName);
                // If we get to this line, then this exception is not
                // defined in the specified policy so treat it as unhandled if not in the default policy
                if (!exceptionPolicyName.Equals(ExceptionShielding.DefaultExceptionPolicy,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    // run first the default exception policy
                    ExceptionPolicy.HandleException(error, ExceptionShielding.DefaultExceptionPolicy);
                }
                // this is an unahndled exception so treat it as server and shield it.
                ProcessUnhandledException(error, ref fault);
            }
            catch (FaultContractWrapperException faultContractWrapperException)
            {
                // This exception was passed by the FaultContractExceptionHandler class 
                // configured in the exception handler pipline.
                // Process the client exception that will be returned unshielded. 
                // This is an application level exception that is already defined in the 
                // corresponding FaultContract.
                HandleFault(faultContractWrapperException, ref fault);
            }
            catch (FaultException)
            {
                // this is a fault that comes from the service, so just send it back to the client.
                // this fault will be thrown from the exception pipeline if it is defined.
                return;
            }
            catch (Exception unhandledException)
            {
                // this is an unahndled exception so treat it as server and shield it.
                ProcessUnhandledException(error, unhandledException, ref fault);
            }
        }

        #endregion

        #region Internal Implementation

        private void ProcessUnhandledException(Exception originalException, ref Message fault)
        {
            ProcessUnhandledException(originalException, originalException, ref fault);
        }

        private void ProcessUnhandledException(Exception originalException, Exception unhandledException, ref Message fault)
        {
            // if the current error is not already a FaultException
            // process and return, otherwise, just return the current FaultException.
            if (!IsFaultException(unhandledException))
            {
                // Log only if we don't get any handling isntance ID in the exception message.
                // in the configuration file. (see exception handlers for logging)
                Guid handlingInstanceId = ExceptionUtility.GetHandlingInstanceId(unhandledException, Guid.Empty);
                if (handlingInstanceId.Equals(Guid.Empty))
                {
                    handlingInstanceId = ExceptionUtility.LogServerException(unhandledException);
                }
                HandleFault(unhandledException, ref fault, handlingInstanceId, null);
            }
        }

        private bool IsFaultException(Exception exception)
        {
            return typeof(FaultException).IsInstanceOfType(exception);
        }

        private void HandleFault(FaultContractWrapperException faultContractWrapper, ref Message fault)
        {
            try
            {
                MessageFault messageFault = BuildMessageFault(faultContractWrapper);
                fault = Message.CreateMessage(fault.Version, messageFault, GetFaultAction(faultContractWrapper) ?? fault.Headers.Action);
            }
            catch (Exception unhandledException)
            {
                // There was an error during MessageFault build process, so treat it as an Unhandled Exception
                // log the exception and send an unhandled server exception
                Guid handlingInstanceId = ExceptionUtility.LogServerException(unhandledException);
                HandleFault(unhandledException, ref fault, handlingInstanceId, null);
            }
        }

        private void HandleFault(Exception error, ref Message fault, Guid handlingInstanceId, FaultContractWrapperException faultContractWrapper)
        {
            MessageFault messageFault = BuildMessageFault(error, handlingInstanceId);
            fault = Message.CreateMessage(fault.Version, messageFault, GetFaultAction(faultContractWrapper) ?? fault.Headers.Action);
        }

        private string GetFaultAction(FaultContractWrapperException faultContractWrapper)
        {
            if (OperationContext.Current == null) // we are running outside a host
            {
                return null;
            }

            string operationAction = OperationContext.Current.RequestContext.RequestMessage.Headers.Action;
            // for unhandled exception use the operation action
            if (faultContractWrapper == null)
            {
                return operationAction;
            }

            Type faultContractType = faultContractWrapper.FaultContract.GetType();
            foreach(DispatchOperation operation in OperationContext.Current.EndpointDispatcher.DispatchRuntime.Operations)
            {
                if (operation.Action.Equals(operationAction, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach(FaultContractInfo fault in operation.FaultContractInfos)
                    {
                        if (fault.Detail == faultContractType)
                        {
                            return fault.Action;
                        }
                    }
                }
            }
            return operationAction;
        }

        /// <summary>
        /// Build the shielded MessageFault.
        /// </summary>
        /// <param name="serverException"></param>
        /// <param name="handlingInstanceId"></param>
        /// <returns></returns>
        private MessageFault BuildMessageFault(Exception serverException, Guid handlingInstanceId)
        {
            string exceptionMessage = ExceptionUtility.FormatExceptionMessage(
                Properties.Resources.ClientUnhandledExceptionMessage,
                ExceptionUtility.GetHandlingInstanceId(serverException, handlingInstanceId));

            FaultException faultException = new FaultException(
                    new FaultReason(new FaultReasonText(exceptionMessage, CultureInfo.CurrentCulture)),
                    FaultCode.CreateReceiverFaultCode(SoapException.ServerFaultCode.Name, SoapException.ServerFaultCode.Namespace));

            return faultException.CreateMessageFault();
        }

        /// <summary>
        /// Build the unshielded MessageFault.
        /// </summary>
        /// <param name="faultContractWrapper"></param>
        /// <returns></returns>
        private MessageFault BuildMessageFault(FaultContractWrapperException faultContractWrapper)
        {
            Type faultExceptionType = typeof(FaultException<>);
            Type constructedFaultExceptionType = faultExceptionType.MakeGenericType(faultContractWrapper.FaultContract.GetType());

            //Encapsulate the FaultContract in the FaultException
            FaultException faultException =
                (FaultException)Activator.CreateInstance(
                    constructedFaultExceptionType,
                    faultContractWrapper.FaultContract,
                    new FaultReason(new FaultReasonText(faultContractWrapper.Message, CultureInfo.CurrentCulture)),
                    FaultCode.CreateSenderFaultCode(SoapException.ClientFaultCode.Name, SoapException.ClientFaultCode.Namespace));

            return faultException.CreateMessageFault();
        }

        private void EnsureMessage(ref Message message, MessageVersion defaultVersion)
        {
            if (message == null)
            {
                message = Message.CreateMessage(defaultVersion ?? MessageVersion.Default, ""); // ExceptionShielding.FaultAction);
            }
        }

        #endregion
    }
}
