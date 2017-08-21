/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public class TraceListenerData : NameTypeConfigurationElement
	{
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
		protected internal const string listenerDataTypeProperty = "listenerDataType";
		protected internal const string traceOutputOptionsProperty = "traceOutputOptions";
        protected internal const string filterProperty = "filter";
		private static IDictionary<string, string> emptyAttributes = new Dictionary<string, string>(0);
		public TraceListenerData()
		{
		}
		protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions)
			: base(name, traceListenerType)
		{
			this.ListenerDataType = this.GetType();
			this.TraceOutputOptions = traceOutputOptions;
		}
        protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
            this.TraceOutputOptions = traceOutputOptions;
            this.Filter = filter;
        }
        public Type ListenerDataType
        {
            get { return (Type)typeConverter.ConvertFrom(ListenerDataTypeName); }
            set { ListenerDataTypeName = typeConverter.ConvertToString(value); }
        }
        [ConfigurationProperty(listenerDataTypeProperty, IsRequired = true)]
        public string ListenerDataTypeName
        {
            get { return (string)this[listenerDataTypeProperty]; }
            set { this[listenerDataTypeProperty] = value; }
        }
		[ConfigurationProperty(traceOutputOptionsProperty, IsRequired= false)]
		public TraceOptions TraceOutputOptions
		{
			get
			{
				return (TraceOptions)this[traceOutputOptionsProperty];
			}
			set
			{
				this[traceOutputOptionsProperty] = value;
			}
		}
        [ConfigurationProperty(filterProperty, IsRequired = false, DefaultValue=SourceLevels.All)]
        public SourceLevels Filter
        {
            get { return (SourceLevels)this[filterProperty]; }
            set { this[filterProperty] = value; }
        }
	}
}
