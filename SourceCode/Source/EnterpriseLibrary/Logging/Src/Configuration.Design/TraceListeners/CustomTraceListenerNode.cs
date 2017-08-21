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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    [Image(typeof(TraceListenerNode))]
    [SelectedImage(typeof(TraceListenerNode))]
    public sealed class CustomTraceListenerNode : TraceListenerNode
    {        
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        private FormatterNode formatterNode;
		private string formatterName;
		private string typeName;
        public CustomTraceListenerNode()
            :this(new CustomTraceListenerData(Resources.CustomTraceListenerNode, null, string.Empty))
        {
        }
       public CustomTraceListenerNode(CustomTraceListenerData customTraceListenerData)
        {
			if (null == customTraceListenerData) throw new ArgumentNullException("customTraceListenerData");
			Rename(customTraceListenerData.Name);
			TraceOutputOptions = customTraceListenerData.TraceOutputOptions;
			this.formatterName = customTraceListenerData.Formatter;
            this.typeName = customTraceListenerData.TypeName;
            foreach (string key in customTraceListenerData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customTraceListenerData.Attributes[key]));
            }
        }       
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);
                formatterName = formatterNode == null ? String.Empty : formatterNode.Name;
            }
        }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomCacheStorageExtensionsDescription", typeof(Resources))]
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
        [BaseType(typeof(CustomTraceListener), typeof(CustomTraceListenerData))]
        [SRDescription("CustomTraceListenerNodeTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }
        [Browsable(false)]
        public override TraceListenerData TraceListenerData
        {
            get
            {
				CustomTraceListenerData data = new CustomTraceListenerData(Name, typeName, string.Empty, TraceOutputOptions);
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    data.Attributes.Add(kv.Key, kv.Value);
                }
				data.Formatter = formatterName;
                return data;
            }
        }
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
		}		
        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterNode = null;
        }
        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }
    }
}
