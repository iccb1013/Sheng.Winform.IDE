/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public class SpecialTraceSourcesData : ConfigurationElement
	{
		private const string mandatoryTraceSourceProperty = "allEvents";
		private const string notProcessedTraceSourceProperty = "notProcessed";
		private const string errorsTraceSourceProperty = "errors";
		public SpecialTraceSourcesData()
		{
		}
		public SpecialTraceSourcesData(TraceSourceData mandatory, TraceSourceData notProcessed, TraceSourceData errors)
		{
			this.AllEventsTraceSource = mandatory;
			this.NotProcessedTraceSource = notProcessed;
			this.ErrorsTraceSource = errors;
		}
		[ConfigurationProperty(mandatoryTraceSourceProperty, IsRequired=false)]
		public TraceSourceData AllEventsTraceSource
		{
			get { return (TraceSourceData)base[mandatoryTraceSourceProperty]; }
			set { base[mandatoryTraceSourceProperty] = value; }
		}
		[ConfigurationProperty(notProcessedTraceSourceProperty, IsRequired = false)]
		public TraceSourceData NotProcessedTraceSource
		{
			get { return (TraceSourceData)base[notProcessedTraceSourceProperty]; }
			set { base[notProcessedTraceSourceProperty] = value; }
		}
		[ConfigurationProperty(errorsTraceSourceProperty, IsRequired = true)]
		public TraceSourceData ErrorsTraceSource
		{
			get { return (TraceSourceData)base[errorsTraceSourceProperty]; }
			set { base[errorsTraceSourceProperty] = value; }
		}
	}
}
