using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        [DefaultProperty("Item")]
        public class PageCollection : IList<Page>, ICollection, ICollection<Page>, IList, IEnumerable
        {
            #region Member Variables
            private PagedControl owner;
            private ControlCollection controls;
            private readonly object syncRoot = new object();
            #endregion

            #region Properties
            public Page this[int index]
            {
                get => (Page)controls[index + owner.FirstPageIndex];
                set
                {
                    Insert(index + owner.FirstPageIndex, value);
                    RemoveAt(index + owner.FirstPageIndex + 1);
                }
            }
            public int Count => owner.PageCount;
            public bool IsReadOnly => false;

            public object SyncRoot => syncRoot;
            public bool IsSynchronized => false;

            public bool IsFixedSize => false;

            object IList.this[int index] { get => this[index]; set => this[index] = (Page)value; }
            #endregion

            #region Constructor
            public PageCollection(PagedControl control)
            {
                owner = control;
                controls = control.Controls;
            }
            #endregion

            #region Public Methods
            public void Add(Page item)
            {
                controls.Add(item);
                if (Count == 1) owner.SelectedIndex = 0;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();
            }

            public void Clear()
            {
                var toRemove = new List<Page>();
                for (int i = 0; i < Count; i++)
                    toRemove.Add(this[i]);
                foreach (var page in toRemove)
                    Remove(page);
            }

            public bool Contains(Page item)
            {
                return controls.Contains(item);
            }

            public void CopyTo(Page[] array, int arrayIndex)
            {
                for (int i = arrayIndex; i < array.Length; i++)
                    array[i] = (Page)controls[i - arrayIndex + owner.FirstPageIndex];
            }

            public IEnumerator<Page> GetEnumerator()
            {
                for (int i = owner.FirstPageIndex; i < owner.FirstPageIndex + owner.PageCount; i++)
                    yield return (Page)controls[i];
            }

            public int IndexOf(Page item)
            {
                return controls.IndexOf(item) - owner.FirstPageIndex;
            }

            public void Insert(int index, Page item)
            {
                index += owner.FirstPageIndex;
                List<Control> removed = new List<Control>();
                for (int i = controls.Count - 1; i >= index; i--)
                {
                    removed.Add(controls[i]);
                    controls.RemoveAt(i);
                }
                controls.Add(item);
                for (int i = removed.Count - 1; i >= 0; i--)
                {
                    controls.Add(removed[i]);
                }
                if (Count == 1) owner.SelectedIndex = 0;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();
            }

            public bool Remove(Page item)
            {
                bool exists = controls.Contains(item);

                controls.Remove(item);

                if (Count == 0)
                    owner.SelectedIndex = -1;
                else if (owner.SelectedIndex > Count - 1)
                    owner.SelectedIndex = 0;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();

                return exists;
            }

            public void RemoveAt(int index)
            {
                controls.RemoveAt(index + owner.FirstPageIndex);

                if (Count == 0)
                    owner.SelectedIndex = -1;
                else if (owner.SelectedIndex > Count - 1)
                    owner.SelectedIndex = 0;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();
            }

            public void CopyTo(Array array, int index)
            {
                for (int i = index; i < array.Length; i++)
                    array.SetValue(this[i - index], i);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int Add(object value)
            {
                Add((Page)value);
                return Count - 1;
            }

            public bool Contains(object value)
            {
                return Contains((Page)value);
            }

            public int IndexOf(object value)
            {
                return IndexOf((Page)value);
            }

            public void Insert(int index, object value)
            {
                Insert(index, (Page)value);
            }

            public void Remove(object value)
            {
                Remove((Page)value);
            }
            #endregion
        }
    }
}
