/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    public class ParameterCache
    {
        private CachingMechanism cache = new CachingMechanism();
		public void SetParameters(DbCommand command, Database database)
        {
			if (command == null) throw new ArgumentNullException("command");
			if (database == null) throw new ArgumentNullException("database");
			if (AlreadyCached(command, database))
            {
				AddParametersFromCache(command, database);
            }
            else
            {
				database.DiscoverParameters(command);
                IDataParameter[] copyOfParameters = CreateParameterCopy(command);
				this.cache.AddParameterSetToCache(database.ConnectionString, command, copyOfParameters);
            }
        }
        protected internal void Clear()
        {
            this.cache.Clear();
        }       
		protected virtual void AddParametersFromCache(DbCommand command, Database database)
        {
			IDataParameter[] parameters = this.cache.GetCachedParameterSet(database.ConnectionString, command);
            foreach (IDataParameter p in parameters)
            {
				command.Parameters.Add(p);
            }
        }
		private bool AlreadyCached(IDbCommand command, Database database)
		{
			return this.cache.IsParameterSetCached(database.ConnectionString, command);
		}
		private static IDataParameter[] CreateParameterCopy(DbCommand command)
        {
			IDataParameterCollection parameters = command.Parameters;
            IDataParameter[] parameterArray = new IDataParameter[parameters.Count];
            parameters.CopyTo(parameterArray, 0);
            return CachingMechanism.CloneParameters(parameterArray);
        }
    }
}
