using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyPath : CollectionBase
    {
        #region 构造

        public PropertyPath()
        {
        }

        public PropertyPath(PropertyPath value)
        {
            this.AddRange(value);
        }

        public PropertyPath(PropertyPathPoint[] value)
        {
            this.AddRange(value);
        }

        #endregion

        #region 基本方法和属性

        public PropertyPathPoint this[int index]
        {
            get
            {
                return ((PropertyPathPoint)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        public virtual int Add(PropertyPathPoint value)
        {
            return List.Add(value);
        }

        public void AddRange(PropertyPathPoint[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(PropertyPath value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        public bool Contains(PropertyPathPoint value)
        {
            return List.Contains(value);
        }

        public void CopyTo(PropertyPathPoint[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public int IndexOf(PropertyPathPoint value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, PropertyPathPoint value)
        {
            List.Insert(index, value);
        }

        public new PropertyPathPointEnumerator GetEnumerator()
        {
            return new PropertyPathPointEnumerator(this);
        }

        public virtual void Remove(PropertyPathPoint value)
        {
            List.Remove(value);
        }

        #endregion

        #region 加的方法和属性

        /// <summary>
        /// 路径中最前面一个节点
        /// </summary>
        public PropertyPathPoint ForePathPoint
        {
            get
            {
                if (this.Count == 0)
                    return null;
                else
                    return this[0];
            }
        }

        /// <summary>
        /// 路径中最后一个节点
        /// </summary>
        public PropertyPathPoint LastPathPoint
        {
            get
            {
                if (this.Count == 0)
                    return null;
                else
                    return this[this.Count - 1];
            }
        }

        /// <summary>
        /// 向属性树的最前面加一个新的属性节点
        /// </summary>
        /// <param name="propertyInfo"></param>
        public void CombineToFore(PropertyInfo propertyInfo)
        {
            PropertyPathPoint pathPoint = new PropertyPathPoint(propertyInfo);

            if (this.ForePathPoint != null)
                pathPoint.Next = this.ForePathPoint;

            this.Insert(0, pathPoint);
        }

        public void CombineToFore(PropertyPath propertyPath)
        {
            for (int i = propertyPath.Count - 1; i >= 0; i--)
            {
                //这里必须传PropertyInfo，new一个新的PropertyPathPoint
                //PropertyPathPoint必须唯一用在一个属性树里，不能共用，防止链表的前后节点出问题
                CombineToFore(propertyPath[i].Property);
            }
        }

        /// <summary>
        /// 判断指定的对象是否与此属性（Property）路径兼容（是事包含这些属性（Property））
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ContainerProperty(object obj)
        {
            if (obj == null)
                return false;

            Type type = obj.GetType();
            PropertyInfo propertyInfo;
            foreach (PropertyPathPoint pathPoint in this)
            {
                propertyInfo =  type.GetProperty(pathPoint.Property.Name, pathPoint.Property.PropertyType);
                if (propertyInfo == null)
                    return false;

                type = propertyInfo.PropertyType;
            }

            return true;
        }

        /// <summary>
        /// 获取对象的属性（Property）值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetValue(object obj)
        {
            object objTemp = obj;
            Type type = obj.GetType();
            PropertyInfo propertyInfo;
            foreach (PropertyPathPoint pathPoint in this)
            {
                propertyInfo = type.GetProperty(pathPoint.Property.Name, pathPoint.Property.PropertyType);
                if (propertyInfo == null)
                {
                    Debug.Assert(false);
                    return null;
                }

                if (propertyInfo.PropertyType.IsEnum)
                {
                    objTemp = (int)Enum.Parse(propertyInfo.PropertyType, propertyInfo.GetValue(objTemp, null).ToString());
                }
                else
                {
                    objTemp = propertyInfo.GetValue(objTemp, null);
                }

                type = propertyInfo.PropertyType;
            }

            return objTemp;
        }

        /// <summary>
        /// 取到指定路径节点的值
        /// </summary>
        /// <param name="pathPoint"></param>
        /// <returns></returns>
        public object GetValue(object obj, PropertyPathPoint point)
        {
            if (this.Count == 1 && this.ForePathPoint == point)
                return obj;

            object objTemp = obj;
            Type type = obj.GetType();
            PropertyInfo propertyInfo;
            foreach (PropertyPathPoint pathPoint in this)
            {
                propertyInfo = type.GetProperty(pathPoint.Property.Name, pathPoint.Property.PropertyType);
                if (propertyInfo == null)
                {
                    Debug.Assert(false);
                    return null;
                }

                if (propertyInfo.PropertyType.IsEnum)
                {
                    objTemp = (int)Enum.Parse(propertyInfo.PropertyType, propertyInfo.GetValue(objTemp, null).ToString());
                }
                else
                {
                    objTemp = propertyInfo.GetValue(objTemp, null);
                }

                if (pathPoint == point)
                    break;

                type = propertyInfo.PropertyType;
            }

            return objTemp;
        }

        /// <summary>
        /// 获取属性路径末端属性（property）的所属对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetParentValue(object obj)
        {
            if (this.LastPathPoint.Pre == null)
            {
                return GetValue(obj, this.LastPathPoint);
            }
            else
            {
                return GetValue(obj, this.LastPathPoint.Pre);
            }
        }

        public void SetValue(object obj,object value)
        {
            object objTemp = obj;
            Type type = obj.GetType();
            PropertyInfo propertyInfo = null;
            foreach (PropertyPathPoint pathPoint in this)
            {
                propertyInfo = type.GetProperty(pathPoint.Property.Name, pathPoint.Property.PropertyType);
                if (propertyInfo == null)
                {
                    Debug.Assert(false);
                    return;
                }

                //如果不是最后一个节点，就取出当前节点的值
                //如果是最后一个节点，不取，因为设置值的操作就发生在最后节点的前一个节点的Value对象上
                if (pathPoint != this.LastPathPoint)
                {
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        objTemp = (int)Enum.Parse(propertyInfo.PropertyType, propertyInfo.GetValue(objTemp, null).ToString());
                    }
                    else
                    {
                        objTemp = propertyInfo.GetValue(objTemp, null);
                    }
                }

                type = propertyInfo.PropertyType;
            }

            propertyInfo.SetValue(objTemp, value,null);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = this.Count - 1; i >= 0; i--)
            {
                sb.Insert(0, this[i].Property.Name);

                if (this[i] != this.ForePathPoint)
                    sb.Insert(0, "\\");
            }

            return sb.ToString();
        }

        #endregion

        #region Enumerator

        /// <summary>
        /// 获取 PropertyPathPoint 枚举数
        /// </summary>
        public IEnumerable<PropertyPathPoint> TypeEnum
        {
            get
            {
                foreach (PropertyPathPoint type in this)
                    yield return type;
            }
        }


        public class PropertyPathPointEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;

            private IEnumerable temp;

            public PropertyPathPointEnumerator(PropertyPath mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }

            public PropertyPathPoint Current
            {
                get
                {
                    return ((PropertyPathPoint)(baseEnumerator.Current));
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

        #endregion
    }
}
