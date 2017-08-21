/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public abstract class BasicCustomTraceListenerData
		: TraceListenerData, IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>
	{
		internal const string initDataProperty = "initializeData";
		private readonly CustomProviderDataHelper<BasicCustomTraceListenerData> helper;
		public BasicCustomTraceListenerData()
		{
			helper = CreateHelper();
		}
		public BasicCustomTraceListenerData(string name, Type type, string initData)
			: this(name, type, initData, TraceOptions.None)
		{
		}
        public BasicCustomTraceListenerData(string name, string typeName, string initData)
            : this(name, typeName, initData, TraceOptions.None)
        {
        }
		public BasicCustomTraceListenerData(string name, Type type, string initData, TraceOptions traceOutputOptions)
            :this(name, new AssemblyQualifiedTypeNameConverter().ConvertToString(type), initData, traceOutputOptions)
		{
		}
        public BasicCustomTraceListenerData(string name, string typeName, string initData, TraceOptions traceOutputOptions)
        {
            helper = CreateHelper();
            ListenerDataType = GetType();
            Name = name;
            TypeName = typeName;
            TraceOutputOptions = traceOutputOptions;
            InitData = initData;
        }
		public void SetAttributeValue(string key, string value)
		{
			helper.HandleSetAttributeValue(key, value);
		}
		public NameValueCollection Attributes
		{
			get { return helper.Attributes; }
		}
		public string InitData
		{
			get { return (string)base[initDataProperty]; }
			set { base[initDataProperty] = value; }
		}
		protected virtual CustomProviderDataHelper<BasicCustomTraceListenerData> CreateHelper()
		{
			return new BasicCustomTraceListenerDataHelper(this);
		}
		protected override ConfigurationPropertyCollection Properties
		{
			get { return helper.Properties; }
		}
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			helper.HandleUnmerge(sourceElement, parentElement, saveMode);
		}
		protected override void Reset(ConfigurationElement parentElement)
		{
			helper.HandleReset(parentElement);
		}
		protected override bool IsModified()
		{
			return helper.HandleIsModified();
		}
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			return helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
		}
		CustomProviderDataHelper<BasicCustomTraceListenerData> IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>.Helper
		{
			get { return helper; }
		}
		object IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>.BaseGetPropertyValue(ConfigurationProperty property)
		{
			return base[property];
		}
		void IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
		{
			base[property] = value;
		}
		void IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>.BaseUnmerge(ConfigurationElement sourceElement,
					ConfigurationElement parentElement,
					ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}
		void IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>.BaseReset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}
		bool IHelperAssistedCustomConfigurationData<BasicCustomTraceListenerData>.BaseIsModified()
		{
			return base.IsModified();
		}
	}
}
