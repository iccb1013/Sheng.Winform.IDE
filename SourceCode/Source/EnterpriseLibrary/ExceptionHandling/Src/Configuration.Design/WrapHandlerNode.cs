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
    public sealed class WrapHandlerNode : ExceptionHandlerNode
    {
        private string message;
        private string messageResourceName;
        private string messageResourceType;
        private string typeName;
        public WrapHandlerNode()
            : this(new WrapHandlerData(Resources.DefaultWrapHandlerNodeName, string.Empty, string.Empty))
        {
        }
        public WrapHandlerNode(WrapHandlerData wrapHandlerData)
        {
            if (null == wrapHandlerData) throw new ArgumentNullException("wrapHandlerData");
            Rename(wrapHandlerData.Name);
            this.message = wrapHandlerData.ExceptionMessage;
            this.typeName = wrapHandlerData.WrapExceptionTypeName;
            this.messageResourceType = wrapHandlerData.ExceptionMessageResourceType;
            this.messageResourceName = wrapHandlerData.ExceptionMessageResourceName;
        }
        [SRDescription("WrapHandlerNodeMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return message; ; }
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
        [SRDescription("ExceptionWrapTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string WrapExceptionType
        {
            get { return typeName; }
            set { typeName = value; }
        }
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                WrapHandlerData handlerData = new WrapHandlerData(Name, message, typeName);
                handlerData.ExceptionMessageResourceName = messageResourceName;
                handlerData.ExceptionMessageResourceType = messageResourceType;
                return handlerData;
            }
        }
    }
}
