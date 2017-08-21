/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
	[TestClass()]
	public class DomainObjectTest
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
		[UrlToTest("http://localhost/Web")]
		public void ConstructorTest()
		{
			object target = Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor.CreatePrivate();
			Assert.Inconclusive("TODO: Implement code to verify target");
		}
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
		[UrlToTest("http://localhost/Web")]
		public void NameTest()
		{
			object target = Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor.CreatePrivate();
			string val = null; 
			Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor accessor = new Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor(target);
			Assert.AreEqual(val, accessor.Name, "DomainModel.DomainObject.Name was not set correctly.");
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
		[UrlToTest("http://localhost/Web")]
		public void ConstructorTest1()
		{
			object target = Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor.CreatePrivate();
			Assert.Inconclusive("TODO: Implement code to verify target");
		}
		[TestMethod()]
		[HostType("ASP.NET")]
		[AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
		[UrlToTest("http://localhost/Web")]
		public void NameTest1()
		{
			object target = Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor.CreatePrivate();
			string val = null; 
			Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor accessor = new Microsoft.Practices.EnterpriseLibrary.Common.Tests.DomainModel_DomainObjectAccessor(target);
			Assert.AreEqual(val, accessor.Name, "DomainModel.DomainObject.Name was not set correctly.");
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}
