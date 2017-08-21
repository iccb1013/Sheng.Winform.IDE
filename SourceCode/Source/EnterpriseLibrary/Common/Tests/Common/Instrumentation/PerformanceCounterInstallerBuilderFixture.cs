//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration.Install;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class PerformanceCounterInstallerBuilderFixture
    {
        [TestMethod]
        public void TypeWithNoCountersReturnsEmptyInstallers()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(new Type[] { typeof(NoCountersType) });
            builder.Fill(parentInstaller);

            Assert.AreEqual(0, parentInstaller.Installers.Count);
        }

        [TestMethod]
        public void SingleCounterIsAddedToInstaller()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(new Type[] { typeof(SingleCounterType) });
            builder.Fill(parentInstaller);

            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());

            PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];

            CounterCreationData createdCounterData = installer.Counters[0];
            Assert.AreEqual("Bill", createdCounterData.CounterName);
            Assert.AreEqual("Help Bill", createdCounterData.CounterHelp);
            Assert.AreEqual(PerformanceCounterType.CounterMultiTimer, createdCounterData.CounterType);
        }

        [TestMethod]
        public void PerformanceCountersFromNonInstrumentedClassesAreNotAdded()
        {
            PerformanceCounterInstaller parentInstaller = new PerformanceCounterInstaller();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(new Type[] { typeof(NonInstrumentedClass) });
            builder.Fill(parentInstaller);

            Assert.AreEqual(0, parentInstaller.Installers.Count);
        }

        [TestMethod]
        public void MultipleCountersInSingleTypeAreAllAddedToInstaller()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(new Type[] { typeof(MultipleCounterTypeForCategoryA) });
            builder.Fill(parentInstaller);

            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());

            PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];

            Assert.AreEqual(2, installer.Counters.Count);
            CounterCreationData firstCounter = installer.Counters[0];
            CounterCreationData secondCounter = installer.Counters[1];
            Assert.IsFalse(ReferenceEquals(firstCounter, secondCounter));
            Assert.IsFalse(firstCounter.Equals(secondCounter));
        }

        [TestMethod]
        public void CounterCategoryInformationIsPopulatedIntoCreatedInstaller()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(new Type[] { typeof(MultipleCounterTypeForCategoryA) });
            builder.Fill(parentInstaller);

            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());

            PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];

            Assert.AreEqual(PerformanceCounterCategoryType.MultiInstance, installer.CategoryType);
            Assert.AreEqual("CategoryName", installer.CategoryName);
            Assert.AreEqual("This is the help", installer.CategoryHelp);
        }

        [TestMethod]
        public void DisjointCountersForSameCategoriesInDifferentTypesCreatesSingleInstaller()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(new Type[] { typeof(MultipleCounterTypeForCategoryA), typeof(AlternativeMultipleCounterTypeForCategoryA) });
            builder.Fill(parentInstaller);

            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());

            PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];
            Assert.AreEqual(PerformanceCounterCategoryType.MultiInstance, installer.CategoryType);
            Assert.AreEqual("CategoryName", installer.CategoryName);
            Assert.AreEqual("This is the help", installer.CategoryHelp);

            Assert.AreEqual(4, installer.Counters.Count);
        }

        [TestMethod]
        public void IntersectingIdenticalCountersForSameCategoriesInDifferentTypesKeepsSingleCounter()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(
                    new Type[]
                        {
                            typeof(MultipleCounterTypeForCategoryA),
                            typeof(AlternativeMultipleCounterTypeForCategoryAWithRepeatedIdenticalCounterDefinition)
                        });
            builder.Fill(parentInstaller);

            Assert.AreEqual(1, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());

            PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];
            Assert.AreEqual(PerformanceCounterCategoryType.MultiInstance, installer.CategoryType);
            Assert.AreEqual("CategoryName", installer.CategoryName);
            Assert.AreEqual("This is the help", installer.CategoryHelp);

            Assert.AreEqual(3, installer.Counters.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IntersectingDifferentCountersForSameCategoriesInDifferentTypesThrows()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(
                    new Type[]
                        {
                            typeof(MultipleCounterTypeForCategoryA),
                            typeof(AlternativeMultipleCounterTypeForCategoryAWithRepeatedDifferentCounterDefinition)
                        });
            builder.Fill(parentInstaller);
        }

        [TestMethod]
        public void IntersectingCountersForDifferentCategoriesInDifferentTypesCreatesMultipleInstallers()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder
                = new PerformanceCounterInstallerBuilder(
                    new Type[]
                        {
                            typeof(MultipleCounterTypeForCategoryA),
                            typeof(MultipleCounterTypeForCategoryB)
                        });
            builder.Fill(parentInstaller);

            Assert.AreEqual(2, parentInstaller.Installers.Count);
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());
            Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[1].GetType());

            PerformanceCounterInstaller installer1 = (PerformanceCounterInstaller)parentInstaller.Installers[0];
            Assert.AreEqual(PerformanceCounterCategoryType.MultiInstance, installer1.CategoryType);
            Assert.AreEqual("CategoryName", installer1.CategoryName);
            Assert.AreEqual("This is the help", installer1.CategoryHelp);
            Assert.AreEqual(2, installer1.Counters.Count);
            Assert.IsFalse(ReferenceEquals(installer1.Counters[0], installer1.Counters[1]));
            Assert.IsFalse(installer1.Counters[0].Equals(installer1.Counters[1]));

            PerformanceCounterInstaller installer2 = (PerformanceCounterInstaller)parentInstaller.Installers[1];
            Assert.AreEqual(PerformanceCounterCategoryType.MultiInstance, installer2.CategoryType);
            Assert.AreEqual("CategoryNameB", installer2.CategoryName);
            Assert.AreEqual("This is the help", installer2.CategoryHelp);
            Assert.AreEqual(2, installer2.Counters.Count);
            Assert.IsFalse(ReferenceEquals(installer2.Counters[0], installer2.Counters[1]));
            Assert.IsFalse(installer2.Counters[0].Equals(installer2.Counters[1]));
        }

        [TestMethod]
        public void NoExceptionThrownIfNoInstrumentedTypesInList()
        {
            PerformanceCounterInstaller installer = new PerformanceCounterInstaller();
            PerformanceCounterInstallerBuilder builder =
                new PerformanceCounterInstallerBuilder(new Type[] { typeof(NonInstrumentedClass) });
            builder.Fill(installer);
        }

        [TestMethod]
        public void CanGetCategoryHelpText()
        {
            PerformanceCountersDefinitionAttribute attribute = new PerformanceCountersDefinitionAttribute("category", "CategoryHelp");
            string translatedHelp = PerformanceCounterInstallerBuilder.GetCategoryHelp(attribute, GetType().Assembly);

            Assert.AreEqual(Resources.CategoryHelp, translatedHelp);
        }

        [TestMethod]
        public void CanGetCategoryHelpTextForNonExistingName()
        {
            PerformanceCountersDefinitionAttribute attribute = new PerformanceCountersDefinitionAttribute("category", "MissingCategoryHelp");
            string translatedHelp = PerformanceCounterInstallerBuilder.GetCategoryHelp(attribute, GetType().Assembly);

            Assert.AreEqual(string.Empty, translatedHelp);
        }

        [TestMethod]
        public void CanGetCategoryHelpTextFromArbitraryResource()
        {
            PerformanceCountersDefinitionAttribute attribute = new PerformanceCountersDefinitionAttribute("category", "CategoryHelpFromArbitraryResource");
            string translatedHelp = PerformanceCounterInstallerBuilder.GetCategoryHelp(attribute, GetType().Assembly);

            Assert.AreEqual(ExtraResources.CategoryHelpFromArbitraryResource, translatedHelp);
        }

        [TestMethod]
        public void CanGetCounterHelpText()
        {
            PerformanceCounterAttribute attribute = new PerformanceCounterAttribute("counter", "CategoryHelp", PerformanceCounterType.NumberOfItems64);
            string translatedHelp = PerformanceCounterInstallerBuilder.GetCounterHelp(attribute.CounterHelp, GetType().Assembly);

            Assert.AreEqual(Resources.CategoryHelp, translatedHelp);
        }

        [TestMethod]
        public void CanGetCounterHelpTextFromArbitraryResource()
        {
            PerformanceCounterAttribute attribute = new PerformanceCounterAttribute("counter", "CategoryHelpFromArbitraryResource", PerformanceCounterType.NumberOfItems64);
            string translatedHelp = PerformanceCounterInstallerBuilder.GetCounterHelp(attribute.CounterHelp, GetType().Assembly);

            Assert.AreEqual(ExtraResources.CategoryHelpFromArbitraryResource, translatedHelp);
        }

        [TestMethod]
        public void CountersWithBaseCountersAreAddedToInstallerInCorrectOrder()
        {
            Installer parentInstaller = new Installer();
            PerformanceCounterInstallerBuilder builder = new PerformanceCounterInstallerBuilder(new Type[] { typeof(TypeWithComplexCounter) });
            builder.Fill(parentInstaller);

            PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];

            Assert.AreEqual(2, installer.Counters.Count);

            CounterCreationData realCounter = installer.Counters[0];
            CounterCreationData baseCounter = installer.Counters[1];

            Assert.AreEqual("real name", realCounter.CounterName);
            Assert.AreEqual(PerformanceCounterType.RawFraction, realCounter.CounterType);

            Assert.AreEqual("base name", baseCounter.CounterName);
            Assert.AreEqual(PerformanceCounterType.RawBase, baseCounter.CounterType);
        }

        /*
 			Installer parentInstaller = new Installer();
			PerformanceCounterInstallerBuilder builder
				= new PerformanceCounterInstallerBuilder(new Type[] { typeof(SingleCounterType) });
			builder.Fill(parentInstaller);

			Assert.AreEqual(1, parentInstaller.Installers.Count);
			Assert.AreSame(typeof(PerformanceCounterInstaller), parentInstaller.Installers[0].GetType());
			
			PerformanceCounterInstaller installer = (PerformanceCounterInstaller)parentInstaller.Installers[0];

			CounterCreationData createdCounterData = installer.Counters[0];
			Assert.AreEqual("Bill", createdCounterData.CounterName);
			Assert.AreEqual("Help Bill", createdCounterData.CounterHelp);
			Assert.AreEqual(PerformanceCounterType.CounterMultiTimer, createdCounterData.CounterType);

		 * */

        [HasInstallableResourcesAttribute]
        public class NoCountersType {}

        public class NonInstrumentedClass
        {
            [PerformanceCounter("Tom", "HelpTomHelpResource", PerformanceCounterType.CounterDelta64)]
            EnterpriseLibraryPerformanceCounter counter = new EnterpriseLibraryPerformanceCounter("foo", "Tom");

            public void Raise()
            {
                counter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("foo", "Help")]
        public class SingleCounterType
        {
            [PerformanceCounter("Bill", "HelpBillHelpResource", PerformanceCounterType.CounterMultiTimer)]
            EnterpriseLibraryPerformanceCounter counter = new EnterpriseLibraryPerformanceCounter("foo", "Bill");

            public void Raise()
            {
                counter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("CategoryName", "CategoryHelp", PerformanceCounterCategoryType.MultiInstance)]
        public class MultipleCounterTypeForCategoryA
        {
            [PerformanceCounter("Bill", "HelpBillHelpResource", PerformanceCounterType.CounterMultiTimer)]
            EnterpriseLibraryPerformanceCounter firstCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Bill");

            [PerformanceCounter("Tom", "HelpTomHelpResource", PerformanceCounterType.CounterTimer)]
            EnterpriseLibraryPerformanceCounter secondCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Tom");

            public void Raise()
            {
                firstCounter.Increment();
                secondCounter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("CategoryName", "CategoryHelp", PerformanceCounterCategoryType.MultiInstance)]
        public class AlternativeMultipleCounterTypeForCategoryA
        {
            [PerformanceCounter("Bill2", "HelpBillHelpResource", PerformanceCounterType.CounterMultiTimer)]
            EnterpriseLibraryPerformanceCounter firstCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Bill2");

            [PerformanceCounter("Tom2", "HelpTomHelpResource", PerformanceCounterType.CounterTimer)]
            EnterpriseLibraryPerformanceCounter secondCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Tom2");

            public void Raise()
            {
                firstCounter.Increment();
                secondCounter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("CategoryName", "CategoryHelp", PerformanceCounterCategoryType.MultiInstance)]
        public class AlternativeMultipleCounterTypeForCategoryAWithRepeatedIdenticalCounterDefinition
        {
            [PerformanceCounter("Bill", "HelpBillHelpResource", PerformanceCounterType.CounterMultiTimer)]
            EnterpriseLibraryPerformanceCounter firstCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Bill");

            [PerformanceCounter("Tom2", "HelpTomHelpResource", PerformanceCounterType.CounterTimer)]
            EnterpriseLibraryPerformanceCounter secondCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Tom2");

            public void Raise()
            {
                firstCounter.Increment();
                secondCounter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("CategoryName", "CategoryHelp", PerformanceCounterCategoryType.MultiInstance)]
        public class AlternativeMultipleCounterTypeForCategoryAWithRepeatedDifferentCounterDefinition
        {
            [PerformanceCounter("Bill", "HelpBillHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
            EnterpriseLibraryPerformanceCounter firstCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Bill2");

            [PerformanceCounter("Tom2", "HelpTomHelpResource", PerformanceCounterType.CounterTimer)]
            EnterpriseLibraryPerformanceCounter secondCounter = new EnterpriseLibraryPerformanceCounter("CategoryName", "Tom3");

            public void Raise()
            {
                firstCounter.Increment();
                secondCounter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("CategoryNameB", "CategoryHelp", PerformanceCounterCategoryType.MultiInstance)]
        public class MultipleCounterTypeForCategoryB
        {
            [PerformanceCounter("Bill", "HelpBillHelpResource", PerformanceCounterType.CounterMultiTimer)]
            EnterpriseLibraryPerformanceCounter firstCounter = new EnterpriseLibraryPerformanceCounter("CategoryNameAlt", "Bill");

            [PerformanceCounter("Tom", "HelpTomHelpResource", PerformanceCounterType.CounterTimer)]
            EnterpriseLibraryPerformanceCounter secondCounter = new EnterpriseLibraryPerformanceCounter("CategoryNameAlt", "Tom");

            public void Raise()
            {
                firstCounter.Increment();
                secondCounter.Increment();
            }
        }

        [HasInstallableResourcesAttribute]
        [PerformanceCountersDefinitionAttribute("CategoryNameB", "CategoryHelp", PerformanceCounterCategoryType.MultiInstance)]
        class TypeWithComplexCounter
        {
            EnterpriseLibraryPerformanceCounter rawBase = new EnterpriseLibraryPerformanceCounter("CategoryNameAlt", "Tom");

            [PerformanceCounter("real name", "real help", PerformanceCounterType.RawFraction,
                BaseCounterName = "base name", BaseCounterHelp = "base help", BaseCounterType = PerformanceCounterType.RawBase)]
            EnterpriseLibraryPerformanceCounter rawFraction = new EnterpriseLibraryPerformanceCounter("CategoryNameAlt", "Bill");

            public void Raise()
            {
                rawFraction.Increment();
                rawBase.Increment();
            }
        }
    }
}
