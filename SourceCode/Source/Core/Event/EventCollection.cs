using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Sheng.SailingEase.Core;

namespace Sheng.SailingEase.Core
{
    /// <summary>
    /// 事件对象集合
    /// 实现IList,是为了和BindingList交互
    /// </summary>
    [Serializable]
    public class EventCollection : CollectionBase,IList<EventBase>
    {
        //反序列化后的引用在EventCollection中处理
        [NonSerialized]
        private WindowEntity _hostFormEntity;
        /// <summary>
        /// 此事件序列的最终宿主FormEntity
        /// </summary>
        public WindowEntity HostFormEntity
        {
            get
            {
                return this._hostFormEntity;
            }
            set
            {
                if ((this._hostFormEntity == null && value != null) || (this._hostFormEntity == null || this._hostFormEntity.Equals(value) == false))
                {
                    this._hostFormEntity = value;

                    for (int i = 0; i < this.Count; i++)
                    {
                        this[i].HostFormEntity = value;
                    }
                }
            }
        }

        //反序列化处理后的引用在EventCollection中处理
        [NonSerialized]
        private EntityBase _hostEntity;
        /// <summary>
        /// 此事件序列（及包含的事件）的直接宿主对象
        /// </summary>
        public EntityBase HostEntity
        {
            get { return _hostEntity; }
            set
            {
                if ((this._hostEntity == null && value != null) || (this._hostEntity == null || this._hostEntity.Equals(value) == false))
                {
                    this._hostEntity = value;

                    for (int i = 0; i < this.Count; i++)
                    {
                        this[i].HostEntity = value;
                    }
                }
            }
        }

        public EventCollection(WindowEntity formEntity,EntityBase hostEntity)
        {
            this._hostFormEntity = formEntity;
            this._hostEntity = hostEntity;
        }

        public EventCollection(EventCollection value)
        {
            this.AddRange(value);
        }

        public EventCollection(EventBase[] value)
        {
            this.AddRange(value);
        }

        public EventBase this[int index]
        {
            get
            {
                return ((EventBase)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(EventBase value)
        {
            if ((value.HostFormEntity == null && this._hostFormEntity != null) || (value.HostFormEntity == null || value.HostFormEntity.Equals(this._hostFormEntity) == false))
            {
                value.HostFormEntity = this._hostFormEntity;
            }

            if ((value.HostEntity == null && this._hostEntity != null) || (value.HostEntity == null || value.HostEntity.Equals(this._hostEntity) == false))
            {
                value.HostEntity = this._hostEntity;
            }

            return List.Add(value);
        }

        public void AddRange(EventBase[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(EventCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        public bool Contains(EventBase value)
        {
            return List.Contains(value);
        }

        public void CopyTo(EventBase[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public int IndexOf(EventBase value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, EventBase value)
        {
            if ((value.HostFormEntity == null && this._hostFormEntity != null) || (value.HostFormEntity == null || value.HostFormEntity.Equals(this._hostFormEntity) == false))
            {
                value.HostFormEntity = this._hostFormEntity;
            }

            if ((value.HostEntity == null && this._hostEntity != null) || (value.HostEntity == null || value.HostEntity.Equals(this._hostEntity) == false))
            {
                value.HostEntity = this._hostEntity;
            }

            List.Insert(index, value);
        }

        //public new EventEnumerator GetEnumerator()
        //{
        //    return new EventEnumerator(this);
        //}

        public void Remove(EventBase value)
        {
            List.Remove(value);
        }

        #region 加的方法

        public EventBase[] ToArray()
        {
            return this.ToList().ToArray();
        }

        public List<EventBase> ToList()
        {
            List<EventBase> list = new List<EventBase>();

            foreach (EventBase e in this)
            {
                list.Add(e);
            }

            return list;
        }

        /// <summary>
        /// 通过Id获取事件对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventBase GetEventById(string id)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Id == id)
                {
                    return this[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 验证指定的代码在窗体元素中是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <param name="withoutId">要跳过的ID</param>
        /// <returns></returns>
        public bool CodeExist(string code, string withoutId)
        {
            bool result = false;

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Code == code && this[i].Id != withoutId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据触发时机查找事件序列
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public EventCollection GetEventsByTime(int code)
        {
            EventCollection events = new EventCollection(this._hostFormEntity, this._hostEntity);

            foreach (EventBase even in this)
            {
                if (even.EventTime == code)
                    events.Add(even);
            }

            return events;
        }

        /// <summary>
        /// 将指定的事件移动到(紧邻)另一个事件之前
        /// </summary>
        /// <param name="targetEvent"></param>
        /// <param name="referEvent"></param>
        public void PreTo(EventBase targetEvent, EventBase referEvent)
        {
            if (targetEvent == null || referEvent == null)
                return;

            if (this.Contains(targetEvent) == false || this.Contains(referEvent) == false)
                return;

            //这里不能因为目标事件是最顶过就直接返回
            //因为此方法的目的是把目标事件放在指定事件 紧挨着 的 前面 一个，而不是前面的任意位置
            //有可能目标事件index是0，指定事件是3，那么此方法要把目标事件的index变为2
            //如果指定事件已经是最顶个了，直接返回
            //int targetIndex = this.IndexOf(targetEvent);
            //if (targetIndex == 0)
            //    return;

            int referIndex = this.IndexOf(referEvent);

            //如果目标事件在指定事件之前的某个位置，这里不能先直接remove目标事件
            //因为这样会使指定事件提前一个index，此时在referIndex上insert，就跑到指定事件后面去了
            //如果目标事件本身在指定事件之后，则无此问题
            //先判断如果在前，就 referIndex--，再insert

            if (this.IndexOf(targetEvent) < referIndex)
                referIndex--;

            this.Remove(targetEvent);
            this.Insert(referIndex, targetEvent);
        }

        /// <summary>
        /// 将指定的事件移动到(紧邻)另一个事件之后
        /// </summary>
        /// <param name="targetEvent"></param>
        /// <param name="referEvent"></param>
        public void NextTo(EventBase targetEvent, EventBase referEvent)
        {
            if (targetEvent == null || referEvent == null)
                return;

            if (this.Contains(targetEvent) == false || this.Contains(referEvent) == false)
                return;

            //如果指定事件已经是最后个了，直接返回
            //int targetIndex = this.IndexOf(targetEvent);
            //if (targetIndex == this.Count - 1)
            //    return;

            int referIndex = this.IndexOf(referEvent);

            //这里在remove之前，也要先判断目标事件是在指定事件之前还是之后
            //如果在指定事件之后，那么referIndex++,不然就insert到指定事件前面了
            if (this.IndexOf(targetEvent) > referIndex)
                referIndex++;

            this.Remove(targetEvent);
            this.Insert(referIndex , targetEvent);
        }

        #endregion

        [Serializable]
        public class EventEnumerator : object, IEnumerator, IEnumerator<EventBase>
        {

            private IEnumerator baseEnumerator;

            private IEnumerable temp;

            public EventEnumerator(EventCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }

            public EventBase Current
            {
                get
                {
                    return ((EventBase)(baseEnumerator.Current));
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

            #region IDisposable 成员

            public void Dispose()
            {
                
            }

            #endregion
        }

        #region ICollection<EventBase> 成员

        void ICollection<EventBase>.Add(EventBase item)
        {
            this.Add(item);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<EventBase>.Remove(EventBase item)
        {
            this.Remove(item);
            return true;
        }

        #endregion

        #region IEnumerable<EventBase> 成员

        public new IEnumerator<EventBase> GetEnumerator()
        {
            return new EventEnumerator(this);
        }

        #endregion
    }
}
