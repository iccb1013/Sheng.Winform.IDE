/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class LogEntrySerializationFixture
    {
        [TestMethod]
        public void CanSerializeStockLogEntry()
        {
            var entry = new LogEntry
            {
                ActivityId = Guid.NewGuid(),
                Categories = {"A", "B", "C"},
                Message = "Message"
            };
            Serialize(entry);
        }
        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void CannotSerializeDerivedList()
        {
            Serialize(new NonSerializableList());
        }
        [TestMethod]
        public void CanSerializeLogEntryThatContainsNonSerializableCategoryCollection()
        {
            var categories = new NonSerializableList
            {
                "one",
                "two",
                "five"
            };
            var entry = new LogEntry()
            {
                Message = "some message",
                Categories = categories
            };
            Serialize(entry);
        }
        [TestMethod]
        public void NonSerializableCategoriesSurviveSerialization()
        {
            var categories = new NonSerializableList
            {
                "one",
                "two",
                "five"
            };
            var entry = new LogEntry()
            {
                Message = "some message",
                Categories = categories
            };
            byte[] data = Serialize(entry);
            LogEntry deserializedEntry = Deserialize<LogEntry>(data);
            AssertAreEqual(entry.Categories, deserializedEntry.Categories);
        }
        private static byte[] Serialize(object entry)
        {
            IFormatter formatter = new BinaryFormatter();
            using(var stream = new MemoryStream())
            {
                formatter.Serialize(stream, entry);
                return stream.ToArray();
            }
        }
        private static T Deserialize<T>(byte[] serializedObject)
        {
            IFormatter formatter = new BinaryFormatter();
            using(var stream = new MemoryStream(serializedObject))
            {
                object result = formatter.Deserialize(stream);
                return (T) result;
            }
        }
        private void AssertAreEqual<T>(ICollection<T> expected, ICollection<T> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Collections have different lengths, expected {0}, actual {1}", expected.Count, actual.Count);
            IEnumerator<T> expectedEnumerator = expected.GetEnumerator();
            IEnumerator<T> actualEnumerator = actual.GetEnumerator();
            for(int i = 0; i < expected.Count; ++i)
            {
                expectedEnumerator.MoveNext();
                actualEnumerator.MoveNext();
                Assert.AreEqual(expectedEnumerator.Current, actualEnumerator.Current,
                    "Collections have different values at index {0}", i);
            }
        }
        internal class NonSerializableList : List<string>
        {
        }
    }
}
