using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Contains event data for events related to a single page.
    /// </summary>
    public class PageEventArgs : EventArgs
    {
        /// <summary>
        /// The page causing the event.
        /// </summary>
        public Page Page { get; private set; }

        public PageEventArgs(Page page)
        {
            Page = page;
        }
    }

    /// <summary>
    /// Contains event data for the <see cref="PagedControl.PageChanging"/> event.
    /// </summary>
    public class PageChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Current page.
        /// </summary>
        public Page CurrentPage { get; private set; }
        /// <summary>
        /// The page that will become the current page after the event.
        /// </summary>
        public Page NewPage { get; set; }

        public PageChangingEventArgs(Page currentPage, Page newPage) : base(false)
        {
            CurrentPage = currentPage;
            NewPage = newPage;
        }
    }

    /// <summary>
    /// Contains event data for the <see cref="PagedControl.PageChanged"/> event.
    /// </summary>
    public class PageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The page that was the current page before the event.
        /// </summary>
        public Page OldPage { get; private set; }
        /// <summary>
        /// Current page.
        /// </summary>
        public Page CurrentPage { get; private set; }

        public PageChangedEventArgs(Page oldPage, Page currentPage)
        {
            OldPage = oldPage;
            CurrentPage = currentPage;
        }
    }

    /// <summary>
    /// Contains event data for the <see cref="PagedControl.PageValidating"/> event.
    /// </summary>
    public class PageValidatingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// The page causing the event.
        /// </summary>
        public Page Page { get; private set; }

        public PageValidatingEventArgs(Page page)
        {
            Page = page;
        }
    }

    /// <summary>
    /// Contains event data for the <see cref="PagedControl.PagePaint"/> event.
    /// </summary>
    public class PagePaintEventArgs : PageEventArgs
    {
        /// <summary>
        /// Gets the graphics used to paint.
        /// </summary>
        public Graphics Graphics { get; private set; }

        public PagePaintEventArgs(Graphics graphics, Page page) : base(page)
        {
            Graphics = graphics;
        }
    }

    /// <summary>
    /// Contains event data for the <see cref="PagedControl.CreateUIControls"/> event.
    /// </summary>
    public class CreateUIControlsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the collection of UI controls.
        /// </summary>
        public Control[] Controls { get; set; }

        public CreateUIControlsEventArgs(Control[] controls)
        {
            Controls = controls;
        }

        public CreateUIControlsEventArgs() : this(new Control[0])
        {

        }
    }
}
