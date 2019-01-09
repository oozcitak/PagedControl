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
                    controls.FromPageCollection = true;
                    controls.RemoveAt(index + owner.FirstPageIndex);
                    controls.Add(value);
                    controls.SetChildIndex(value, index + owner.FirstPageIndex);
                    controls.FromPageCollection = false;

                    if (owner.SelectedIndex == index) owner.ChangePage(value, false);

                    owner.UpdatePages();
                    owner.OnUpdateUIControls(new EventArgs());
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
                controls.FromPageCollection = true;
                controls.Add(item);
                controls.FromPageCollection = false;

                owner.OnPageAdded(new PageEventArgs(item));

                if (Count == 1)
                {
                    // Just added a page to an empty control
                    // Set this page as the selected page; this event cannot be cancelled
                    owner.OnPageChanging(new PageChangingEventArgs(null, item));
                    owner.selectedPage = item;
                    owner.selectedIndex = 0;
                    owner.OnPageShown(new PageEventArgs(item));
                    owner.OnPageChanged(new PageChangedEventArgs(null, item));
                }

                owner.UpdatePages();
                owner.OnUpdateUIControls(new EventArgs());
            }

            public void Clear()
            {
                if (Count == 0)
                    return;

                var toRemove = new List<Page>();
                for (int i = 0; i < Count; i++)
                    toRemove.Add(this[i]);

                var lastSelectedPage = owner.selectedPage;

                // Set the selected page to null; this event cannot be cancelled
                owner.OnPageChanging(new PageChangingEventArgs(lastSelectedPage, null));
                owner.selectedPage = null;
                owner.selectedIndex = -1;
                owner.OnPageChanged(new PageChangedEventArgs(lastSelectedPage, null));

                controls.FromPageCollection = true;
                foreach (var page in toRemove)
                {
                    controls.Remove(page);
                    if (page.Visible)
                        owner.OnPageHidden(new PageEventArgs(page));
                    owner.OnPageRemoved(new PageEventArgs(page));
                }
                controls.FromPageCollection = false;

                owner.UpdatePages();
                owner.OnUpdateUIControls(new EventArgs());
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
                if (owner.PageCount == 0)
                {
                    Add(item);
                    return;
                }
                else
                {
                    bool insertBeforeSelected = (index <= owner.SelectedIndex);

                    controls.FromPageCollection = true;
                    controls.Add(item);
                    controls.SetChildIndex(item, index + owner.FirstPageIndex);
                    controls.FromPageCollection = false;

                    if (insertBeforeSelected)
                        owner.selectedIndex = owner.selectedIndex + 1;

                    owner.UpdatePages();
                    owner.OnUpdateUIControls(new EventArgs());

                    owner.OnPageAdded(new PageEventArgs(item));
                }
            }

            public bool Remove(Page item)
            {
                int index = owner.Pages.IndexOf(item);
                bool exists = (index != -1);
                if (!exists)
                {
                    throw new ArgumentException("Page not found in collection.");
                }

                controls.FromPageCollection = true;
                controls.Remove(item);
                controls.FromPageCollection = false;

                if (Count == 0)
                {
                    // Just removed the last page from the collection
                    // Set the selected page to null; this event cannot be cancelled
                    owner.OnPageChanging(new PageChangingEventArgs(item, null));
                    owner.selectedPage = null;
                    owner.selectedIndex = -1;
                    owner.OnPageChanged(new PageChangedEventArgs(item, null));
                }
                else if (ReferenceEquals(owner.selectedPage, item))
                {
                    // Just removed the selected page from the collection
                    // Set the selected page to the page before it; this event cannot be cancelled
                    int newSelectedIndex = (owner.selectedIndex == Count ? Count - 1 : owner.selectedIndex);
                    var newSelectedPage = this[newSelectedIndex];

                    owner.OnPageChanging(new PageChangingEventArgs(item, newSelectedPage));
                    owner.selectedPage = newSelectedPage;
                    owner.selectedIndex = newSelectedIndex;
                    owner.OnPageChanged(new PageChangedEventArgs(item, newSelectedPage));
                }

                owner.OnPageHidden(new PageEventArgs(item));
                owner.OnPageRemoved(new PageEventArgs(item));

                owner.UpdatePages();
                owner.OnUpdateUIControls(new EventArgs());

                return exists;
            }

            public void RemoveAt(int index)
            {
                Remove(this[index]);
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
