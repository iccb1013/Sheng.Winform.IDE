/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    public sealed class ReplaceHandlerNode : ExceptionHandlerNode
    {
        private string message;
        private string messageResourceName;
        private string messageResourceType;
        private string typeName;
        public ReplaceHandlerNode()
            : this(new ReplaceHandlerData(Resources.DefaultReplaceHandlerNodeName, string.Empty, string.Empty))
        {
        }
        public ReplaceHandlerNode(ReplaceHandlerData replaceHandlerData)
        {
            if (null == replaceHandlerData) throw new ArgumentNullException("replaceHandlerData");
            Rename(replaceHandlerData.Name);
            this.message = replaceHandlerData.ExceptionMessage;
            this.typeName = replaceHandlerData.ReplaceExceptionTypeName;
            this.messageResourceType = replaceHandlerData.ExceptionMessageResourceType;
            this.messageResourceName = replaceHandlerData.ExceptionMessageResourceName;
        }
        [SRDescription("ExceptionReplaceMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return message; }
            set { message = value; }
        }
        [SRDescription("ExceptionMessageResourceNameDescription", typeof(Resources))]
        [SRCategory("CategoryLocalization", typeof(Resources))]
        public string ExceptionMessageResourceName
        {
            get { return messageResourceName; }
            set { messageResourceName = value; }
        }
        [SRDescription("ExceptionMessageTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryLocalization", typeof(Resources))]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Object), TypeSelectorIncludes.None)]
        public string ExceptionMessageResourceType
        {
            get { return messageResourceType; }
            set { messageResourceType = value; }
        }
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [SRDescription("ExceptionReplaceTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ReplaceExceptionType
        {
            get { return typeName; }
            set { typeName = value; }
        }
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                ReplaceHandlerData handlerData = new ReplaceHandlerData(Name, message, typeName);
                handlerData.ExceptionMessageResourceType = messageResourceType;
                handlerData.ExceptionMessageResourceName = messageResourceName;
                return handlerData;
            }
        }
    }
}
