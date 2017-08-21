/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class ConfigurationNameMappingStrategyFixture
	{
		private IBuilder builder;
		private StagedStrategyChain<BuilderStage> strategies;
		[TestInitialize]
		public void SetUp()
		{
			builder = new Builder();
			strategies = new StagedStrategyChain<BuilderStage>();
			strategies.AddNew<ConfigurationNameMappingStrategy>(BuilderStage.PreCreation);
			MockNameMapper.invoked = false;
		}
		[TestMethod]
		public void ChainForNullIdOnTypeWithoutNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithoutNameMappingAttribute>(),
			                null);
			Assert.IsFalse(MockNameMapper.invoked);
		}
		[TestMethod]
		public void ChainForNullIdOnTypeWithNameMappingDoesInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithNameMappingAttribute>(),
			                null);
			Assert.IsTrue(MockNameMapper.invoked);
		}
		[TestMethod]
		public void ChainForNonNullIdOnTypeWithoutNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithoutNameMappingAttribute>("id"),
			                null);
			Assert.IsFalse(MockNameMapper.invoked);
		}
		[TestMethod]
		public void ChainForNonNullIdOnTypeWithNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp(null,
			                null,
			                new PolicyList(),
			                strategies.MakeStrategyChain(),
			                NamedTypeBuildKey.Make<TypeWithNameMappingAttribute>("id"),
			                null);
			Assert.IsFalse(MockNameMapper.invoked);
		}
	}
    public class TypeWithoutNameMappingAttribute {}
    [ConfigurationNameMapper(typeof(MockNameMapper))]
    public class TypeWithNameMappingAttribute {}
    public class MockNameMapper : IConfigurationNameMapper
    {
        internal static bool invoked;
        public string MapName(string name,
                              IConfigurationSource configSource)
        {
            invoked = true;
            return name;
        }
    }
}
