using System;
using System.Collections;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    [Serializable()]
    public class ToolboxItemCollection : CollectionBase
    {

        ///  <summary>
        ///       Initializes a new instance of <see cref="ToolboxLibrary.ToolboxItemCollection"/>.
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxItemCollection()
        {
        }

        ///  <summary>
        ///       Initializes a new instance of <see cref="ToolboxLibrary.ToolboxItemCollection"/> based on another <see cref="ToolboxLibrary.ToolboxItemCollection"/>.
        ///  </summary>
        ///  <param name="value">
        ///       A <see cref="ToolboxLibrary.ToolboxItemCollection"/> from which the contents are copied
        ///  </param>
        ///  <remarks></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxItemCollection(ToolboxItemCollection value)
        {
            this.AddRange(value);
        }

        ///  <summary>
        ///       Initializes a new instance of <see cref="ToolboxLibrary.ToolboxItemCollection"/> containing any array of <see cref="ToolboxLibrary.ToolboxItem"/> objects.
        ///  </summary>
        ///  <param name="value">
        ///       A array of <see cref="ToolboxLibrary.ToolboxItem"/> objects with which to intialize the collection
        ///  </param>
        ///  <remarks></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxItemCollection(ToolboxItem[] value)
        {
            this.AddRange(value);
        }

        ///  <summary>
        ///  Represents the entry at the specified index of the <see cref="ToolboxLibrary.ToolboxItem"/>.
        ///  </summary>
        ///  <param name="index">The zero-based index of the entry to locate in the collection.</param>
        ///  <value>
        ///  The entry at the specified index of the collection.
        ///  </value>
        ///  <remarks><exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is outside the valid range of indexes for the collection.</exception></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxItem this[int index]
        {
            get
            {
                return ((ToolboxItem)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        ///  <summary>
        ///    Adds a <see cref="ToolboxLibrary.ToolboxItem"/> with the specified value to the 
        ///    <see cref="ToolboxLibrary.ToolboxItemCollection"/> .
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxLibrary.ToolboxItem"/> to add.</param>
        ///  <returns>
        ///    The index at which the new element was inserted.
        ///  </returns>
        ///  <remarks><seealso cref="ToolboxLibrary.ToolboxItemCollection.AddRange"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public int Add(ToolboxItem value)
        {
            return List.Add(value);
        }

        ///  <summary>
        ///  Copies the elements of an array to the end of the <see cref="ToolboxLibrary.ToolboxItemCollection"/>.
        ///  </summary>
        ///  <param name="value">
        ///    An array of type <see cref="ToolboxLibrary.ToolboxItem"/> containing the objects to add to the collection.
        ///  </param>
        ///  <remarks><seealso cref="ToolboxLibrary.ToolboxItemCollection.Add"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void AddRange(ToolboxItem[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        ///  <summary>
        ///     
        ///       Adds the contents of another <see cref="ToolboxLibrary.ToolboxItemCollection"/> to the end of the collection.
        ///    
        ///  </summary>
        ///  <param name="value">
        ///    A <see cref="ToolboxLibrary.ToolboxItemCollection"/> containing the objects to add to the collection.
        ///  </param>
        ///  <remarks><seealso cref="ToolboxLibrary.ToolboxItemCollection.Add"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void AddRange(ToolboxItemCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        ///  <summary>
        ///  Gets a value indicating whether the 
        ///    <see cref="ToolboxLibrary.ToolboxItemCollection"/> contains the specified <see cref="ToolboxLibrary.ToolboxItem"/>.
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxLibrary.ToolboxItem"/> to locate.</param>
        ///  <returns>
        ///  <see langword="true"/> if the <see cref="ToolboxLibrary.ToolboxItem"/> is contained in the collection; 
        ///   otherwise, <see langword="false"/>.
        ///  </returns>
        ///  <remarks><seealso cref="ToolboxLibrary.ToolboxItemCollection.IndexOf"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public bool Contains(ToolboxItem value)
        {
            return List.Contains(value);
        }

        ///  <summary>
        ///  Copies the <see cref="ToolboxLibrary.ToolboxItemCollection"/> values to a one-dimensional <see cref="System.Array"/> instance at the 
        ///    specified index.
        ///  </summary>
        ///  <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the values copied from <see cref="ToolboxLibrary.ToolboxItemCollection"/> .</param>
        ///  <param name="index">The index in <paramref name="array"/> where copying begins.</param>
        ///  <remarks><exception cref="System.ArgumentException"><paramref name="array"/> is multidimensional. <para>-or-</para> <para>The number of elements in the <see cref="ToolboxLibrary.ToolboxItemCollection"/> is greater than the available space between <paramref name="arrayIndex"/> and the end of <paramref name="array"/>.</para></exception>
        ///  <exception cref="System.ArgumentNullException"><paramref name="array"/> is <see langword="null"/>. </exception>
        ///  <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than <paramref name="array"/>"s lowbound. </exception>
        ///  <seealso cref="System.Array"/>
        ///  </remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void CopyTo(ToolboxItem[] array, int index)
        {
            List.CopyTo(array, index);
        }

        ///  <summary>
        ///    Returns the index of a <see cref="ToolboxLibrary.ToolboxItem"/> in 
        ///       the <see cref="ToolboxLibrary.ToolboxItemCollection"/> .
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxLibrary.ToolboxItem"/> to locate.</param>
        ///  <returns>
        ///  The index of the <see cref="ToolboxLibrary.ToolboxItem"/> of <paramref name="value"/> in the 
        ///  <see cref="ToolboxLibrary.ToolboxItemCollection"/>, if found; otherwise, -1.
        ///  </returns>
        ///  <remarks><seealso cref="ToolboxLibrary.ToolboxItemCollection.Contains"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public int IndexOf(ToolboxItem value)
        {
            return List.IndexOf(value);
        }

        ///  <summary>
        ///  Inserts a <see cref="ToolboxLibrary.ToolboxItem"/> into the <see cref="ToolboxLibrary.ToolboxItemCollection"/> at the specified index.
        ///  </summary>
        ///  <param name="index">The zero-based index where <paramref name="value"/> should be inserted.</param>
        ///  <param name=" value">The <see cref="ToolboxLibrary.ToolboxItem"/> to insert.</param>
        ///  <remarks><seealso cref="ToolboxLibrary.ToolboxItemCollection.Add"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void Insert(int index, ToolboxItem value)
        {
            List.Insert(index, value);
        }

        ///  <summary>
        ///    Returns an enumerator that can iterate through 
        ///       the <see cref="ToolboxLibrary.ToolboxItemCollection"/> .
        ///  </summary>
        ///  <returns>An enumerator for the collection</returns>
        ///  <remarks><seealso cref="System.Collections.IEnumerator"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public new ToolboxItemEnumerator GetEnumerator()
        {
            return new ToolboxItemEnumerator(this);
        }

        ///  <summary>
        ///     Removes a specific <see cref="ToolboxLibrary.ToolboxItem"/> from the 
        ///    <see cref="ToolboxLibrary.ToolboxItemCollection"/> .
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxLibrary.ToolboxItem"/> to remove from the <see cref="ToolboxLibrary.ToolboxItemCollection"/> .</param>
        ///  <remarks><exception cref="System.ArgumentException"><paramref name="value"/> is not found in the Collection. </exception></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void Remove(ToolboxItem value)
        {
            List.Remove(value);
        }

        public class ToolboxItemEnumerator : object, IEnumerator
        {

            private IEnumerator baseEnumerator;

            private IEnumerable temp;

            public ToolboxItemEnumerator(ToolboxItemCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }

            public ToolboxItem Current
            {
                get
                {
                    return ((ToolboxItem)(baseEnumerator.Current));
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
