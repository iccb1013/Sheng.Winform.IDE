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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    [Image(typeof(ExceptionPolicyNode))]
    [SelectedImage(typeof(ExceptionPolicyNode))]
    public sealed class ExceptionPolicyNode : ConfigurationNode
    {
        public ExceptionPolicyNode() : this(new ExceptionPolicyData(Resources.DefaultExceptionPolicyNodeName))
        {
        }
        public ExceptionPolicyNode(ExceptionPolicyData exceptionPolicyData)
            : base(exceptionPolicyData == null ? Resources.DefaultExceptionPolicyNodeName : exceptionPolicyData.Name)
        {            
        }
    }
}
