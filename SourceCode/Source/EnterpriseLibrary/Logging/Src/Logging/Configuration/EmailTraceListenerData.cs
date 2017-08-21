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

using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="EmailTraceListener"/>.
	/// </summary>
	[Assembler(typeof(EmailTraceListenerAssembler))]
	public class EmailTraceListenerData : TraceListenerData
	{
		private const string toAddressProperty = "toAddress";
		private const string fromAddressProperty = "fromAddress";
		private const string subjectLineStarterProperty = "subjectLineStarter";
		private const string subjectLineEnderProperty = "subjectLineEnder";
		private const string smtpServerProperty = "smtpServer";
		private const string smtpPortProperty = "smtpPort";
		private const string formatterNameProperty = "formatter";

		/// <summary>
		/// Initializes a <see cref="EmailTraceListenerData"/>.
		/// </summary>
		public EmailTraceListenerData()
		{
		}

		/// <summary>
		/// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
		/// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
		/// Default value for the SMTP port is 25
		/// </summary>
		/// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
		/// <param name="fromAddress">Represents from whom the email is sent.</param>
		/// <param name="subjectLineStarter">Starting text for the subject line.</param>
		/// <param name="subjectLineEnder">Ending text for the subject line.</param>
		/// <param name="smtpServer">The name of the SMTP server.</param>
		/// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
		///email message should be formatted</param>
		public EmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, string formatterName)
			: this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatterName)
		{

		}

		/// <summary>
		/// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
		/// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
		/// </summary>
		/// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
		/// <param name="fromAddress">Represents from whom the email is sent.</param>
		/// <param name="subjectLineStarter">Starting text for the subject line.</param>
		/// <param name="subjectLineEnder">Ending text for the subject line.</param>
		/// <param name="smtpServer">The name of the SMTP server.</param>
		/// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
		/// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
		///email message should be formatted</param>
		public EmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
			: this("unnamed", toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName)
		{
		}

		/// <summary>
		/// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
		/// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
		/// </summary>
		/// <param name="name">The name of this listener</param>        
		/// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
		/// <param name="fromAddress">Represents from whom the email is sent.</param>
		/// <param name="subjectLineStarter">Starting text for the subject line.</param>
		/// <param name="subjectLineEnder">Ending text for the subject line.</param>
		/// <param name="smtpServer">The name of the SMTP server.</param>
		/// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
		/// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
		///email message should be formatted</param>
		public EmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
			: this(name, toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName, TraceOptions.None)
		{
		}

		/// <summary>
		/// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
		/// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
		/// </summary>
		/// <param name="name">The name of this listener</param>        
		/// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
		/// <param name="fromAddress">Represents from whom the email is sent.</param>
		/// <param name="subjectLineStarter">Starting text for the subject line.</param>
		/// <param name="subjectLineEnder">Ending text for the subject line.</param>
		/// <param name="smtpServer">The name of the SMTP server.</param>
		/// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
		/// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
		///email message should be formatted</param>
		///<param name="traceOutputOptions">The trace options.</param>
		public EmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions)
			: base(name, typeof(EmailTraceListener), traceOutputOptions)
		{
			this.ToAddress = toAddress;
			this.FromAddress = fromAddress;
			this.SubjectLineStarter = subjectLineStarter;
			this.SubjectLineEnder = subjectLineEnder;
			this.SmtpServer = smtpServer;
			this.SmtpPort = smtpPort;
			this.Formatter = formatterName;
		}

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        public EmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(EmailTraceListener), traceOutputOptions, filter)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
        }

		/// <summary>
		/// Gets and sets the ToAddress.  One or more email semicolon separated addresses.
		/// </summary>
		[ConfigurationProperty(toAddressProperty, IsRequired = true)]
		public string ToAddress
		{
			get { return (string)base[toAddressProperty]; }
			set { base[toAddressProperty] = value; }
		}

		/// <summary>
		/// Gets and sets the FromAddress. Email address that messages will be sent from.
		/// </summary>
		[ConfigurationProperty(fromAddressProperty, IsRequired = true)]
		public string FromAddress
		{
			get { return (string)base[fromAddressProperty]; }
			set { base[fromAddressProperty] = value; }
		}

		/// <summary>
		/// Gets and sets the Subject prefix.
		/// </summary>
		[ConfigurationProperty(subjectLineStarterProperty)]
		public string SubjectLineStarter
		{
			get { return (string)base[subjectLineStarterProperty]; }
			set { base[subjectLineStarterProperty] = value; }
		}

		/// <summary>
		/// Gets and sets the Subject suffix.
		/// </summary>
		[ConfigurationProperty(subjectLineEnderProperty)]
		public string SubjectLineEnder
		{
			get { return (string)base[subjectLineEnderProperty]; }
			set { base[subjectLineEnderProperty] = value; }
		}

		/// <summary>
		/// Gets and sets the SMTP server to use to send messages.
		/// </summary>
		[ConfigurationProperty(smtpServerProperty)]
		public string SmtpServer
		{
			get { return (string)base[smtpServerProperty]; }
			set { base[smtpServerProperty] = value; }
		}

		/// <summary>
		/// Gets and sets the SMTP port.
		/// </summary>
		[ConfigurationProperty(smtpPortProperty)]
		public int SmtpPort
		{
			get { return (int)base[smtpPortProperty]; }
			set { base[smtpPortProperty] = value; }
		}

		/// <summary>
		/// Gets and sets the formatter name.
		/// </summary>
		[ConfigurationProperty(formatterNameProperty, IsRequired = false)]
		public string Formatter
		{
			get { return (string)base[formatterNameProperty]; }
			set { base[formatterNameProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build an <see cref="EmailTraceListener"/> described by a <see cref="EmailTraceListenerData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="EmailTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
	/// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
	/// </remarks>
	public class EmailTraceListenerAssembler : TraceListenerAsssembler
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds an <see cref="EmailTraceListener"/> based on an instance of <see cref="EmailTraceListenerData"/>.
		/// </summary>
		/// <seealso cref="TraceListenerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="EmailTraceListenerData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="EmailTraceListener"/>.</returns>
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			EmailTraceListenerData castedObjectConfiguration
				= (EmailTraceListenerData)objectConfiguration;

			ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);

			TraceListener createdObject
				= new EmailTraceListener(
					castedObjectConfiguration.ToAddress,
					castedObjectConfiguration.FromAddress,
					castedObjectConfiguration.SubjectLineStarter,
					castedObjectConfiguration.SubjectLineEnder,
					castedObjectConfiguration.SmtpServer,
					castedObjectConfiguration.SmtpPort,
					formatter);

			return createdObject;
		}
	}
}
