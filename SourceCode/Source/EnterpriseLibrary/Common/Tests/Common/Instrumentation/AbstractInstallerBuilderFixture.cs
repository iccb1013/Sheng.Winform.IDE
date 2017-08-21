/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
    [TestClass]
    public class AbstractInstallerBuilderFixture
    {
        [TestMethod]
        public void EmptyCandidatesTypesCollectionWillSetEmptyEmtpyInstrumentedTypes()
        {
            Type[] candidateTypes = new Type[0];
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(0, builder.InstrumentedTypes.Count);
        }
        [TestMethod]
        public void CandidatesTypesCollectionWithNullReferenceWillSetEmptyEmtpyInstrumentedTypes()
        {
            Type[] candidateTypes = new Type[] { null };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(0, builder.InstrumentedTypes.Count);
        }
        [TestMethod]
        public void NotInstrumentedCandidateTypesCollectionWithoutCorrectAttributeTypesWillSetEmtpyInstrumentedTypes()
        {
            Type[] candidateTypes = new Type[]
                {
                    typeof(TypeWithoutAttributes),
                    typeof(TypeWithBAttribute)
                };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(0, builder.InstrumentedTypes.Count);
        }
        [TestMethod]
        public void NotInstrumentedCandidateTypesCollectionWithCorrectAttributeTypesWillSetEmtpyInstrumentedTypes()
        {
            Type[] candidateTypes = new Type[]
                {
                    typeof(TypeWithAAttribute),
                    typeof(TypeWithAAndBAttributes)
                };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(0, builder.InstrumentedTypes.Count);
        }
        [TestMethod]
        public void InstrumentedCandidateTypesCollectionWithoutCorrectAttributeTypesWillSetEmtpyInstrumentedTypes()
        {
            Type[] candidateTypes = new Type[]
                {
                    typeof(InstrumentedTypeWithoutAttributes),
                    typeof(InstrumentedTypeWithBAttribute)
                };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(0, builder.InstrumentedTypes.Count);
        }
        [TestMethod]
        public void InstrumentedCandidateTypesCollectionWithCorrectAttributeTypesWillSetNonEmtpyInstrumentedTypes()
        {
            Type[] candidateTypes = new Type[]
                {
                    typeof(InstrumentedTypeWithAAttribute),
                    typeof(InstrumentedTypeWithAAndBAttributes)
                };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(2, builder.InstrumentedTypes.Count);
            Assert.IsTrue(builder.InstrumentedTypes.Contains(typeof(InstrumentedTypeWithAAttribute)));
            Assert.IsTrue(builder.InstrumentedTypes.Contains(typeof(InstrumentedTypeWithAAndBAttributes)));
        }
        [TestMethod]
        public void BuilderWillOnlyKeepTypesBothInstrumentedAndWithTheCorrectAttribute()
        {
            Type[] candidateTypes = new Type[]
                {
                    typeof(TypeWithoutAttributes),
                    typeof(TypeWithBAttribute),
                    typeof(TypeWithAAttribute),
                    typeof(TypeWithAAndBAttributes),
                    typeof(InstrumentedTypeWithoutAttributes),
                    typeof(InstrumentedTypeWithBAttribute),
                    typeof(InstrumentedTypeWithAAttribute),
                    null,
                    typeof(InstrumentedTypeWithAAndBAttributes)
                };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Assert.AreEqual(2, builder.InstrumentedTypes.Count);
            Assert.IsTrue(builder.InstrumentedTypes.Contains(typeof(InstrumentedTypeWithAAttribute)));
            Assert.IsTrue(builder.InstrumentedTypes.Contains(typeof(InstrumentedTypeWithAAndBAttributes)));
        }
        [TestMethod]
        public void BuilderWillFillInstallerWithCreatedChildInstallers()
        {
            Type[] candidateTypes = new Type[]
                {
                    typeof(InstrumentedTypeWithAAttribute),
                    typeof(InstrumentedTypeWithAAndBAttributes)
                };
            MockInstallerBuilder builder = new MockInstallerBuilder(candidateTypes, typeof(InstrumentationAAttribute));
            Installer parentInstaller = new Installer();
        }
    }
    public class MockInstallerBuilder : AbstractInstallerBuilder
    {
        public MockInstallerBuilder(Type[] availableTypes,
                                    Type instrumentationAttributeType)
            : base(availableTypes, instrumentationAttributeType) {}
        public new IList<Type> InstrumentedTypes
        {
            get { return base.InstrumentedTypes; }
        }
        protected override ICollection<Installer> CreateInstallers(ICollection<Type> instrumentedTypes)
        {
            List<Installer> result = new List<Installer>(instrumentedTypes.Count);
            foreach (Type type in instrumentedTypes)
            {
                result.Add(new MockInstaller(type));
            }
            return result;
        }
    }
    public class MockInstaller : Installer
    {
        internal Type type;
        internal MockInstaller(Type type)
        {
            this.type = type;
        }
    }
    class InstrumentationAAttribute : Attribute {}
    class InstrumentationBAttribute : Attribute {}
    class TypeWithoutAttributes {}
    [InstrumentationA]
    class TypeWithAAttribute {}
    [InstrumentationB]
    class TypeWithBAttribute {}
    [InstrumentationA]
    [InstrumentationB]
    class TypeWithAAndBAttributes {}
    [HasInstallableResourcesAttribute]
    class InstrumentedTypeWithoutAttributes {}
    [HasInstallableResourcesAttribute]
    [InstrumentationA]
    class InstrumentedTypeWithAAttribute {}
    [HasInstallableResourcesAttribute]
    [InstrumentationB]
    class InstrumentedTypeWithBAttribute {}
    [HasInstallableResourcesAttribute]
    [InstrumentationA]
    [InstrumentationB]
    class InstrumentedTypeWithAAndBAttributes {}
}
