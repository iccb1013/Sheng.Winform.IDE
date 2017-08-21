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
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	/// <summary>
	/// Represents a template based formatter for <see cref="LogEntry"/> messages.
	/// </summary>
	/// <remarks>
	/// The <see cref="TextFormatter"/> uses a <see cref="GenericTextFormatter{T}"/> initialized with a set of
	/// <see cref="TokenHandler{T}"/> that can manage the default template tokens. Subclasses can supply extra
	/// token handlers that will be added to the default set for additional template processing.
	/// </remarks>
	/// <seealso cref="GenericTextFormatter{T}"/>
	/// <seealso cref="TokenHandler{T}"/>
	/// <seealso cref="Formatter{T}"/>
	[ConfigurationElementType(typeof(TextFormatterData))]
	public class TextFormatter : LogFormatter
	{
		private const string TimestampLocalStartDelimiter = "local";
		private const string TimestampLocalStartDelimiterWithFormat = "local:";

		private readonly static Dictionary<string, TokenHandler<LogEntry>> defaultTokenHandlers;
		private readonly static Dictionary<string, TokenHandler<LogEntry>> emptyExtraTokenHandlers;

		private string template;
		private GenericTextFormatter<LogEntry> formatter;

		/// <summary>
		/// Sets up the default token handlers for the <see cref="TextFormatter"/>;
		/// </summary>
		static TextFormatter()
		{
			defaultTokenHandlers = new Dictionary<string, TokenHandler<LogEntry>>();
			emptyExtraTokenHandlers = new Dictionary<string, TokenHandler<LogEntry>>();

			// constants
			defaultTokenHandlers["newline"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(Environment.NewLine);
			defaultTokenHandlers["tab"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler("\t");

			// simple properties on log entry
			defaultTokenHandlers["message"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.Message);
			defaultTokenHandlers["category"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => FormatCategoriesCollection(le.Categories));
			defaultTokenHandlers["priority"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.Priority.ToString(CultureInfo.CurrentCulture));
			defaultTokenHandlers["eventid"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.EventId.ToString(CultureInfo.CurrentCulture));
			defaultTokenHandlers["severity"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.Severity.ToString());
			defaultTokenHandlers["title"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.Title);
			defaultTokenHandlers["errorMessages"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.ErrorMessages);

			defaultTokenHandlers["machine"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.MachineName);
			defaultTokenHandlers["appDomain"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.AppDomainName);
			defaultTokenHandlers["processId"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.ProcessId);
			defaultTokenHandlers["processName"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.ProcessName);
			defaultTokenHandlers["threadName"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.ManagedThreadName);
			defaultTokenHandlers["win32ThreadId"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.Win32ThreadId);
			defaultTokenHandlers["activity"]
				= GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => le.ActivityId.ToString("D", CultureInfo.CurrentCulture));

			// parameterized tokens
			defaultTokenHandlers["timestamp"]
				= GenericTextFormatter<LogEntry>.CreateParameterizedTokenHandler(TimestampFormatterFactory);
			defaultTokenHandlers["property"]
				= GenericTextFormatter<LogEntry>.CreateParameterizedTokenHandler(ReflectedPropertyFormatterFactory);
			defaultTokenHandlers["keyvalue"]
				= GenericTextFormatter<LogEntry>.CreateParameterizedTokenHandler(KeyValueFormatterFactory);
			defaultTokenHandlers["dictionary"]
				= GenericTextFormatter<LogEntry>.CreateParameterizedTokenHandler(DictionaryTokenHandlerFactory);
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="TextFormatter"></see> with a default template.
		/// </summary>
		public TextFormatter()
			: this(Resources.DefaultTextFormat)
		{ }

		/// <summary>
		/// Initializes a new instance of a <see cref="TextFormatter"> with a template and no extra token handlers.</see>
		/// </summary>
		/// <param name="template">Template to be used when formatting.</param>
		public TextFormatter(string template)
			: this(template, emptyExtraTokenHandlers)
		{ }

		/// <summary>
		/// Initializes a new instance of a <see cref="TextFormatter"> with a template and additional token handlers.</see>
		/// </summary>
		/// <param name="template">Template to be used when formatting.</param>
		/// <param name="extraTokenHandlers">The additional token handlers to use when processing the template.</param>
		protected TextFormatter(string template, IDictionary<string, TokenHandler<LogEntry>> extraTokenHandlers)
		{
			if (!string.IsNullOrEmpty(template))
			{
				this.template = template;
			}
			else
			{
				this.template = Resources.DefaultTextFormat;
			}

			Dictionary<string, TokenHandler<LogEntry>> tokenHandlers = defaultTokenHandlers;
			if (extraTokenHandlers.Count > 0)
			{
				tokenHandlers = new Dictionary<string, TokenHandler<LogEntry>>(defaultTokenHandlers);
				foreach (var kvp in extraTokenHandlers)
				{
					tokenHandlers.Add(kvp.Key, kvp.Value);
				}
			}

			this.formatter = new GenericTextFormatter<LogEntry>(this.template, tokenHandlers);
		}

		/// <summary>
		/// Creates a <see cref="Formatter{T}"/> for the <see cref="LogEntry.TimeStamp"/> property.
		/// </summary>
		private static Formatter<LogEntry> TimestampFormatterFactory(string parameter)
		{
			bool isLocal = false;
			string format = parameter;

			if (parameter.Equals(TimestampLocalStartDelimiter, StringComparison.InvariantCultureIgnoreCase))
			{
				format = string.Empty;
				isLocal = true;
			}
			else if (parameter.StartsWith(TimestampLocalStartDelimiterWithFormat, StringComparison.InvariantCultureIgnoreCase))
			{
				format
					= parameter.Substring(TimestampLocalStartDelimiterWithFormat.Length, parameter.Length - TimestampLocalStartDelimiterWithFormat.Length);
				isLocal = true;
			}

			if (isLocal)
			{
				return le => le.TimeStamp.ToLocalTime().ToString(format, CultureInfo.CurrentCulture);
			}
			else
			{
				return le => le.TimeStamp.ToString(format, CultureInfo.CurrentCulture);
			}
		}

		/// <summary>
		/// Creates a <see cref="Formatter{T}"/> for any property on a <see cref="LogEntry"/>
		/// retrieved through reflection.
		/// </summary>
		private static Formatter<LogEntry> ReflectedPropertyFormatterFactory(string parameter)
		{
			return le =>
			{
				Type logType = le.GetType();
				PropertyInfo property = logType.GetProperty(parameter);
				if (property == null)
				{
					return String.Format(Resources.Culture, Resources.ReflectedPropertyTokenNotFound, parameter);
				}
				if (!property.CanRead)
				{
					return String.Format(Resources.Culture, Resources.ReflectedPropertyTokenNotReadable, parameter);
				}
				if (property.GetIndexParameters().Length > 0)
				{
					return String.Format(Resources.Culture, Resources.ReflectedPropertyTokenIndexer, parameter);
				}

				try
				{
					object value = property.GetValue(le, null);
					return value != null ? value.ToString() : string.Empty;
				}
				catch
				{
					return String.Format(Resources.Culture, Resources.ReflectedPropertyTokenException, parameter);
				}
			};
		}

		/// <summary>
		/// Creates a <see cref="Formatter{T}"/> for an entry in the <see cref="LogEntry.ExtendedProperties"/>
		/// dictionary.
		/// </summary>
		private static Formatter<LogEntry> KeyValueFormatterFactory(string parameter)
		{
			return le =>
			{
				string propertyString = string.Empty;
				object propertyObject;

				if (le.ExtendedProperties.TryGetValue(parameter, out propertyObject))
				{
					propertyString = propertyObject.ToString();
				}

				return propertyString;
			};
		}

		/// <summary>
		/// Creates a <see cref="Formatter{T}"/> for all the entries in the <see cref="LogEntry.ExtendedProperties"/>
		/// dictionary.
		/// </summary>
		private static Formatter<LogEntry> DictionaryTokenHandlerFactory(string parameter)
		{
			Dictionary<string, TokenHandler<KeyValuePair<string, object>>> handlers
				= new Dictionary<string, TokenHandler<KeyValuePair<string, object>>>();
			handlers["newline"]
				= GenericTextFormatter<KeyValuePair<string, object>>.CreateSimpleTokenHandler(Environment.NewLine);
			handlers["tab"]
				= GenericTextFormatter<KeyValuePair<string, object>>.CreateSimpleTokenHandler("\t");
			handlers["key"]
				= GenericTextFormatter<KeyValuePair<string, object>>.CreateSimpleTokenHandler(kvp => kvp.Key);
			handlers["value"]
				= GenericTextFormatter<KeyValuePair<string, object>>.CreateSimpleTokenHandler(kvp => kvp.Value.ToString());

			GenericTextFormatter<KeyValuePair<string, object>> entryFormatter
				= new GenericTextFormatter<KeyValuePair<string, object>>(parameter, handlers);

			return le =>
			{
				StringBuilder entries = new StringBuilder();

				foreach (var kvp in le.ExtendedProperties)
				{
					entryFormatter.Format(kvp, entries);
				}

				return entries.ToString();
			};
		}

		/// <summary>
		/// Gets or sets the formatting template.
		/// </summary>
		public string Template
		{
			get { return template; }
			set { template = value; }
		}

		/// <overloads>
		/// Formats the <see cref="LogEntry"/> object by replacing tokens with values
		/// </overloads>
		/// <summary>
		/// Formats the <see cref="LogEntry"/> object by replacing tokens with values.
		/// </summary>
		/// <param name="log">Log entry to format.</param>
		/// <returns>Formatted string with tokens replaced with property values.</returns>
		public override string Format(LogEntry log)
		{
			StringBuilder output = new StringBuilder();

			this.formatter.Format(log, output);

			return output.ToString();
		}

		/// <summary>
		/// Provides a textual representation of a categories list.
		/// </summary>
		/// <param name="categories">The collection of categories.</param>
		/// <returns>A comma delimited textural representation of the categories.</returns>
		public static string FormatCategoriesCollection(ICollection<string> categories)
		{
			StringBuilder categoriesListBuilder = new StringBuilder();
			int i = 0;
			foreach (String category in categories)
			{
				categoriesListBuilder.Append(category);
				if (++i < categories.Count)
				{
					categoriesListBuilder.Append(", ");
				}
			}
			return categoriesListBuilder.ToString();
		}
	}
}
