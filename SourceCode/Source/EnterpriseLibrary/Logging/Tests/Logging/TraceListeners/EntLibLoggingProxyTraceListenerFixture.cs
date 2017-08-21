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
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    /// <summary>
    /// Summary description for EntLibLoggingProxyTraceListenerFixture
    /// </summary>
    [TestClass]
    public class EntLibLoggingProxyTraceListenerFixture
    {
        EntLibLoggingProxyTraceListener proxy;

        [TestInitialize]
        public void SetUp()
        {
            proxy = new EntLibLoggingProxyTraceListener();
            MockTraceListener.Reset();
        }

        [TestCleanup]
        public void Teardown()
        {
            proxy.Dispose();
        }

        [TestMethod]
        public void ProxyXPathNavigatorData()
        {
            XPathNavigator navigator = new XPathDocument(new StringReader(CommonUtil.Xml)).CreateNavigator();
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, navigator);

            Assert.IsTrue(MockTraceListener.LastEntry is XmlLogEntry);

            XmlLogEntry lastEntry = MockTraceListener.LastEntry as XmlLogEntry;
            Assert.AreEqual(lastEntry.Xml, navigator);
        }

        [TestMethod]
        public void ProxyStringData()
        {
            string data = "someData";
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, data);

            LogEntry lastEntry = MockTraceListener.LastEntry;
            Assert.IsNotNull(lastEntry);
            Assert.AreEqual(lastEntry.Message, data);
        }

        [TestMethod]
        public void ProxyLogEntryData()
        {
            LogEntry entry = CommonUtil.CreateLogEntry();
            int eventId = 1;
            entry.EventId = eventId;
            entry.Categories.Add(CommonUtil.ServiceModelCategory);
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, eventId, entry);

            LogEntry lastEntry = MockTraceListener.LastEntry;
            Assert.IsNotNull(lastEntry);
            CommonUtil.AssertLogEntries(lastEntry, entry);
        }

        [TestMethod]
        public void ProxyXmlLogEntryData()
        {
            XmlLogEntry entry = CommonUtil.CreateXmlLogEntry();
            int eventId = 1;
            entry.EventId = eventId;
            entry.Categories.Add(CommonUtil.ServiceModelCategory);
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, eventId, entry);

            Assert.IsTrue(MockTraceListener.LastEntry is XmlLogEntry);
            XmlLogEntry lastEntry = MockTraceListener.LastEntry as XmlLogEntry;
            Assert.IsNotNull(lastEntry);
            CommonUtil.AssertXmlLogEntries(lastEntry, entry);
        }

        [TestMethod]
        public void CanGetInstanceFromConfigurationObject()
        {
            TraceSource traceSource = new TraceSource("entlibproxy");
            EntLibLoggingProxyTraceListener listener = traceSource.Listeners["entlibproxy"] as EntLibLoggingProxyTraceListener;

            Assert.IsNotNull(listener);
            Assert.AreEqual(0, listener.CategoriesXPathQueries.Count);
            Assert.AreEqual(0, listener.NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.Local).Count);
        }

        [TestMethod]
        public void CanGetInstanceWithSinglePathAndSingleNamespaceFromConfigurationObject()
        {
            TraceSource traceSource = new TraceSource("entlibproxy");
            EntLibLoggingProxyTraceListener listener = traceSource.Listeners["entlibproxywithxpath"] as EntLibLoggingProxyTraceListener;

            Assert.IsNotNull(listener);
            Assert.AreEqual(1, listener.CategoriesXPathQueries.Count);
            Assert.AreEqual("//MessageLogTraceRecord/@Source", listener.CategoriesXPathQueries[0]);
            Assert.AreEqual(1, listener.NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.Local).Count);
            Assert.AreEqual("urn:test", listener.NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.Local)["pre"]);
        }

        [TestMethod]
        public void CanGetInstanceWithMultiplePathsFromConfigurationObject()
        {
            TraceSource traceSource = new TraceSource("entlibproxy");
            EntLibLoggingProxyTraceListener listener = traceSource.Listeners["entlibproxywithmultiplexpaths"] as EntLibLoggingProxyTraceListener;

            Assert.IsNotNull(listener);
            Assert.AreEqual(2, listener.CategoriesXPathQueries.Count);
            Assert.AreEqual("//MessageLogTraceRecord/@Source", listener.CategoriesXPathQueries[0]);
            Assert.AreEqual("//MessageLogTraceRecord/@Source2", listener.CategoriesXPathQueries[1]);
            Assert.AreEqual(2, listener.NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.Local).Count);
            Assert.AreEqual("urn:test", listener.NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.Local)["pre"]);
            Assert.AreEqual("urn:test2", listener.NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.Local)["pre2"]);
        }

        [TestMethod]
        public void SplittingEmptyXPathsStringReturnsEmptyList()
        {
            string xpathsStrings = "";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(0, xpaths.Count);
        }

        [TestMethod]
        public void SplittingSinglePathXPathsStringReturnsSingleElementList()
        {
            string xpathsStrings = "single path";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(1, xpaths.Count);
            Assert.AreEqual("single path", xpaths[0]);
        }

        [TestMethod]
        public void SplittingEscapedSemicolonReturnsSingleElementList()
        {
            string xpathsStrings = @"single\;path";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(1, xpaths.Count);
            Assert.AreEqual("single;path", xpaths[0]);
        }

        [TestMethod]
        public void SplittingWithSingleSemicolonReturnsTwoElementsList()
        {
            string xpathsStrings = @"multiple;paths";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(2, xpaths.Count);
            Assert.AreEqual("multiple", xpaths[0]);
            Assert.AreEqual("paths", xpaths[1]);
        }

        [TestMethod]
        public void SplittingWithTwoSemicolonsReturnsThreeElementsList()
        {
            string xpathsStrings = @"three;multiple;paths";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(3, xpaths.Count);
            Assert.AreEqual("three", xpaths[0]);
            Assert.AreEqual("multiple", xpaths[1]);
            Assert.AreEqual("paths", xpaths[2]);
        }

        [TestMethod]
        public void SplittingWithTrailingSemiColonIsOk()
        {
            string xpathsStrings = @"three;multiple;paths;";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(3, xpaths.Count);
            Assert.AreEqual("three", xpaths[0]);
            Assert.AreEqual("multiple", xpaths[1]);
            Assert.AreEqual("paths", xpaths[2]);
        }

        [TestMethod]
        public void SplittingMixedEscapedAndNonEscapedSemicolonsIsOk()
        {
            string xpathsStrings = @"thr\;ee;\;multiple;paths\;;";

            IList<string> xpaths = EntLibLoggingProxyTraceListener.SplitXPathQueriesString(xpathsStrings);

            Assert.AreEqual(3, xpaths.Count);
            Assert.AreEqual("thr;ee", xpaths[0]);
            Assert.AreEqual(";multiple", xpaths[1]);
            Assert.AreEqual("paths;", xpaths[2]);
        }

        [TestMethod]
        public void ExtractingNamespacesWithEmptyStringReturnsEmptyDictionary()
        {
            string namespacesString = @"";

            IDictionary<string, string> namespaces = EntLibLoggingProxyTraceListener.SplitNamespacesString(namespacesString);

            Assert.AreEqual(0, namespaces.Count);
        }

        [TestMethod]
        public void ExtractingNamespacesWithSingleEntryReturnsSingleEntryDictionary()
        {
            string namespacesString = @"xmlns:pre='urn:test'";

            IDictionary<string, string> namespaces = EntLibLoggingProxyTraceListener.SplitNamespacesString(namespacesString);

            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("urn:test", namespaces["pre"]);
        }

        [TestMethod]
        public void ExtractingNamespacesWithSingleEntryWithNoPrefixReturnsEmptyDictionary()
        {
            string namespacesString = @"xmlns='urn:test'";

            IDictionary<string, string> namespaces = EntLibLoggingProxyTraceListener.SplitNamespacesString(namespacesString);

            Assert.AreEqual(0, namespaces.Count);
        }

        [TestMethod]
        public void ExtractingNamespacesWithMultipleEntriesWithNoPrefixReturnsMultiEntryDictionaryAndIgnoresNoPrefix()
        {
            string namespacesString = @"xmlns='urn:test' xmlns:='urn:test' xmlns:pre2='urn:test2'	xmlns:pre1='http://microsoft.com'";

            IDictionary<string, string> namespaces = EntLibLoggingProxyTraceListener.SplitNamespacesString(namespacesString);

            Assert.AreEqual(2, namespaces.Count);
            Assert.AreEqual("urn:test2", namespaces["pre2"]);
            Assert.AreEqual("http://microsoft.com", namespaces["pre1"]);
        }

        [TestMethod]
        public void ExtractingNamespacesWithMultipleEntriesWithWrongSyntaxIgnoresWrongEntries()
        {
            string namespacesString = @"xmlns'urn:test' xmlns:pre2='urn:test2'	xmlns:pre1'http://microsoft.com'";

            IDictionary<string, string> namespaces = EntLibLoggingProxyTraceListener.SplitNamespacesString(namespacesString);

            Assert.AreEqual(1, namespaces.Count);
            Assert.AreEqual("urn:test2", namespaces["pre2"]);
        }

        string xmlPayloadString = @"<?xml version='1.0'?>
<E2ETraceEvent xmlns='http://schemas.microsoft.com/2004/06/E2ETraceEvent'>
	<System xmlns='http://schemas.microsoft.com/2004/06/windows/eventlog/system'>
		<EventID>0</EventID>
		<Type>3</Type>
		<SubType Name='Information'>0</SubType>
		<Level>8</Level>
		<TimeCreated SystemTime='2006-10-18T02:58:03.8287806Z'/>
		<Source Name='System.ServiceModel.MessageLogging'/>
		<Correlation ActivityID='{de8d38a9-dcb2-4a18-97c7-e026fbe610b0}'/>
		<Execution ProcessName='CalculatorService' ProcessID='592' ThreadID='3'/>
		<Channel/>
		<Computer>HKAWANO-VISTA</Computer>
	</System>
	<ApplicationData>
		<TraceData>
			<DataItem>
				<MessageLogTraceRecord Time='2006-10-18T11:58:03.8287806+09:00' Source='TransportSend' Type='System.ServiceModel.Channels.BodyWriterMessage' xmlns='http://schemas.microsoft.com/2004/06/ServiceModel/Management/MessageTrace'>
					<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/' xmlns:a='http://www.w3.org/2005/08/addressing'>
						<s:Header>
							<a:Action s:mustUnderstand='1'>http://RMCalculator/20061017/RMCalculator/MultiplyResponse</a:Action>
							<ActivityId CorrelationId='52ab5a81-e8ee-4f15-aecb-6bc5104c2c10' xmlns='http://schemas.microsoft.com/2004/09/ServiceModel/Diagnostics'>de8d38a9-dcb2-4a18-97c7-e026fbe610b0</ActivityId>
							<a:RelatesTo>urn:uuid:2778c51a-9941-4c19-b423-6c76b2374bdd</a:RelatesTo>
						</s:Header>
						<s:Body>
							<MultiplyResponse xmlns='http://RMCalculator/20061017/'>
								<MultiplyResult>10</MultiplyResult>
							</MultiplyResponse>
						</s:Body>
					</s:Envelope>
				</MessageLogTraceRecord>
			</DataItem>
		</TraceData>
	</ApplicationData>
</E2ETraceEvent>";

        [TestMethod]
        public void ShouldFilterTrace()
        {
            proxy.Filter = new EventTypeFilter(SourceLevels.Off);

            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, "Message");

            Assert.AreEqual(0, MockTraceListener.ProcessedTraceRequests);
        }

        [TestMethod]
        public void ShouldTraceUsingFilter()
        {
            proxy.Filter = new EventTypeFilter(SourceLevels.Error);

            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Critical, 1, "Message");

            Assert.AreEqual(1, MockTraceListener.ProcessedTraceRequests);
        }

        [TestMethod]
        public void TracingXPathNavigatorWithoutConfiguredCategoriesQueryCreatesLogEntryWithoutCategories()
        {
            XPathNavigator navigator = new XPathDocument(new StringReader(xmlPayloadString)).CreateNavigator();
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, navigator);

            Assert.IsTrue(MockTraceListener.LastEntry is XmlLogEntry);

            XmlLogEntry lastEntry = MockTraceListener.LastEntry as XmlLogEntry;
            Assert.AreSame(lastEntry.Xml, navigator);
            Assert.AreEqual(1, lastEntry.Categories.Count);
            Assert.IsTrue(lastEntry.Categories.Contains(CommonUtil.ServiceModelCategory));
        }

        [TestMethod]
        public void TracingXPathNavigatorWithConfiguredCategoriesQueryForNonExistingValueCreatesLogEntryWithoutCategories()
        {
            XPathNavigator navigator = new XPathDocument(new StringReader(xmlPayloadString)).CreateNavigator();
            proxy.Attributes.Add("categoriesXPathQueries", @"//NonExistingNode/@NonExistingAttribute");
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, navigator);

            Assert.IsTrue(MockTraceListener.LastEntry is XmlLogEntry);

            XmlLogEntry lastEntry = MockTraceListener.LastEntry as XmlLogEntry;
            Assert.AreSame(lastEntry.Xml, navigator);
            Assert.AreEqual(1, lastEntry.Categories.Count);
            Assert.IsTrue(lastEntry.Categories.Contains(CommonUtil.ServiceModelCategory));
        }

        [TestMethod]
        public void TracingXPathNavigatorWithConfiguredCategoriesQueryForExistingValueCreatesLogEntryWithExtraCategories()
        {
            XPathNavigator navigator = new XPathDocument(new StringReader(xmlPayloadString)).CreateNavigator();
            proxy.Attributes.Add("categoriesXPathQueries", @"//mt:MessageLogTraceRecord/@Source");
            proxy.Attributes.Add("namespaces", @"xmlns:mt='http://schemas.microsoft.com/2004/06/ServiceModel/Management/MessageTrace'");
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, navigator);

            Assert.IsTrue(MockTraceListener.LastEntry is XmlLogEntry);

            XmlLogEntry lastEntry = MockTraceListener.LastEntry as XmlLogEntry;
            Assert.AreSame(lastEntry.Xml, navigator);
            Assert.AreEqual(2, lastEntry.Categories.Count);
            Assert.IsTrue(lastEntry.Categories.Contains(CommonUtil.ServiceModelCategory));
            Assert.IsTrue(lastEntry.Categories.Contains(@"TransportSend"));
        }

        [TestMethod]
        public void TracingXPathNavigatorWithMultipleConfiguredCategoriesQueryForExistingValueCreatesLogEntryWithExtraCategories()
        {
            XPathNavigator navigator = new XPathDocument(new StringReader(xmlPayloadString)).CreateNavigator();
            proxy.Attributes.Add("categoriesXPathQueries", @"//mt:MessageLogTraceRecord/@Source;//a:Action/text()");
            proxy.Attributes.Add("namespaces", @"xmlns:mt='http://schemas.microsoft.com/2004/06/ServiceModel/Management/MessageTrace'
													xmlns:a='http://www.w3.org/2005/08/addressing'");
            proxy.TraceData(new TraceEventCache(), CommonUtil.ServiceModelCategory, TraceEventType.Error, 1, navigator);

            Assert.IsTrue(MockTraceListener.LastEntry is XmlLogEntry);

            XmlLogEntry lastEntry = MockTraceListener.LastEntry as XmlLogEntry;
            Assert.AreSame(lastEntry.Xml, navigator);
            Assert.AreEqual(3, lastEntry.Categories.Count);
            Assert.IsTrue(lastEntry.Categories.Contains(CommonUtil.ServiceModelCategory));
            Assert.IsTrue(lastEntry.Categories.Contains(@"TransportSend"));
            Assert.IsTrue(lastEntry.Categories.Contains(@"http://RMCalculator/20061017/RMCalculator/MultiplyResponse"));
        }

        [TestMethod]
        public void WriteLineGoesThroughLoggingBlock()
        {
            proxy.WriteLine("test");

            LogEntry lastEntry = MockTraceListener.LastEntry;
            Assert.IsNotNull(lastEntry);
            Assert.AreEqual("test", lastEntry.Message);
        }

        [TestMethod]
        public void TraceTransferGoesThroughLoggingBlock()
        {
            Guid relatedActivityGuid = Guid.NewGuid();

            Assert.AreEqual(0, MockTraceListener.TransferGuids.Count);

            proxy.TraceTransfer(new TraceEventCache(), string.Empty, 1000, "message", relatedActivityGuid);

            Assert.AreEqual(1, MockTraceListener.TransferGuids.Count);
            Assert.AreEqual(relatedActivityGuid, MockTraceListener.TransferGuids[0]);
        }
    }
}
