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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
    /// <summary>
    /// Summary description for LogFilterHelperFixture
    /// </summary>
    [TestClass]
    public class LogFilterHelperFixture
    {
        [TestMethod]
        public void CanRetrieveLogFiltersByType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enable", true));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            CategoryFilter categoryFilter = helper.GetFilter<CategoryFilter>();
            PriorityFilter priorityFilter = helper.GetFilter<PriorityFilter>();
            LogEnabledFilter enabledFilter = helper.GetFilter<LogEnabledFilter>();

            Assert.IsNotNull(categoryFilter);
            Assert.AreEqual(4, categoryFilter.CategoryFilters.Count);
            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(100, priorityFilter.MinimumPriority);
            Assert.IsNotNull(enabledFilter);
            Assert.IsTrue(enabledFilter.Enabled);
        }

        [TestMethod]
        public void NonExistentFilterReturnsNullByType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();
            filters.Add(new CategoryFilter("category", new List<string>(), CategoryFilterMode.AllowAllExceptDenied));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            CategoryFilter categoryFilter = helper.GetFilter<CategoryFilter>();
            PriorityFilter priorityFilter = helper.GetFilter<PriorityFilter>();
            LogEnabledFilter enabledFilter = helper.GetFilter<LogEnabledFilter>();

            Assert.IsNotNull(categoryFilter);
            Assert.IsNull(priorityFilter);
            Assert.IsNull(enabledFilter);
        }

        [TestMethod]
        public void MultipleFiltersOfSameTypeReturnsFirstByType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority1", 100));
            filters.Add(new PriorityFilter("priority2", 200));
            filters.Add(new LogEnabledFilter("enable", true));
            filters.Add(new PriorityFilter("priority2", 300));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            PriorityFilter priorityFilter = helper.GetFilter<PriorityFilter>();

            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(100, priorityFilter.MinimumPriority);
        }

        [TestMethod]
        public void CanRetrieveLogFiltersByName()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enabled", true));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            ILogFilter categoryFilter = helper.GetFilter("category");

            Assert.AreEqual(4, ((CategoryFilter)categoryFilter).CategoryFilters.Count);
        }

        [TestMethod]
        public void NonExistentNameReturnNullByName()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enabled", true));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            ILogFilter categoryFilter = helper.GetFilter("unknown");

            Assert.IsNull(categoryFilter);
        }

        [TestMethod]
        public void CanRetrieveLogFiltersByTypeAndName()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enabled", true));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            CategoryFilter categoryFilter = helper.GetFilter<CategoryFilter>("category");
            PriorityFilter priorityFilter = helper.GetFilter<PriorityFilter>("priority");
            LogEnabledFilter enabledFilter = helper.GetFilter<LogEnabledFilter>("enabled");

            Assert.IsNotNull(categoryFilter);
            Assert.AreEqual(4, categoryFilter.CategoryFilters.Count);
            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(100, priorityFilter.MinimumPriority);
            Assert.IsNotNull(enabledFilter);
            Assert.IsTrue(enabledFilter.Enabled);
        }

        [TestMethod]
        public void NonExistentNameReturnNullByNameAndType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enabled", true));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            CategoryFilter categoryFilter = helper.GetFilter<CategoryFilter>("unknown");

            Assert.IsNull(categoryFilter);
        }

        [TestMethod]
        public void ExistingNameForDifferentFilterTypeNameReturnNullByNameAndType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enabled", true));

            LogFilterHelper helper = new LogFilterHelper(filters, new MockLogFilterErrorHandler(true));
            CategoryFilter categoryFilter = helper.GetFilter<CategoryFilter>("priority");

            Assert.IsNull(categoryFilter);
        }

        [TestMethod]
        public void NoErrorsForFiltersResultsInNoNotifications()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(true);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();

            filterHelper.CheckFilters(log);
            Assert.AreEqual(0, handler.failingFilters.Count);
        }

        [TestMethod]
        public void SingleErrorForFiltersResultsInSingleNotification()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new ExceptionThrowingLogFilter("exception"));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(true);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();

            filterHelper.CheckFilters(log);
            Assert.AreEqual(1, handler.failingFilters.Count);
        }

        [TestMethod]
        public void MultipleErrorForFiltersResultsInMatchingNotificationsIfHandlerReturnsTrue()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new ExceptionThrowingLogFilter("exception1"));
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new ExceptionThrowingLogFilter("exception2"));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(true);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();

            filterHelper.CheckFilters(log);
            Assert.AreEqual(2, handler.failingFilters.Count);
        }

        [TestMethod]
        public void MultipleErrorForFiltersResultsInSingleNotificationIfHandlerReturnsFalse()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new ExceptionThrowingLogFilter("exception1"));
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new ExceptionThrowingLogFilter("exception2"));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(false);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();

            filterHelper.CheckFilters(log);
            Assert.AreEqual(1, handler.failingFilters.Count);
        }

        [TestMethod]
        public void HandlerReturnTrueWhenOthersFilterSucceedIsSuccess()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new ExceptionThrowingLogFilter("exception1"));
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(true);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();

            Assert.IsTrue(filterHelper.CheckFilters(log));
            Assert.AreEqual(1, handler.failingFilters.Count);
        }

        [TestMethod]
        public void HandlerReturnTrueWhenOthersFilterFailIsFailure()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new ExceptionThrowingLogFilter("exception1"));
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(true);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();
            log.Priority = 1;

            Assert.IsFalse(filterHelper.CheckFilters(log));
            Assert.AreEqual(1, handler.failingFilters.Count);
        }

        [TestMethod]
        public void HandlerReturnFalseWhenOthersFilterSucceedIsFailure()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new ExceptionThrowingLogFilter("exception1"));
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(false);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();

            Assert.IsFalse(filterHelper.CheckFilters(log));
            Assert.AreEqual(1, handler.failingFilters.Count);
        }

        [TestMethod]
        public void HandlerReturnFalseWhenOthersFilterFailIsFailure()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new ExceptionThrowingLogFilter("exception1"));
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));

            filters.Add(new LogEnabledFilter("enable", true));

            MockLogFilterErrorHandler handler = new MockLogFilterErrorHandler(false);
            LogFilterHelper filterHelper = new LogFilterHelper(filters, handler);

            LogEntry log = CommonUtil.GetDefaultLogEntry();
            log.Priority = 1;

            Assert.IsFalse(filterHelper.CheckFilters(log));
            Assert.AreEqual(1, handler.failingFilters.Count);
        }
    }
}
