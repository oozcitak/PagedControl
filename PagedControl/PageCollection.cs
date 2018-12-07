using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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

                    controls.RaisePageEvents = true;

                    owner.OnPageAdded(new PageEventArgs(value));

                    if (owner.PageCount == 1) owner.ChangePage(value, true);

                    owner.OnUpdateUIControls(new EventArgs());
                    owner.UpdatePages();
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
                if (Count == 0)
                    return;

                controls.RaisePageEvents = false;

                var toRemove = new List<Page>();
                for (int i = 0; i < Count; i++)
                    toRemove.Add(this[i]);
                foreach (var page in toRemove)
                {
                    Remove(page);
                    owner.OnPageRemoved(new PageEventArgs(page));
                }

                controls.RaisePageEvents = true;

                owner.ChangePage(null, true);

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();
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

                controls.Add(item);
                controls.SetChildIndex(item, index + owner.FirstPageIndex);

                controls.RaisePageEvents = true;

                owner.OnPageAdded(new PageEventArgs(item));

                if (Count == 1)
                    owner.ChangePage(item, true);
                else if (insertBeforeSelected)
                    owner.selectedIndex = owner.selectedIndex + 1;

                owner.OnUpdateUIControls(new EventArgs());
                owner.UpdatePages();
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
