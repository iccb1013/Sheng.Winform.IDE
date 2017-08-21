/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class DictionarySourceFixture
    {
        [TestMethod]
        public void CanRetrieveSectionFromSource()
        {
            DictionaryConfigurationSource source = LocalConfigurationSource.Create();
            Assert.IsTrue(source.Contains("test"));
            Assert.AreEqual(source.GetSection("test").GetType(), typeof(LocalConfigurationSection));
            Assert.IsTrue(source.Remove("test"));
            Assert.IsNull(source.GetSection("random"));
        }
        class LocalConfigurationSection : SerializableConfigurationSection {}
        static class LocalConfigurationSource
        {
            public static DictionaryConfigurationSource Create()
            {
                DictionaryConfigurationSource source = new DictionaryConfigurationSource();
                source.Add("test", new LocalConfigurationSection());
                return source;
            }
        }
    }
}
