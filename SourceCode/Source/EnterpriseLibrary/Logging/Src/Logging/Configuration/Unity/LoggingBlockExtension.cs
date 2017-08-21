/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	public class LoggingBlockExtension : EnterpriseLibraryBlockExtension
	{
		private const string ErrorsTraceSourceKey = "___ERRORS";
		private const string AllTraceSourceKey = "___ALL";
		private const string NoMatchesTraceSourceKey = "___NO_MATCHES";
		protected override void Initialize()
		{
			LoggingSettings settings = (LoggingSettings)this.ConfigurationSource.GetSection(LoggingSettings.SectionName);
            InstrumentationConfigurationSection instrumentationSettings = (InstrumentationConfigurationSection)this.ConfigurationSource.GetSection(InstrumentationConfigurationSection.SectionName);
			if (settings == null)
			{
				return;
			}
			CreateProvidersPolicies<ILogFormatter, FormatterData>(
				Context.Policies,
				null,
				settings.Formatters,
				ConfigurationSource);
			CreateProvidersPolicies<ILogFilter, LogFilterData>(
				Context.Policies,
				null,
				settings.LogFilters,
				ConfigurationSource);
			CreateProvidersPolicies<TraceListener, TraceListenerData>(
				Context.Policies,
				null,
				settings.TraceListeners,
				ConfigurationSource);
			CreateTraceListenersAdditionalPolicies(
				Context.Policies,
				Context.Container,
				settings.TraceListeners);
			CreateTraceSourcesPolicies(
				Context.Policies,
				settings.TraceSources,
				ConfigurationSource);
			CreateTraceSourcePolicies(
				Context.Policies,
				AllTraceSourceKey,
				settings.SpecialTraceSources.AllEventsTraceSource,
				ConfigurationSource);
			CreateTraceSourcePolicies(
				Context.Policies,
				NoMatchesTraceSourceKey,
				settings.SpecialTraceSources.NotProcessedTraceSource,
				ConfigurationSource);
			CreateTraceSourcePolicies(
				Context.Policies,
				ErrorsTraceSourceKey,
				settings.SpecialTraceSources.ErrorsTraceSource,
				ConfigurationSource);
			CreateLogWriterPolicies(
				Context.Policies,
				Context.Container,
				settings,
				ConfigurationSource);
            CreateTraceManagerPolicies(
                Context.Policies,
                instrumentationSettings);
		}
        private static void CreateTraceManagerPolicies(IPolicyList policyList,
            InstrumentationConfigurationSection instrumentationSettings
            )
        {
            new PolicyBuilder<TraceManager, InstrumentationConfigurationSection>(
                null,
                instrumentationSettings,
                c => new TraceManager(
                        Resolve.Reference<LogWriter>(null),
                        new TracerInstrumentationListener(GetPerformanceCountersEnabled(c)))
                    ).AddPoliciesToPolicyList(policyList);
        }
		private static void CreateLogWriterPolicies(IPolicyList policyList,
			IUnityContainer container,
			LoggingSettings settings,
			IConfigurationSource ConfigurationSource)
		{
			new PolicyBuilder<LogWriter, LoggingSettings>(
					null,
					settings,
					c => new LogWriter(
						Resolve.ReferenceCollection<List<ILogFilter>, ILogFilter>(from f in settings.LogFilters select f.Name),
						Resolve.ReferenceCollection<List<LogSource>, LogSource>(from ts in settings.TraceSources select ts.Name),
						Resolve.OptionalReference<LogSource>(AllTraceSourceKey),
						Resolve.OptionalReference<LogSource>(NoMatchesTraceSourceKey),
						Resolve.Reference<LogSource>(ErrorsTraceSourceKey),
						settings.DefaultCategory,
						settings.TracingEnabled,
						settings.LogWarningWhenNoCategoriesMatch))
				.AddPoliciesToPolicyList(policyList);
			container.RegisterType(typeof(LogWriter), new ContainerControlledLifetimeManager());
		}
		private static void CreateTraceListenersAdditionalPolicies(
			IPolicyList policyList,
			IUnityContainer container,
			TraceListenerDataCollection traceListenerDataCollection)
		{
			foreach (TraceListenerData data in traceListenerDataCollection)
			{
				new PolicyBuilder<TraceListener, TraceListenerData>(new NamedTypeBuildKey(data.Type, data.Name), data)
					.SetProperty(o => o.Name).To(c => c.Name)
					.SetProperty(o => o.TraceOutputOptions).To(c => c.TraceOutputOptions)
					.SetProperty(o => o.Filter).To(c => c.Filter != SourceLevels.All ? new EventTypeFilter(c.Filter) : null)
					.AddPoliciesToPolicyList(policyList);
				container.RegisterType(data.Type, data.Name, new ContainerControlledLifetimeManager());
			}
		}
		private static void CreateTraceSourcesPolicies(
			IPolicyList policyList,
			IEnumerable<TraceSourceData> traceSources,
			IConfigurationSource configurationSource)
		{
			foreach (TraceSourceData traceSourceData in traceSources)
			{
				CreateTraceSourcePolicies(policyList, traceSourceData.Name, traceSourceData, configurationSource);
			}
		}
		private static void CreateTraceSourcePolicies(
			IPolicyList policyList,
			string traceSourceName,
			TraceSourceData traceSourceData,
			IConfigurationSource configurationSource)
		{
			new PolicyBuilder<LogSource, TraceSourceData>(traceSourceName,
					traceSourceData,
					c => new LogSource(
						traceSourceData.Name,
						Resolve.ReferenceCollection<List<TraceListener>, TraceListener>(from r in traceSourceData.TraceListeners select r.Name),
						traceSourceData.DefaultLevel,
						traceSourceData.AutoFlush))
				.AddPoliciesToPolicyList(policyList);
		}
        private static bool GetPerformanceCountersEnabled(InstrumentationConfigurationSection instrumentationSettings)
        {
            bool performanceCountersEnabled = false;
            if (instrumentationSettings == null)
            {
                performanceCountersEnabled = false;
            }
            else
            {
                performanceCountersEnabled = instrumentationSettings.PerformanceCountersEnabled;
            }
            return performanceCountersEnabled;
        }
		protected override IContainerPolicyCreator GetDefaultContainerPolicyCreator(Type targetType)
		{
			return typeof(TraceListener).IsAssignableFrom(targetType)
				? new TraceListenerConstructorArgumentMatchingPolicyCreator(targetType)
				: base.GetDefaultContainerPolicyCreator(targetType);
		}
	}
}
