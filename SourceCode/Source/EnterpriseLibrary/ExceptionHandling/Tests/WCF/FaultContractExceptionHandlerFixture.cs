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
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class FaultContractExceptionHandlerFixture
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), null);
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowOnNullFaultContractType()
        {
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(null, null);
        }

        [TestMethod]
        public void CanHandleSimpleException()
        {
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), new NameValueCollection());
            Exception result = instance.HandleException(new Exception(), Guid.NewGuid());

            Assert.IsTrue(result is FaultContractWrapperException);
            Assert.IsNotNull(((FaultContractWrapperException)result).FaultContract);
            Assert.IsTrue(((FaultContractWrapperException)result).FaultContract is MockFaultContract);
        }

        [TestMethod]
        public void CanInjectAttributesIntoFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Message", "{Message}");
            attributes.Add("Data", "{Data}");
            attributes.Add("SomeNumber", "{OffendingNumber}");

            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            NotFiniteNumberException exception = new NotFiniteNumberException("MyException", 12341234123412);
            exception.Data.Add("someKey", "someValue");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            MockFaultContract fault = (MockFaultContract)result.FaultContract;
            Assert.AreEqual(exception.Message, fault.Message);
            Assert.AreEqual(exception.Data.Count, fault.Data.Count);
            Assert.AreEqual(exception.Data["someKey"], fault.Data["someKey"]);
            Assert.AreEqual(exception.OffendingNumber, fault.SomeNumber);
        }

        [TestMethod]
        public void CanInjectGuidAttributesIntoFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Id", "{Guid}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Guid id = Guid.NewGuid();
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(new Exception(), id);

            Assert.AreEqual(id, ((MockFaultContract)result.FaultContract).Id);
        }

        [TestMethod]
        public void CannotInjectInvalidPropertyNameIntoFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Message", "{Invalid}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Exception exception = new Exception("MyException");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.IsTrue(string.IsNullOrEmpty(((MockFaultContract)result.FaultContract).Message));
        }

        [TestMethod]
        public void CannotInjectIndexerPropertyNameIntoFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Message", "{Item}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Exception exception = new ExceptionWithIndexer("MyException");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.IsTrue(string.IsNullOrEmpty(((MockFaultContract)result.FaultContract).Message));
        }

        [TestMethod]
        public void CannotInjectNonReadablePropertyNameIntoFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Message", "{NonReadableProperty}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Exception exception = new ExceptionWithNonReadableProperty("MyException");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.IsTrue(string.IsNullOrEmpty(((MockFaultContract)result.FaultContract).Message));
        }

        [TestMethod]
        public void CanPreventFieldMapping()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Message", "");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Exception exception = new Exception("Should not copy this message");
            FaultContractWrapperException result =
                (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());
            Assert.IsNull(((MockFaultContract)result.FaultContract).Message);
        }

        [TestMethod]
        public void CanInjectInvalidPropertyNameIntoCustomPropertyFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("SomeNumber", "{Invalid}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Exception exception = new Exception("MyException");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.AreEqual((double)0, ((MockFaultContract)result.FaultContract).SomeNumber);
        }

        [TestMethod]
        public void CanInjectInvalidPropertyValueIntoFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Invalid", "{Message}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            Exception exception = new Exception("MyException");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.AreEqual(exception.Message, ((MockFaultContract)result.FaultContract).Message);
        }

        [TestMethod]
        public void CanInjectInvalidPropertyValueIntoCustomPropertyFaultContract()
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("Invalid", "{OffendingNumber}");
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), attributes);
            NotFiniteNumberException exception = new NotFiniteNumberException("MyException", 1231254);
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.AreEqual((double)0, ((MockFaultContract)result.FaultContract).SomeNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void ThrowOnFaultContractWithNoDefaultConstructor()
        {
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContractNoDefaultCtor), new NameValueCollection());
            instance.HandleException(new Exception(), Guid.NewGuid());
        }

        [TestMethod]
        public void ShouldGetReplacedExceptionMessage()
        {
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), "NewValue", null);
            Exception exception = new Exception("MyException");
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, Guid.NewGuid());

            Assert.AreEqual("NewValue", result.Message);
        }

        [TestMethod]
        public void ShouldGetReplacedExceptionMessageWithGuid()
        {
            FaultContractExceptionHandler instance = new FaultContractExceptionHandler(typeof(MockFaultContract), "NewValue: {handlingInstanceID}", null);
            Exception exception = new Exception("MyException");
            Guid guid = Guid.NewGuid();
            FaultContractWrapperException result = (FaultContractWrapperException)instance.HandleException(exception, guid);

            Assert.IsTrue(result.Message.Contains(guid.ToString()));
        }
    }

    class ExceptionWithIndexer : Exception
    {
        public ExceptionWithIndexer(string message)
            : base(message) {}

        public string this[int indexer]
        {
            get { return null; }
        }
    }

    class ExceptionWithNonReadableProperty : Exception
    {
        public ExceptionWithNonReadableProperty(string message)
            : base(message) {}

        public string NonReadableProperty
        {
            set { ; }
        }
    }
}
