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
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    /// <summary>
    /// Summary description for ExceptionShieldingErrorHandlerFixture
    /// </summary>
    [TestClass]
    public class ExceptionShieldingErrorHandlerFixture
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            ExceptionShieldingErrorHandler instance = new ExceptionShieldingErrorHandler();
            Assert.IsNotNull(instance);
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, instance.ExceptionPolicyName);
        }

        [TestMethod]
        public void CanCreateInstanceWithPolicyName()
        {
            ExceptionShieldingErrorHandler instance = new ExceptionShieldingErrorHandler("Policy");
            Assert.AreEqual("Policy", instance.ExceptionPolicyName);
        }

        [TestMethod]
        public void CanHandleErrorWithMessageFault()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            bool result = shielding.HandleError(null);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanProvideFaultWithNullException()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            Message message = GetDefaultMessage();
            shielding.ProvideFault(null, MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.Code.IsReceiverFault);
            CheckHandlingInstanceId("DefaultLogs.txt", fault.Reason.ToString());
        }

        [TestMethod]
        public void CanProvideFaultWithNullVersion()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            Message message = GetDefaultMessage();
            shielding.ProvideFault(new ArithmeticException(), null, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.Code.IsSenderFault);
        }

        [TestMethod]
        public void CanProvideFaultWithNullMessage()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            Message message = null;
            shielding.ProvideFault(new ArithmeticException(), MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.Code.IsSenderFault);
        }

        [TestMethod]
        public void CanProvideFaultWithMockHandler()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            ArithmeticException exception = new ArithmeticException("My Exception");
            Message message = GetDefaultMessage();
            shielding.ProvideFault(exception, MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.Code.IsSenderFault);
            //DataContractSerializer serializer = new DataContractSerializer(typeof(MockFaultContract));
            MockFaultContract details = fault.GetDetail<MockFaultContract>();
            Assert.IsNotNull(details);
            Assert.AreEqual(exception.Message, details.Message);
        }

        [TestMethod]
        public void CanProvideFaultOnInvalidPolicyName()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("Invalid Policy");
            Message message = GetDefaultMessage();
            shielding.ProvideFault(new Exception(), MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsFalse(fault.HasDetail);
            Assert.IsTrue(fault.Code.IsReceiverFault);
            CheckHandlingInstanceId("DefaultLogs.txt", fault.Reason.ToString());
        }

        [TestMethod]
        public void CanProvideFaultOnCustomPolicyName()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("CustomPolicy");
            Message message = GetDefaultMessage();
            shielding.ProvideFault(new ArgumentException("Arg"), MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.HasDetail);
            Assert.IsTrue(fault.Code.IsSenderFault);
            //DataContractSerializer serializer = new DataContractSerializer(typeof(MockFaultContract));
            MockFaultContract details = fault.GetDetail<MockFaultContract>();
            Assert.IsNotNull(details);
            Assert.AreEqual("Arg", details.Message);
        }

        [TestMethod]
        public void CanProvideFaultOnUnhandledLoggedExceptions()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("UnhandledLoggedExceptions");
            Message message = GetDefaultMessage();
            shielding.ProvideFault(new Exception(), MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsFalse(fault.HasDetail);
            Assert.IsTrue(fault.Code.IsReceiverFault);
            CheckHandlingInstanceId("UnhandledLogs.txt", fault.Reason.ToString());
        }

        [TestMethod]
        public void CanProvideFaultOnHandledLoggedExceptions()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("HandledLoggedExceptions");
            Message message = GetDefaultMessage();
            shielding.ProvideFault(new ArithmeticException(), MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.HasDetail);
            Assert.IsTrue(fault.Code.IsSenderFault);
            CheckLoggedMessage("HandledLogs.txt", fault.Reason.ToString());
        }

        [TestMethod]
        public void CanProvideFaultOnExceptionTypeNotFound()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("ExceptionTypeNotFound");
            Message message = GetDefaultMessage();
            shielding.ProvideFault(new Exception(), MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsFalse(fault.HasDetail);
            Assert.IsTrue(fault.Code.IsReceiverFault);
            CheckHandlingInstanceId("DefaultLogs.txt", fault.Reason.ToString());
        }

        [TestMethod]
        public void ShouldReturnReceiverFault()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("CustomPolicy");
            Message message = GetDefaultMessage();
            Exception exception = new NotSupportedException("NotSupportedException");
            shielding.ProvideFault(exception, MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.Code.IsReceiverFault);
            Assert.IsFalse(string.IsNullOrEmpty(fault.Reason.ToString()));
            Assert.IsFalse(fault.HasDetail);
            CheckHandlingInstanceId("DefaultLogs.txt", fault.Reason.ToString());
        }

        [TestMethod]
        public void ShouldReturnSenderFault()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            Message message = GetDefaultMessage();
            Exception exception = new ArithmeticException("Message");
            shielding.ProvideFault(exception, MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.Code.IsSenderFault);
            Assert.IsFalse(string.IsNullOrEmpty(fault.Reason.ToString()));
            Assert.IsTrue(fault.HasDetail);
            MockFaultContract details = fault.GetDetail<MockFaultContract>();
            Assert.IsNotNull(details);
            Assert.AreEqual(exception.Message, details.Message);
        }

        [TestMethod]
        public void CanPopulateFaultContractFromExceptionProperties()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            Message message = GetDefaultMessage();
            Exception exception = new ArgumentNullException("MyMessage");
            shielding.ProvideFault(exception, MessageVersion.Default, ref message);

            MessageFault fault = GetFaultFromMessage(message);
            Assert.IsTrue(fault.HasDetail);
            Assert.IsTrue(fault.Code.IsSenderFault);
            MockFaultContract details = fault.GetDetail<MockFaultContract>();
            Assert.IsNotNull(details);
            Assert.AreEqual(exception.Message, details.Message);
            Assert.IsTrue(details.Id != Guid.Empty);
        }

        [TestMethod]
        public void ShouldGetFaultExceptionWithPolicy()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler("FaultException");
            FaultException faultException = GetFaultException("test", SoapException.ServerFaultCode.Name);
            Message message = Message.CreateMessage(MessageVersion.Default, faultException.CreateMessageFault(), "");
            shielding.ProvideFault(faultException, MessageVersion.Default, ref message);

            MessageFault actualFault = GetFaultFromMessage(message);
            MessageFault expectedFault = faultException.CreateMessageFault();
            Assert.AreEqual(expectedFault.Reason.ToString(), actualFault.Reason.ToString());
            Assert.AreEqual(expectedFault.HasDetail, actualFault.HasDetail);
            Assert.AreEqual(expectedFault.Code.IsReceiverFault, actualFault.Code.IsReceiverFault);
        }

        [TestMethod]
        public void ShouldGetFaultExceptionWithoutPolicy()
        {
            ExceptionShieldingErrorHandler shielding = new ExceptionShieldingErrorHandler();
            FaultException faultException = GetFaultException("test", SoapException.ServerFaultCode.Name);
            Message message = Message.CreateMessage(MessageVersion.Default, faultException.CreateMessageFault(), "");
            shielding.ProvideFault(faultException, MessageVersion.Default, ref message);

            MessageFault actualFault = GetFaultFromMessage(message);
            MessageFault expectedFault = faultException.CreateMessageFault();
            Assert.AreEqual(expectedFault.Reason.ToString(), actualFault.Reason.ToString());
            Assert.AreEqual(expectedFault.HasDetail, actualFault.HasDetail);
            Assert.AreEqual(expectedFault.Code.IsReceiverFault, actualFault.Code.IsReceiverFault);
        }

        void CheckLoggedMessage(string logFileName,
                                string message)
        {
            // close the current logger
            Logger.Writer.Dispose();
            string logFileContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName));
            Assert.IsTrue(logFileContent.Contains(message));
        }

        void CheckHandlingInstanceId(string logFileName,
                                     string message)
        {
            // close the current logger
            Logger.Writer.Dispose();
            string logFileContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName));
            Regex regex = new Regex("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}");
            Match match = regex.Match(message);

            Assert.IsFalse(string.IsNullOrEmpty(match.Value));
            Assert.IsTrue(logFileContent.Contains(match.Value));
        }

        FaultException GetFaultException(string faultReason,
                                         string faultCode)
        {
            FaultException faultException =
                new FaultException(
                    new FaultReason(faultReason),
                    FaultCode.CreateReceiverFaultCode(faultCode,
                                                      SoapException.ServerFaultCode.Namespace));
            return faultException;
        }

        Message GetDefaultMessage()
        {
            return Message.CreateMessage(MessageVersion.Default, "testing");
        }

        MessageFault GetFaultFromMessage(Message message)
        {
            Assert.IsNotNull(message);
            Assert.IsTrue(message.IsFault);
            return MessageFault.CreateFault(message, 0x1000);
        }
    }
}
