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
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using System.Collections.Generic;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Represents a performance tracing class to log method entry/exit and duration.
	/// </summary>
	/// <remarks>
	/// <para>Lifetime of the Tracer object will determine the beginning and the end of
	/// the trace.  The trace message will include, method being traced, start time, end time 
	/// and duration.</para>
	/// <para>Since Tracer uses the logging block to log the trace message, you can include application
	/// data as part of your trace message. Configured items from call context will be logged as
	/// part of the message.</para>
	/// <para>Trace message will be logged to the log category with the same name as the tracer operation name.
	/// You must configure the operation categories, or the catch-all categories, with desired log sinks to log 
	/// the trace messages.</para>
	/// </remarks>
	public class Tracer : IDisposable
	{
		/// <summary>
		/// Priority value for Trace messages
		/// </summary>
		public const int priority = 5;

		/// <summary>
		/// Event id for Trace messages
		/// </summary>
		public const int eventId = 1;

		/// <summary>
		/// Title for operation start Trace messages
		/// </summary>
		public const string startTitle = "TracerEnter";

		/// <summary>
		/// Title for operation end Trace messages
		/// </summary>
		public const string endTitle = "TracerExit";

		/// <summary>
		/// Name of the entry in the ExtendedProperties having the activity id
		/// </summary>
		public const string ActivityIdPropertyKey = "TracerActivityId";

		private TracerInstrumentationListener instrumentationListener;

		private Stopwatch stopwatch;
		private long tracingStartTicks;
		private bool tracerDisposed = false;
		private bool tracingAvailable = false;

		private LogWriter writer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
		/// </summary>
		/// <remarks>
		/// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
		/// </remarks>
		/// <param name="operation">The operation for the <see cref="Tracer"/></param>
		public Tracer(string operation)
		{
			if (CheckTracingAvailable())
			{
				if (GetActivityId().Equals(Guid.Empty))
				{
					SetActivityId(Guid.NewGuid());
				}

                Initialize(operation, GetInstrumentationListener(ConfigurationSourceFactory.Create()));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
		/// </summary>
		/// <remarks>
		/// The activity id will override a previous activity id
		/// </remarks>
		/// <param name="operation">The operation for the <see cref="Tracer"/></param>
		/// <param name="activityId">The activity id</param>
		public Tracer(string operation, Guid activityId)
		{
			if (CheckTracingAvailable())
			{
				SetActivityId(activityId);

                Initialize(operation, GetInstrumentationListener(ConfigurationSourceFactory.Create()));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
		/// </summary>
		/// <remarks>
		/// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
		/// </remarks>
		/// <param name="operation">The operation for the <see cref="Tracer"/></param>
		/// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
		/// <param name="instrumentationConfiguration">configuration source that is used to determine instrumentation should be enabled</param>
		public Tracer(string operation, LogWriter writer, IConfigurationSource instrumentationConfiguration):
            this(operation, writer, GetInstrumentationListener(instrumentationConfiguration))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
		/// </summary>
		/// <remarks>
		/// The activity id will override a previous activity id
		/// </remarks>
		/// <param name="operation">The operation for the <see cref="Tracer"/></param>
		/// <param name="activityId">The activity id</param>
		/// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
		/// <param name="instrumentationConfiguration">configuration source that is used to determine instrumentation should be enabled</param>
		public Tracer(string operation, Guid activityId, LogWriter writer, IConfigurationSource instrumentationConfiguration) :
            this(operation, activityId, writer, GetInstrumentationListener(instrumentationConfiguration))
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// This is meant to be used internally
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationListener">The <see cref="TracerInstrumentationListener"/> used to determine if instrumentation should be enabled</param>
        internal Tracer(string operation, Guid activityId, LogWriter writer, TracerInstrumentationListener instrumentationListener)
        {
            if (instrumentationListener == null)
            {
                instrumentationListener = new TracerInstrumentationListener(false);
            }

            if (CheckTracingAvailable())
            {
                if (writer == null) throw new ArgumentNullException("writer", Resources.ExceptionWriterShouldNotBeNull);

                SetActivityId(activityId);

                this.writer = writer;

                Initialize(operation, instrumentationListener);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// This is meant to be used internally
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationListener">The <see cref="TracerInstrumentationListener"/> used to determine if instrumentation should be enabled</param>
        internal Tracer(string operation, LogWriter writer, TracerInstrumentationListener instrumentationListener)
        {
            if (instrumentationListener == null)
            {
                instrumentationListener = new TracerInstrumentationListener(false);
            }

            if (CheckTracingAvailable())
            {
                if (writer == null) throw new ArgumentNullException("writer", Resources.ExceptionWriterShouldNotBeNull);

                if (GetActivityId().Equals(Guid.Empty))
                {
                    SetActivityId(Guid.NewGuid());
                }

                this.writer = writer;

                Initialize(operation, instrumentationListener);
            }
        }


		/// <summary>
		/// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="Tracer"/> is 
		/// reclaimed by garbage collection</para>
		/// </summary>
		~Tracer()
		{
			Dispose(false);
		}

		/// <summary>
		/// Causes the <see cref="Tracer"/> to output its closing message.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// <para>Releases the unmanaged resources used by the <see cref="Tracer"/> and optionally releases 
		/// the managed resources.</para>
		/// </summary>
		/// <param name="disposing">
		/// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> 
		/// to release only unmanaged resources.</para>
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !tracerDisposed)
			{
				if (tracingAvailable)
				{
					try
					{
						if (IsTracingEnabled()) WriteTraceEndMessage(endTitle);
					}
					finally
					{
						try
						{
							StopLogicalOperation();
						}
						catch (SecurityException)
						{
						}
					}
				}

				this.tracerDisposed = true;
			}
		}

		/// <summary>
		/// Answers whether tracing is enabled
		/// </summary>
		/// <returns>true if tracing is enabled</returns>
		public bool IsTracingEnabled()
		{
			LogWriter writer = GetWriter();
			return writer.IsTracingEnabled();
		}

		internal static bool IsTracingAvailable()
		{
			bool tracingAvailable = false;

			try
			{
				tracingAvailable = SecurityManager.IsGranted(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			}
			catch (SecurityException)
			{ }

			return tracingAvailable;
		}

		private bool CheckTracingAvailable()
		{
			tracingAvailable = IsTracingAvailable();

			return tracingAvailable;
		}

		private static TracerInstrumentationListener GetInstrumentationListener(IConfigurationSource configurationSource)
		{
            TracerInstrumentationListener instrumentationListener;
			if (configurationSource != null)
			{
				instrumentationListener = EnterpriseLibraryFactory.BuildUp<TracerInstrumentationListener>(configurationSource);
			}
			else
			{
				instrumentationListener = new TracerInstrumentationListener(false);
			}

            return instrumentationListener;
		}

        private void Initialize(string operation, TracerInstrumentationListener instrumentationListener)
        {
            this.instrumentationListener = instrumentationListener;

            StartLogicalOperation(operation);
            if (IsTracingEnabled())
            {
                instrumentationListener.TracerOperationStarted(PeekLogicalOperationStack() as string);

                stopwatch = Stopwatch.StartNew();
                tracingStartTicks = Stopwatch.GetTimestamp();

                WriteTraceStartMessage(startTitle);
            }
        }

		private void WriteTraceStartMessage(string entryTitle)
		{
			string methodName = GetExecutingMethodName();
			string message = string.Format(Resources.Culture, Resources.Tracer_StartMessageFormat, GetActivityId(), methodName, tracingStartTicks);

			WriteTraceMessage(message, entryTitle, TraceEventType.Start);
		}

		private void WriteTraceEndMessage(string entryTitle)
		{
			long tracingEndTicks = Stopwatch.GetTimestamp();
			decimal secondsElapsed = GetSecondsElapsed(stopwatch.ElapsedMilliseconds);

			string methodName = GetExecutingMethodName();
			string message = string.Format(Resources.Culture, Resources.Tracer_EndMessageFormat, GetActivityId(), methodName, tracingEndTicks, secondsElapsed);
			WriteTraceMessage(message, entryTitle, TraceEventType.Stop);

			instrumentationListener.TracerOperationEnded(PeekLogicalOperationStack() as string, stopwatch.ElapsedMilliseconds);
		}

		private void WriteTraceMessage(string message, string entryTitle, TraceEventType eventType)
		{
			Dictionary<string, object> extendedProperties = new Dictionary<string, object>();
			LogEntry entry = new LogEntry(message, PeekLogicalOperationStack() as string, priority, eventId, eventType, entryTitle, extendedProperties);

			LogWriter writer = GetWriter();
			writer.Write(entry);
		}

		private string GetExecutingMethodName()
		{
			string result = "Unknown";
			StackTrace trace = new StackTrace(false);

			for (int index = 0; index < trace.FrameCount; ++index)
			{
				StackFrame frame = trace.GetFrame(index);
				MethodBase method = frame.GetMethod();
				if (method.DeclaringType != GetType())
				{
					result = string.Concat(method.DeclaringType.FullName, ".", method.Name);
					break;
				}
			}

			return result;
		}

		private decimal GetSecondsElapsed(long milliseconds)
		{
			decimal result = Convert.ToDecimal(milliseconds) / 1000m;
			return Math.Round(result, 6);
		}

		private LogWriter GetWriter()
		{
			return (writer != null) ? writer : Logger.Writer;
		}

		private static Guid GetActivityId()
		{
			return Trace.CorrelationManager.ActivityId;
		}

		private static Guid SetActivityId(Guid activityId)
		{
			return Trace.CorrelationManager.ActivityId = activityId;
		}

		private static void StartLogicalOperation(string operation)
		{
			Trace.CorrelationManager.StartLogicalOperation(operation);
		}

		private static void StopLogicalOperation()
		{
			Trace.CorrelationManager.StopLogicalOperation();
		}

		private static object PeekLogicalOperationStack()
		{
			return Trace.CorrelationManager.LogicalOperationStack.Peek();
		}
	}
}
