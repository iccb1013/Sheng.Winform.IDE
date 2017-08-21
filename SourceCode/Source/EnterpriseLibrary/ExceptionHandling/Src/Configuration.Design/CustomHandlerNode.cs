/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel;
using System.Drawing.Design;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	public sealed class CustomHandlerNode : ExceptionHandlerNode
	{
		private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        private string typeName;
		public CustomHandlerNode()
			: this(new CustomHandlerData(Resources.DefaultCustomHandlerNodeName, string.Empty))
		{
		}
		public CustomHandlerNode(CustomHandlerData customHandlerData)
		{
			if (null == customHandlerData) throw new ArgumentNullException("customHandlerData");
			Rename(customHandlerData.Name);
			foreach (string key in customHandlerData.Attributes)
			{
				editableAttributes.Add(new EditableKeyValue(key, customHandlerData.Attributes[key]));
            }
            this.typeName = customHandlerData.TypeName;
		}
		[SRDescription("ExceptionHandlerAdditionalPropertiesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[CustomAttributesValidation]
		public List<EditableKeyValue> Attributes
		{
			get { return editableAttributes; }
		}
		[Required]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(IExceptionHandler), typeof(CustomHandlerData))]
		[SRDescription("ExceptionHandlerTypeDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public String Type
		{
            get { return typeName; }
            set { typeName = value; }
		}
		public override ExceptionHandlerData ExceptionHandlerData
		{
			get
			{
				CustomHandlerData customHandlerData = new CustomHandlerData(Name, typeName);
				customHandlerData.Attributes.Clear();
				foreach (EditableKeyValue kv in editableAttributes)
				{
					customHandlerData.Attributes.Add(kv.Key, kv.Value);
				}
				return customHandlerData;
			}
		}
	}
}
