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
        public class PageCollection : IList<Page>
        {
            #region Member Variables
            private PagedControl owner;
            private PagedControlControlCollection controls;
            private readonly object syncRoot = new object();
            #endregion

            #region Properties
            public Page this[int index]
            {
                get => (Page)controls[index + owner.FirstPageIndex];
                set
                {
                    controls.RaisePageEvents = false;

                    controls.Add(value);
                    controls.SetChildIndex(value, index + owner.FirstPageIndex);

                    owner.OnPageAdded(new PageEventArgs(value, index));

                    if (owner.PageCount == 1) owner.ChangePage(value, true);

                    owner.OnUpdateUIControls(new EventArgs());
                    owner.UpdatePages();

                    controls.RaisePageEvents = true;
                }
            }

            public int Count => owner.PageCount;
            public bool IsReadOnly => false;
            #endregion

            #region Constructor
            public PageCollection(PagedControl control)
            {
                owner = control;
                controls = (PagedControlControlCollection)control.Controls;
            }
            #endregion

            #region Public Methods
            public void Add(Page item)
            {
                controls.Add(item);
            }

            public void Clear()
            {
                controls.RaisePageEvents = false;

                var toRemove = new List<Page>();
                for (int i = 0; i < Count; i++)
                    toRemove.Add(this[i]);
                foreach (var page in toRemove)
                {
                    Remove(page);
                    owner.OnPageRemoved(new PageEventArgs(page, -1));
                }

                owner.SelectedIndex = -1;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();

                controls.RaisePageEvents = true;
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
                bool insertBeforeSelected = (index <= owner.SelectedIndex);

                controls.RaisePageEvents = false;

                List<Control> removed = new List<Control>();
                for (int i = controls.Count - 1; i >= index + owner.FirstPageIndex; i--)
                {
                    removed.Add(controls[i]);
                    controls.RemoveAt(i);
                }
                controls.Add(item);
                for (int i = removed.Count - 1; i >= 0; i--)
                {
                    controls.Add(removed[i]);
                }

                owner.OnPageAdded(new PageEventArgs(item, index));

                if (Count == 1)
                    owner.SelectedIndex = 0;
                else if (insertBeforeSelected)
                    owner.selectedIndex = owner.selectedIndex + 1;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();

                controls.RaisePageEvents = true;
            }

            public bool Remove(Page item)
            {
                bool exists = controls.Contains(item);
                controls.Remove(item);
                return exists;
            }

            public void RemoveAt(int index)
            {
                controls.Remove(this[index]);
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
            #endregion
        }
    }
}
