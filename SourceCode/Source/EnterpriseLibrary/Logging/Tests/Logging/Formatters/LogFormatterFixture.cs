//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
	[TestClass]
	public class LogFormatterFixture
	{
		IBuilderContext context;
		ConfigurationReflectionCache reflectionCache;

		[TestInitialize]
		public void SetUp()
		{
			AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
			context = new BuilderContext(new StrategyChain(), null, null, new PolicyList(), null, null);
			reflectionCache = new ConfigurationReflectionCache();
		}

		[TestMethod]
		public void FormatsWithNoTokens()
		{
			TextFormatter formatter = new TextFormatter("no tokens");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("no tokens", actual);
		}

		[TestMethod]
		public void FormatsWithSingleConstant()
		{
			TextFormatter formatter = new TextFormatter("{newline}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual(Environment.NewLine, actual);
		}

		[TestMethod]
		public void FormatsWithTwoConstants()
		{
			TextFormatter formatter = new TextFormatter("{newline}{tab}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual(Environment.NewLine + "\t", actual);
		}

		[TestMethod]
		public void FormatsMixedTextAndTokens()
		{
			TextFormatter formatter = new TextFormatter("some {newline} mixed{tab}text");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("some " + Environment.NewLine + " mixed\ttext", actual);
		}

		[TestMethod]
		public void FormatsMessageToken()
		{
			TextFormatter formatter = new TextFormatter("Message: {message}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Message: " + entry.Message, actual);
		}

		[TestMethod]
		public void FormatsPriorityToken()
		{
			TextFormatter formatter = new TextFormatter("Priority: {priority}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Priority: " + entry.Priority.ToString(CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsEventIdToken()
		{
			TextFormatter formatter = new TextFormatter("EventId: {eventid}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("EventId: " + entry.EventId.ToString(CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsSeverityToken()
		{
			TextFormatter formatter = new TextFormatter("Severity: {severity}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Severity: " + entry.Severity.ToString(), actual);
		}

		[TestMethod]
		public void FormatsTitleToken()
		{
			TextFormatter formatter = new TextFormatter("Title: {title}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Title: " + entry.Title, actual);
		}

		[TestMethod]
		public void FormatsMachineToken()
		{
			TextFormatter formatter = new TextFormatter("Machine name: {machine}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Machine name: " + entry.MachineName, actual);
		}

		[TestMethod]
		public void FormatsAppDomainToken()
		{
			TextFormatter formatter = new TextFormatter("App domain: {appDomain}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("App domain: " + entry.AppDomainName, actual);
		}

		[TestMethod]
		public void FormatsProcessIdToken()
		{
			TextFormatter formatter = new TextFormatter("Process id: {processId}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Process id: " + entry.ProcessId, actual);
		}

		[TestMethod]
		public void FormatsProcessNameToken()
		{
			TextFormatter formatter = new TextFormatter("Process name: {processName}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Process name: " + entry.ProcessName, actual);
		}

		[TestMethod]
		public void FormatsThreadNameToken()
		{
			TextFormatter formatter = new TextFormatter("Thread name: {threadName}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Thread name: " + entry.ManagedThreadName, actual);
		}

		[TestMethod]
		public void FormatsThreadIdToken()
		{
			TextFormatter formatter = new TextFormatter("Thread id: {win32ThreadId}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Thread id: " + entry.Win32ThreadId, actual);
		}

		[TestMethod]
		public void FormatsActivityIdToken()
		{
			TextFormatter formatter = new TextFormatter("Activity id: {activity}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Activity id: " + entry.ActivityId.ToString("D", CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsErrorMessagesToken()
		{
			TextFormatter formatter = new TextFormatter("Errors: {errorMessages}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.AddErrorMessage("an error message");

			string actual = formatter.Format(entry);

			Assert.AreEqual("Errors: " + entry.ErrorMessages, actual);
		}

		[TestMethod]
		public void FormatsCategoryToken()
		{
			TextFormatter formatter = new TextFormatter("Categories: {category}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Categories: " + TextFormatter.FormatCategoriesCollection(entry.Categories), actual);
		}

		[TestMethod]
		public void FormatsTimestampWithNoFormat()
		{
			TextFormatter formatter = new TextFormatter("Timestamp: {timestamp}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Timestamp: " + entry.TimeStamp.ToString(CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsTimestampWithCustomFormat()
		{
			TextFormatter formatter = new TextFormatter("Timestamp: {timestamp(dd-MM-yyyy hh:mm)}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Timestamp: " + entry.TimeStamp.ToString("dd-MM-yyyy hh:mm", CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsLocalTimestamp()
		{
			TextFormatter formatter = new TextFormatter("Timestamp: {timestamp(local)}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Timestamp: " + entry.TimeStamp.ToLocalTime().ToString(CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsLocalTimestampWithCustomFormat()
		{
			TextFormatter formatter = new TextFormatter("Timestamp: {timestamp(local:dd-MM-yyyy hh:mm)}");
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Timestamp: " + entry.TimeStamp.ToLocalTime().ToString("dd-MM-yyyy hh:mm", CultureInfo.CurrentCulture), actual);
		}

		[TestMethod]
		public void FormatsPropertyToken()
		{
			TextFormatter formatter = new TextFormatter("Reflected Property Value: {property(MyProperty)}");
			CustomLogEntry entry = new CustomLogEntry();

			string actual = formatter.Format(entry);

			Assert.AreEqual("Reflected Property Value: myPropertyValue", actual);
		}

		[TestMethod]
		public void DictionaryTokenCanHandleInternalParenthesisAsLongAsTheyAreNotFollowedByACurlyBracket()
		{
			TextFormatter formatter = new TextFormatter("{dictionary(({key})-{value} - )}");
			CustomLogEntry entry = new CustomLogEntry();
			Dictionary<string, object> hash = new Dictionary<string, object>();
			hash["key1"] = "value1";
			hash["key2"] = "value2";
			entry.ExtendedProperties = hash;

			string actual = formatter.Format(entry);

			Assert.AreEqual("(key1)-value1 - (key2)-value2 - ", actual);
		}

		// previous tests

		[TestMethod]
		public void ApplyTextFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = FormatEntry("{timestamp}: {title} - {message}", entry);
			string expected = entry.TimeStampString + ": " + entry.Title + " - " + entry.Message;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ApplyTextXmlFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			string template = "<EntLibLog><message>{message}</message><timestamp>{timestamp}</timestamp><title>{title}</title></EntLibLog>";
			string actual = FormatEntry(template, entry);

			string expected = "<EntLibLog><message>My message body</message><timestamp>" + DateTime.Parse("12/31/9999 11:59:59 PM", CultureInfo.InvariantCulture).ToString() + "</timestamp><title>=== Header ===</title></EntLibLog>";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TextFormatExtendedProperties()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "DictionaryCategory" };
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "{timestamp}: {title} - {message}{newline}{dictionary({key} = {value}{newline})}";
			string actual = FormatEntry(template, entry);

			string expected = DateTime.Parse("12/31/9999 11:59:59 PM", CultureInfo.InvariantCulture).ToString() + ": === Header === - My message body";
			Assert.IsTrue(actual.IndexOf(expected) > -1);
			Assert.IsTrue(actual.IndexOf("key1") > -1);
			Assert.IsTrue(actual.IndexOf("value1") > -1);
			Assert.IsTrue(actual.IndexOf("key2") > -1);
			Assert.IsTrue(actual.IndexOf("value2") > -1);
			Assert.IsTrue(actual.IndexOf("key3") > -1);
			Assert.IsTrue(actual.IndexOf("value3") > -1);
		}

		[TestMethod]
		public void TextFormatMutipleExtendedProperties()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "DictionaryCategory" };
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "{timestamp}: {title} - {message}{newline}" +
							  "Dictionary 1:{newline}{dictionary(((({key} =-= {value}{newline}))))}{newline}{newline}" +
							  "Dictionary 1 reformatted:{newline}{dictionary([[{key} === {value}{newline}]])}";
			string actual = FormatEntry(template, entry);

			Assert.IsTrue(actual.IndexOf("Dictionary 1:") > -1);
			Assert.IsTrue(actual.IndexOf("(((key1 =-= value1\r\n)))") > -1);
			Assert.IsTrue(actual.IndexOf("Dictionary 1 reformatted:") > -1);
			Assert.IsTrue(actual.IndexOf("[[key1 === value1\r\n]]") > -1);
		}

		[TestMethod]
		public void KeyValuePairExtendedPropertiesFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "DictionaryCategory" };
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "{timestamp}: {title} - {message}{newline}" +
							  "{dictionary({key} = {value}{newline})}{newline}" +
							  "== KeyValue Pair Format Function =={newline}" +
							  "Key1 = {keyvalue(key1)}{newline}" +
							  "Key2 = {keyvalue(key2)}{newline}" +
							  "Key3 = {keyvalue(key3)}";
			string actual = FormatEntry(template, entry);

			Assert.IsTrue(actual.IndexOf("== KeyValue Pair Format Function ==") > -1);
		}

		[TestMethod]
		public void TimestampFormat()
		{
			string template = "Time is: {timestamp(D)}";
			string actual = FormatEntry(template, CommonUtil.GetDefaultLogEntry());

			string expected = string.Format("Time is: {0}", DateTime.Parse("9999-12-31", CultureInfo.InvariantCulture).ToString("D"));
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimestampTokenWithEmptyTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string template = "Time is: {timestamp()}";
			string actual = FormatEntry(template, entry);

			string expected = "Time is: " + DateTime.Parse("12/31/9999 11:59:59 PM", CultureInfo.InvariantCulture).ToString();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DictionaryTokenWithEmptyTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Dictionary: {dictionary()} value";
			string actual = FormatEntry(template, entry);

			string expected = "Dictionary:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void KeyValueTokenWithEmptyTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Key is: {keyvalue()} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void KeyValueTokenWithInvalidTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Key is: {keyvalue(INVALIDKEY)} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void KeyValueTokenWithMissingDictionary()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			// do not set extended properties field

			string template = "Key is: {keyvalue(key1)} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DictionaryTokenWithMissingDictionary()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			// do not set extended properties field

			string template = "Key is: {dictionary({key} - {value}\n)} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimestampCustomFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string template = "Time is: {timestamp(MM - dd - yyyy @ hh:mm:ss)}";
			string actual = FormatEntry(template, entry);

			string expected = "Time is: 12 - 31 - 9999 @ 11:59:59";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MultipleTimestampCustomFormats()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string template = "Month: {timestamp(MM )}, Day: {timestamp(dd )}, Year: {timestamp(yyyy )}";
			string actual = FormatEntry(template, entry);

			string expected = "Month: 12 , Day: 31 , Year: 9999 ";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MultipleCustomFormats()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Key1 = \"{keyvalue(key1)}\", Month: {timestamp(MM )}, Day: {timestamp(dd )}, Year: {timestamp(yyyy )}";
			string actual = FormatEntry(template, entry);
			string expected = "Key1 = \"value1\", Month: 12 , Day: 31 , Year: 9999 ";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimestampFormatWithInvalidFormatString()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = FormatEntry("Time is: {timestamp(INVALIDFORMAT)}", entry);

			string expected = "Time is: INVALID";
			Assert.IsTrue(actual.IndexOf(expected) > -1);
		}

		[TestMethod]
		public void TextFormatterWillNotAddErrorMessagesIfTokenIsNotPresent()
		{
			string errorMessage = "ERROR MESSAGE";
			string template = "Message: {message}";

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.AddErrorMessage(errorMessage);
			string actual = FormatEntry(template, entry);

			Assert.IsFalse(actual.IndexOf(errorMessage) > -1);
		}

		[TestMethod]
		public void TextFormatterWillNotAddErrorMessagesIfTokenIsPresentButEntryHasNoMessages()
		{
			string errorMessage = "ERROR MESSAGE";
			string template = "Message: {message}{newline}Errors: {errorMessages}";

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			string actual = FormatEntry(template, entry);

			Assert.IsFalse(actual.IndexOf(errorMessage) > -1);
		}

		[TestMethod]
		public void TextFormatterWillAddErrorMessagesIfTokenIsPresentAndEntryMessages()
		{
			string errorMessage = "ERROR MESSAGE";
			string template = "Message: {message}{newline}Errors: {errorMessages}";

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.AddErrorMessage(errorMessage);
			string actual = FormatEntry(template, entry);

			Assert.IsTrue(actual.IndexOf(errorMessage) > -1);
		}

		[TestMethod]
		public void TextFromatterWithoutTemplateUsesDefaultTemplate()
		{
			TextFormatter formatter = new TextFormatter();

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Title = Guid.NewGuid().ToString();
			entry.AppDomainName = Guid.NewGuid().ToString();
			entry.MachineName = Guid.NewGuid().ToString();
			entry.ManagedThreadName = Guid.NewGuid().ToString();
			entry.Message = Guid.NewGuid().ToString();
			string category = Guid.NewGuid().ToString();
			entry.Categories = new string[] { category };
			entry.ProcessName = Guid.NewGuid().ToString();

			string formattedMessage = formatter.Format(entry);

			Assert.IsTrue(formattedMessage.IndexOf(entry.AppDomainName) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.MachineName) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.ManagedThreadName) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.Message) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(category) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.ProcessName) != -1);
		}

		[TestMethod]
		public void TextFromatterWithEmptyTemplateUsesDefaultTemplate()
		{
			TextFormatter formatter = new TextFormatter("");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Title = Guid.NewGuid().ToString();
			entry.AppDomainName = Guid.NewGuid().ToString();
			entry.MachineName = Guid.NewGuid().ToString();
			entry.ManagedThreadName = Guid.NewGuid().ToString();
			entry.Message = Guid.NewGuid().ToString();
			string category = Guid.NewGuid().ToString();
			entry.Categories = new string[] { category };
			entry.ProcessName = Guid.NewGuid().ToString();

			string formattedMessage = formatter.Format(entry);

			Assert.IsTrue(formattedMessage.IndexOf(entry.AppDomainName) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.MachineName) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.ManagedThreadName) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.Message) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(category) != -1);
			Assert.IsTrue(formattedMessage.IndexOf(entry.ProcessName) != -1);
		}

		string FormatEntry(string template,
						   LogEntry entry)
		{
			TextFormatter formatter = new TextFormatter(template);
			return formatter.Format(entry);
		}

		// this is an obsolete test. Custom functions are still allowed, but the preferred
		// mechanism is to define a token handler/formatter combo
		[TestMethod]
		public void FormatCustomTokenFunction()
		{
			CustomLogEntry entry = new CustomLogEntry();

			ILogFormatter formatter = new CustomTextFormatter("Acme custom token template: [[AcmeDBLookup{value1}]]");
			string actual = formatter.Format(entry);

			string expected = "Acme custom token template: 1234";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FormatReflectedPropertyTokenFunctionPropertyFoundAndValue()
		{
			CustomLogEntry entry = new CustomLogEntry();

			ILogFormatter formatter = new TextFormatter("Reflected Property Value: {property(MyProperty)}");
			string actual = formatter.Format(entry);

			string expected = "Reflected Property Value: myPropertyValue";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FormatReflectedPropertyTokenFunctionPropertyFoundAndNullValue()
		{
			CustomLogEntry entry = new CustomLogEntry();

			ILogFormatter formatter = new TextFormatter("Reflected Property Value: {property(MyPropertyThatReturnsNull)}");
			string actual = formatter.Format(entry);

			string expected = "Reflected Property Value: ";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimeStampTokenUtcTime()
		{
			CustomLogEntry entry = new CustomLogEntry();
			entry.TimeStamp = DateTime.MaxValue;

			ILogFormatter formatter = new TextFormatter("TimeStamp: {timestamp}");
			string actual = formatter.Format(entry);

			string expected = string.Concat("TimeStamp: " + DateTime.MaxValue.ToString());
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimeStampTokenLocalTime()
		{
			CustomLogEntry entry = new CustomLogEntry();
			entry.TimeStamp = DateTime.MaxValue;

			ILogFormatter formatter = new TextFormatter("TimeStamp: {timestamp(local)}");
			string actual = formatter.Format(entry);

			string expected = string.Concat("TimeStamp: " + DateTime.MaxValue.ToLocalTime().ToString());
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimeStampTokenLocalTimeWithFormat()
		{
			CustomLogEntry entry = new CustomLogEntry();
			entry.TimeStamp = DateTime.MaxValue;
			ILogFormatter formatter = new TextFormatter("TimeStamp: {timestamp(local:F)}");
			string actual = formatter.Format(entry);

			string expected = string.Concat("TimeStamp: " + DateTime.MaxValue.ToLocalTime().ToString("F", CultureInfo.CurrentCulture));
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FormatterIsReusableBug1769()
		{
			ILogFormatter formatter = new TextFormatter("{message}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			entry.Message = "message1";
			Assert.AreEqual(entry.Message, formatter.Format(entry));

			entry.Message = "message2";
			Assert.AreEqual(entry.Message, formatter.Format(entry));

			entry.Message = "message3";
			Assert.AreEqual(entry.Message, formatter.Format(entry));
		}

		[TestMethod]
		public void TextFormatterHandlesCategoriesForSingleCategoryBug1816()
		{
			ILogFormatter formatter = new TextFormatter("{category}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "category1" };

			Assert.AreEqual("category1", formatter.Format(entry));
		}

		[TestMethod]
		public void TextFormatterHandlesCategoriesForTwoCategoriesBug1816()
		{
			ILogFormatter formatter = new TextFormatter("{category}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "category1", "category2" };

			Assert.AreEqual("category1, category2", formatter.Format(entry));
		}

		[TestMethod]
		public void TextFormatterHandlesCategoriesForManyTwoCategoriesBug1816()
		{
			ILogFormatter formatter = new TextFormatter("{category}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "category1", "category2", "category3", "category4" };

			Assert.AreEqual("category1, category2, category3, category4", formatter.Format(entry));
		}

		[TestMethod]
		public void CanCreateFormatterFromFactory()
		{
			FormatterData data = new TextFormatterData("ignore", "template");
			LoggingSettings settings = new LoggingSettings();
			settings.Formatters.Add(data);
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, settings);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, "ignore", configurationSource, reflectionCache);

			Assert.IsNotNull(formatter);
			Assert.AreEqual(formatter.GetType(), typeof(TextFormatter));
			Assert.AreEqual("template", ((TextFormatter)formatter).Template);
		}

		[TestMethod]
		public void CanCreateTextFormatterFromUnnamedDataBug1883()
		{
			string template = "{message}";
			TextFormatterData data = new TextFormatterData(template);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, data, null, reflectionCache);

			Assert.AreSame(typeof(TextFormatter), formatter.GetType());
			Assert.AreEqual(template, ((TextFormatter)formatter).Template);
		}

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			LoggingSettings rwLoggingSettings = new LoggingSettings();
			rwLoggingSettings.Formatters.Add(new TextFormatterData("formatter1", "template1"));
			rwLoggingSettings.Formatters.Add(new TextFormatterData("formatter2", "template2"));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[LoggingSettings.SectionName] = rwLoggingSettings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

			Assert.AreEqual(2, roLoggingSettings.Formatters.Count);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter1"));
			Assert.AreSame(typeof(TextFormatterData), roLoggingSettings.Formatters.Get("formatter1").GetType());
			Assert.AreSame(typeof(TextFormatter), roLoggingSettings.Formatters.Get("formatter1").Type);
			Assert.AreEqual("template1", ((TextFormatterData)roLoggingSettings.Formatters.Get("formatter1")).Template);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter2"));
			Assert.AreSame(typeof(TextFormatterData), roLoggingSettings.Formatters.Get("formatter2").GetType());
			Assert.AreSame(typeof(TextFormatter), roLoggingSettings.Formatters.Get("formatter2").Type);
			Assert.AreEqual("template2", ((TextFormatterData)roLoggingSettings.Formatters.Get("formatter2")).Template);
		}
	}
}
