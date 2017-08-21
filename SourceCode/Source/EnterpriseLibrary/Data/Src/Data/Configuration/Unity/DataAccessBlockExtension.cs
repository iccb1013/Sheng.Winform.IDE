/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity
{
    public class DataAccessBlockExtension : EnterpriseLibraryBlockExtension
    {
        protected override void Initialize()
        {
            DatabaseConfigurationView configurationView = new DatabaseConfigurationView(ConfigurationSource);
            string defaultDatabaseName = configurationView.DefaultName;
            foreach (ConnectionStringSettings connectionStringSettings
                in configurationView.GetConnectionStringSettingsCollection())
            {
                if (IsValidProviderName(connectionStringSettings.ProviderName))
                {
                    DbProviderMapping mapping
                        = configurationView.GetProviderMapping(
                            connectionStringSettings.Name,
                            connectionStringSettings.ProviderName);
                    Type databaseType = mapping.DatabaseType;
                    this.Context.Policies.Set<IBuildKeyMappingPolicy>(
                        new BuildKeyMappingPolicy(new NamedTypeBuildKey(databaseType, connectionStringSettings.Name)),
                        NamedTypeBuildKey.Make<Database>(connectionStringSettings.Name));
                    if (connectionStringSettings.Name == defaultDatabaseName)
                    {
                        this.Context.Policies.Set<IBuildKeyMappingPolicy>(
                            new BuildKeyMappingPolicy(new NamedTypeBuildKey(databaseType, connectionStringSettings.Name)),
                            NamedTypeBuildKey.Make<Database>());
                    }
                    IContainerPolicyCreator policyCreator = GetContainerPolicyCreator(databaseType, null);
                    policyCreator.CreatePolicies(
                        this.Context.Policies,
                        connectionStringSettings.Name,
                        connectionStringSettings,
                        this.ConfigurationSource);
                }
            }
        }
        private static bool IsValidProviderName(string providerName)
        {
            try
            {
                return !string.IsNullOrEmpty(providerName) && DbProviderFactories.GetFactory(providerName) != null;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
