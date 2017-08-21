/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public class StoredProcedureCreatingFixture
    {
        Database db;
        public StoredProcedureCreatingFixture(Database db)
        {
            this.db = db;
        }
        public void ArgumentExceptionWhenThereAreTooFewParameters()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", "ALFKI");
            db.ExecuteNonQuery(storedProcedure);
        }
        public void ArgumentExceptionWhenThereAreTooManyParameters()
        {
            DbCommand invalidCommand = db.GetStoredProcCommand("TestProc", "ALFKI", "EIEIO", "Hello");
            db.ExecuteNonQuery(invalidCommand);
        }
        public void CanGetOutputValueFromStoredProcedure()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", null, "ALFKI");
            db.ExecuteNonQuery(storedProcedure);
            int resultCount = Convert.ToInt32(db.GetParameterValue(storedProcedure, "vCount"));
            Assert.AreEqual(6, resultCount);
        }
        public void CanGetOutputValueFromStoredProcedureWithCachedParameters()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", null, "ALFKI");
            db.ExecuteNonQuery(storedProcedure);
            DbCommand duplicateStoredProcedure = db.GetStoredProcCommand("TestProc", null, "CHOPS");
            db.ExecuteNonQuery(duplicateStoredProcedure);
            int resultCount = Convert.ToInt32(db.GetParameterValue(duplicateStoredProcedure, "vCount"));
            Assert.AreEqual(8, resultCount);
        }
        public void ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues()
        {
            DbCommand storedProcedure = db.GetStoredProcCommand("TestProc", null, "ALFKI");
            db.ExecuteNonQuery(storedProcedure);
            DbCommand duplicateStoredProcedure = db.GetStoredProcCommand("TestProc", "CHOPS");
            db.ExecuteNonQuery(duplicateStoredProcedure);
        }
    }
}
