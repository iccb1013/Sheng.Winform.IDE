//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents the base class from which all implementations of exception formatters must derive. The formatter provides functionality for formatting <see cref="Exception"/> objects.
    /// </summary>	
    public abstract class ExceptionFormatter
    {
        private static readonly ArrayList IgnoredProperties = new ArrayList(
            new String[] { "Source", "Message", "HelpLink", "InnerException", "StackTrace" });

        private readonly Guid handlingInstanceId;
        private readonly Exception exception;
        private NameValueCollection additionalInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFormatter"/> class with an <see cref="Exception"/> to format.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> object to format.</param>
        /// <param name="handlingInstanceId">The id of the handling chain.</param>
        protected ExceptionFormatter(Exception exception, Guid handlingInstanceId)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            this.exception = exception;
            this.handlingInstanceId = handlingInstanceId;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> to format.
        /// </summary>
        /// <value>
        /// The <see cref="Exception"/> to format.
        /// </value>
        public Exception Exception
        {
            get { return this.exception; }
        }

        /// <summary>
        /// Gets the id of the handling chain requesting a formatting.
        /// </summary>
        /// <value>
        /// The id of the handling chain requesting a formatting, or <see cref="Guid.Empty"/> if no such id is available.
        /// </value>
        public Guid HandlingInstanceId
        {
            get { return this.handlingInstanceId; }
        }

        /// <summary>
        /// Gets additional information related to the <see cref="Exception"/> but not
        /// stored in the exception (eg: the time in which the <see cref="Exception"/> was 
        /// thrown).
        /// </summary>
        /// <value>
        /// Additional information related to the <see cref="Exception"/> but not
        /// stored in the exception (for example, the time when the <see cref="Exception"/> was 
        /// thrown).
        /// </value>
        public NameValueCollection AdditionalInfo
        {
            get
            {
                if (this.additionalInfo == null)
                {
                    this.additionalInfo = new NameValueCollection();
                    this.additionalInfo.Add("MachineName", GetMachineName());
                    this.additionalInfo.Add("TimeStamp", DateTime.UtcNow.ToString(CultureInfo.CurrentCulture));
                    this.additionalInfo.Add("FullName", Assembly.GetExecutingAssembly().FullName);
                    this.additionalInfo.Add("AppDomainName", AppDomain.CurrentDomain.FriendlyName);
                    this.additionalInfo.Add("ThreadIdentity", Thread.CurrentPrincipal.Identity.Name);
                    this.additionalInfo.Add("WindowsIdentity", GetWindowsIdentity());
                }

                return this.additionalInfo;
            }
        }

        /// <summary>
        /// Formats the <see cref="Exception"/> into the underlying stream.
        /// </summary>
        public virtual void Format()
        {
            WriteDescription();
            WriteDateTime(DateTime.UtcNow);
            WriteException(this.exception, null);
        }

        /// <summary>
        /// Formats the exception and all nested inner exceptions.
        /// </summary>
        /// <param name="exceptionToFormat">The exception to format.</param>
        /// <param name="outerException">The outer exception. This 
        /// value will be null when writing the outer-most exception.</param>
        /// <remarks>
        /// <para>This method calls itself recursively until it reaches
        /// an exception that does not have an inner exception.</para>
        /// <para>
        /// This is a template method which calls the following
        /// methods in order
        /// <list type="number">
        /// <item>
        /// <description><see cref="WriteExceptionType"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteMessage"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteSource"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteHelpLink"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteReflectionInfo"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteStackTrace"/></description>
        /// </item>
        /// <item>
        /// <description>If the specified exception has an inner exception
        /// then it makes a recursive call. <see cref="WriteException"/></description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        protected virtual void WriteException(Exception exceptionToFormat, Exception outerException)
        {
            if (exceptionToFormat == null) throw new ArgumentNullException("exceptionToFormat");

            this.WriteExceptionType(exceptionToFormat.GetType());
            this.WriteMessage(exceptionToFormat.Message);
            this.WriteSource(exceptionToFormat.Source);
            this.WriteHelpLink(exceptionToFormat.HelpLink);
            this.WriteReflectionInfo(exceptionToFormat);
            this.WriteStackTrace(exceptionToFormat.StackTrace);

            // We only want additional information on the top most exception
            if (outerException == null)
            {
                this.WriteAdditionalInfo(this.AdditionalInfo);
            }

            Exception inner = exceptionToFormat.InnerException;

            if (inner != null)
            {
                // recursive call
                this.WriteException(inner, exceptionToFormat);
            }
        }

        /// <summary>
        /// Formats an <see cref="Exception"/> using reflection to get the information.
        /// </summary>
        /// <param name="exceptionToFormat">
        /// The <see cref="Exception"/> to be formatted.
        /// </param>
        /// <remarks>
        /// <para>This method reflects over the public, instance properties 
        /// and public, instance fields
        /// of the specified exception and prints them to the formatter.  
        /// Certain property names are ignored
        /// because they are handled explicitly in other places.</para>
        /// </remarks>
        protected void WriteReflectionInfo(Exception exceptionToFormat)
        {
            if (exceptionToFormat == null) throw new ArgumentNullException("exceptionToFormat");

            Type type = exceptionToFormat.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            object value;

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && IgnoredProperties.IndexOf(property.Name) == -1 && property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        value = property.GetValue(exceptionToFormat, null);
                    }
                    catch (TargetInvocationException)
                    {
                        value = Resources.PropertyAccessFailed;
                    }
                    WritePropertyInfo(property, value);
                }
            }

            foreach (FieldInfo field in fields)
            {
                try
                {
                    value = field.GetValue(exceptionToFormat);
                }
                catch (TargetInvocationException)
                {
                    value = Resources.FieldAccessFailed;
                }
                WriteFieldInfo(field, value);
            }
        }

        /// <summary>
        /// When overridden by a class, writes a description of the caught exception.
        /// </summary>
        protected abstract void WriteDescription();

        /// <summary>
        /// When overridden by a class, writes the current time.
        /// </summary>
        /// <param name="utcNow">The current time.</param>
        protected abstract void WriteDateTime(DateTime utcNow);

        /// <summary>
        /// When overridden by a class, writes the <see cref="Type"/> of the current exception.
        /// </summary>
        /// <param name="exceptionType">The <see cref="Type"/> of the exception.</param>
        protected abstract void WriteExceptionType(Type exceptionType);

        /// <summary>
        /// When overridden by a class, writes the <see cref="System.Exception.Message"/>.
        /// </summary>
        /// <param name="message">The message to write.</param>
        protected abstract void WriteMessage(string message);

        /// <summary>
        /// When overridden by a class, writes the value of the <see cref="System.Exception.Source"/> property.
        /// </summary>
        /// <param name="source">The source of the exception.</param>
        protected abstract void WriteSource(string source);

        /// <summary>
        /// When overridden by a class, writes the value of the <see cref="System.Exception.HelpLink"/> property.
        /// </summary>
        /// <param name="helpLink">The help link for the exception.</param>
        protected abstract void WriteHelpLink(string helpLink);

        /// <summary>
        /// When overridden by a class, writes the value of the <see cref="System.Exception.StackTrace"/> property.
        /// </summary>
        /// <param name="stackTrace">The stack trace of the exception.</param>
        protected abstract void WriteStackTrace(string stackTrace);

        /// <summary>
        /// When overridden by a class, writes the value of a <see cref="PropertyInfo"/> object.
        /// </summary>
        /// <param name="propertyInfo">The reflected <see cref="PropertyInfo"/> object.</param>
        /// <param name="value">The value of the <see cref="PropertyInfo"/> object.</param>
        protected abstract void WritePropertyInfo(PropertyInfo propertyInfo, object value);

        /// <summary>
        /// When overridden by a class, writes the value of a <see cref="FieldInfo"/> object.
        /// </summary>
        /// <param name="fieldInfo">The reflected <see cref="FieldInfo"/> object.</param>
        /// <param name="value">The value of the <see cref="FieldInfo"/> object.</param>
        protected abstract void WriteFieldInfo(FieldInfo fieldInfo, object value);

        /// <summary>
        /// When overridden by a class, writes additional properties if available.
        /// </summary>
        /// <param name="additionalInformation">Additional information to be included with the exception report</param>
        protected abstract void WriteAdditionalInfo(NameValueCollection additionalInformation);

        private static string GetMachineName()
        {
            string machineName = String.Empty;
            try
            {
                machineName = Environment.MachineName;
            }
            catch (SecurityException)
            {
                machineName = Resources.PermissionDenied;
            }

            return machineName;
        }

        private static string GetWindowsIdentity()
        {
            string windowsIdentity = String.Empty;
            try
            {
                windowsIdentity = WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
                windowsIdentity = Resources.PermissionDenied;
            }

            return windowsIdentity;
        }
    }
}
