/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    public sealed class AddLoggingDatabaseCommand : AddChildNodeCommand
    {
        public AddLoggingDatabaseCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(LoggingDatabaseNode)) {}
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            LoggingDatabaseNode node = ChildNode as LoggingDatabaseNode;
            if (null == node) return;
            if (null == CurrentHierarchy.FindNodeByType(typeof(LoggingSettingsNode)))
            {
                new AddLoggingSettingsNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
            }
            if (null == CurrentHierarchy.FindNodeByType(typeof(DatabaseSectionNode)))
            {
                new AddDatabaseSectionNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
            }
        }
    }
}
