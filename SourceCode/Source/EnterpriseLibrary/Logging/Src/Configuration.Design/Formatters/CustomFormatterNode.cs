/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    public class CustomFormatterNode : FormatterNode
    {
        private string customFormatterTypeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        public CustomFormatterNode()
            : this(new CustomFormatterData(Resources.CustomFormatter, string.Empty))
        {
        }
        public CustomFormatterNode(CustomFormatterData customFormatterData)            
        {
			if (null == customFormatterData) throw new ArgumentNullException("customFormatterData");
            customFormatterTypeName = customFormatterData.TypeName;
            foreach (string key in customFormatterData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customFormatterData.Attributes[key]));
            }
			Rename(customFormatterData.Name);
        }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("customFormatterAttributesDescription", typeof(Resources))]
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
        [BaseType(typeof(LogFormatter), typeof(CustomFormatterData))]
        [SRDescription("customFormatterDataTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return customFormatterTypeName; }
            set { customFormatterTypeName = value; }
        }
        [Browsable(false)]
		public override FormatterData FormatterData
        {
            get
            {
                CustomFormatterData customFormatterData = new CustomFormatterData(Name, customFormatterTypeName);                
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customFormatterData.Attributes.Add(kv.Key, kv.Value);
                }
				return customFormatterData;
            }
        }
    }
}
