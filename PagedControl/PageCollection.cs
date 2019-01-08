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
        public class PageCollection : ControlCollection, IList<Page>
        {
            #region Member Variables
            private PagedControl owner;
            private readonly object syncRoot = new object();
            #endregion

            #region Properties
            public new Page this[int index]
            {
                get => (Page)base[index];
                set
                {
                    base.RemoveAt(index);
                    base.Add(value);
                    base.SetChildIndex(value, index);

                    if (owner.SelectedIndex == index) owner.ChangePage(value, false);

                    owner.UpdatePages();
                    owner.OnUpdateUIControls(new EventArgs());
                }
            }
            #endregion

            #region Constructor
            public PageCollection(PagedControl control) : base(control)
            {
                owner = control;
            }
            #endregion

            #region Public Methods
            public override void Add(Control item)
            {
                if (!(item is Page page))
                {
                    throw new ArgumentException(string.Format("Only a Page can be added to a PagedControl. Expected type {0}, supplied type {1}.", typeof(Page).AssemblyQualifiedName, item.GetType().AssemblyQualifiedName));
                }

                Add(page);
            }

            public void Add(Page page)
            {
                base.Add(page);

                // site the page
                ISite site = owner.Site;
                if (site != null && page.Site == null)
                {
                    IContainer container = site.Container;
                    if (container != null)
                    {
                        container.Add(page);
                    }
                }

                owner.OnPageAdded(new PageEventArgs(page));

                if (Count == 1)
                {
                    // Just added a page to an empty control
                    // Set this page as the selected page; this event cannot be cancelled
                    owner.OnCurrentPageChanging(new PageChangingEventArgs(null, page));
                    owner.selectedPage = page;
                    owner.selectedIndex = 0;
                    owner.OnPageShown(new PageEventArgs(page));
                    owner.OnCurrentPageChanged(new PageChangedEventArgs(null, page));
                }

                owner.UpdatePages();
                owner.OnUpdateUIControls(new EventArgs());
            }

            public override void Clear()
            {
                if (Count == 0)
                    return;

                var toRemove = new List<Page>();
                for (int i = 0; i < Count; i++)
                    toRemove.Add(this[i]);

                var lastSelectedPage = owner.selectedPage;

                // Set the selected page to null; this event cannot be cancelled
                owner.OnCurrentPageChanging(new PageChangingEventArgs(lastSelectedPage, null));
                owner.selectedPage = null;
                owner.selectedIndex = -1;
                owner.OnCurrentPageChanged(new PageChangedEventArgs(lastSelectedPage, null));

                foreach (var page in toRemove)
                {
                    base.Remove(page);
                    if (page.Visible)
                        owner.OnPageHidden(new PageEventArgs(page));
                    owner.OnPageRemoved(new PageEventArgs(page));
                }

                owner.UpdatePages();
                owner.OnUpdateUIControls(new EventArgs());
            }

            public bool Contains(Page item)
            {
                return base.Contains(item);
            }

            public void CopyTo(Page[] array, int arrayIndex)
            {
                for (int i = arrayIndex; i < array.Length; i++)
                    array[i] = (Page)base[i - arrayIndex];
            }

            public new IEnumerator<Page> GetEnumerator()
            {
                for (int i = 0; i < base.Count; i++)
                    yield return (Page)base[i];
            }

            public int IndexOf(Page page)
            {
                return base.IndexOf(page);
            }

            public void Insert(int index, Page page)
            {
                if (Count == 0)
                {
                    Add(page);
                    return;
                }
                else
                {
                    bool insertBeforeSelected = (index <= owner.SelectedIndex);

                    base.Add(page);
                    base.SetChildIndex(page, index);

                    if (insertBeforeSelected)
                        owner.selectedIndex = owner.selectedIndex + 1;

                    owner.UpdatePages();
                    owner.OnUpdateUIControls(new EventArgs());

                    owner.OnPageAdded(new PageEventArgs(page));
                }
            }

            public bool Remove(Page page)
            {
                int index = owner.Pages.IndexOf(page);
                bool exists = (index != -1);
                if (!exists)
                {
                    throw new ArgumentException("Page not found in collection.");
                }

                base.Remove(page);

                // unsite the page
                ISite site = owner.Site;
                if (site != null && page.Site == null)
                {
                    IContainer container = site.Container;
                    if (container != null)
                    {
                        container.Remove(page);
                    }
                }

                if (Count == 0)
                {
                    // Just removed the last page from the collection
                    // Set the selected page to null; this event cannot be cancelled
                    owner.OnCurrentPageChanging(new PageChangingEventArgs(page, null));
                    owner.selectedPage = null;
                    owner.selectedIndex = -1;
                    owner.OnCurrentPageChanged(new PageChangedEventArgs(page, null));
                }
                else if (ReferenceEquals(owner.selectedPage, page))
                {
                    // Just removed the selected page from the collection
                    // Set the selected page to the page before it; this event cannot be cancelled
                    int newSelectedIndex = (owner.selectedIndex == Count ? Count - 1 : owner.selectedIndex);
                    var newSelectedPage = this[newSelectedIndex];

                    owner.OnCurrentPageChanging(new PageChangingEventArgs(page, newSelectedPage));
                    owner.selectedPage = newSelectedPage;
                    owner.selectedIndex = newSelectedIndex;
                    owner.OnCurrentPageChanged(new PageChangedEventArgs(page, newSelectedPage));
                }

                owner.OnPageHidden(new PageEventArgs(page));
                owner.OnPageRemoved(new PageEventArgs(page));

                owner.UpdatePages();
                owner.OnUpdateUIControls(new EventArgs());

                return exists;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion
        }
    }
}
