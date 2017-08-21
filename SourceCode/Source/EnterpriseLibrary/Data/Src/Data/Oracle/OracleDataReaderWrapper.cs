//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
    /// <summary>
    /// A wrapper to convert data for oracle for the reader.
    /// </summary>
    /// <remarks>
    /// The wrapper performs type conversions to enable retrieving values for types not supported natively by the
    /// <see cref="OracleDataReader"/>.
    /// <para/>
    /// The wrapped data reader can be accessed through the <see cref="OracleDataReaderWrapper.InnerReader"/>
    /// property.
    /// </remarks>
    /// <seealso cref="IDataReader"/>
    /// <seealso cref="OracleDataReader"/>
    public class OracleDataReaderWrapper : MarshalByRefObject, IDataReader, IEnumerable
    {
        private readonly OracleDataReader innerReader;

        internal OracleDataReaderWrapper(OracleDataReader reader)
        {
            this.innerReader = reader;
        }

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the column to get.</param>
        /// <returns>The column located at the specified index as an <see cref="Object"/>.</returns>
        public object this[int index]
        {
            get { return InnerReader[index]; }
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as an <see cref="Object"/>.</returns>
        public object this[string name]
        {
            get { return InnerReader[name]; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerReader).GetEnumerator();
        }

        /// <summary>
        /// Disposes the wrapped <see cref="OracleDataReader"/> object. 
        /// </summary>
        void IDisposable.Dispose()
        {
            InnerReader.Dispose();
        }

        /// <summary>
        /// Closes the wrapped <see cref="OracleDataReader"/> object. 
        /// </summary>
        public void Close()
        {
            InnerReader.Close();
        }

        /// <summary>
        /// Returns a <see cref="DataTable"/> that describes the column metadata of the 
        /// wrapped <see cref="OracleDataReader"/>. 
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that describes the column metadata</returns>
        public DataTable GetSchemaTable()
        {
            return InnerReader.GetSchemaTable();
        }

        /// <summary>
        /// Advances the wrapped data reader to the next result, when reading the results of batch SQL statements. 
        /// </summary>
        /// <returns><see langword="true"/> if there are more rows; otherwise, <see langword="false"/>.</returns>
        public bool NextResult()
        {
            return InnerReader.NextResult();
        }

        /// <summary>
        /// Advances the wrapped <see cref="OracleDataReader"/> to the next record.
        /// </summary>
        /// <returns><see langword="true"/> if there are more rows; otherwise, <see langword="false"/>.</returns>
        public bool Read()
        {
            return InnerReader.Read();
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <value>The level of nesting.</value>
        public int Depth
        {
            get { return InnerReader.Depth; }
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value><see langword="true"/> if the data reader is closed; otherwise, <see langword="false"/>.</value>
        public bool IsClosed
        {
            get { return InnerReader.IsClosed; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <value>The number of rows changed, inserted, or deleted; 0 if no rows were affected or the statement failed; 
        /// and -1 for SELECT statements. </value>
        public int RecordsAffected
        {
            get { return InnerReader.RecordsAffected; }
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. 
        /// The default is -1.</value>
        public int FieldCount
        {
            get { return InnerReader.FieldCount; }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The converted value of the column.</returns>
        /// <remarks>
        /// Bit data type is mapped to a number in Oracle database. When reading bit data from Oracle database,
        /// it will map to 0 as false and everything else as true.  This method uses System.Convert.ToBoolean() method
        /// for type conversions.
        /// </remarks>        
        public bool GetBoolean(int index)
        {
            return Convert.ToBoolean(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value of the specified column converted to an 8-bit unsigned integer. 
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        /// <remarks> This method uses System.Convert.ToByte() method
        /// for type conversions.</remarks>
        public byte GetByte(int index)
        {
            return Convert.ToByte(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, 
        /// starting at the given buffer offset. 
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="dataIndex">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferIndex">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        public long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            return InnerReader.GetBytes(ordinal, dataIndex, buffer, bufferIndex, length);
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <returns>The character value of the specified column.</returns>
        public Char GetChar(int index)
        {
            return InnerReader.GetChar(index);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array, 
        /// starting at the given buffer offset.
        /// </summary>
        /// <param name="index">The zero-based column ordinal.</param>
        /// <param name="dataIndex">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferIndex">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        public long GetChars(int index, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            return InnerReader.GetChars(index, dataIndex, buffer, bufferIndex, length);
        }

        /// <summary>
        /// Returns an <see cref="IDataReader"/> for the specified column ordinal.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>An <see cref="IDataReader"/>.</returns>
        public IDataReader GetData(int index)
        {
            return InnerReader.GetData(index);
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        public string GetDataTypeName(int index)
        {
            return InnerReader.GetDataTypeName(index);
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="ordinal_">The index of the field to find.</param>
        /// <returns>The date and time data value of the specified field.</returns>
        public DateTime GetDateTime(int ordinal_)
        {
            return InnerReader.GetDateTime(ordinal_);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public decimal GetDecimal(int index)
        {
            return InnerReader.GetDecimal(index);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field. 
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The double-precision floating point number of the specified field.</returns>
        public double GetDouble(int index)
        {
            return InnerReader.GetDouble(index);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> information corresponding to the type of 
        /// <see cref="Object"/> that would be returned from <see cref="OracleDataReaderWrapper.GetValue"/>. 
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The <see cref="Type"/> information corresponding to the type of 
        /// <see cref="Object"/> that would be returned from <see cref="OracleDataReaderWrapper.GetValue"/>.</returns>
        public Type GetFieldType(int index)
        {
            return InnerReader.GetFieldType(index);
        }

        /// <summary>
        /// Gets the value of the specified field converted to a single-precision floating point number.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The single-precision floating point number of the specified field.</returns>
        /// <remarks>
        /// When reading number from Oracle, data reader gets it back at decimal regardless of data type in
        /// Oracle database. This will cast the result to float data type.
        /// </remarks>        
        public float GetFloat(int index)
        {
            return InnerReader.GetFloat(index);
        }

        /// <summary>
        /// Gets the value of the specified field converted to a GUID.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The GUID of the specified field.</returns>
        /// <remarks>
        /// This method will cast the result data Guid data type. In Oracle you must use that as Raw(16) so
        /// that this method can convert that to Guid properly.
        /// </remarks>        
        public Guid GetGuid(int index)
        {
            byte[] guidBuffer = (byte[])InnerReader[index];
            return new Guid(guidBuffer);
        }

        /// <summary>
        /// Gets the value of the specified field converted to a 16-bit signed integer.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public short GetInt16(int index)
        {
            return Convert.ToInt16(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public int GetInt32(int index)
        {
            return InnerReader.GetInt32(index);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public long GetInt64(int index)
        {
            return InnerReader.GetInt64(index);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        public string GetName(int index)
        {
            return InnerReader.GetName(index);
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="index">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        public int GetOrdinal(string index)
        {
            return InnerReader.GetOrdinal(index);
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public string GetString(int index)
        {
            return InnerReader.GetString(index);
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="index">The index of the field to find. </param>
        /// <returns>The <see cref="Object"/> which will contain the field value upon return.</returns>
        public object GetValue(int index)
        {
            return InnerReader.GetValue(index);
        }

        /// <summary>
        /// Gets all the attribute fields in the collection for the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="Object"/> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="Object"/> in the array.</returns>
        public int GetValues(object[] values)
        {
            return InnerReader.GetValues(values);
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>
        /// <see langword="true"/> if the specified field is set to null; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsDBNull(int index)
        {
            return InnerReader.IsDBNull(index);
        }

        /// <summary>
        /// Gets the wrapped <see cref="OracleDataReader"/>.
        /// </summary>
        public OracleDataReader InnerReader
        {
            get { return this.innerReader; }
        }
    }
}
