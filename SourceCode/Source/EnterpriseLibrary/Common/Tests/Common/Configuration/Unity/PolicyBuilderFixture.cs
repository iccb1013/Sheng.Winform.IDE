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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class PolicyBuilderFixture
	{
		private IUnityContainer container;
		private TestHelperExtension extension;
		private TestClassConfiguration configuration;

		[TestInitialize]
		public void SetUp()
		{
			this.container = new UnityContainer();
			this.extension = new TestHelperExtension();

			this.configuration
				= new TestClassConfiguration("string 1", "string 2", "42", "bar", "bar-baz");
			this.configuration.Prop6 = new Dictionary<string, int>();
			this.configuration.Prop6.Add("bar", 0);
			this.configuration.Prop6.Add("baz", 42);
		}

		[TestCleanup]
		public void TearDown()
		{
			this.container.Dispose();
			this.container = null;
		}

		[TestMethod]
		public void CanCreatePolicyForNoArgsConstructor()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration, c => new TestClass())
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(null, createdObject.Prop1);
			Assert.AreEqual(null, createdObject.Prop2);
			Assert.AreEqual(0, createdObject.Prop3);
			Assert.IsNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);
		}

		[TestMethod]
		public void CanCreatePolicyForConstructorWithSimpleArguments()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration, c => new TestClass(c.Prop1, c.Prop2))
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual("string 1", createdObject.Prop1);
			Assert.AreEqual("string 2", createdObject.Prop2);
			Assert.AreEqual(0, createdObject.Prop3);
			Assert.IsNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);
		}

		[TestMethod]
		public void CanCreatePolicyForConstructorWithSimpleArgumentsWithConversion()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration, c => new TestClass(c.Prop1, int.Parse(c.Prop3)))
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual("string 1", createdObject.Prop1);
			Assert.AreEqual(null, createdObject.Prop2);
			Assert.AreEqual(42, createdObject.Prop3);
			Assert.IsNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);
		}

		[TestMethod]
		public void CanCreatePolicyForConstructorWithSimpleResolution()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
					c => new TestClass(
							int.Parse(c.Prop3),
							Resolve.Reference<TestClass>(c.Prop4)))
					.AddPoliciesToPolicyList(context.Policies);

				new PolicyBuilder<TestClass, TestClassConfiguration>("bar", this.configuration,
					c => new TestClass(c.Prop1, c.Prop2))
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(null, createdObject.Prop1);
			Assert.AreEqual(null, createdObject.Prop2);
			Assert.AreEqual(42, createdObject.Prop3);
			Assert.IsNotNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);
			Assert.AreEqual("string 1", createdObject.Prop4.Prop1);
			Assert.AreEqual("string 2", createdObject.Prop4.Prop2);
			Assert.AreEqual(0, createdObject.Prop4.Prop3);
			Assert.IsNull(createdObject.Prop4.Prop4);
			Assert.IsNull(createdObject.Prop4.Prop5);
			Assert.IsNull(createdObject.Prop4.Prop6);
		}

		[TestMethod]
		public void CanCreatePolicyForConstructorWithCollectionResolution()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
					c => new TestClass(
								c.Prop2,
								Resolve.ReferenceCollection<List<TestClass>, TestClass>(c.Prop5.Split('-'))))
					.AddPoliciesToPolicyList(context.Policies);

				new PolicyBuilder<TestClass, TestClassConfiguration>("bar", this.configuration,
						c => new TestClass(c.Prop1, c.Prop2))
					.AddPoliciesToPolicyList(context.Policies);

				new PolicyBuilder<TestClass, TestClassConfiguration>("baz", this.configuration,
						c => new TestClass(c.Prop1, int.Parse(c.Prop3)))
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(null, createdObject.Prop1);
			Assert.AreEqual("string 2", createdObject.Prop2);
			Assert.AreEqual(0, createdObject.Prop3);
			Assert.IsNull(createdObject.Prop4);
			Assert.IsNotNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);

			IEnumerator<TestClass> enumerator = createdObject.Prop5.GetEnumerator();

			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual("string 1", enumerator.Current.Prop1);
			Assert.AreEqual("string 2", enumerator.Current.Prop2);
			Assert.AreEqual(0, enumerator.Current.Prop3);
			Assert.IsNull(enumerator.Current.Prop4);
			Assert.IsNull(enumerator.Current.Prop5);
			Assert.IsNull(enumerator.Current.Prop6);

			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual("string 1", enumerator.Current.Prop1);
			Assert.AreEqual(null, enumerator.Current.Prop2);
			Assert.AreEqual(42, enumerator.Current.Prop3);
			Assert.IsNull(enumerator.Current.Prop4);
			Assert.IsNull(enumerator.Current.Prop5);
			Assert.IsNull(enumerator.Current.Prop6);

			Assert.IsFalse(enumerator.MoveNext());
		}

		[TestMethod]
		public void CanCreatePolicyForConstructorWithDictionaryResolution()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>(
					"foo", 
					this.configuration,
					c => new TestClass(
								Resolve.ReferenceDictionary<Dictionary<int, TestClass>, TestClass, int>(
									//could just use c.Prop6, but wouldn't be a usual case
									from kvp in c.Prop6 select new KeyValuePair<string, int>(kvp.Key, kvp.Value))))
					.AddPoliciesToPolicyList(context.Policies);

				new PolicyBuilder<TestClass, TestClassConfiguration>(
					"bar", 
					this.configuration,
					c => new TestClass(c.Prop1, c.Prop2))
					.AddPoliciesToPolicyList(context.Policies);

				new PolicyBuilder<TestClass, TestClassConfiguration>(
					"baz", 
					this.configuration,
					c => new TestClass(c.Prop1, int.Parse(c.Prop3)))
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(null, createdObject.Prop1);
			Assert.AreEqual(null, createdObject.Prop2);
			Assert.AreEqual(0, createdObject.Prop3);
			Assert.IsNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNotNull(createdObject.Prop6);

			Assert.AreEqual(2, createdObject.Prop6.Count);

			Assert.AreEqual("string 1", (createdObject.Prop6[0]).Prop1);
			Assert.AreEqual("string 2", (createdObject.Prop6[0]).Prop2);
			Assert.AreEqual(0, (createdObject.Prop6[0]).Prop3);
			Assert.IsNull((createdObject.Prop6[0]).Prop4);
			Assert.IsNull((createdObject.Prop6[0]).Prop5);
			Assert.IsNull((createdObject.Prop6[0]).Prop6);

			Assert.AreEqual("string 1", (createdObject.Prop6[42]).Prop1);
			Assert.AreEqual(null, (createdObject.Prop6[42]).Prop2);
			Assert.AreEqual(42, (createdObject.Prop6[42]).Prop3);
			Assert.IsNull((createdObject.Prop6[42]).Prop4);
			Assert.IsNull((createdObject.Prop6[42]).Prop5);
			Assert.IsNull((createdObject.Prop6[42]).Prop6);
		}

		[TestMethod]
		public void CanCreatePolicySimpleProperty()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass())
					.SetProperty(o => o.Prop1).To(c => c.Prop1)
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual("string 1", createdObject.Prop1);
			Assert.AreEqual(null, createdObject.Prop2);
			Assert.AreEqual(0, createdObject.Prop3);
			Assert.IsNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);
		}

		[TestMethod]
		public void CanCreatePoliciesForMultipleProperties()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass(c.Prop1, c.Prop2))
					.SetProperty(o => o.Prop3).To(c => int.Parse(c.Prop3))
					.SetProperty(o => o.Prop4).To(c => Resolve.Reference<TestClass>(c.Prop4))
					.AddPoliciesToPolicyList(context.Policies);

				new PolicyBuilder<TestClass, TestClassConfiguration>("bar", this.configuration,
						c => new TestClass(c.Prop1, c.Prop2))
					.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);

			TestClass createdObject = container.Resolve<TestClass>("foo");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual("string 1", createdObject.Prop1);
			Assert.AreEqual("string 2", createdObject.Prop2);
			Assert.AreEqual(42, createdObject.Prop3);
			Assert.IsNotNull(createdObject.Prop4);
			Assert.IsNull(createdObject.Prop5);
			Assert.IsNull(createdObject.Prop6);
			Assert.AreEqual("string 1", createdObject.Prop4.Prop1);
			Assert.AreEqual("string 2", createdObject.Prop4.Prop2);
			Assert.AreEqual(0, createdObject.Prop4.Prop3);
			Assert.IsNull(createdObject.Prop4.Prop4);
			Assert.IsNull(createdObject.Prop4.Prop5);
			Assert.IsNull(createdObject.Prop4.Prop6);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IncompletePropertyMappingThrows()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				// can't use the fluent approach, the fluent API doesn't allow for this scenario.
				Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent.IPropertyAndFinishPolicyBuilding<TestClass, TestClassConfiguration> builder
					= new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass());
				builder.SetProperty(o => o.Prop1);	// unfinished property
				builder.AddPoliciesToPolicyList(context.Policies);
			};
			container.AddExtension(this.extension);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NonConstructorCallOnCreateWithThrows()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				// it's not necessary to add the policies for this to throw
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => (TestClass)null);	// the expression has the appropriate type, but it's not a ctor
			};
			container.AddExtension(this.extension);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void MethdoInvocationOnSetPropertyThrows()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass())
					// it's not necessary to add the policies for this to throw
					.SetProperty(o => o.ToString()).To(c => c.Prop1);
			};
			container.AddExtension(this.extension);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void FieldAccessOnSetPropertyThrows()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass())
					// it's not necessary to add the policies for this to throw
					.SetProperty(o => o.Field).To(c => c.Prop1);
			};
			container.AddExtension(this.extension);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NonMemberAccessOnSetPropertyThrows()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass())
					// it's not necessary to add the policies for this to throw
					.SetProperty(o => (string)o.Prop1).To(c => c.Prop1);	// note the cast
			};
			container.AddExtension(this.extension);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AttemptToMapAPropertyAfterFinishThrows()
		{
			this.extension.initialize = delegate(ExtensionContext context)
			{
				PolicyBuilder<TestClass, TestClassConfiguration> policyBuilder
					= new PolicyBuilder<TestClass, TestClassConfiguration>("foo", this.configuration,
						c => new TestClass());
				policyBuilder.AddPoliciesToPolicyList(context.Policies);
				policyBuilder.SetProperty(o => o.Prop1).To(c => c.Prop1);
			};
			container.AddExtension(this.extension);
		}

		public class TestClass
		{
			public TestClass()
			{ }

			public TestClass(string Prop1, string Prop2)
			{
				this.Prop1 = Prop1;
				this.Prop2 = Prop2;
			}

			public TestClass(string Prop1, int Prop3)
			{
				this.Prop1 = Prop1;
				this.Prop3 = Prop3;
			}

			public TestClass(int Prop3, TestClass Prop4)
			{
				this.Prop3 = Prop3;
				this.Prop4 = Prop4;
			}

			public TestClass(string Prop2, IEnumerable<TestClass> Prop5)
			{
				this.Prop2 = Prop2;
				this.Prop5 = Prop5;
			}

			public TestClass(IDictionary<int, TestClass> Prop6)
			{
				this.Prop6 = Prop6;
			}

			public string Prop1 { get; set; }
			public string Prop2 { get; set; }
			public int Prop3 { get; set; }
			public TestClass Prop4 { get; set; }
			public IEnumerable<TestClass> Prop5 { get; set; }
			public IDictionary<int, TestClass> Prop6 { get; set; }
			public string Field;
		}

		public class TestClassConfiguration
		{
			public TestClassConfiguration(string Prop1, string Prop2, string Prop3, string Prop4, string Prop5)
			{
				this.Prop1 = Prop1;
				this.Prop2 = Prop2;
				this.Prop3 = Prop3;
				this.Prop4 = Prop4;
				this.Prop5 = Prop5;
			}

			public string Prop1 { get; set; }
			public string Prop2 { get; set; }
			public string Prop3 { get; set; }
			public string Prop4 { get; set; }
			public string Prop5 { get; set; }
			public IDictionary<string, int> Prop6 { get; set; }
		}
	}
}
