/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    public sealed class ConfigurationDesignManager : Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationDesignManager
    {
        public override void Register(IServiceProvider serviceProvider)
        {
            CommandRegistrar cmdRegistrar = new CommandRegistrar(serviceProvider);
            cmdRegistrar.Register();
            NodeMapRegistrar nodeRegistrar = new NodeMapRegistrar(serviceProvider);
            nodeRegistrar.Register();
        }
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, System.Configuration.ConfigurationSection section)
        {
            base.OpenCore(serviceProvider, rootNode, section);
        }
    }
}
