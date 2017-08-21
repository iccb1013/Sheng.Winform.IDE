/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Modules.StartPageModule
{
    static class History
    {
        static HistoryItemCollection _historyItemCollection = new HistoryItemCollection();
        static IEnvironmentService _environmentService = ServiceUnity.Container.Resolve<IEnvironmentService>();
        static string _fileName = Path.Combine(_environmentService.DataPath, Constant.HISTORY_FILENAME);
        static History()
        {
            Load();
        }
        public static void Add(string value)
        {
            Debug.Assert(String.IsNullOrEmpty(value) == false, "History.Add(value),value为空");
            if (String.IsNullOrEmpty(value))
                return;
            if (_historyItemCollection.Contains(value))
            {
                _historyItemCollection.Remove(value);
            }
            _historyItemCollection.Add(value);
            if (_historyItemCollection.Count > Constant.HISTORY_MAXCOUNT)
            {
                _historyItemCollection.RemoveAt(_historyItemCollection.Count - 1);
            }
            Save();
        }
        public static void Remove(string value)
        {
            Debug.Assert(String.IsNullOrEmpty(value) == false, "History.Remove(value),value为空");
            if (String.IsNullOrEmpty(value))
                return;
            _historyItemCollection.Remove(value);
            Save();
        }
        public static List<string> List
        {
            get
            {
                List<string> list = new List<string>();
                for (int i = 0; i < _historyItemCollection.Count; i++)
                {
                    list.Add(_historyItemCollection[i]);
                }
                list.Reverse();
                return list;
            }
        }
        private static void Save()
        {
            XElement xmlDoc = new XElement("History");
            for (int i = 0; i < _historyItemCollection.Count; i++)
            {
                xmlDoc.Add(new XElement("Item", _historyItemCollection[i]));
            }
            xmlDoc.Save(_fileName);
        }
        private static void Load()
        {
            _historyItemCollection.Clear();
            FileInfo fi = new FileInfo(_fileName);
            if (fi.Exists == false)
                return;
            XElement xmlDoc = XElement.Load(fi.FullName);
            foreach (XElement element in xmlDoc.Elements("Item"))
            {
                _historyItemCollection.Add(element.Value);
            }
        }
    }
    class HistoryItemCollection : CollectionBase
    {
        public HistoryItemCollection()
        {
        }
        public HistoryItemCollection(HistoryItemCollection value)
        {
            this.AddRange(value);
        }
        public HistoryItemCollection(String[] value)
        {
            this.AddRange(value);
        }
        public String this[int index]
        {
            get
            {
                return ((String)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(String value)
        {
            return List.Add(value);
        }
        public void AddRange(String[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(HistoryItemCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(String value)
        {
            return List.Contains(value);
        }
        public void CopyTo(String[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(String value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, String value)
        {
            List.Insert(index, value);
        }
        public new EntityEnumerator GetEnumerator()
        {
            return new EntityEnumerator(this);
        }
        public void Remove(String value)
        {
            List.Remove(value);
        }
        public class EntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public EntityEnumerator(HistoryItemCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public String Current
            {
                get
                {
                    return ((String)(baseEnumerator.Current));
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return baseEnumerator.Current;
                }
            }
            public bool MoveNext()
            {
                return baseEnumerator.MoveNext();
            }
            bool IEnumerator.MoveNext()
            {
                return baseEnumerator.MoveNext();
            }
            public void Reset()
            {
                baseEnumerator.Reset();
            }
            void IEnumerator.Reset()
            {
                baseEnumerator.Reset();
            }
        }
    }
}
