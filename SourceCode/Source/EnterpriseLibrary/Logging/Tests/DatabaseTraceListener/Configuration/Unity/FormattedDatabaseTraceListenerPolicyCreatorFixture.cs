/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Tests.Configuration.Unity
{
	[TestClass]
	public class FormattedDatabaseTraceListenerPolicyCreatorFixture
	{
		private IUnityContainer container;
		private LoggingSettings loggingSettings;
		private ConnectionStringsSection connectionStringsSection;
		private DictionaryConfigurationSource configurationSource;
		[TestInitialize]
		public void SetUp()
		{
			loggingSettings = new LoggingSettings();
			connectionStringsSection = new ConnectionStringsSection();
			configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, loggingSettings);
			configurationSource.Add("connectionStrings", connectionStringsSection);
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}
		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}
		[TestMethod]
		public void CanCreatePoliciesForProvider()
		{
			FormattedDatabaseTraceListenerData listenerData
				= new FormattedDatabaseTraceListenerData("listener", "write", "add", "database", "");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);
			connectionStringsSection.ConnectionStrings.Add(
				new ConnectionStringSettings("database", "foo=bar;", "System.Data.SqlClient"));
			container.AddExtension(new LoggingBlockExtension());
			container.AddExtension(new DataAccessBlockExtension());
			FormattedDatabaseTraceListener createdObject = (FormattedDatabaseTraceListener)container.Resolve<TraceListener>("listener");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNull(createdObject.Formatter);
		}
		[TestMethod]
		public void CanCreatePoliciesForProviderWithFormatter()
		{
			FormattedDatabaseTraceListenerData listenerData
				= new FormattedDatabaseTraceListenerData("listener", "write", "add", "database", "formatter");
			listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
			listenerData.Filter = SourceLevels.Error;
			loggingSettings.TraceListeners.Add(listenerData);
			FormatterData formatterData = new TextFormatterData("formatter", "template");
			loggingSettings.Formatters.Add(formatterData);
			connectionStringsSection.ConnectionStrings.Add(
				new ConnectionStringSettings("database", "foo=bar;", "System.Data.SqlClient"));
			container.AddExtension(new LoggingBlockExtension());
			container.AddExtension(new DataAccessBlockExtension());
			FormattedDatabaseTraceListener createdObject = (FormattedDatabaseTraceListener)container.Resolve<TraceListener>("listener");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
			Assert.IsNotNull(createdObject.Filter);
			Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
			Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
			Assert.IsNotNull(createdObject.Formatter);
			Assert.AreSame(typeof(TextFormatter), createdObject.Formatter.GetType());
			Assert.AreEqual("template", ((TextFormatter)createdObject.Formatter).Template);
		}
	}
}
