//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// Provides the concrete instrumentation for the logical events raised by a <see cref="CachingInstrumentationProvider"/> object.
    /// </summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(CounterCategoryName, "CounterCategoryHelpResourceName")]
    [EventLogDefinition("Application", EventLogSourceName)]
    public class CachingInstrumentationListener : InstrumentationListener
    {
        /// <summary>
        /// The name of the caching counters.
        /// </summary>
        public const string CounterCategoryName = "Enterprise Library Caching Counters";

        /// <summary>
        /// The name of the event log source.
        /// </summary>
        public const string EventLogSourceName = "Enterprise Library Caching";

        /// <summary>
        /// The total cache expires counter name.
        /// </summary>
        public const string TotalCacheExpiriesCounterName = "Total Cache Expiries";

        /// <summary>
        /// The total cache hits counter name.
        /// </summary>
        public const string TotalCacheHitsCounterName = "Total Cache Hits";

        /// <summary>
        /// The total cache misses counter name.
        /// </summary>
        public const string TotalCacheMissesCounterName = "Total Cache Misses";

        /// <summary>
        /// The total cache scavenged items counter name.
        /// </summary>
        public const string TotalCacheScavengedItemsCounterName = "Total Cache Scavenged Items";

        /// <summary>
        /// The total updated entries counter name.
        /// </summary>
        public const string TotalUpdatedEntriesItemsCounterName = "Total Updated Entries";
        static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

        EnterpriseLibraryPerformanceCounter cacheAccessAttemptsCounter;

        [PerformanceCounter("Cache Expiries/sec", "CacheExpiriesPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheExpiriesCounter;

        [PerformanceCounter("Cache Hit Ratio", "CacheHitRatioCounterHelpResource", PerformanceCounterType.RawFraction,
            BaseCounterName = "Total # of Cache Access Attempts", BaseCounterHelp = "CacheAccessAttemptsCounterHelpResource", BaseCounterType = PerformanceCounterType.RawBase)]
        EnterpriseLibraryPerformanceCounter cacheHitRatioCounter;

        [PerformanceCounter("Cache Hits/sec", "CacheHitsPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheHitsCounter;

        [PerformanceCounter("Cache Misses/sec", "CacheMissesPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheMissesCounter;

        [PerformanceCounter("Cache Scavenged Items/sec", "CacheScavengedItemsPerSecCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheScavengedItemsCounter;

        [PerformanceCounter("Total Cache Entries", "CacheTotalEntriesCounterHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter cacheTotalEntriesCounter;

        [PerformanceCounter("Updated Entries/sec", "CacheUpdatedEntriesPerSecHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter cacheUpdatedEntriesCounter;

        string counterInstanceName;
        IEventLogEntryFormatter eventLogEntryFormatter;
        string instanceName;

        [PerformanceCounter(TotalCacheExpiriesCounterName, "TotalCacheExpiriesCounterHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter totalCacheExpiriesCounter;

        [PerformanceCounter(TotalCacheHitsCounterName, "TotalCacheHitsCounterHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalCacheHitsCounter;

        [PerformanceCounter(TotalCacheMissesCounterName, "TotalCacheMissesCounterHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalCacheMissesCounter;

        [PerformanceCounter(TotalCacheScavengedItemsCounterName, "TotalCacheScavengedItemsCounterHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter totalCacheScavengedItemsCounter;

        [PerformanceCounter(TotalUpdatedEntriesItemsCounterName, "TotalCacheUpdatedEntriesHelpResource", PerformanceCounterType.NumberOfItems64)]
        EnterpriseLibraryPerformanceCounter totalUpdatedEntriesItemsCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingInstrumentationListener"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="CacheManager"/> instance this instrumentation listener is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
        public CachingInstrumentationListener(string instanceName,
                                              bool performanceCountersEnabled,
                                              bool eventLoggingEnabled,
                                              bool wmiEnabled,
                                              string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName)) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingInstrumentationListener"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="CacheManager"/> instance this instrumentation listener is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public CachingInstrumentationListener(string instanceName,
                                              bool performanceCountersEnabled,
                                              bool eventLoggingEnabled,
                                              bool wmiEnabled,
                                              IPerformanceCounterNameFormatter nameFormatter)
            : base(new string[] { instanceName }, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
        {
            this.instanceName = instanceName;
            counterInstanceName = CreateInstanceName(instanceName);

            eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="CachingInstrumentationProvider.cacheAccessed"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("CacheAccessed")]
        public void CacheAccessed(object sender,
                                  CacheAccessedEventArgs e)
        {
            if (PerformanceCountersEnabled)
            {
                cacheAccessAttemptsCounter.Increment();
                if (e.Hit)
                {
                    cacheHitRatioCounter.Increment();
                    cacheHitsCounter.Increment();
                    totalCacheHitsCounter.Increment();
                }
                else
                {
                    cacheMissesCounter.Increment();
                    totalCacheMissesCounter.Increment();
                }
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="CachingInstrumentationProvider.cacheCallbackFailed"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("CacheCallbackFailed")]
        public void CacheCallbackFailed(object sender,
                                        CacheCallbackFailureEventArgs e)
        {
            if (WmiEnabled)
            {
                FireManagementInstrumentation(new CacheCallbackFailureEvent(instanceName, e.Key, e.Exception.ToString()));
            }
            if (EventLoggingEnabled)
            {
                string errorMessage
                    = string.Format(
                        Resources.Culture,
                        Resources.ErrorCacheCallbackFailedMessage,
                        instanceName,
                        e.Key);
                string entryText = eventLogEntryFormatter.GetEntryText(errorMessage, e.Exception);

                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="CachingInstrumentationProvider.cacheExpired"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("CacheExpired")]
        public void CacheExpired(object sender,
                                 CacheExpiredEventArgs e)
        {
            if (PerformanceCountersEnabled)
            {
                cacheExpiriesCounter.IncrementBy(e.ItemsExpired);
                totalCacheExpiriesCounter.IncrementBy(e.ItemsExpired);
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="CachingInstrumentationProvider.cacheFailed"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("CacheFailed")]
        public void CacheFailed(object sender,
                                CacheFailureEventArgs e)
        {
            if (WmiEnabled)
            {
                FireManagementInstrumentation(new CacheFailureEvent(instanceName, e.ErrorMessage, e.Exception.ToString()));
            }
            if (EventLoggingEnabled)
            {
                string errorMessage
                    = string.Format(
                        Resources.Culture,
                        Resources.ErrorCacheOperationFailedMessage,
                        instanceName);
                string entryText = eventLogEntryFormatter.GetEntryText(errorMessage, e.Exception, e.ErrorMessage);

                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="CachingInstrumentationProvider.cacheScavenged"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("CacheScavenged")]
        public void CacheScavenged(object sender,
                                   CacheScavengedEventArgs e)
        {
            if (PerformanceCountersEnabled)
            {
                cacheScavengedItemsCounter.IncrementBy(e.ItemsScavenged);
                totalCacheScavengedItemsCounter.IncrementBy(e.ItemsScavenged);
            }
            if (WmiEnabled)
            {
                FireManagementInstrumentation(new CacheScavengedEvent(instanceName, e.ItemsScavenged));
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="CachingInstrumentationProvider.cacheUpdated"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("CacheUpdated")]
        public void CacheUpdated(object sender,
                                 CacheUpdatedEventArgs e)
        {
            if (PerformanceCountersEnabled)
            {
                cacheTotalEntriesCounter.SetValueFor(counterInstanceName, e.TotalEntriesCount);
                cacheUpdatedEntriesCounter.IncrementBy(e.UpdatedEntriesCount);
                totalUpdatedEntriesItemsCounter.IncrementBy(e.UpdatedEntriesCount);
            }
        }

        /// <summary>
        /// Creates the performance counters to instrument the caching events for the specified instance names.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            cacheHitsCounter = factory.CreateCounter(CounterCategoryName, "Cache Hits/sec", instanceNames);
            totalCacheHitsCounter = factory.CreateCounter(CounterCategoryName, TotalCacheHitsCounterName, instanceNames);

            cacheMissesCounter = factory.CreateCounter(CounterCategoryName, "Cache Misses/sec", instanceNames);
            totalCacheMissesCounter = factory.CreateCounter(CounterCategoryName, TotalCacheMissesCounterName, instanceNames);

            cacheHitRatioCounter = factory.CreateCounter(CounterCategoryName, "Cache Hit Ratio", instanceNames);
            cacheAccessAttemptsCounter = factory.CreateCounter(CounterCategoryName, "Total # of Cache Access Attempts", instanceNames);

            cacheExpiriesCounter = factory.CreateCounter(CounterCategoryName, "Cache Expiries/sec", instanceNames);
            totalCacheExpiriesCounter = factory.CreateCounter(CounterCategoryName, TotalCacheExpiriesCounterName, instanceNames);

            cacheScavengedItemsCounter = factory.CreateCounter(CounterCategoryName, "Cache Scavenged Items/sec", instanceNames);
            totalCacheScavengedItemsCounter = factory.CreateCounter(CounterCategoryName, TotalCacheScavengedItemsCounterName, instanceNames);

            cacheTotalEntriesCounter = factory.CreateCounter(CounterCategoryName, "Total Cache Entries", instanceNames);
            cacheUpdatedEntriesCounter = factory.CreateCounter(CounterCategoryName, "Updated Entries/sec", instanceNames);
            totalUpdatedEntriesItemsCounter = factory.CreateCounter(CounterCategoryName, TotalUpdatedEntriesItemsCounterName, instanceNames);
        }
    }
}
