/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    public sealed class CustomLogFilterNode : LogFilterNode
    {
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        private string customLogFilterTypeName;
        public CustomLogFilterNode()
            :this(new CustomLogFilterData(Resources.CustomFilterNode, string.Empty))
        {
        }
        public CustomLogFilterNode(CustomLogFilterData customLogFilterData)
            : base(customLogFilterData == null ? string.Empty : customLogFilterData.Name)
        {
			if (null == customLogFilterData) throw new ArgumentNullException("customLogFilterData");
            customLogFilterTypeName = customLogFilterData.TypeName;
            foreach (string key in customLogFilterData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customLogFilterData.Attributes[key]));
            }
        }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomFilterExtensionsDescription", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get
            {
                return editableAttributes;
            }
        }
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(LogFilter), typeof(CustomLogFilterData))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomFilterTypeDescription", typeof(Resources))]
        public string Type
        {
            get { return customLogFilterTypeName; }
            set { customLogFilterTypeName = value; }
        }
        public override LogFilterData LogFilterData
        {
            get
            {
                CustomLogFilterData customLogFilterData = new CustomLogFilterData(Name, customLogFilterTypeName);                                
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customLogFilterData.Attributes.Add(kv.Key, kv.Value);
                }
				return customLogFilterData;
            }
        }
    }
}
