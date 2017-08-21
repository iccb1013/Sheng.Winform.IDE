/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestClass]
	public class LogWriterCustomFactoryFixture
	{
		private object createdObject1 = null;
		private object createdObject2 = null;
		[TestCleanup]
		public void TearDown()
		{
			if (createdObject1 != null && createdObject1 is IDisposable)
				(createdObject1 as IDisposable).Dispose();
			if (createdObject2 != null && createdObject2 is IDisposable)
				(createdObject2 as IDisposable).Dispose();
		}
		[TestMethod]
		public void CanBuildLogWriterFromConfiguration()
		{
			BuilderContext context = CreateContext(null, null, new SystemConfigurationSource());
			createdObject1 = context.Strategies.ExecuteBuildUp(context);
			Assert.IsNotNull(createdObject1);
			Assert.AreSame(typeof(LogWriter), createdObject1.GetType());
		}
		[TestMethod]
		[ExpectedException(typeof(BuildFailedException))]
		public void RequestForLogWriterWithoutSettingsThrows()
		{
			BuilderContext context = CreateContext(null, null, new DictionaryConfigurationSource());
			context.Strategies.ExecuteBuildUp(context);
		}
		[TestMethod]
		public void RequestForLogWriterWithLocatorAddsSingleton()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			BuilderContext context = CreateContext(locator, container, new SystemConfigurationSource());
			createdObject1
				= context.Strategies.ExecuteBuildUp(context);
			createdObject2
				= context.Strategies.ExecuteBuildUp(context);
			Assert.IsNotNull(createdObject1);
			Assert.AreSame(typeof(LogWriter), createdObject1.GetType());
			Assert.IsTrue(object.ReferenceEquals(createdObject1, createdObject2));
		}
		[TestMethod]
		public void RequestForLogWriterWithoutLocatorDoesNotAddSingleton()
		{
			BuilderContext context = CreateContext(null, null, new SystemConfigurationSource());
			createdObject1
				= context.Strategies.ExecuteBuildUp(context);
			createdObject2
				= context.Strategies.ExecuteBuildUp(context);
			Assert.IsNotNull(createdObject1);
			Assert.AreSame(typeof(LogWriter), createdObject1.GetType());
			Assert.IsFalse(object.ReferenceEquals(createdObject1, createdObject2));
		}
		[TestMethod]
		public void SameLogWriterFactoryReturnsDifferentInstances()
		{
			LogWriterFactory factory = new LogWriterFactory();
			createdObject1 = factory.Create();
			createdObject2 = factory.Create();
			Assert.IsNotNull(createdObject1);
			Assert.IsFalse(object.ReferenceEquals(createdObject1, createdObject2));
		}
		[TestMethod]
		public void DifferentLogWriterFactoryReturnsDifferentInstance()
		{
			LogWriterFactory factory = new LogWriterFactory();
			createdObject1 = factory.Create();
			factory = new LogWriterFactory();
			createdObject2 = factory.Create();
			Assert.IsNotNull(createdObject1);
			Assert.IsFalse(object.ReferenceEquals(createdObject1, createdObject2));
		}
		private BuilderContext CreateContext(IReadWriteLocator locator, ILifetimeContainer container, IConfigurationSource configurationSource)
		{
			BuilderContext context
				= new BuilderContext(
					new StrategyChain(
						new object[] { 
                            new LocatorLookupStrategy(),
							new MockFactoryStrategy(new LogWriterCustomFactory(), configurationSource, new ConfigurationReflectionCache())}),
					locator,
					container,
					new PolicyList(),
					NamedTypeBuildKey.Make<LogWriter>(),
					null);
			return context;
		}
	}
}
