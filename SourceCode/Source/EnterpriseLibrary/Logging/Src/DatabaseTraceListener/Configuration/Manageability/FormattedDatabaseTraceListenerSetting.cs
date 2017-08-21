/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
	[ManagementEntity]
	public partial class FormattedDatabaseTraceListenerSetting : TraceListenerSetting
	{
		private string addCategoryStoredProcName;
		private string databaseInstanceName;
		private string formatter;
		private string writeLogStoredProcName;
		public FormattedDatabaseTraceListenerSetting(FormattedDatabaseTraceListenerData sourceElement,
			string name,
			string databaseInstanceName,
			string writeLogStoredProcName,
			string addCategoryStoredProcName,
			string formatter,
			string traceOutputOptions,
			string filter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{
			this.addCategoryStoredProcName = addCategoryStoredProcName;
			this.databaseInstanceName = databaseInstanceName;
			this.formatter = formatter;
			this.writeLogStoredProcName = writeLogStoredProcName;
		}
		[ManagementConfiguration]
		public string AddCategoryStoredProcName
		{
			get { return addCategoryStoredProcName; }
			set { addCategoryStoredProcName = value; }
		}
		[ManagementConfiguration]
		public string DatabaseInstanceName
		{
			get { return databaseInstanceName; }
			set { databaseInstanceName = value; }
		}
		[ManagementConfiguration]
		public string Formatter
		{
			get { return formatter; }
			set { formatter = value; }
		}
		[ManagementConfiguration]
		public string WriteLogStoredProcName
		{
			get { return writeLogStoredProcName; }
			set { writeLogStoredProcName = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<FormattedDatabaseTraceListenerSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<FormattedDatabaseTraceListenerSetting>();
		}
		[ManagementBind]
		public static FormattedDatabaseTraceListenerSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<FormattedDatabaseTraceListenerSetting>(ApplicationName, SectionName, Name);
		}
	    protected override bool SaveChanges(ConfigurationElement sourceElement)
	    {
	        return FormattedDatabaseTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
	    }
	}
}
