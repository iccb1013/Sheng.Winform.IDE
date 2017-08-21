/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage
{
    internal class ConfigurationChangeWatcherPauser : IDisposable
    {
        private readonly IConfigurationChangeWatcher watcher;
        public ConfigurationChangeWatcherPauser(IConfigurationChangeWatcher watcher)
        {
            this.watcher = watcher;
            if(watcher != null) watcher.StopWatching();
        }
        public void Dispose()
        {
            if(watcher != null) watcher.StartWatching();
        }
    }
}
