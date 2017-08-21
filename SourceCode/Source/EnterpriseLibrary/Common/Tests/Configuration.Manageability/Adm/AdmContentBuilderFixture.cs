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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Adm
{
    [TestClass]
    public class AdmContentBuilderFixture
    {
        [TestMethod]
        public void CreationFromFreshBuilderReturnsEmptyContent()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            Assert.IsNotNull(builder.GetContent());
            IEnumerator<AdmCategory> categoriesEnumerator = builder.GetContent().Categories.GetEnumerator();
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EndCategoryWithoutStartThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.EndCategory();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EndCategoryWithStartedPolicyThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();
            builder.StartCategory("category");
            builder.StartPolicy("name", "key");
            builder.EndCategory();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartCategoryWithStartedPolicyThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();
            builder.StartCategory("category");
            builder.StartPolicy("name", "key");
            builder.StartCategory("subcategory");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetContentWithStartedCategoriesThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.GetContent();
        }

        [TestMethod]
        public void CanCreateCategory()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanCreateManyCategories()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category1");
            builder.EndCategory();
            builder.StartCategory("category2");
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category1", categoriesEnumerator.Current.Name);
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category2", categoriesEnumerator.Current.Name);
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanCreateNestedCategory()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartCategory("subcategory");
            builder.EndCategory();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual("subcategory", subCategoriesEnumerator.Current.Name);
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanCreateManyNestedCategories()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartCategory("subcategory1");
            builder.EndCategory();
            builder.StartCategory("subcategory2");
            builder.EndCategory();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual("subcategory1", subCategoriesEnumerator.Current.Name);
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual("subcategory2", subCategoriesEnumerator.Current.Name);
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartCategoryWithNullNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartCategoryWithEmptyNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartCategoryWithInvalidNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("dfhfdas\"fdjd");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartPolicyFailsIfThereAreNoCategories()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartPolicy("name", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartPolicyFailsIfThereIsACurrentStartedPolicy()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("name", "key");
            builder.StartPolicy("name", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EndPolicyFailsIfThereIsNoCurrentStartedPolicy()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.EndPolicy();
        }

        [TestMethod]
        public void CanCreatePolicyInTopLevelCategory()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.AreEqual("policy", policiesEnumerator.Current.Name);
            Assert.AreEqual("key", policiesEnumerator.Current.KeyName);
            Assert.AreEqual(AdmContentBuilder.AvailableValueName, policiesEnumerator.Current.ValueName);
            Assert.IsFalse(policiesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanCreatePolicyInNestedCategory()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartCategory("subcategory");
            builder.StartPolicy("policy", "key");
            builder.EndPolicy();
            builder.EndCategory();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual("subcategory", subCategoriesEnumerator.Current.Name);
            IEnumerator<AdmPolicy> policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.AreEqual("policy", policiesEnumerator.Current.Name);
            Assert.IsFalse(policiesEnumerator.MoveNext());
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartPolicyWithNullNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy(null, "key");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartPolicyWithEmptyNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartPolicyWithInvalidNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("dfhfdas\"fdjd", "key");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartPolicyWithNullPolicyKeyThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartPolicyWithEmptyPolicyKeyThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartPolicyWithInvalidPolicyKeyThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "rkjerlejkre\\fe\"lkfelk*(*\\teflkj");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartPolicyWithLongPolicyKeyThrows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            String keyName = stringBuilder.ToString();
            Assert.IsTrue(keyName.Length > 255);

            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "parent\\" + keyName + "\\child");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPartWithoutStartedPolicyThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.AddPart(new AdmTextPart("text"));
        }

        [TestMethod]
        public void CanAddPartInStartedPolicy()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddPart(new AdmTextPart("text"));
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("text", ((AdmTextPart)partsEnumerator.Current).PartName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddManyPartsInStartedPolicy()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddPart(new AdmTextPart("text1"));
            builder.AddPart(new AdmTextPart("text2"));
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            Assert.IsNotNull(content);
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            Assert.AreEqual("category", categoriesEnumerator.Current.Name);
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("text1", ((AdmTextPart)partsEnumerator.Current).PartName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("text2", ((AdmTextPart)partsEnumerator.Current).PartName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddCheckboxPart()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddCheckboxPart("part", "value", true);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmCheckboxPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmCheckboxPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmCheckboxPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(true, ((AdmCheckboxPart)partsEnumerator.Current).CheckedByDefault);
        }

        [TestMethod]
        public void CanAddCheckboxPartWithKeyName()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddCheckboxPart("part", "key", "value", true, true, false);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmCheckboxPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmCheckboxPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmCheckboxPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(true, ((AdmCheckboxPart)partsEnumerator.Current).CheckedByDefault);
            Assert.AreEqual(true, ((AdmCheckboxPart)partsEnumerator.Current).ValueForOn);
            Assert.AreEqual(false, ((AdmCheckboxPart)partsEnumerator.Current).ValueForOff);
        }

        [TestMethod]
        public void CanAddCheckboxPartWithValueOnAndOff()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart("part", "value", true, true, false);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmCheckboxPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmCheckboxPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmCheckboxPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(true, ((AdmCheckboxPart)partsEnumerator.Current).CheckedByDefault);
            Assert.AreEqual(true, ((AdmCheckboxPart)partsEnumerator.Current).ValueForOn);
            Assert.AreEqual(false, ((AdmCheckboxPart)partsEnumerator.Current).ValueForOff);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCheckBoxWithNullPartNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart(null, "value", true, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCheckBoxWithEmptyPartNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart("", "value", true, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCheckBoxWithInvalidPartNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart("fjdlkjfd\"flkjej", "value", true, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCheckBoxWithInvalidPolicyNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart("policy", "invalid\"key", "value", true, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCheckBoxWithInvalidValueNameThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart("policy", "invalid\"value", true, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCheckBoxWithLongValueNameThrows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            String valueName = stringBuilder.ToString();
            Assert.IsTrue(valueName.Length > 255);

            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            ;
            builder.AddCheckboxPart("policy", valueName, true, true, false);
        }

        [TestMethod]
        public void CanAddComboBoxPart()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddComboBoxPart("part", "value", "default", 10, true, "sug1", "sug2");
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmComboBoxPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmComboBoxPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmComboBoxPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmComboBoxPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("default", ((AdmComboBoxPart)partsEnumerator.Current).DefaultValue);
            //Assert.AreEqual(10, ((AdmComboBoxPart)partsEnumerator.Current).Maxlen);	// because of workaround for GPMC bug
            Assert.AreEqual(true, ((AdmComboBoxPart)partsEnumerator.Current).Required);
            IEnumerator<String> suggestionsEnumerator = ((AdmComboBoxPart)partsEnumerator.Current).Suggestions.GetEnumerator();
            Assert.IsTrue(suggestionsEnumerator.MoveNext());
            Assert.AreEqual("sug1", suggestionsEnumerator.Current);
            Assert.IsTrue(suggestionsEnumerator.MoveNext());
            Assert.AreEqual("sug2", suggestionsEnumerator.Current);
            Assert.IsFalse(suggestionsEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddComboBoxPartWithKeyName()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddComboBoxPart("part", "key", "value", "default", 10, true, "sug1", "sug2");
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmComboBoxPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmComboBoxPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmComboBoxPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmComboBoxPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("default", ((AdmComboBoxPart)partsEnumerator.Current).DefaultValue);
            //Assert.AreEqual(10, ((AdmComboBoxPart)partsEnumerator.Current).Maxlen);	// because of workaround for GPMC bug
            Assert.AreEqual(true, ((AdmComboBoxPart)partsEnumerator.Current).Required);
            IEnumerator<String> suggestionsEnumerator = ((AdmComboBoxPart)partsEnumerator.Current).Suggestions.GetEnumerator();
            Assert.IsTrue(suggestionsEnumerator.MoveNext());
            Assert.AreEqual("sug1", suggestionsEnumerator.Current);
            Assert.IsTrue(suggestionsEnumerator.MoveNext());
            Assert.AreEqual("sug2", suggestionsEnumerator.Current);
            Assert.IsFalse(suggestionsEnumerator.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddComboBoxPartWithInvalidSugestionThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddComboBoxPart("part", "key", "value", "default", 10, true, "sug1", "sug2", "invalid\"suggestion");
        }

        [Ignore()] // because of workaround for GPMC bug
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddComboBoxPartWithInvalidMaxLengthThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddComboBoxPart("part", "key", "value", "default", 1100, true, "sug1", "sug2", "suggestion");
        }

        [TestMethod]
        public void CanAddEditTextPart()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddEditTextPart("part", "value", "default", 10, true);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmEditTextPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmEditTextPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("default", ((AdmEditTextPart)partsEnumerator.Current).DefaultValue);
            Assert.AreEqual(10, ((AdmEditTextPart)partsEnumerator.Current).Maxlen);
            Assert.AreEqual(true, ((AdmEditTextPart)partsEnumerator.Current).Required);
        }

        [TestMethod]
        public void CanAddEditTextPartWithKeyName()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddEditTextPart("part", "key", "value", "default", 10, true);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmEditTextPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmEditTextPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("default", ((AdmEditTextPart)partsEnumerator.Current).DefaultValue);
            Assert.AreEqual(10, ((AdmEditTextPart)partsEnumerator.Current).Maxlen);
            Assert.AreEqual(true, ((AdmEditTextPart)partsEnumerator.Current).Required);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEditTextWithInvalidMaxLengthThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddEditTextPart("part", "key", "value", "default", 1100, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEditTextWithDefaultValueLongerThanMaxLengthThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddEditTextPart("part", "key", "value", "123456789012345678901", 20, true);
        }

        [TestMethod]
        public void CanAddTextPart()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddTextPart("part");
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmTextPart)partsEnumerator.Current).PartName);
        }

        [TestMethod]
        public void CanAddDropDownListPart()
        {
            AdmContentBuilder builder = new AdmContentBuilder();
            List<AdmDropDownListItem> items = new List<AdmDropDownListItem>();
            items.Add(new AdmDropDownListItem("1", "1"));
            items.Add(new AdmDropDownListItem("2", "2"));
            items.Add(new AdmDropDownListItem("3", "3"));

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPart("part", "value", items, "1");
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("1", ((AdmDropDownListPart)partsEnumerator.Current).DefaultValue);
            IEnumerator<AdmDropDownListItem> itemsEnumerator = ((AdmDropDownListPart)partsEnumerator.Current).Items.GetEnumerator();
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("1", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("2", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("3", itemsEnumerator.Current.Name);
            Assert.IsFalse(itemsEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddDropDownListPartWithKeyName()
        {
            List<AdmDropDownListItem> items = new List<AdmDropDownListItem>();
            items.Add(new AdmDropDownListItem("1", "1"));
            items.Add(new AdmDropDownListItem("2", "2"));
            items.Add(new AdmDropDownListItem("3", "3"));

            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPart("part", "key", "value", items, "1");
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("1", ((AdmDropDownListPart)partsEnumerator.Current).DefaultValue);
            IEnumerator<AdmDropDownListItem> itemsEnumerator = ((AdmDropDownListPart)partsEnumerator.Current).Items.GetEnumerator();
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("1", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("2", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("3", itemsEnumerator.Current.Name);
            Assert.IsFalse(itemsEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddDropDownListPartForEnumeration()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPartForEnumeration<UriFormat>("part", "value", UriFormat.UriEscaped);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(UriFormat.UriEscaped.ToString(), ((AdmDropDownListPart)partsEnumerator.Current).DefaultValue);
            Dictionary<String, String> items = new Dictionary<String, String>();
            foreach (AdmDropDownListItem item in ((AdmDropDownListPart)partsEnumerator.Current).Items)
            {
                items.Add(item.Name, item.Value);
            }
            foreach (String name in Enum.GetNames(typeof(UriFormat)))
            {
                Assert.IsTrue(items.ContainsKey(name));
            }
        }

        [TestMethod]
        public void CanAddDropDownListPartForEnumerationWithKeyName()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPartForEnumeration<UriFormat>("part", "key", "value", UriFormat.UriEscaped);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(UriFormat.UriEscaped.ToString(), ((AdmDropDownListPart)partsEnumerator.Current).DefaultValue);
            Dictionary<String, String> items = new Dictionary<String, String>();
            foreach (AdmDropDownListItem item in ((AdmDropDownListPart)partsEnumerator.Current).Items)
            {
                items.Add(item.Name, item.Value);
            }
            foreach (String name in Enum.GetNames(typeof(UriFormat)))
            {
                Assert.IsTrue(items.ContainsKey(name));
            }
        }

        [TestMethod]
        public void CanAddDropDownListPartForConfigurationElementsCollection()
        {
            NamedElementCollection<NamedConfigurationElement> collection = new NamedElementCollection<NamedConfigurationElement>();
            collection.Add(new NamedConfigurationElement("name1"));
            collection.Add(new NamedConfigurationElement("name2"));
            collection.Add(new NamedConfigurationElement("name3"));

            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPartForNamedElementCollection<NamedConfigurationElement>("part", "value", collection, "name1", false);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("name1", ((AdmDropDownListPart)partsEnumerator.Current).DefaultValue);
            IEnumerator<AdmDropDownListItem> itemsEnumerator = ((AdmDropDownListPart)partsEnumerator.Current).Items.GetEnumerator();
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name1", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name2", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name3", itemsEnumerator.Current.Name);
            Assert.IsFalse(itemsEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddDropDownListPartForConfigurationElementsCollectionAllowingNone()
        {
            NamedElementCollection<NamedConfigurationElement> collection = new NamedElementCollection<NamedConfigurationElement>();
            collection.Add(new NamedConfigurationElement("name1"));
            collection.Add(new NamedConfigurationElement("name2"));
            collection.Add(new NamedConfigurationElement("name3"));

            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPartForNamedElementCollection<NamedConfigurationElement>("part", "value", collection, "name1", true);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual("name1", ((AdmDropDownListPart)partsEnumerator.Current).DefaultValue);
            IEnumerator<AdmDropDownListItem> itemsEnumerator = ((AdmDropDownListPart)partsEnumerator.Current).Items.GetEnumerator();
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual(AdmContentBuilder.NoneListItem, itemsEnumerator.Current.Value);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name1", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name2", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name3", itemsEnumerator.Current.Name);
            Assert.IsFalse(itemsEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddDropDownListPartForConfigurationElementsCollectionAllowingNoneAndEmptyDefault()
        {
            NamedElementCollection<NamedConfigurationElement> collection = new NamedElementCollection<NamedConfigurationElement>();
            collection.Add(new NamedConfigurationElement("name1"));
            collection.Add(new NamedConfigurationElement("name2"));
            collection.Add(new NamedConfigurationElement("name3"));

            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddDropDownListPartForNamedElementCollection<NamedConfigurationElement>("part", "value", collection, "", true);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmDropDownListPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmDropDownListPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmDropDownListPart)partsEnumerator.Current).ValueName);
            IEnumerator<AdmDropDownListItem> itemsEnumerator = ((AdmDropDownListPart)partsEnumerator.Current).Items.GetEnumerator();
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual(AdmContentBuilder.NoneListItem, itemsEnumerator.Current.Value);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name1", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name2", itemsEnumerator.Current.Name);
            Assert.IsTrue(itemsEnumerator.MoveNext());
            Assert.AreEqual("name3", itemsEnumerator.Current.Name);
            Assert.IsFalse(itemsEnumerator.MoveNext());
        }

        [TestMethod]
        public void CanAddNumericPart()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "value", 10);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmNumericPart)partsEnumerator.Current).PartName);
            Assert.AreEqual(null, ((AdmNumericPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmNumericPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(10, ((AdmNumericPart)partsEnumerator.Current).DefaultValue);
            Assert.AreEqual(0, ((AdmNumericPart)partsEnumerator.Current).MinValue);
            Assert.AreEqual(null, ((AdmNumericPart)partsEnumerator.Current).MaxValue);
        }

        [TestMethod]
        public void CanAddNumericPartWithKeyName()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 10);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmNumericPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmNumericPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmNumericPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(10, ((AdmNumericPart)partsEnumerator.Current).DefaultValue);
            Assert.AreEqual(0, ((AdmNumericPart)partsEnumerator.Current).MinValue);
            Assert.AreEqual(null, ((AdmNumericPart)partsEnumerator.Current).MaxValue);
        }

        [TestMethod]
        public void CanAddNumericPartWithKeyNameAndMinMaxValues()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 150, 100, 200);
            builder.EndPolicy();
            builder.EndCategory();
            AdmContent content = builder.GetContent();

            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.AreEqual("part", ((AdmNumericPart)partsEnumerator.Current).PartName);
            Assert.AreEqual("key", ((AdmNumericPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual("value", ((AdmNumericPart)partsEnumerator.Current).ValueName);
            Assert.AreEqual(150, ((AdmNumericPart)partsEnumerator.Current).DefaultValue);
            Assert.AreEqual(100, ((AdmNumericPart)partsEnumerator.Current).MinValue);
            Assert.AreEqual(200, ((AdmNumericPart)partsEnumerator.Current).MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNumericPartWithMinValueBelowValidRangeThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 10, -1, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNumericPartWithMaxValueAboveValidRangeThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 10, 1, 1000000000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNumericPartWithMaxValueBelowMinValueThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 10, 5, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNumericPartWithDefaultValueBelowMinMaxRangeThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 0, 2, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNumericPartWithDefaultValueAboveMinMaxRangeThrows()
        {
            AdmContentBuilder builder = new AdmContentBuilder();

            builder.StartCategory("category");
            builder.StartPolicy("policy", "key");
            builder.AddNumericPart("part", "key", "value", 10, 2, 4);
        }
    }
}
