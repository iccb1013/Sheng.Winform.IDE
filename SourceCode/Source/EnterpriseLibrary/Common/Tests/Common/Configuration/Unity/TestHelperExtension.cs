/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.Unity;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	public class TestHelperExtension : UnityContainerExtension
	{
		public TestHelperExtension()
			: this(context => { })
		{ }
		public TestHelperExtension(InitializeDelegate initialize)
		{
			this.initialize = initialize;
		}
		public delegate void InitializeDelegate(ExtensionContext context);
		internal InitializeDelegate initialize;
		protected override void Initialize()
		{
			this.initialize(this.Context);
		}
	}
}
