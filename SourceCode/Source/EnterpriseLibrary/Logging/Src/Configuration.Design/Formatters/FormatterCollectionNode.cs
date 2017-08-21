/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    [Image(typeof (FormatterCollectionNode))]
    public sealed class FormatterCollectionNode : ConfigurationNode
    {
        public FormatterCollectionNode()
            : base(Resources.Formatters)
        {            
        }
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }        
    }
}
