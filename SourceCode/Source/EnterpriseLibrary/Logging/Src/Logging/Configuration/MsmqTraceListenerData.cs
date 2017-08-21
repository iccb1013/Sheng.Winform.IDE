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
using System.Diagnostics;
using System.Messaging;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="MsmqTraceListener"/>.
	/// </summary>
	[Assembler(typeof(MsmqTraceListenerAssembler))]
	public class MsmqTraceListenerData : TraceListenerData
	{
		private const string queuePathProperty = "queuePath";
		private const string formatterNameProperty = "formatter";
		private const string messagePriorityProperty = "messagePriority";
		private const string timeToReachQueueProperty = "timeToReachQueue";
		private const string timeToBeReceivedProperty = "timeToBeReceived";
		private const string recoverableProperty = "recoverable";
		private const string useAuthenticationProperty = "useAuthentication";
		private const string useDeadLetterQueueProperty = "useDeadLetterQueue";
		private const string useEncryptionProperty = "useEncryption";
		private const string transactionTypeProperty = "transactionType";

		private static ConfigurationPropertyCollection properties;
		
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The Priority value for the Priority property.
		/// </summary>
		public const MessagePriority DefaultPriority = MessagePriority.Normal;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the Recoverable property.
		/// </summary>
		public const bool DefaultRecoverable = false;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the UseAuthentication property.
		/// </summary>
		public const bool DefaultUseAuthentication = false;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the UseDeadLetter property.
		/// </summary>
		public const bool DefaultUseDeadLetter = false;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the UseEncryption property.
		/// </summary>
		public const bool DefaultUseEncryption = false;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the TimeToReachQueue property.
		/// </summary>
		public static readonly TimeSpan DefaultTimeToReachQueue = Message.InfiniteTimeout;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the TimeToBeReceived property.
		/// </summary>
		public static readonly TimeSpan DefaultTimeToBeReceived = Message.InfiniteTimeout;
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// The default value for the TransactionType property.
		/// </summary>
		public const MessageQueueTransactionType DefaultTransactionType = MessageQueueTransactionType.None;

		static MsmqTraceListenerData()
		{
			properties = new ConfigurationPropertyCollection();
			properties.Add(
				new ConfigurationProperty(
					nameProperty, 
					typeof(string),
					"Name",
					null,
					new StringValidator(1),
					ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey));
			properties.Add(
				new ConfigurationProperty(
					typeProperty, 
					typeof(string), 
					null, 
					null, 
					null, 
					ConfigurationPropertyOptions.IsRequired));
			properties.Add(
				new ConfigurationProperty(
					listenerDataTypeProperty, 
					typeof(string), 
					null, 
					null, 
					null, 
					ConfigurationPropertyOptions.IsRequired));
			properties.Add(
				new ConfigurationProperty(
					traceOutputOptionsProperty, 
					typeof (TraceOptions)));
            properties.Add(
                new ConfigurationProperty(
                    filterProperty,
                    typeof(SourceLevels),
                    SourceLevels.All));
			properties.Add(
				new ConfigurationProperty(
					queuePathProperty, 
					typeof (string),
					null,
					ConfigurationPropertyOptions.IsRequired));
			properties.Add(
				new ConfigurationProperty(
					formatterNameProperty, 
					typeof(string),
					null,
					ConfigurationPropertyOptions.IsRequired));
			properties.Add(new ConfigurationProperty(messagePriorityProperty, typeof(MessagePriority), DefaultPriority));
			properties.Add(new ConfigurationProperty(timeToReachQueueProperty, typeof(TimeSpan), DefaultTimeToReachQueue));
			properties.Add(new ConfigurationProperty(timeToBeReceivedProperty, typeof(TimeSpan), DefaultTimeToBeReceived));
			properties.Add(new ConfigurationProperty(recoverableProperty, typeof(bool), DefaultRecoverable));
			properties.Add(new ConfigurationProperty(useAuthenticationProperty, typeof(bool), DefaultUseAuthentication));
			properties.Add(new ConfigurationProperty(useDeadLetterQueueProperty, typeof(bool), DefaultUseDeadLetter));
			properties.Add(new ConfigurationProperty(useEncryptionProperty, typeof(bool), DefaultUseEncryption));
			properties.Add(new ConfigurationProperty(transactionTypeProperty, typeof(MessageQueueTransactionType), DefaultTransactionType));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class with default values.
		/// </summary>
		public MsmqTraceListenerData()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class with name, path and formatter name.
		/// </summary>
		/// <param name="name">The name for the represented trace listener.</param>
		/// <param name="queuePath">The path name for the represented trace listener.</param>
		/// <param name="formatterName">The formatter name for the represented trace listener.</param>
		public MsmqTraceListenerData(string name, string queuePath, string formatterName)
			: this(name, queuePath, formatterName, DefaultPriority, DefaultRecoverable, 
				DefaultTimeToBeReceived, DefaultTimeToReachQueue, DefaultUseAuthentication,
				DefaultUseDeadLetter, DefaultUseEncryption, DefaultTransactionType)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class.
		/// </summary>
		/// <param name="name">The name for the represented trace listener.</param>
		/// <param name="queuePath">The path name for the represented trace listener.</param>
		/// <param name="formatterName">The formatter name for the represented trace listener.</param>
		/// <param name="messagePriority">The priority for the represented trace listener.</param>
		/// <param name="recoverable">The recoverable flag for the represented trace listener.</param>
		/// <param name="timeToReachQueue">The timeToReachQueue for the represented trace listener.</param>
		/// <param name="timeToBeReceived">The timeToReachQueue for the represented trace listener.</param>
		/// <param name="useAuthentication">The use authentication flag for the represented trace listener.</param>
		/// <param name="useDeadLetterQueue">The use dead letter flag for the represented trace listener.</param>
		/// <param name="useEncryption">The use encryption flag for the represented trace listener.</param>
		/// <param name="transactionType">The transaction type for the represented trace listener.</param>
		public MsmqTraceListenerData(string name, string queuePath, string formatterName, 
		                             MessagePriority messagePriority, bool recoverable, 
		                             TimeSpan timeToReachQueue, TimeSpan timeToBeReceived, 
		                             bool useAuthentication, bool useDeadLetterQueue, bool useEncryption,
		                             MessageQueueTransactionType transactionType)
			: this(name, queuePath, formatterName, messagePriority, recoverable, timeToReachQueue, timeToBeReceived, 
		                             useAuthentication, useDeadLetterQueue, useEncryption, transactionType, TraceOptions.None, SourceLevels.All)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MsmqTraceListenerData"/> class.
		/// </summary>
		/// <param name="name">The name for the represented trace listener.</param>
		/// <param name="queuePath">The path name for the represented trace listener.</param>
		/// <param name="formatterName">The formatter name for the represented trace listener.</param>
		/// <param name="messagePriority">The priority for the represented trace listener.</param>
		/// <param name="recoverable">The recoverable flag for the represented trace listener.</param>
		/// <param name="timeToReachQueue">The timeToReachQueue for the represented trace listener.</param>
		/// <param name="timeToBeReceived">The timeToReachQueue for the represented trace listener.</param>
		/// <param name="useAuthentication">The use authentication flag for the represented trace listener.</param>
		/// <param name="useDeadLetterQueue">The use dead letter flag for the represented trace listener.</param>
		/// <param name="useEncryption">The use encryption flag for the represented trace listener.</param>
		/// <param name="transactionType">The transaction type for the represented trace listener.</param>
		/// <param name="traceOutputOptions">The trace output options for the represented trace listener.</param>
        /// <param name="filter">The filter for the represented trace listener.</param>
		public MsmqTraceListenerData(string name, string queuePath, string formatterName,
									 MessagePriority messagePriority, bool recoverable,
									 TimeSpan timeToReachQueue, TimeSpan timeToBeReceived,
									 bool useAuthentication, bool useDeadLetterQueue, bool useEncryption,
									 MessageQueueTransactionType transactionType, TraceOptions traceOutputOptions, SourceLevels filter)
			: base(name, typeof(MsmqTraceListener), traceOutputOptions, filter)
		{
			this.QueuePath = queuePath;
			this.Formatter = formatterName;
			this.MessagePriority = messagePriority;
			this.Recoverable = recoverable;
			this.TimeToReachQueue = timeToReachQueue;
			this.TimeToBeReceived = timeToBeReceived;
			this.UseAuthentication = useAuthentication;
			this.UseDeadLetterQueue = useDeadLetterQueue;
			this.UseEncryption = useEncryption;
			this.TransactionType = transactionType;
		}

		/// <summary>
		/// Gets or sets the message queue path.
		/// </summary>
		public string QueuePath
		{
			get
			{
				return (string)this[queuePathProperty];
			}
			set
			{
				this[queuePathProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets formatter name.
		/// </summary>
		public string Formatter
		{
			get
			{
				return (string)this[formatterNameProperty];
			}
			set
			{
				this[formatterNameProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the message priority.
		/// </summary>
		public MessagePriority MessagePriority
		{
			get
			{
				return (MessagePriority)this[messagePriorityProperty];
			}
			set
			{
				this[messagePriorityProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the time to reach queue.
		/// </summary>
		public TimeSpan TimeToReachQueue
		{
			get
			{
				return (TimeSpan)this[timeToReachQueueProperty];
			}
			set
			{
				this[timeToReachQueueProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the time to be received.
		/// </summary>
		public TimeSpan TimeToBeReceived
		{
			get
			{
				return (TimeSpan)this[timeToBeReceivedProperty];
			}
			set
			{
				this[timeToBeReceivedProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the recoverable value.
		/// </summary>
		public bool Recoverable
		{
			get
			{
				return (bool)this[recoverableProperty];
			}
			set
			{
				this[recoverableProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the use authentication value.
		/// </summary>
		public bool UseAuthentication
		{
			get
			{
				return (bool)this[useAuthenticationProperty];
			}
			set
			{
				this[useAuthenticationProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the use dead letter value.
		/// </summary>
		public bool UseDeadLetterQueue
		{
			get
			{
				return (bool)this[useDeadLetterQueueProperty];
			}
			set
			{
				this[useDeadLetterQueueProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the use encryption value.
		/// </summary>
		public bool UseEncryption
		{
			get
			{
				return (bool)this[useEncryptionProperty];
			}
			set
			{
				this[useEncryptionProperty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the transaction type.
		/// </summary>
		public MessageQueueTransactionType TransactionType
		{
			get
			{
				return (MessageQueueTransactionType)this[transactionTypeProperty];
			}
			set
			{
				this[transactionTypeProperty] = value;
			}
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="EmailTraceListener"/> based on an instance of <see cref="EmailTraceListenerData"/>.
		/// Gets the collection of properties.
		/// </summary>
		/// <remarks>
		/// The default implementation is overriden to deal with non-constant defaults.
		/// </remarks>
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return properties;
			}
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="MsmqTraceListener"/> described by a <see cref="MsmqTraceListenerData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="MsmqTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
	/// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
	/// </remarks>
	public class MsmqTraceListenerAssembler : TraceListenerAsssembler
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="MsmqTraceListener"/> based on an instance of <see cref="MsmqTraceListenerData"/>.
		/// </summary>
		/// <seealso cref="TraceListenerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="MsmqTraceListenerData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="MsmqTraceListener"/>.</returns>
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			MsmqTraceListenerData castedObjectConfiguration
				= (MsmqTraceListenerData)objectConfiguration;

			ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);

			TraceListener createdObject
				= new MsmqTraceListener(
					castedObjectConfiguration.Name,
					castedObjectConfiguration.QueuePath,
					formatter,
					castedObjectConfiguration.MessagePriority,
					castedObjectConfiguration.Recoverable,
					castedObjectConfiguration.TimeToReachQueue,
					castedObjectConfiguration.TimeToBeReceived,
					castedObjectConfiguration.UseAuthentication,
					castedObjectConfiguration.UseDeadLetterQueue,
					castedObjectConfiguration.UseEncryption,
					castedObjectConfiguration.TransactionType);

			return createdObject;
		}
	}
}
