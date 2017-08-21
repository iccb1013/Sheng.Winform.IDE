/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationElementManageabilityProviderAttributesParsingFixture
    {
        IDictionary<string, string> dictionary;
        [TestInitialize]
        public void SetUp()
        {
            dictionary = new Dictionary<string, string>();
        }
        [TestMethod]
        public void EmptyStringResultsInEmptyDictionary()
        {
            String attributes = "";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(0, dictionary.Count);
        }
        [TestMethod]
        public void CanExtractSingleValueString()
        {
            String attributes = "name=value";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("value", dictionary["name"]);
        }
        [TestMethod]
        public void CanExtractSingleValueStringWithTrailingSemicolon()
        {
            String attributes = "name=value;";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("value", dictionary["name"]);
        }
        [TestMethod]
        public void CanExtractSingleValueWithSpacesString()
        {
            String attributes = "name=value with spaces";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("value with spaces", dictionary["name"]);
        }
        [TestMethod]
        public void CanExtractMultipleValueStrings()
        {
            String attributes = "name1=value1;name2=value2";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual("value1", dictionary["name1"]);
            Assert.AreEqual("value2", dictionary["name2"]);
        }
        [TestMethod]
        public void CanExtractSingleEmptyValueString()
        {
            String attributes = "name1=";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("", dictionary["name1"]);
        }
        [TestMethod]
        public void CanExtractMultipleValueStringsWithEmtpyValues()
        {
            String attributes = "name1=;name2=;name3=value3";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("", dictionary["name1"]);
            Assert.AreEqual("", dictionary["name2"]);
            Assert.AreEqual("value3", dictionary["name3"]);
        }
        [TestMethod]
        public void EntryWithEmptyNameIsIgnored()
        {
            String attributes = "=value1;name2=;=value3";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("", dictionary["name2"]);
        }
        [TestMethod]
        public void SpacesSurroundingValueNameAreIgnored()
        {
            String attributes = " name1=value1;name2 =; name3 =value3";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("value1", dictionary["name1"]);
            Assert.AreEqual("", dictionary["name2"]);
            Assert.AreEqual("value3", dictionary["name3"]);
        }
        [TestMethod]
        public void EscapedSemicolonsCanBePartOfTheValue()
        {
            String attributes = " name1=va;;lue1;name2= ; name3 =va;;lue3";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("va;lue1", dictionary["name1"]);
            Assert.AreEqual(" ", dictionary["name2"]);
            Assert.AreEqual("va;lue3", dictionary["name3"]);
        }
        [TestMethod]
        public void TrailingSemicolonIsNotPartOfTheValue()
        {
            String attributes = "name1=value1;;value2;";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("value1;value2", dictionary["name1"]);
        }
        [TestMethod]
        public void EscapedTrailingSemicolonIsPartOfTheValue()
        {
            String attributes = "name1=value1;;value2;;";
            KeyValuePairParsingTestHelper.ExtractKeyValueEntries(attributes, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("value1;value2;", dictionary["name1"]);
        }
    }
}
