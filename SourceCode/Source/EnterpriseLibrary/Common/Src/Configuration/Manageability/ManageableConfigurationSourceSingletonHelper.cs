/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public class ManageableConfigurationSourceSingletonHelper : IDisposable
	{
		private readonly IDictionary<ImplementationKey, ManageableConfigurationSourceImplementation> instances;
		private readonly object lockObject = new object();
		internal bool refresh;
		public ManageableConfigurationSourceSingletonHelper()
			: this(true)
		{
		}
		public ManageableConfigurationSourceSingletonHelper(bool refresh)
		{
			this.refresh = refresh;
			instances
				= new Dictionary<ImplementationKey, ManageableConfigurationSourceImplementation>(new ImplementationKeyComparer());
		}
		public void Dispose()
		{
			foreach (ManageableConfigurationSourceImplementation instance in instances.Values)
			{
				instance.Dispose();
			}
		}
		public ManageableConfigurationSourceImplementation GetInstance(String configurationFilePath,
		                                                               IDictionary
		                                                               	<String, ConfigurationSectionManageabilityProvider>
		                                                               	manageabilityProviders,
		                                                               bool readGroupPolicies,
		                                                               bool generateWmiObjects,
		                                                               String applicationName)
		{
			if (String.IsNullOrEmpty(configurationFilePath))
				throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "configurationFilePath");
			String rootedConfigurationFilePath = RootConfigurationFilePath(configurationFilePath);
			if (!File.Exists(rootedConfigurationFilePath))
				throw new FileNotFoundException(
					String.Format(Resources.Culture, Resources.ExceptionConfigurationLoadFileNotFound, rootedConfigurationFilePath));
			ImplementationKey key = new ImplementationKey(rootedConfigurationFilePath, applicationName, readGroupPolicies);
			ManageableConfigurationSourceImplementation instance;
			lock (lockObject)
			{
				instances.TryGetValue(key, out instance);
				if (instance == null)
				{
					instance = new ManageableConfigurationSourceImplementation(rootedConfigurationFilePath,
					                                                           refresh,
					                                                           manageabilityProviders,
					                                                           readGroupPolicies,
					                                                           generateWmiObjects,
					                                                           applicationName);
					instances.Add(key, instance);
				}
			}
			return instance;
		}
		private static String RootConfigurationFilePath(String configurationFilePath)
		{
			String rootedConfigurationFile = configurationFilePath;
			if (!Path.IsPathRooted(rootedConfigurationFile))
			{
				rootedConfigurationFile
					= Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootedConfigurationFile));
			}
			return rootedConfigurationFile;
		}
	}
}
