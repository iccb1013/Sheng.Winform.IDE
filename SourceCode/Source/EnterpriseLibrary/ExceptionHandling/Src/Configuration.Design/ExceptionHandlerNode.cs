/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    [Image(typeof(ExceptionHandlerNode))]
    [SelectedImage(typeof(ExceptionHandlerNode))]
    public abstract class ExceptionHandlerNode : ConfigurationNode
    {        
        protected ExceptionHandlerNode()
        {            
        }
        [Browsable(false)]
        public abstract ExceptionHandlerData ExceptionHandlerData { get; }
    }
}
