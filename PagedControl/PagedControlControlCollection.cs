using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        internal class PagedControlControlCollection : ControlCollection
        {
            #region Properties
            public new PagedControl Owner { get; private set; }
            public bool FromPageCollection { get; set; }
            #endregion

            #region Constructor
            public PagedControlControlCollection(PagedControl owner) : base(owner)
            {
                FromPageCollection = false;

                Owner = owner;
            }
            #endregion

            #region Overrides
            public override void Add(Control value)
            {
                if (FromPageCollection)
                {
                    base.Add(value);
                    return;
                }
                else if (Owner.creatingUIControls)
                {
                    base.Add(value);
                    return;
                }
                else
                {
                    if (!(value is Page page))
                    {
                        throw new ArgumentException(string.Format("Only a Page can be added to a PagedControl. Expected type {0}, supplied type {1}.", typeof(Page).AssemblyQualifiedName, value.GetType().AssemblyQualifiedName));
                    }

                    Owner.Pages.Add(page);

                    // site the page
                    ISite site = Owner.Site;
                    if (site != null && page.Site == null)
                    {
                        IContainer container = site.Container;
                        if (container != null)
                        {
                            container.Add(page);
                        }
                    }
                }
            }

            public override void Remove(Control value)
            {
                if (FromPageCollection)
                {
                    base.Remove(value);
                    return;
                }
                else if (Owner.creatingUIControls)
                {
                    base.Remove(value);
                    return;
                }
                else
                {
                    if (!(value is Page page))
                    {
                        throw new ArgumentException(string.Format("Only a Page can be removed from a PagedControl. Expected type {0}, supplied type {1}.", typeof(Page).AssemblyQualifiedName, value.GetType().AssemblyQualifiedName));
                    }

                    Owner.Pages.Remove(page);

                    // unsite the page
                    ISite site = Owner.Site;
                    if (site != null && page.Site == null)
                    {
                        IContainer container = site.Container;
                        if (container != null)
                        {
                            container.Remove(page);
                        }
                    }
                }
            }
            #endregion
        }
    }
}
