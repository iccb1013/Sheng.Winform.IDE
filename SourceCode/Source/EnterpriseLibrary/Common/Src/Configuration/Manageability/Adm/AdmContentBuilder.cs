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
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    /// <summary>
    /// Represents the process of building the contents of an ADM file.
    /// </summary>
    /// <remarks>
    /// During the building process categories and policies are started and ended: several levels of nested categories
    /// can be started, but only one level of policies are allowed. When a category or policy is started, it becomes the current
    /// until it is ended.
    /// Parts are added to the current policy; parts are not started and stopped.
    /// </remarks>
    public class AdmContentBuilder
    {
        /// <summary>
        /// String representing the name of the registry value that represents the availability of a configuration element.
        /// </summary>
        /// <remarks>
        /// The value names for policies are set to this name.
        /// </remarks>
        public const String AvailableValueName = RegistryKeyBase.PolicyValueName;

        const int MaxNumericConstraint = 999999999;
        const int MinNumericConstraint = 0;

        /// <summary>
        /// String representing the value for a <see langword="null"/> value in a drop down list. 
        /// </summary>
        public const String NoneListItem = "__none__";

        static readonly Regex identifierRe = new Regex(@"^[^""]*$");
        static readonly Regex keyRe = new Regex(@"^([^\""]+)(?:\\([^\""]+))*$");

        readonly Stack<AdmCategory> categoriesStack;
        readonly AdmContent content;
        AdmPolicy currentPolicy;

        /// <summary>
        /// Initialize a new instance of the <see cref="AdmContentBuilder"/> class.
        /// </summary>
        public AdmContentBuilder()
            : this(new AdmContent()) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmContentBuilder"/> class.
        /// </summary>
        /// <param name="content">The content that will be built.</param>
        protected AdmContentBuilder(AdmContent content)
        {
            this.content = content;
            categoriesStack = new Stack<AdmCategory>();
        }

        /// <summary>
        /// Adds a checkbox part to the current policy, with values for "on" and "off" states.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="checkedByDefault">Whether the new part should be checked by default.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddCheckboxPart(String partName,
                                    String valueName,
                                    bool checkedByDefault)
        {
            AddCheckboxPart(partName, valueName, checkedByDefault, true, true);
        }

        /// <summary>
        /// Adds a checkbox part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="checkedByDefault">Whether the new part should be checked by default.</param>
        /// <param name="valueForOn">Whether a value should be set for the checked state.</param>
        /// <param name="valueForOff">Whether a value should be set for the unchecked state.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddCheckboxPart(String partName,
                                    String valueName,
                                    bool checkedByDefault,
                                    bool valueForOn,
                                    bool valueForOff)
        {
            AddCheckboxPart(partName, null, valueName, checkedByDefault, valueForOn, valueForOff);
        }

        /// <summary>
        /// Adds a checkbox part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="checkedByDefault">Whether the new part should be checked by default.</param>
        /// <param name="valueForOn">Whether a value should be set for the checked state.</param>
        /// <param name="valueForOff">Whether a value should be set for the unchecked state.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddCheckboxPart(String partName,
                                    String keyName,
                                    String valueName,
                                    bool checkedByDefault,
                                    bool valueForOn,
                                    bool valueForOff)
        {
            CheckPartParameters(partName, keyName, valueName);

            AddPart(new AdmCheckboxPart(partName, keyName, valueName, checkedByDefault, valueForOn, valueForOff));
        }

        /// <summary>
        /// Adds a combo box part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <param name="maxlen">The max length for the new part's values</param>
        /// <param name="required">Whether values for the new part are mandatory.</param>
        /// <param name="suggestions">The suggested values for the new part.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddComboBoxPart(String partName,
                                    String valueName,
                                    String defaultValue,
                                    int maxlen,
                                    bool required,
                                    params String[] suggestions)
        {
            AddComboBoxPart(partName, null, valueName, defaultValue, maxlen, required, suggestions);
        }

        /// <summary>
        /// Adds a combo box part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <param name="maxlen">The max length for the new part's values</param>
        /// <param name="required">Whether values for the new part are mandatory.</param>
        /// <param name="suggestions">The suggested values for the new part.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddComboBoxPart(String partName,
                                    String keyName,
                                    String valueName,
                                    String defaultValue,
                                    int maxlen,
                                    bool required,
                                    params String[] suggestions)
        {
            maxlen = 0; // workaround for GPMC Settings bug
            CheckPartParameters(partName, keyName, valueName);
            CheckDefaultValue(defaultValue, maxlen, partName);
            CheckSuggestions(suggestions);
            CheckMaxlen(maxlen, partName);

            AddPart(new AdmComboBoxPart(partName, keyName, valueName, defaultValue, suggestions, maxlen, required));
        }

        /// <summary>
        /// Adds a new drop down list part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="items">The list of items to include in the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddDropDownListPart(String partName,
                                        String valueName,
                                        IEnumerable<AdmDropDownListItem> items,
                                        String defaultValue)
        {
            AddDropDownListPart(partName, null, valueName, items, defaultValue);
        }

        /// <summary>
        /// Adds a new drop down list part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="items">The list of items to include in the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddDropDownListPart(String partName,
                                        String keyName,
                                        String valueName,
                                        IEnumerable<AdmDropDownListItem> items,
                                        String defaultValue)
        {
            CheckPartParameters(partName, keyName, valueName);
            CheckDefaultValue(defaultValue, 255, partName);

            AddPart(new AdmDropDownListPart(partName, keyName, valueName, items, defaultValue));
        }

        /// <summary>
        /// Adds a new drop down list part to the current policy with items representing an enumeration's values.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddDropDownListPartForEnumeration<T>(String partName,
                                                         String valueName,
                                                         T defaultValue)
            where T : struct
        {
            AddDropDownListPartForEnumeration(partName, null, valueName, defaultValue);
        }

        /// <summary>
        /// Adds a new drop down list part to the current policy with items representing an enumeration's values.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddDropDownListPartForEnumeration<T>(String partName,
                                                         String keyName,
                                                         String valueName,
                                                         T defaultValue)
            where T : struct
        {
            List<AdmDropDownListItem> items = new List<AdmDropDownListItem>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                items.Add(new AdmDropDownListItem(value.ToString(), value.ToString()));
            }

            AddDropDownListPart(partName, keyName, valueName, items, defaultValue.ToString());
        }

        /// <summary>
        /// Adds a new drop down list part to the current policy with items representing the elements in a configuration 
        /// elements collection.
        /// </summary>
        /// <typeparam name="T">The base class for the configuration elements in the collection.</typeparam>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="elements">The collection of configuration elements.</param>
        /// <param name="defaultElementName">The name for the default element, or <see langword="null"/> if there is no such default name.</param>
        /// <param name="allowNone">Whether an additional entry to represent that no element is selected should be added.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        /// <devdoc>
        /// FxCop message CA1004 is supressed because it seems like the rule does not detect the
        /// existing 'elements' method parameter that uses the generic parameter T.
        /// </devdoc>
        [SuppressMessage("Microsoft.Design", "CA1004")]
        public void AddDropDownListPartForNamedElementCollection<T>(String partName,
                                                                    String valueName,
                                                                    NamedElementCollection<T> elements,
                                                                    String defaultElementName,
                                                                    bool allowNone)
            where T : NamedConfigurationElement, new()
        {
            AddDropDownListPartForNamedElementCollection(partName, null, valueName,
                                                            elements, defaultElementName, allowNone);
        }

        /// <summary>
        /// Adds a new drop down list part to the current policy with items representing the elements in a configuration 
        /// elements collection.
        /// </summary>
        /// <typeparam name="T">The base class for the configuration elements in the collection.</typeparam>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="elements">The collection of configuration elements.</param>
        /// <param name="defaultElementName">The name for the default element, or <see langword="null"/> if there is no such default name.</param>
        /// <param name="allowNone">Whether an additional entry to represent that no element is selected should be added.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        /// <devdoc>
        /// FxCop message CA1004 is supressed because it seems like the rule does not detect the
        /// existing 'elements' method parameter that uses the generic parameter T.
        /// </devdoc>
        [SuppressMessage("Microsoft.Design", "CA1004")]
        public void AddDropDownListPartForNamedElementCollection<T>(String partName,
                                                                    String keyName,
                                                                    String valueName,
                                                                    NamedElementCollection<T> elements,
                                                                    String defaultElementName,
                                                                    bool allowNone)
            where T : NamedConfigurationElement, new()
        {
            List<AdmDropDownListItem> items = new List<AdmDropDownListItem>();

            if (allowNone)
            {
                items.Add(new AdmDropDownListItem(Resources.NoneListItem, NoneListItem));
            }
            foreach (T element in elements)
            {
                items.Add(new AdmDropDownListItem(element.Name, element.Name));
            }

            AddDropDownListPart(partName, keyName, valueName, items,
                                !String.IsNullOrEmpty(defaultElementName) ? defaultElementName : Resources.NoneListItem);
        }

        /// <summary>
        /// Adds a new edit text part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <param name="maxlen">The max length for the new part's values</param>
        /// <param name="required">Whether values for the new part are mandatory.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddEditTextPart(String partName,
                                    String valueName,
                                    String defaultValue,
                                    int maxlen,
                                    bool required)
        {
            AddEditTextPart(partName, null, valueName, defaultValue, maxlen, required);
        }

        /// <summary>
        /// Adds a new edit text part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <param name="maxlen">The max length for the new part's values</param>
        /// <param name="required">Whether values for the new part are mandatory.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddEditTextPart(String partName,
                                    String keyName,
                                    String valueName,
                                    String defaultValue,
                                    int maxlen,
                                    bool required)
        {
            CheckPartParameters(partName, keyName, valueName);
            CheckDefaultValue(defaultValue, maxlen, partName);
            CheckMaxlen(maxlen, partName);

            AddPart(new AdmEditTextPart(partName, keyName, valueName, defaultValue, maxlen, required));
        }

        /// <summary>
        /// Adds a new numeric part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddNumericPart(String partName,
                                   String valueName,
                                   int? defaultValue)
        {
            AddNumericPart(partName, null, valueName, defaultValue, 0, null);
        }

        /// <summary>
        /// Adds a new numeric part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/> if there is no such default value.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddNumericPart(String partName,
                                   String keyName,
                                   String valueName,
                                   int? defaultValue)
        {
            AddNumericPart(partName, keyName, valueName, defaultValue, 0, null);
        }

        /// <summary>
        /// Adds a new numeric part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <param name="keyName">The registry key for the part, to override its policy's key.</param>
        /// <param name="valueName">The value name for the new part.</param>
        /// <param name="defaultValue">The default value for the new part, or <see langword="null"/>
        /// if there is no such default value.</param>
        /// <param name="minValue">The minimum value, or <see langword="null"/>
        /// if there is no minimum value.</param>
        /// <param name="maxValue">The maximum value, or <see langword="null"/>
        /// if there is no maximum value.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddNumericPart(String partName,
                                   String keyName,
                                   String valueName,
                                   int? defaultValue,
                                   int? minValue,
                                   int? maxValue)
        {
            CheckPartParameters(partName, keyName, valueName);
            CheckMinMaxValues(minValue, maxValue, partName);
            CheckDefaultNumericValue(defaultValue, minValue, maxValue, partName);

            AddPart(new AdmNumericPart(partName, keyName, valueName, defaultValue, minValue, maxValue));
        }

        /// <summary>
        /// Add a part to the builder.
        /// </summary>
        /// <param name="part">
        /// The <see cref="AdmPart"/> to add.
        /// </param>
        public void AddPart(AdmPart part)
        {
            CheckStartedPolicy();

            currentPolicy.AddPart(part);
        }

        /// <summary>
        /// Adds a new text part to the current policy.
        /// </summary>
        /// <param name="partName">The name for the new part.</param>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void AddTextPart(String partName)
        {
            CheckPartName(partName);

            AddPart(new AdmTextPart(partName));
        }

        static void CheckCategoryName(string categoryName)
        {
            CheckName(categoryName, "categoryName");
        }

        void CheckCurrentCategory()
        {
            if (categoriesStack.Count == 0)
            {
                throw new InvalidOperationException(Resources.ExceptionAdmBuildingNoCurrentCategory);
            }
        }

        void CheckDefaultNumericValue(int? defaultValue,
                                      int? minValue,
                                      int? maxValue,
                                      String partName)
        {
            if (defaultValue != null)
            {
                if (minValue != null && defaultValue < minValue)
                {
                    throw new ArgumentException(
                        String.Format(Resources.Culture,
                                      Resources.ExceptionAdmDefaultValueBelowMinValue,
                                      partName,
                                      currentPolicy.Name,
                                      defaultValue,
                                      minValue),
                        "defaultValue");
                }
                if (maxValue != null && defaultValue > maxValue)
                {
                    throw new ArgumentException(
                        String.Format(Resources.Culture,
                                      Resources.ExceptionAdmDefaultValueAboveMaxValue,
                                      partName,
                                      currentPolicy.Name,
                                      defaultValue,
                                      maxValue),
                        "defaultValue");
                }
            }
        }

        void CheckDefaultValue(string defaultValue,
                               int maxlen,
                               string partName)
        {
            if (defaultValue == null)
            {
                throw new ArgumentNullException("defaultValue");
            }
            if (!identifierRe.IsMatch(defaultValue))
            {
                throw new ArgumentException(String.Format(Resources.Culture, Resources.ExceptionAdmInvalidDefaultValue, defaultValue),
                                            "defaultValue");
            }
            if (defaultValue.Length > (maxlen != 0 ? maxlen : 1024))
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmDefaultValueLongerThanMaxlen,
                                  partName,
                                  currentPolicy.Name,
                                  maxlen),
                    "defaultValue");
            }
        }

        static void CheckKeyName(string policyKey)
        {
            CheckKeyName(policyKey, false);
        }

        static void CheckKeyName(string policyKey,
                                 bool keyMustBeNotNull)
        {
            if (policyKey == null)
            {
                if (keyMustBeNotNull)
                {
                    throw new ArgumentNullException("policyKey");
                }
                else
                {
                    return;
                }
            }

            Match keyMatch = keyRe.Match(policyKey);
            if (!keyMatch.Success)
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmInvalidCharactersInRegistryKey,
                                  policyKey),
                    "policyKey");
            }
            for (int i = 1; i < keyMatch.Groups.Count; i++) // start at 1, the first group is the whole RE.
            {
                Group group = keyMatch.Groups[i];
                for (int j = 0; j < group.Captures.Count; j++)
                {
                    String key = group.Captures[j].Value;
                    if (key.Length > 255)
                    {
                        throw new ArgumentException(
                            String.Format(Resources.Culture,
                                          Resources.ExceptionAdmRegistryKeyPathSegmentTooLong,
                                          key),
                            "policyKey");
                    }
                }
            }
        }

        void CheckMaxlen(int maxlen,
                         string partName)
        {
            if (maxlen < 0 || maxlen > 1024)
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmInvalidMaxlen,
                                  partName,
                                  currentPolicy.Name,
                                  maxlen),
                    "maxlen");
            }
        }

        void CheckMinMaxValues(int? minValue,
                               int? maxValue,
                               String partName)
        {
            if (minValue != null && (minValue < MinNumericConstraint || minValue > MaxNumericConstraint))
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmNumericConstraintOutsideRange,
                                  partName,
                                  currentPolicy.Name,
                                  minValue),
                    "minValue");
            }
            if (maxValue != null && (maxValue < MinNumericConstraint || maxValue > MaxNumericConstraint))
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmNumericConstraintOutsideRange,
                                  partName,
                                  currentPolicy.Name,
                                  maxValue),
                    "maxValue");
            }
            if (minValue != null && maxValue != null && minValue.Value > maxValue.Value)
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmMinValueLargerThanMaxValue,
                                  partName,
                                  currentPolicy.Name,
                                  minValue,
                                  maxValue),
                    "maxValue");
            }
        }

        static void CheckName(string name,
                              string parameterName)
        {
            if (name == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (name.Length == 0 || !identifierRe.IsMatch(name))
            {
                throw new ArgumentException(String.Format(Resources.Culture,
                                                          Resources.ExceptionAdmInvalidName,
                                                          name),
                                            parameterName);
            }
        }

        void CheckNoStartedPolicy()
        {
            if (currentPolicy != null)
            {
                throw new InvalidOperationException(Resources.ExceptionAdmBuildingPolicyNotFinished);
            }
        }

        static void CheckPartName(string partName)
        {
            CheckName(partName, "partName");
        }

        static void CheckPartParameters(String partName,
                                        String keyName,
                                        String valueName)
        {
            CheckPartName(partName);
            CheckKeyName(keyName);
            CheckValueName(valueName);
        }

        static void CheckPolicyName(string policyName)
        {
            CheckName(policyName, "policyName");
        }

        void CheckStartedPolicy()
        {
            if (currentPolicy == null)
            {
                throw new InvalidOperationException(Resources.ExceptionAdmBuildingNoCurrentPolicy);
            }
        }

        static void CheckSuggestions(IEnumerable<string> suggestions)
        {
            foreach (String suggestion in suggestions)
            {
                if (!identifierRe.IsMatch(suggestion))
                {
                    throw new ArgumentException(
                        String.Format(Resources.Culture,
                                      Resources.ExceptionAdmInvalidSuggestion,
                                      suggestion),
                        "suggestions");
                }
            }
        }

        static void CheckValueName(string valueName)
        {
            CheckName(valueName, "valueName");
            if (valueName.Length > 255)
            {
                throw new ArgumentException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionAdmRegistryValueNameTooLong,
                                  valueName),
                    "valueName");
            }
        }

        /// <summary>
        /// Ends the current category.
        /// </summary>
        /// <remarks>
        /// If the current category has a parent category, the parent category is made the current category.
        /// </remarks>
        /// <exception cref="InvalidOperationException">when there is no current category.</exception>
        /// <exception cref="InvalidOperationException">when there is an unfinished policy being built.</exception>
        public void EndCategory()
        {
            CheckCurrentCategory();
            CheckNoStartedPolicy();

            AdmCategory currentCategory = categoriesStack.Pop();
            if (categoriesStack.Count == 0)
            {
                content.AddCategory(currentCategory);
            }
            else
            {
                categoriesStack.Peek().AddCategory(currentCategory);
            }
        }

        /// <summary>
        /// Ends the current policy.
        /// </summary>
        /// <exception cref="InvalidOperationException">when there is no current policy.</exception>
        public void EndPolicy()
        {
            CheckStartedPolicy();

            categoriesStack.Peek().AddPolicy(currentPolicy);
            currentPolicy = null;
        }

        /// <summary>
        /// Gets the content for the builder.
        /// </summary>
        /// <returns>
        /// A <see cref="AdmContent"/> object.
        /// </returns>
        public AdmContent GetContent()
        {
            if (categoriesStack.Count > 0)
            {
                throw new InvalidOperationException(Resources.ExceptionAdmBuildingProcessNotFinished);
            }
            return content;
        }

        /// <summary>
        /// Starts a new category in the built content and makes it the current category.
        /// </summary>
        /// <param name="categoryName">The name for the new category</param>
        /// <remarks>
        /// The category is created as a child of the current category, or as a top level category if there is
        /// no such current category.
        /// </remarks>
        /// <exception cref="InvalidOperationException">when there is an unfinished policy being built.</exception>
        public void StartCategory(String categoryName)
        {
            CheckCategoryName(categoryName);
            CheckNoStartedPolicy();

            categoriesStack.Push(new AdmCategory(categoryName));
        }

        /// <summary>
        /// Starts a new policy on the current category.
        /// </summary>
        /// <param name="policyName">The name for the new policy.</param>
        /// <param name="policyKey">The registry key for the new policy.</param>
        /// <exception cref="InvalidOperationException">when there is no current category.</exception>
        /// <exception cref="InvalidOperationException">when there is an unfinished policy being built.</exception>
        public void StartPolicy(String policyName,
                                String policyKey)
        {
            CheckPolicyName(policyName);
            CheckKeyName(policyKey, true);
            CheckCurrentCategory();
            CheckNoStartedPolicy();

            currentPolicy = new AdmPolicy(policyName, policyKey, AvailableValueName);
        }
    }
}
