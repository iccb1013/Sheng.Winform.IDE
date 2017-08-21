/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Instrumentation
{
    [TestClass]
    public class InstrumentationEventBroadcastFixture
    {
        const string connectionString = @"server=(local)\sqlexpress;database=northwind;integrated security=true;";
        Database db;
        MockListener listener;
        [TestInitialize]
        public void SetUp()
        {
            db = new SqlDatabase(connectionString);
            listener = new MockListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(db.GetInstrumentationEventProvider(), listener);
        }
        [TestMethod]
        public void ConnectionOpenedEventBroadcast()
        {
            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            Assert.IsNotNull(listener.connectionOpenedArgs);
        }
        [TestMethod]
        public void ConnectionFailedEventBroadcast()
        {
            db = new SqlDatabase("invalid;");
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(db.GetInstrumentationEventProvider(), listener);
            try
            {
                db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            }
            catch (ArgumentException) {}
            Assert.AreEqual("invalid;", listener.connectionDataArgs.ConnectionString);
        }
        [TestMethod]
        public void ConnectionExecutedEventBroadcast()
        {
            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            AssertDateIsWithinBounds(DateTime.Now, listener.commandExecutedArgs.StartTime, 2);
        }
        [TestMethod]
        public void CommandFailedEventBroadcast()
        {
            try
            {
                db.ExecuteNonQuery(CommandType.StoredProcedure, "NonExistentStoredProcedure");
            }
            catch (SqlException) {}
            Assert.AreEqual(connectionString, listener.commandFailedArgs.ConnectionString);
            Assert.AreEqual("NonExistentStoredProcedure", listener.commandFailedArgs.CommandText);
        }
        void AssertDateIsWithinBounds(DateTime expectedTime,
                                      DateTime actualTime,
                                      int maxDifference)
        {
            int diff = (expectedTime - actualTime).Seconds;
            Assert.IsTrue(diff <= maxDifference);
        }
        public class MockListener
        {
            public CommandExecutedEventArgs commandExecutedArgs;
            public CommandFailedEventArgs commandFailedArgs;
            public ConnectionFailedEventArgs connectionDataArgs;
            public EventArgs connectionOpenedArgs;
            [InstrumentationConsumer("CommandExecuted")]
            public void CommandExecutedHandler(object sender,
                                               CommandExecutedEventArgs e)
            {
                commandExecutedArgs = e;
            }
            [InstrumentationConsumer("CommandFailed")]
            public void CommandFailedHandler(object sender,
                                             CommandFailedEventArgs e)
            {
                commandFailedArgs = e;
            }
            [InstrumentationConsumer("ConnectionFailed")]
            public void ConnectionFailedHandler(object sender,
                                                ConnectionFailedEventArgs e)
            {
                connectionDataArgs = e;
            }
            [InstrumentationConsumer("ConnectionOpened")]
            public void ConnectionOpenedHandler(object sender,
                                                EventArgs e)
            {
                connectionOpenedArgs = e;
            }
        }
    }
}
