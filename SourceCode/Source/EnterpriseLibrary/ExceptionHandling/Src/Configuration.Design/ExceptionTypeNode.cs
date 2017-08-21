/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    [Image(typeof(ExceptionTypeNode))]
    [SelectedImage(typeof(ExceptionTypeNode))]
    public sealed class ExceptionTypeNode : ConfigurationNode
    {
        private static ExceptionTypeNodeNameFormatter nodeNameFormatter = new ExceptionTypeNodeNameFormatter();
		private string typeName;
		private PostHandlingAction postHandlingAction;
        public ExceptionTypeNode() : this(new ExceptionTypeData(Resources.DefaultExceptionTypeNodeName, typeof(Exception), PostHandlingAction.NotifyRethrow))
        {
        }
        public ExceptionTypeNode(ExceptionTypeData exceptionTypeData)
            : base(nodeNameFormatter.CreateName(exceptionTypeData))
        {
            if (exceptionTypeData == null)
            {
                throw new ArgumentNullException("exceptionTypeData");
            }
            this.typeName = exceptionTypeData.TypeName;
			this.postHandlingAction = exceptionTypeData.PostHandlingAction;
        }
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }
        [Required]
        [SRDescription("ExceptionTypeNodeNameDescription", typeof(Resources))]
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set
            {
				if (null == value) throw new ArgumentNullException("value");
                typeName = value;
                Name = nodeNameFormatter.CreateName(typeName);
            }
        }
        [SRDescription("ExceptionTypePostHandlingActionDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public PostHandlingAction PostHandlingAction
        {
            get { return postHandlingAction; }
            set { postHandlingAction = value; }
        }        
        public override bool SortChildren
        {
            get { return false; }
        }		
    }
}
