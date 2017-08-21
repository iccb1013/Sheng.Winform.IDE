/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Data;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    internal class CachingMechanism
    {
        private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        public static IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
        {
            IDataParameter[] clonedParameters = new IDataParameter[originalParameters.Length];
            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (IDataParameter)((ICloneable)originalParameters[i]).Clone();
            }
            return clonedParameters;
        }
        public void Clear()
        {
            this.paramCache.Clear();
        }
		public void AddParameterSetToCache(string connectionString, IDbCommand command, IDataParameter[] parameters)
        {
            string storedProcedure = command.CommandText;
            string key = CreateHashKey(connectionString, storedProcedure);
            this.paramCache[key] = parameters;
        }
		public IDataParameter[] GetCachedParameterSet(string connectionString, IDbCommand command)
        {
            string storedProcedure = command.CommandText;
            string key = CreateHashKey(connectionString, storedProcedure);
            IDataParameter[] cachedParameters = (IDataParameter[])(this.paramCache[key]);
            return CloneParameters(cachedParameters);
        }
		public bool IsParameterSetCached(string connectionString, IDbCommand command)
        {
            string hashKey = CreateHashKey(
				connectionString,
                command.CommandText);
            return this.paramCache[hashKey] != null;
        }
        private static string CreateHashKey(string connectionString, string storedProcedure)
        {
            return connectionString + ":" + storedProcedure;
        }
    }
}
