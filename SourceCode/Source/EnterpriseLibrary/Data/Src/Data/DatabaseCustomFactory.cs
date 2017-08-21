/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	public class DatabaseCustomFactory : ICustomFactory
	{
		private IDictionary<Type, IDatabaseAssembler> assemblersMapping = new Dictionary<Type, IDatabaseAssembler>(5);
		private object assemblersMappingLock = new object();
		public IDatabaseAssembler GetAssembler(Type type, string name, ConfigurationReflectionCache reflectionCache)
		{
			bool exists = false;
			IDatabaseAssembler assembler;
			lock (assemblersMappingLock)
			{
				exists = assemblersMapping.TryGetValue(type, out assembler);
			}
			if (!exists)
			{
				DatabaseAssemblerAttribute assemblerAttribute
					= reflectionCache.GetCustomAttribute<DatabaseAssemblerAttribute>(type);
				if (assemblerAttribute == null)
					throw new InvalidOperationException(
						string.Format(
							Resources.Culture,
							Resources.ExceptionDatabaseTypeDoesNotHaveAssemblerAttribute,
							type.FullName,
							name));
				assembler
					= (IDatabaseAssembler)Activator.CreateInstance(assemblerAttribute.AssemblerType);
				lock (assemblersMappingLock)
				{
					assemblersMapping[type] = assembler;
				}
			}
			return assembler;
		}
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			DatabaseConfigurationView configurationView = new DatabaseConfigurationView(configurationSource);
			ConnectionStringSettings connectionStringSettings = configurationView.GetConnectionStringSettings(name);
			DbProviderMapping mapping = configurationView.GetProviderMapping(name, connectionStringSettings.ProviderName);
			IDatabaseAssembler assembler = GetAssembler(mapping.DatabaseType, name, reflectionCache);
			Database database = assembler.Assemble(name, connectionStringSettings, configurationSource);
			return database;
		}
	}
}
