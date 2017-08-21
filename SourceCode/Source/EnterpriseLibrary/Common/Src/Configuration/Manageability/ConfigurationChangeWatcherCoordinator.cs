/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public class ConfigurationChangeWatcherCoordinator : IDisposable
    {
        public const String MainConfigurationFileSource = "";
        readonly Dictionary<String, ConfigurationChangeFileWatcher> configSourceWatcherMapping;
        readonly String mainConfigurationFileName;
        readonly String mainConfigurationFilePath;
        readonly bool refresh;
        public ConfigurationChangeWatcherCoordinator(String mainConfigurationFileName,
                                                     bool refresh)
        {
            this.mainConfigurationFileName = mainConfigurationFileName;
            mainConfigurationFilePath = Path.GetDirectoryName(mainConfigurationFileName);
            this.refresh = refresh;
            configSourceWatcherMapping = new Dictionary<String, ConfigurationChangeFileWatcher>();
            CreateWatcherForConfigSource(MainConfigurationFileSource);
        }
        public ICollection<String> WatchedConfigSources
        {
            get { return configSourceWatcherMapping.Keys; }
        }
        public event ConfigurationChangedEventHandler ConfigurationChanged;
        void CreateWatcherForConfigSource(String configSource)
        {
            ConfigurationChangeFileWatcher watcher;
            if (MainConfigurationFileSource.Equals(configSource))
            {
                watcher = new ConfigurationChangeFileWatcher(mainConfigurationFileName,
                                                             configSource);
            }
            else
            {
                watcher = new ConfigurationChangeFileWatcher(Path.Combine(mainConfigurationFilePath, configSource),
                                                             configSource);
            }
            watcher.ConfigurationChanged += OnConfigurationChanged;
            configSourceWatcherMapping.Add(configSource, watcher);
            if (refresh)
            {
                watcher.StartWatching();
            }
            return;
        }
        public void Dispose()
        {
            foreach (IDisposable watcher in configSourceWatcherMapping.Values)
            {
                watcher.Dispose();
            }
        }
        public bool IsWatchingConfigSource(String configSource)
        {
            return configSourceWatcherMapping.ContainsKey(configSource);
        }
        public void OnConfigurationChanged(object sender,
                                           ConfigurationChangedEventArgs args)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, args);
            }
        }
        public void RemoveWatcherForConfigSource(String configSource)
        {
            ConfigurationChangeFileWatcher watcher;
            configSourceWatcherMapping.TryGetValue(configSource, out watcher);
            if (watcher != null)
            {
                configSourceWatcherMapping.Remove(configSource);
                watcher.Dispose();
            }
        }
        public void SetWatcherForConfigSource(String configSource)
        {
            if (!IsWatchingConfigSource(configSource))
            {
                CreateWatcherForConfigSource(configSource);
            }
        }
    }
}
