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

using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Facade for writing a log entry to one or more <see cref="TraceListener"/>s.  This class is sealed.
	/// </summary>
	public static class Logger
	{
		private static object sync = new object();
		private static volatile LogWriter writer;

		private static LogWriterFactory factory = new LogWriterFactory(ConfigurationSourceFactory.Create());

		private const int DefaultPriority = -1;
		private const TraceEventType DefaultSeverity = TraceEventType.Information;
		private const int DefaultEventId = 1;
		private const string DefaultTitle = "";
		private static readonly ICollection<string> emptyCategoriesList = new List<string>(0);

		/// <summary>
		/// Add a key/value pair to the <see cref="System.Runtime.Remoting.Messaging.CallContext"/> dictionary.  
		/// Context items will be recorded with every log entry.
		/// </summary>
		/// <param name="key">Hashtable key</param>
		/// <param name="value">Value.  Objects will be serialized.</param>
		/// <example>The following example demonstrates use of the AddContextItem method.
		/// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
		public static void SetContextItem(object key, object value)
		{
			Writer.SetContextItem(key, value);
		}

		/// <summary>
		/// Empty the context items dictionary.
		/// </summary>
		public static void FlushContextItems()
		{
			Writer.FlushContextItems();
		}

		/// <overloads>
		/// Write a new log entry to the default category.
		/// </overloads>
		/// <summary>
		/// Write a new log entry to the default category.
		/// </summary>
		/// <example>The following example demonstrates use of the Write method with
		/// one required parameter, message.
		/// <code>Logger.Write("My message body");</code></example>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		public static void Write(object message)
		{
			Write(message, emptyCategoriesList, DefaultPriority,
				  DefaultEventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry to a specific category.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		public static void Write(object message, string category)
		{
			Write(message, category, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific category and priority.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		public static void Write(object message, string category, int priority)
		{
			Write(message, category, priority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific category, priority and event id.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		public static void Write(object message, string category, int priority, int eventId)
		{
			Write(message, category, priority, eventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific category, priority, event id and severity.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		public static void Write(object message, string category, int priority, int eventId, TraceEventType severity)
		{
			Write(message, category, priority, eventId, severity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific category, priority, event id, severity
		/// and title.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message</param>
		public static void Write(object message, string category, int priority, int eventId,
								 TraceEventType severity, string title)
		{
			Write(message, category, priority, eventId, severity, title, null);
		}

		/// <summary>
		/// Write a new log entry and a dictionary of extended properties.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, IDictionary<string, object> properties)
		{
			Write(message, emptyCategoriesList, DefaultPriority,
				  DefaultEventId, DefaultSeverity, DefaultTitle, properties);
		}

		/// <summary>
		/// Write a new log entry to a specific category with a dictionary 
		/// of extended properties.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, string category, IDictionary<string, object> properties)
		{
			Write(message, category, DefaultPriority, DefaultEventId, DefaultSeverity,
				  DefaultTitle, properties);
		}

		/// <summary>
		/// Write a new log entry to with a specific category, priority and a dictionary 
		/// of extended properties.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, string category, int priority, IDictionary<string, object> properties)
		{
			Write(message, category, priority, DefaultEventId, DefaultSeverity, DefaultTitle, properties);
		}

		/// <summary>
		/// Write a new log entry with a specific category, priority, event Id, severity
		/// title and dictionary of extended properties.
		/// </summary>
		/// <example>The following example demonstrates use of the Write method with
		/// a full set of parameters.
		/// <code></code></example>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, string category, int priority, int eventId,
								 TraceEventType severity, string title, IDictionary<string, object> properties)
		{
			Write(message, new string[] { category }, priority, eventId, severity, title, properties);
		}

		/// <summary>
		/// Write a new log entry to a specific collection of categories.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		public static void Write(object message, ICollection<string> categories)
		{
			Write(message, categories, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific collection of categories and priority.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		public static void Write(object message, ICollection<string> categories, int priority)
		{
			Write(message, categories, priority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific collection of categories, priority and event id.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		public static void Write(object message, ICollection<string> categories, int priority, int eventId)
		{
			Write(message, categories, priority, eventId, DefaultSeverity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific collection of categories, priority, event id and severity.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		public static void Write(object message, ICollection<string> categories, int priority, int eventId, TraceEventType severity)
		{
			Write(message, categories, priority, eventId, severity, DefaultTitle, null);
		}

		/// <summary>
		/// Write a new log entry with a specific collection of categories, priority, event id, severity
		/// and title.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message</param>
		public static void Write(object message, ICollection<string> categories, int priority, int eventId,
								 TraceEventType severity, string title)
		{
			Write(message, categories, priority, eventId, severity, title, null);
		}

		/// <summary>
		/// Write a new log entry to a specific collection of categories with a dictionary of extended properties.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, ICollection<string> categories, IDictionary<string, object> properties)
		{
			Write(message, categories, DefaultPriority, DefaultEventId, DefaultSeverity,
				  DefaultTitle, properties);
		}

		/// <summary>
		/// Write a new log entry to with a specific collection of categories, priority and a dictionary 
		/// of extended properties.
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, ICollection<string> categories, int priority, IDictionary<string, object> properties)
		{
			Write(message, categories, priority, DefaultEventId, DefaultSeverity, DefaultTitle, properties);
		}

		/// <summary>
		/// Write a new log entry with a specific category, priority, event Id, severity
		/// title and dictionary of extended properties.
		/// </summary>
		/// <example>The following example demonstrates use of the Write method with
		/// a full set of parameters.
		/// <code></code></example>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message.</param>
		/// <param name="properties">Dictionary of key/value pairs to log.</param>
		public static void Write(object message, ICollection<string> categories, int priority, int eventId,
								 TraceEventType severity, string title, IDictionary<string, object> properties)
		{
			LogEntry log = new LogEntry();
			log.Message = message.ToString();
			log.Categories = categories;
			log.Priority = priority;
			log.EventId = eventId;
			log.Severity = severity;
			log.Title = title;
			log.ExtendedProperties = properties;

			Write(log);
		}

		/// <summary>
		/// Write a new log entry as defined in the <see cref="LogEntry"/> parameter.
		/// </summary>
		/// <example>The following examples demonstrates use of the Write method using
		/// a <see cref="LogEntry"/> type.
		/// <code>
		/// LogEntry log = new LogEntry();
		/// log.Category = "MyCategory1";
		/// log.Message = "My message body";
		/// log.Severity = TraceEventType.Error;
		/// log.Priority = 100;
		/// Logger.Write(log);</code></example>
		/// <param name="log">Log entry object to write.</param>
		public static void Write(LogEntry log)
		{
			Writer.Write(log);
		}

		/// <summary>
		/// Returns the filter of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of filter requiered.</typeparam>
		/// <returns>The instance of <typeparamref name="T"/> in the filters collection, or <see langword="null"/> 
		/// if there is no such instance.</returns>
		public static T GetFilter<T>()
			where T : class, ILogFilter
		{
			return Writer.GetFilter<T>();
		}

		/// <summary>
		/// Returns the filter of type <typeparamref name="T"/> named <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="T">The type of filter required.</typeparam>
		/// <param name="name">The name of the filter required.</param>
		/// <returns>The instance of <typeparamref name="T"/> named <paramref name="name"/> in 
		/// the filters collection, or <see langword="null"/> if there is no such instance.</returns>
		public static T GetFilter<T>(string name)
			where T : class, ILogFilter
		{
			return Writer.GetFilter<T>(name);
		}

		/// <summary>
		/// Returns the filter named <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the filter required.</param>
		/// <returns>The filter named <paramref name="name"/> in 
		/// the filters collection, or <see langword="null"/> if there is no such filter.</returns>
		public static ILogFilter GetFilter(string name)
		{
			return Writer.GetFilter(name);
		}

		/// <summary>
		/// Query whether logging is enabled.
		/// </summary>
		/// <returns><code>true</code> if logging is enabled.</returns>
		public static bool IsLoggingEnabled()
		{
			return Writer.IsLoggingEnabled();
		}

		/// <summary>
		/// Query whether a <see cref="LogEntry"/> shold be logged.
		/// </summary>
		/// <param name="log">The log entry to check</param>
		/// <returns>Returns <code>true</code> if the entry should be logged.</returns>
		public static bool ShouldLog(LogEntry log)
		{
			return Writer.ShouldLog(log);
		}

		/// <summary>
		/// Public for testing purposes.
		/// Reset the writer used by the <see cref="Logger"/> facade.
		/// </summary>
		/// <remarks>
		/// Threads that already acquired the reference to the old writer will fail when it gets disposed.
		/// </remarks>
		public static void Reset()
		{
			lock (sync)
			{
				LogWriter oldWriter = writer;

				// this will be seen by threads requesting the writer (because of the double check locking pattern the query is outside the lock).
				// these threads should be stopped when trying to lock to create the writer.
				writer = null;

				// the old writer is disposed inside the lock to avoid having two instances with the same configuration.
				if (oldWriter != null)
					oldWriter.Dispose();
			}
		}

		/// <summary>
		/// Gets the instance of <see cref="LogWriter"/> used by the facade.
		/// </summary>
		/// <remarks>
		/// The lifetime of this instance is managed by the facade.
		/// </remarks>
		public static LogWriter Writer
		{
			get
			{
				if (writer == null)
				{
					lock (sync)
					{
						if (writer == null)
						{
							try
							{
								writer = factory.Create();
							}
							catch (ConfigurationErrorsException configurationException)
							{
								TryLogConfigurationFailure(configurationException);

								throw;
							}
						}
					}
				}
				return writer;
			}
		}

		internal static void TryLogConfigurationFailure(ConfigurationErrorsException configurationException)
		{
			try
			{
				DefaultLoggingEventLogger logger = EnterpriseLibraryFactory.BuildUp<DefaultLoggingEventLogger>();
				logger.LogConfigurationError(configurationException);
			}
			catch
			{ }
		}
	}
}
