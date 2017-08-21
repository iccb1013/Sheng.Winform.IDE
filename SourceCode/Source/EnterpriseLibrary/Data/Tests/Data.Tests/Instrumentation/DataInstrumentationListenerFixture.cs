//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation.Tests
{
    [TestClass]
    public class DataInstrumentationListenerFixture
    {
        [TestMethod]
        public void DoLotsOfConnectionFailures()
        {
            int numberOfEvents = 50;
            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                SqlDatabase db = new SqlDatabase("BadConnectionString");
                DataInstrumentationListener listener = new DataInstrumentationListener("foo", true, true, true, "ApplicationInstanceName");
                DataInstrumentationListenerBinder binder = new DataInstrumentationListenerBinder();
                binder.Bind(db.GetInstrumentationEventProvider(), listener);

                for (int i = 0; i < numberOfEvents; i++)
                {
                    try
                    {
                        db.ExecuteScalar(CommandType.Text, "Select count(*) from Region");
                    }
                    catch {}
                }

                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
                Assert.AreEqual("ConnectionFailedEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual("foo", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
                Assert.AreEqual(db.ConnectionStringWithoutCredentials, eventListener.EventsReceived[0].GetPropertyValue("ConnectionString"));
            }
        }

        [TestMethod]
        public void CommandFailureFiresWmiEvent()
        {
            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                Database db = DatabaseFactory.CreateDatabase();

                try
                {
                    db.ExecuteNonQuery(CommandType.StoredProcedure, "BadCommandText");
                }
                catch {}

                eventListener.WaitForEvents();
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("CommandFailedEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual("Service_Dflt", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
                Assert.AreEqual(db.ConnectionStringWithoutCredentials, eventListener.EventsReceived[0].GetPropertyValue("ConnectionString"));
                Assert.AreEqual("BadCommandText", eventListener.EventsReceived[0].GetPropertyValue("CommandText"));
            }
        }

        const string counterCategoryName = "Enterprise Library Data Counters";
        const string TotalConnectionOpenedCounter = "Total Connections Opened";
        const string TotalConnectionFailedCounter = "Total Connections Failed";
        const string TotalCommandsExecutedCounter = "Total Commands Executed";
        const string TotalCommandsFailedCounter = "Total Commands Failed";
        const string instanceName = "Foo";
        string formattedInstanceName;

        EnterpriseLibraryPerformanceCounter totalConnectionOpenedCounter;
        EnterpriseLibraryPerformanceCounter totalConnectionFailedCounter;
        EnterpriseLibraryPerformanceCounter totalCommandsExecutedCounter;
        EnterpriseLibraryPerformanceCounter totalCommandsFailedCounter;

        DataInstrumentationListener listener;
        IPerformanceCounterNameFormatter nameFormatter;

        [TestInitialize]
        public void SetUp()
        {
            nameFormatter = new FixedPrefixNameFormatter("Prefix - ");
            listener = new DataInstrumentationListener(instanceName, true, true, true, nameFormatter);
            formattedInstanceName = nameFormatter.CreateName(instanceName);
            totalConnectionOpenedCounter = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalConnectionOpenedCounter, formattedInstanceName);
            totalConnectionFailedCounter = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalConnectionFailedCounter, formattedInstanceName);
            totalCommandsExecutedCounter = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalCommandsExecutedCounter, formattedInstanceName);
            totalCommandsFailedCounter = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalCommandsFailedCounter, formattedInstanceName);
        }

        [TestMethod]
        public void TotalConnectionOpenedCounterIncremented()
        {
            listener.ConnectionOpened(this, EventArgs.Empty);

            long expected = 1;
            Assert.AreEqual(expected, totalConnectionOpenedCounter.Value);
        }

        [TestMethod]
        public void TotalConnectionFailedCounterIncremented()
        {
            listener.ConnectionFailed(this, new ConnectionFailedEventArgs("BadConnectionString", new Exception()));

            long expected = 1;
            Assert.AreEqual(expected, totalConnectionFailedCounter.Value);
        }

        [TestMethod]
        public void TotalCommandExecutedCounterIncremented()
        {
            listener.CommandExecuted(this, new CommandExecutedEventArgs(DateTime.Now));
            listener.CommandExecuted(this, new CommandExecutedEventArgs(DateTime.Now));
            long expected = 2;
            Assert.AreEqual(expected, totalCommandsExecutedCounter.Value);
        }

        [TestMethod]
        public void TotalCommandFailedCounterIncremented()
        {
            long maxCount = 5;
            for (long i = 0; i < maxCount; i++)
            {
                listener.CommandFailed(this, new CommandFailedEventArgs("fooCommand", "badConnection", new Exception()));
            }

            Assert.AreEqual(maxCount, totalCommandsFailedCounter.Value);
        }

        void ClearExistingCounts()
        {
            totalConnectionOpenedCounter.Clear();
            totalConnectionFailedCounter.Clear();
            totalCommandsExecutedCounter.Clear();
            totalCommandsFailedCounter.Clear();
        }

        /*
        private static readonly string counterCategoryName = DataInstrumentationListener.CounterCategoryName;
        private static readonly string connectionOpenedCounterName = DataInstrumentationListener.ConnectionOpenedCounterName;
        private static readonly string commandExecutedCounterName = DataInstrumentationListener.CommandExecutedCounterName;

        private const string instanceName = "Foo";

        private static readonly string[] allInstanceNamesUsedInThisFixture = new string[] { DataInstrumentationListener.DefaultCounterName, instanceName, "first", "second" };

        EnterpriseLibraryPerformanceCounter connectionOpenedCounter;
        EnterpriseLibraryPerformanceCounter commandExecutedCounter;
        
        DataInstrumentationListener listener;
        IPerformanceCounterNameFormatter nameFormatter;

        [TestInitialize]
        public void SetUp()
        {
            nameFormatter = new FixedPrefixNameFormatter("Prefix - ");
            listener = new DataInstrumentationListener(instanceName, true, true, true, nameFormatter);
            connectionOpenedCounter = new EnterpriseLibraryPerformanceCounter(counterCategoryName, connectionOpenedCounterName);
            commandExecutedCounter = new EnterpriseLibraryPerformanceCounter(counterCategoryName, commandExecutedCounterName);

            ClearExistingCounts(connectionOpenedCounterName);
            ClearExistingCounts(commandExecutedCounterName);
        }

        [TestMethod]
        public void SinglePerformanceCounterIncremented()
        {
            listener.ConnectionOpened(this, new ConnectionDataEventArgs("connection string"));

            long expected = 1;
            Assert.AreEqual(expected, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(instanceName)));
        }

        [TestMethod]
        public void SinglePerformanceCounterIncrementedAndNamedProperly()
        {
            listener.ConnectionOpened(this, new ConnectionDataEventArgs("connection string"));

            long expected = 1;
            Assert.AreEqual(expected, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(DataInstrumentationListener.DefaultCounterName)));
        }

        [TestMethod]
        public void CounterWithMultipleInstancesIncrementedAndNamedProperly()
        {
            listener.ConnectionOpened(this, new ConnectionDataEventArgs("connection string"));

            long expected = 1;
            Assert.AreEqual(expected, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(DataInstrumentationListener.DefaultCounterName)));
            Assert.AreEqual(expected, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(instanceName)));
        }

        [TestMethod]
        public void IncrementsMultipleInstancesIndependently()
        {
            string firstInstanceName = "first";
            string secondInstanceName = "second";

            FixedPrefixNameFormatter nameFormater = new FixedPrefixNameFormatter("Baz - ");
            DataInstrumentationListener firstListener = new DataInstrumentationListener(firstInstanceName, true, true, true, nameFormatter);
            DataInstrumentationListener secondListener = new DataInstrumentationListener(secondInstanceName, true, true, true, nameFormatter);
            firstListener.ConnectionOpened(this, new ConnectionDataEventArgs("connection string"));

            Assert.AreEqual((long)1, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(firstInstanceName)));
            Assert.AreEqual((long)0, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(secondInstanceName)));

            secondListener.ConnectionOpened(this, new ConnectionDataEventArgs("connection string"));

            Assert.AreEqual((long)1, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(firstInstanceName)));
            Assert.AreEqual((long)1, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(secondInstanceName)));

            Assert.AreEqual((long)2, connectionOpenedCounter.GetValueFor(nameFormatter.CreateName(DataInstrumentationListener.DefaultCounterName)));
        }
        
        [TestMethod]
        public void CommandExecutedIncrementedCorrectly()
        {
            listener.CommandExecuted(this, new CommandExecutedEventArgs(DateTime.Now));
            
            Assert.AreEqual((long)1, commandExecutedCounter.GetValueFor(nameFormatter.CreateName(DataInstrumentationListener.DefaultCounterName)));
            Assert.AreEqual((long)1, commandExecutedCounter.GetValueFor(nameFormatter.CreateName(instanceName)));
        }
        
        [TestMethod]
        public void DataIsHookedUpToInstrumentationAtConstruction()
        {
            Database db = DatabaseFactory.CreateDatabase();
            db.ExecuteScalar(CommandType.Text, "select count(*) from Region");
        }

        private void ClearExistingCounts(string counterName)
        {
            string[] formattedNames = new string[allInstanceNamesUsedInThisFixture.Length];
            for (int i = 0; i < formattedNames.Length; i++)
            {
                formattedNames[i] = nameFormatter.CreateName(allInstanceNamesUsedInThisFixture[i]);
            }
            new EnterpriseLibraryPerformanceCounter(counterCategoryName, counterName, formattedNames)
                .Clear();
        }
*/
    }

    public class FixedPrefixNameFormatter : IPerformanceCounterNameFormatter
    {
        string prefix;

        public FixedPrefixNameFormatter(string prefix)
        {
            this.prefix = prefix;
        }

        public string CreateName(string nameSuffix)
        {
            return prefix + nameSuffix;
        }
    }
}
