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
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents an exception formatter that formats exception objects as XML.
    /// </summary>	
    public class XmlExceptionFormatter : ExceptionFormatter
    {
        private readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlExceptionFormatter"/> class using the specified <see cref="XmlWriter"/> and <see cref="Exception"/> objects.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> in which to write the XML.</param>
        /// <param name="exception">The <see cref="Exception"/> to format.</param>
        /// <param name="handlingInstanceId">The id of the handling chain.</param>
        public XmlExceptionFormatter(XmlWriter xmlWriter, Exception exception, Guid handlingInstanceId)
            : base(exception, handlingInstanceId)
        {
            if (xmlWriter == null) throw new ArgumentNullException("xmlWriter");

            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlExceptionFormatter"/> class using the specified <see cref="TextWriter"/> and <see cref="Exception"/> objects.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> in which to write the XML.</param>
        /// <param name="exception">The <see cref="Exception"/> to format.</param>
        /// <remarks>
        /// An <see cref="XmlTextWriter"/> with indented formatting is created from the  specified <see cref="TextWriter"/>.
        /// </remarks>
        /// <param name="handlingInstanceId">The id of the handling chain.</param>
        public XmlExceptionFormatter(TextWriter writer, Exception exception, Guid handlingInstanceId)
            : base(exception, handlingInstanceId)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            XmlTextWriter textWriter = new XmlTextWriter(writer);
            textWriter.Formatting = Formatting.Indented;
            xmlWriter = textWriter;
        }

        /// <summary>
        /// Gets the underlying <see cref="XmlWriter"/> that the formatted exception is written to.
        /// </summary>
        /// <value>
        /// The underlying <see cref="XmlWriter"/> that the formatted exception is written to.
        /// </value>
        public XmlWriter Writer
        {
            get { return xmlWriter; }
        }

        /// <summary>
        /// Formats the <see cref="Exception"/> into the underlying stream.
        /// </summary>       
        public override void Format()
        {
            Writer.WriteStartElement("Exception");
            if (this.HandlingInstanceId != Guid.Empty)
            {
                Writer.WriteAttributeString(
                    "handlingInstanceId",
                    this.HandlingInstanceId.ToString("D", CultureInfo.InvariantCulture));
            }

            base.Format();

            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the current date and time to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="utcNow">The current time.</param>
        protected override void WriteDateTime(DateTime utcNow)
        {
            DateTime localTime = utcNow.ToLocalTime();
            string localTimeString = localTime.ToString("u", DateTimeFormatInfo.InvariantInfo);
            WriteSingleElement("DateTime", localTimeString);
        }

        /// <summary>
        /// Writes the value of the <see cref="Exception.Message"/> property to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="message">The message to write.</param>
        protected override void WriteMessage(string message)
        {
            WriteSingleElement("Message", message);
        }

        /// <summary>
        /// Writes a generic description to the <see cref="XmlWriter"/>.
        /// </summary>
        protected override void WriteDescription()
        {
            WriteSingleElement("Description", string.Format(Resources.Culture, Resources.ExceptionWasCaught, base.Exception.GetType().FullName));
        }

        /// <summary>
        /// Writes the value of the specified help link taken
        /// from the value of the <see cref="Exception.HelpLink"/>
        /// property to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="helpLink">The exception's help link.</param>
        protected override void WriteHelpLink(string helpLink)
        {
            WriteSingleElement("HelpLink", helpLink);
        }

        /// <summary>
        /// Writes the value of the specified stack trace taken from the value of the <see cref="Exception.StackTrace"/> property to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="stackTrace">The stack trace of the exception.</param>
        protected override void WriteStackTrace(string stackTrace)
        {
            WriteSingleElement("StackTrace", stackTrace);
        }

        /// <summary>
        /// Writes the value of the specified source taken from the value of the <see cref="Exception.Source"/> property to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="source">The source of the exception.</param>
        protected override void WriteSource(string source)
        {
            WriteSingleElement("Source", source);
        }

        /// <summary>
        /// Writes the value of the <see cref="Type.AssemblyQualifiedName"/>
        /// property for the specified exception type to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="exceptionType">The <see cref="Type"/> of the exception.</param>
        protected override void WriteExceptionType(Type exceptionType)
        {
            WriteSingleElement("ExceptionType", exceptionType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Writes and formats the exception and all nested inner exceptions to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="exceptionToFormat">The exception to format.</param>
        /// <param name="outerException">The outer exception. This value will be null when writing the outer-most exception.</param>
        protected override void WriteException(Exception exceptionToFormat, Exception outerException)
        {
            if (outerException != null)
            {
                Writer.WriteStartElement("InnerException");

                base.WriteException(exceptionToFormat, outerException);

                Writer.WriteEndElement();
            }
            else
            {
                base.WriteException(exceptionToFormat, outerException);
            }
        }

        /// <summary>
        /// Writes the name and value of the specified property to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="propertyInfo">The reflected <see cref="PropertyInfo"/> object.</param>
        /// <param name="value">The value of the <see cref="PropertyInfo"/> object.</param>
        protected override void WritePropertyInfo(PropertyInfo propertyInfo, object value)
        {
            string propertyValueString = Resources.UndefinedValue;

            if (value != null)
            {
                propertyValueString = value.ToString();
            }

            Writer.WriteStartElement("Property");
            Writer.WriteAttributeString("name", propertyInfo.Name);
            Writer.WriteString(propertyValueString);
            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the name and value of the <see cref="FieldInfo"/> object to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="fieldInfo">The reflected <see cref="FieldInfo"/> object.</param>
        /// <param name="value">The value of the <see cref="FieldInfo"/> object.</param>
        protected override void WriteFieldInfo(FieldInfo fieldInfo, object value)
        {
            string fieldValueString = Resources.UndefinedValue;

            if (fieldValueString != null)
            {
                fieldValueString = value.ToString();
            }

            Writer.WriteStartElement("Field");
            Writer.WriteAttributeString("name", fieldInfo.Name);
            Writer.WriteString(value.ToString());
            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes additional information to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="additionalInformation">Additional information to be included with the exception report</param>
        protected override void WriteAdditionalInfo(NameValueCollection additionalInformation)
        {
            Writer.WriteStartElement("additionalInfo");

            foreach (string name in additionalInformation.AllKeys)
            {
                Writer.WriteStartElement("info");
                Writer.WriteAttributeString("name", name);
                Writer.WriteAttributeString("value", additionalInformation[name]);
                Writer.WriteEndElement();
            }

            Writer.WriteEndElement();
        }

        private void WriteSingleElement(string elementName, string elementText)
        {
            Writer.WriteStartElement(elementName);
            Writer.WriteString(elementText);
            Writer.WriteEndElement();
        }
    }
}
