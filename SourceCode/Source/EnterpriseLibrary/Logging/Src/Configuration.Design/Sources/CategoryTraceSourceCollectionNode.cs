/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    [Image(typeof(CategoryTraceSourceCollectionNode))]
    public sealed class CategoryTraceSourceCollectionNode : ConfigurationNode
    {
        public CategoryTraceSourceCollectionNode()
            : base(Resources.CategorySources)
        {
        }
        [ReadOnly(false)]
        public override string Name
        {
            get{return base.Name;}            
        }
    }
}
