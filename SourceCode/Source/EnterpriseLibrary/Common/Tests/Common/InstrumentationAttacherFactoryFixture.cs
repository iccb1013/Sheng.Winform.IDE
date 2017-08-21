/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
	[TestClass]
	public class InstrumentationAttacherFactoryFixture
	{
		[TestMethod]
		public void NoBinderInstanceReturnedIfNoAttributeOnSourceClass()
		{
			NoListenerSource source = new NoListenerSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);
			Assert.AreSame(typeof(NoBindingInstrumentationAttacher), createdAttacher.GetType());
		}
		[TestMethod]
		public void ReflectionBinderInstanceReturnedIfSingleArgumentAttributeOnSourceClass()
		{
			ReflectionBindingSource source = new ReflectionBindingSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);
			Assert.AreSame(typeof(ReflectionInstrumentationAttacher), createdAttacher.GetType());
		}
		[TestMethod]
		public void ExplicitBinderInstanceReturnedIfTwoArgumentAttributeOnSourceClass()
		{
			ExplicitBindingSource source = new ExplicitBindingSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);
			Assert.AreSame(typeof(ExplicitInstrumentationAttacher), createdAttacher.GetType());
		}
		public class NoListenerSource { }
		[InstrumentationListener(typeof(FooListener))]
		public class ReflectionBindingSource : IInstrumentationEventProvider
		{
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}
		[InstrumentationListener(typeof(FooListener), typeof(FooBinder))]
		public class ExplicitBindingSource : IInstrumentationEventProvider
		{
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}
		public class FooListener { }
		public class FooBinder { }
	}
}
