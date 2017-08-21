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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Performs logging to a file and rolls the output file when either time or size thresholds are 
    /// exceeded.
    /// </summary>
    /// <remarks>
    /// Logging always occurs to the configured file name, and when roll occurs a new rolled file name is calculated
    /// by adding the timestamp pattern to the configured file name.
    /// <para/>
    /// The need of rolling is calculated before performing a logging operation, so even if the thresholds are exceeded
    /// roll will not occur until a new entry is logged.
    /// <para/>
    /// Both time and size thresholds can be configured, and when the first of them occurs both will be reset.
    /// <para/>
    /// The elapsed time is calculated from the creation date of the logging file.
    /// </remarks>
    public class RollingFlatFileTraceListener : FlatFileTraceListener
    {
        RollFileExistsBehavior rollFileExistsBehavior;
        StreamWriterRollingHelper rollingHelper;

        RollInterval rollInterval;
        int rollSizeInBytes;
        string timeStampPattern;

        /// <summary>
        /// Initializes a new instance of <see cref="RollingFlatFileTraceListener"/> 
        /// </summary>
        /// <param name="fileName">The filename where the entries will be logged.</param>
        /// <param name="header">The header to add before logging an entry.</param>
        /// <param name="footer">The footer to add after logging an entry.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="rollSizeKB">The maxium file size (KB) before rolling.</param>
        /// <param name="timeStampPattern">The date format that will be appended to the new roll file.</param>
        /// <param name="rollFileExistsBehavior">Expected behavior that will be used when the rool file has to be created.</param>
        /// <param name="rollInterval">The time interval that makes the file rolles.</param>
        public RollingFlatFileTraceListener(string fileName,
                                            string header,
                                            string footer,
                                            ILogFormatter formatter,
                                            int rollSizeKB,
                                            string timeStampPattern,
                                            RollFileExistsBehavior rollFileExistsBehavior,
                                            RollInterval rollInterval)
            : base(fileName, header, footer, formatter)
        {
            rollSizeInBytes = rollSizeKB * 1024;
            this.timeStampPattern = timeStampPattern;
            this.rollFileExistsBehavior = rollFileExistsBehavior;
            this.rollInterval = rollInterval;

            rollingHelper = new StreamWriterRollingHelper(this);
        }

        /// <summary>
        /// Gets the <see cref="StreamWriterRollingHelper"/> for the flat file.
        /// </summary>
        /// <value>
        /// The <see cref="StreamWriterRollingHelper"/> for the flat file.
        /// </value>
        public StreamWriterRollingHelper RollingHelper
        {
            get { return rollingHelper; }
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the file, performing a roll if necessary.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        public override void TraceData(TraceEventCache eventCache,
                                       string source,
                                       TraceEventType eventType,
                                       int id,
                                       object data)
        {
            rollingHelper.RollIfNecessary();

            base.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// A data time provider.
        /// </summary>
        public class DateTimeProvider
        {
            /// <summary>
            /// Gets the current data time.
            /// </summary>
            /// <value>
            /// The current data time.
            /// </value>
            public virtual DateTime CurrentDateTime
            {
                get { return DateTime.Now; }
            }
        }

        /// <summary>
        /// Encapsulates the logic to perform rolls.
        /// </summary>
        /// <remarks>
        /// If no rolling behavior has been configured no further processing will be performed.
        /// </remarks>
        public sealed class StreamWriterRollingHelper
        {
            DateTimeProvider dateTimeProvider;

            /// <summary>
            /// A tally keeping writer used when file size rolling is configured.<para/>
            /// The original stream writer from the base trace listener will be replaced with
            /// this listener.
            /// </summary>
            TallyKeepingFileStreamWriter managedWriter;

            DateTime? nextRollDateTime;

            /// <summary>
            /// The trace listener for which rolling is being managed.
            /// </summary>
            RollingFlatFileTraceListener owner;

            /// <summary>
            /// A flag indicating whether at least one rolling criteria has been configured.
            /// </summary>
            bool performsRolling;

            /// <summary>
            /// Initialize a new instance of the <see cref="StreamWriterRollingHelper"/> class with a <see cref="RollingFlatFileTraceListener"/>.
            /// </summary>
            /// <param name="owner">The <see cref="RollingFlatFileTraceListener"/> to use.</param>
            public StreamWriterRollingHelper(RollingFlatFileTraceListener owner)
            {
                this.owner = owner;
                dateTimeProvider = new DateTimeProvider();

                performsRolling = this.owner.rollInterval != RollInterval.None || this.owner.rollSizeInBytes > 0;
            }

            /// <summary>
            /// Gets the provider for the current date. Necessary for unit testing.
            /// </summary>
            /// <value>
            /// The provider for the current date. Necessary for unit testing.
            /// </value>
            public DateTimeProvider DateTimeProvider
            {
                set { dateTimeProvider = value; }
            }

            /// <summary>
            /// Gets the next date when date based rolling should occur if configured.
            /// </summary>
            /// <value>
            /// The next date when date based rolling should occur if configured.
            /// </value>
            public DateTime? NextRollDateTime
            {
                get { return nextRollDateTime; }
            }

            /// <summary>
            /// Calculates the next roll date for the file.
            /// </summary>
            /// <param name="dateTime">The new date.</param>
            /// <returns>The new date time to use.</returns>
            public DateTime CalculateNextRollDate(DateTime dateTime)
            {
                switch (owner.rollInterval)
                {
                    case RollInterval.Minute:
                        return dateTime.AddMinutes(1);
                    case RollInterval.Hour:
                        return dateTime.AddHours(1);
                    case RollInterval.Day:
                        return dateTime.AddDays(1);
                    case RollInterval.Week:
                        return dateTime.AddDays(7);
                    case RollInterval.Month:
                        return dateTime.AddMonths(1);
                    case RollInterval.Year:
                        return dateTime.AddYears(1);
                    case RollInterval.Midnight:
                        return dateTime.AddDays(1).Date;
                    default:
                        return DateTime.MaxValue;
                }
            }

            /// <summary>
            /// Checks whether rolling should be performed, and returns the date to use when performing the roll.
            /// </summary>
            /// <returns>The date roll to use if performing a roll, or <see langword="null"/> if no rolling should occur.</returns>
            /// <remarks>
            /// Defer request for the roll date until it is necessary to avoid overhead.<para/>
            /// Information used for rolling checks should be set by now.
            /// </remarks>
            public DateTime? CheckIsRollNecessary()
            {
                // check for size roll, if enabled.
                if (owner.rollSizeInBytes > 0
                    && (managedWriter != null && managedWriter.Tally > owner.rollSizeInBytes))
                {
                    return dateTimeProvider.CurrentDateTime;
                }

                // check for date roll, if enabled.
                DateTime currentDateTime = dateTimeProvider.CurrentDateTime;
                if (owner.rollInterval != RollInterval.None
                    && (nextRollDateTime != null && currentDateTime.CompareTo(nextRollDateTime.Value) >= 0))
                {
                    return currentDateTime;
                }

                // no roll is necessary, return a null roll date
                return null;
            }

            /// <summary>
            /// Gets the file name to use for archiving the file.
            /// </summary>
            /// <param name="actualFileName">The actual file name.</param>
            /// <param name="currentDateTime">The current date and time.</param>
            /// <returns>The new file name.</returns>
            public string ComputeArchiveFileName(string actualFileName,
                                                 DateTime currentDateTime)
            {
                string archiveFileName;

                string directory = Path.GetDirectoryName(actualFileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(actualFileName);
                string extension = Path.GetExtension(actualFileName);

                string archiveFileNameWithTimestampWithoutExtension
                    = string.IsNullOrEmpty(owner.timeStampPattern)
                        ? fileNameWithoutExtension
                        : fileNameWithoutExtension
                            + "."
                            + currentDateTime.ToString(owner.timeStampPattern, CultureInfo.InvariantCulture);

                if (owner.rollFileExistsBehavior == RollFileExistsBehavior.Overwrite)
                {
                    archiveFileName = Path.Combine(directory, archiveFileNameWithTimestampWithoutExtension + extension);
                }
                else
                {
                    // look for max sequence for date
                    int maxSequence = FindMaxSequenceNumber(directory, archiveFileNameWithTimestampWithoutExtension, extension);
                    archiveFileName = Path.Combine(directory, archiveFileNameWithTimestampWithoutExtension + "." + (maxSequence + 1) + extension);
                }

                return archiveFileName;
            }

            /// <summary>
            /// Finds the max sequence number for a log file.
            /// </summary>
            /// <param name="directoryName">The directory to scan.</param>
            /// <param name="fileName">The file name.</param>
            /// <param name="extension">The extension to use.</param>
            /// <returns>The next sequence number.</returns>
            public static int FindMaxSequenceNumber(string directoryName,
                                                    string fileName,
                                                    string extension)
            {
                string[] existingFiles = Directory.GetFiles(directoryName,
                                                            string.Format("{0}*{1}", fileName, extension));

                int maxSequence = 0;
                Regex regex = new Regex(string.Format(@"{0}\.(?<sequence>\d+){1}", fileName, extension));
                for (int i = 0; i < existingFiles.Length; i++)
                {
                    Match sequenceMatch = regex.Match(existingFiles[i]);
                    if (sequenceMatch.Success)
                    {
                        int currentSequence = 0;

                        string sequenceInFile = sequenceMatch.Groups["sequence"].Value;
                        if (!int.TryParse(sequenceInFile, out currentSequence))
                            continue; // very unlikely

                        if (currentSequence > maxSequence)
                        {
                            maxSequence = currentSequence;
                        }
                    }
                }

                return maxSequence;
            }

            static Encoding GetEncodingWithFallback()
            {
                Encoding encoding = (Encoding)new UTF8Encoding(false).Clone();
                encoding.EncoderFallback = EncoderFallback.ReplacementFallback;
                encoding.DecoderFallback = DecoderFallback.ReplacementFallback;
                return encoding;
            }

            /// <summary>
            /// Perform the roll for the next date.
            /// </summary>
            /// <param name="rollDateTime">The roll date.</param>
            public void PerformRoll(DateTime rollDateTime)
            {
                string actualFileName = ((FileStream)((StreamWriter)owner.Writer).BaseStream).Name;

                if (this.owner.rollFileExistsBehavior == RollFileExistsBehavior.Overwrite
                    && string.IsNullOrEmpty(this.owner.timeStampPattern))
                {
                    // no roll will be actually performed: no timestamp pattern is available, and 
                    // the roll behavior is overwrite, so the original file will be truncated
                    owner.Writer.Close();
                    File.WriteAllText(actualFileName, string.Empty);
                }
                else
                {
                    // calculate archive name
                    string archiveFileName = ComputeArchiveFileName(actualFileName, rollDateTime);
                    // close file
                    owner.Writer.Close();
                    // move file
                    SafeMove(actualFileName, archiveFileName, rollDateTime);
                }

                // update writer - let TWTL open the file as needed to keep consistency
                owner.Writer = null;
                managedWriter = null;
                nextRollDateTime = null;
                UpdateRollingInformationIfNecessary();
            }

            /// <summary>
            /// Rolls the file if necessary.
            /// </summary>
            public void RollIfNecessary()
            {
                if (!performsRolling)
                {
                    // avoid further processing if no rolling has been configured.
                    return;
                }

                if (!UpdateRollingInformationIfNecessary())
                {
                    // an error was detected while handling roll information - avoid further processing
                    return;
                }

                DateTime? rollDateTime;
                if ((rollDateTime = CheckIsRollNecessary()) != null)
                {
                    PerformRoll(rollDateTime.Value);
                }
            }

            void SafeMove(string actualFileName,
                          string archiveFileName,
                          DateTime currentDateTime)
            {
                try
                {
                    if (File.Exists(archiveFileName))
                    {
                        File.Delete(archiveFileName);
                    }
                    // take care of tunneling issues http://support.microsoft.com/kb/172190
                    File.SetCreationTime(actualFileName, currentDateTime);
                    File.Move(actualFileName, archiveFileName);
                }
                catch (IOException)
                {
                    // catch errors and attempt move to a new file with a GUID
                    archiveFileName = archiveFileName + Guid.NewGuid().ToString();

                    try
                    {
                        File.Move(actualFileName, archiveFileName);
                    }
                    catch (IOException) { }
                }
            }

            /// <summary>
            /// Updates bookeeping information necessary for rolling, as required by the specified
            /// rolling configuration.
            /// </summary>
            /// <returns>true if update was successful, false if an error occurred.</returns>
            public bool UpdateRollingInformationIfNecessary()
            {
                StreamWriter currentWriter = null;

                // replace writer with the tally keeping version if necessary for size rolling
                if (owner.rollSizeInBytes > 0 && managedWriter == null)
                {
                    currentWriter = owner.Writer as StreamWriter;
                    if (currentWriter == null)
                    {
                        // TWTL couldn't acquire the writer - abort
                        return false;
                    }
                    String actualFileName = ((FileStream)currentWriter.BaseStream).Name;

                    currentWriter.Close();

                    FileStream fileStream = null;
                    try
                    {
                        fileStream = File.Open(actualFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                        managedWriter = new TallyKeepingFileStreamWriter(fileStream, GetEncodingWithFallback());
                        owner.Writer = managedWriter;
                    }
                    catch (Exception)
                    {
                        // there's a slight chance of error here - abort if this occurs and just let TWTL handle it without attempting to roll
                        return false;
                    }
                }

                // compute the next roll date if necessary
                if (owner.rollInterval != RollInterval.None && nextRollDateTime == null)
                {
                    try
                    {
                        // casting should be safe at this point - only file stream writers can be the writers for the owner trace listener.
                        // it should also happen rarely
                        nextRollDateTime
                            = CalculateNextRollDate(File.GetCreationTime(((FileStream)((StreamWriter)owner.Writer).BaseStream).Name));
                    }
                    catch (Exception)
                    {
                        nextRollDateTime = DateTime.MaxValue; // disable rolling if not date could be retrieved.

                        // there's a slight chance of error here - abort if this occurs and just let TWTL handle it without attempting to roll
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Represents a file stream writer that keeps a tally of the length of the file.
        /// </summary>
        public sealed class TallyKeepingFileStreamWriter : StreamWriter
        {
            long tally;

            /// <summary>
            /// Initialize a new instance of the <see cref="TallyKeepingFileStreamWriter"/> class with a <see cref="FileStream"/>.
            /// </summary>
            /// <param name="stream">The <see cref="FileStream"/> to write to.</param>
            public TallyKeepingFileStreamWriter(FileStream stream)
                : base(stream)
            {
                tally = stream.Length;
            }

            /// <summary>
            /// Initialize a new instance of the <see cref="TallyKeepingFileStreamWriter"/> class with a <see cref="FileStream"/>.
            /// </summary>
            /// <param name="stream">The <see cref="FileStream"/> to write to.</param>
            /// <param name="encoding">The <see cref="Encoding"/> to use.</param>
            public TallyKeepingFileStreamWriter(FileStream stream,
                                                Encoding encoding)
                : base(stream, encoding)
            {
                tally = stream.Length;
            }

            /// <summary>
            /// Gets the tally of the length of the string.
            /// </summary>
            /// <value>
            /// The tally of the length of the string.
            /// </value>
            public long Tally
            {
                get { return tally; }
            }

            ///<summary>
            ///Writes a character to the stream.
            ///</summary>
            ///
            ///<param name="value">The character to write to the text stream. </param>
            ///<exception cref="T:System.ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and current writer is closed. </exception>
            ///<exception cref="T:System.NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter"></see> is at the end the stream. </exception>
            ///<exception cref="T:System.IO.IOException">An I/O error occurs. </exception><filterpriority>1</filterpriority>
            public override void Write(char value)
            {
                base.Write(value);
                tally += Encoding.GetByteCount(new char[] { value });
            }

            ///<summary>
            ///Writes a character array to the stream.
            ///</summary>
            ///
            ///<param name="buffer">A character array containing the data to write. If buffer is null, nothing is written. </param>
            ///<exception cref="T:System.ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and current writer is closed. </exception>
            ///<exception cref="T:System.NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter"></see> is at the end the stream. </exception>
            ///<exception cref="T:System.IO.IOException">An I/O error occurs. </exception><filterpriority>1</filterpriority>
            public override void Write(char[] buffer)
            {
                base.Write(buffer);
                tally += Encoding.GetByteCount(buffer);
            }

            ///<summary>
            ///Writes a subarray of characters to the stream.
            ///</summary>
            ///
            ///<param name="count">The number of characters to read from buffer. </param>
            ///<param name="buffer">A character array containing the data to write. </param>
            ///<param name="index">The index into buffer at which to begin writing. </param>
            ///<exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            ///<exception cref="T:System.ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and current writer is closed. </exception>
            ///<exception cref="T:System.NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter"></see> is at the end the stream. </exception>
            ///<exception cref="T:System.ArgumentOutOfRangeException">index or count is negative. </exception>
            ///<exception cref="T:System.ArgumentException">The buffer length minus index is less than count. </exception>
            ///<exception cref="T:System.ArgumentNullException">buffer is null. </exception><filterpriority>1</filterpriority>
            public override void Write(char[] buffer,
                                       int index,
                                       int count)
            {
                base.Write(buffer, index, count);
                tally += Encoding.GetByteCount(buffer, index, count);
            }

            ///<summary>
            ///Writes a string to the stream.
            ///</summary>
            ///
            ///<param name="value">The string to write to the stream. If value is null, nothing is written. </param>
            ///<exception cref="T:System.ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and current writer is closed. </exception>
            ///<exception cref="T:System.NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter"></see> is at the end the stream. </exception>
            ///<exception cref="T:System.IO.IOException">An I/O error occurs. </exception><filterpriority>1</filterpriority>
            public override void Write(string value)
            {
                base.Write(value);
                tally += Encoding.GetByteCount(value);
            }
        }
    }
}
