using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(PagedControl))]
    [Designer(typeof(PagedControlDesigner))]
    [Docking(DockingBehavior.Ask)]
    [DefaultEvent("PageChanged")]
    [DefaultProperty("SelectedPage")]
    public partial class PagedControl : Control
    {
        #region Events
        /// <summary>
        /// Contains event data for events related to a single page.
        /// </summary>
        public class PageEventArgs : EventArgs
        {
            /// <summary>
            /// The page causing the event.
            /// </summary>
            public Page Page { get; private set; }
            /// <summary>
            /// The index of the page causing the event.
            /// </summary>
            public int PageIndex { get; private set; }

            public PageEventArgs(Page page, int index)
            {
                Page = page;
                PageIndex = index;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PageChanging"/> event.
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
            /// <summary>
            /// The index of the current page.
            /// </summary>
            public int CurrentPageIndex { get; private set; }
            /// <summary>
            /// The index of the new page.
            /// </summary>
            public int NewPageIndex { get; private set; }

            public PageChangingEventArgs(Page currentPage, int currentPageIndex, Page newPage, int newPageIndex) : base(false)
            {
                CurrentPage = currentPage;
                CurrentPageIndex = currentPageIndex;
                NewPage = newPage;
                NewPageIndex = newPageIndex;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PageChanged"/> event.
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
            /// <summary>
            /// The index of the old page.
            /// </summary>
            public int OldPageIndex { get; private set; }
            /// <summary>
            /// The index of the current page.
            /// </summary>
            public int CurrentPageIndex { get; private set; }

            public PageChangedEventArgs(Page oldPage, int oldPageIndex, Page currentPage, int currentPageIndex)
            {
                OldPage = oldPage;
                OldPageIndex = oldPageIndex;
                CurrentPage = currentPage;
                CurrentPageIndex = currentPageIndex;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PageValidating"/> event.
        /// </summary>
        public class PageValidatingEventArgs : CancelEventArgs
        {
            /// <summary>
            /// The page causing the event.
            /// </summary>
            public Page Page { get; private set; }
            /// <summary>
            /// The index of the page causing the event.
            /// </summary>
            public int PageIndex { get; private set; }

            public PageValidatingEventArgs(Page page, int index)
            {
                Page = page;
                PageIndex = index;
            }
        }

        /// <summary>
        /// Contains event data for the <see cref="PagePaint"/> event.
        /// </summary>
        public class PagePaintEventArgs : PageEventArgs
        {
            /// <summary>
            /// Gets the graphics used to paint.
            /// </summary>
            public Graphics Graphics { get; private set; }

            public PagePaintEventArgs(Graphics graphics, Page page, int index) : base(page, index)
            {
                Graphics = graphics;
            }
        }

        public delegate void PageEventHandler(object sender, PageEventArgs e);
        public delegate void PageChangingEventHandler(object sender, PageChangingEventArgs e);
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);
        public delegate void PageValidatingEventHandler(object sender, PageValidatingEventArgs e);
        public delegate void PagePaintEventHandler(object sender, PagePaintEventArgs e);

        protected internal virtual void OnPageAdded(PageEventArgs e) { PageAdded?.Invoke(this, e); }
        protected internal virtual void OnPageRemoved(PageEventArgs e) { PageRemoved?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanging(PageChangingEventArgs e) { PageChanging?.Invoke(this, e); }
        protected internal virtual void OnCurrentPageChanged(PageChangedEventArgs e) { PageChanged?.Invoke(this, e); }
        protected internal virtual void OnPageValidating(PageValidatingEventArgs e) { PageValidating?.Invoke(this, e); }
        protected internal virtual void OnPageValidated(PageEventArgs e) { PageValidated?.Invoke(this, e); }
        protected internal virtual void OnPageHidden(PageEventArgs e) { PageHidden?.Invoke(this, e); }
        protected internal virtual void OnPageShown(PageEventArgs e) { PageShown?.Invoke(this, e); }
        protected internal virtual void OnPagePaint(PagePaintEventArgs e) { PagePaint?.Invoke(this, e); }
        protected internal virtual void OnUpdateUIControls(EventArgs e) { UpdateUIControls?.Invoke(this, e); }

        /// <summary>
        /// Occurs when a new page is added.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a new page is added.")]
        public event PageEventHandler PageAdded;
        /// <summary>
        /// Occurs when a page is removed.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a page is removed.")]
        public event PageEventHandler PageRemoved;
        /// <summary>
        /// Occurs before the current page is changed.
        /// </summary>
        [Category("Behavior"), Description("Occurs before the current page is changed.")]
        public event PageChangingEventHandler PageChanging;
        /// <summary>
        /// Occurs after the current page is changed.
        /// </summary>
        [Category("Behavior"), Description("Occurs after the current page is changed.")]
        public event PageChangedEventHandler PageChanged;
        /// <summary>
        /// Occurs when the current page is validating.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the current page is validating.")]
        public event PageValidatingEventHandler PageValidating;
        /// <summary>
        /// Occurs after the current page is successfully validated.
        /// </summary>
        [Category("Behavior"), Description("Occurs after the current page is successfully validated.")]
        public event PageEventHandler PageValidated;
        /// <summary>
        /// Occurs while the current page is changing and the previous current page is hidden.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the current page is changing and the previous current page is hidden.")]
        public event PageEventHandler PageHidden;
        /// <summary>
        /// Occurs while the current page is changing and the new current page is shown.
        /// </summary>
        [Category("Behavior"), Description("Occurs while the current page is changing and the new current page is shown.")]
        public event PageEventHandler PageShown;
        /// <summary>
        /// Occurs when a page is painted.
        /// </summary>
        [Category("Appearance"), Description("Occurs when a page is painted.")]
        public event PagePaintEventHandler PagePaint;
        /// <summary>
        /// Occurs when UI controls need to be updated.
        /// </summary>
        [Category("Appearance"), Description("Occurs when UI controls need to be updated.")]
        public event EventHandler UpdateUIControls;
        #endregion

        #region Constants
        private const int WS_EX_TRANSPARENT = 0x00000020;
        #endregion

        #region Member Variables
        private int selectedIndex;
        private Page lastSelectedPage;
        private BorderStyle borderStyle;
        private Color backColor;
        private bool creatingUIControls;
        private int uiControlCount;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the index of the first page in the controls collection.
        /// </summary>
        internal int FirstPageIndex => uiControlCount;
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        internal int PageCount => Controls.Count - uiControlCount;

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        [Editor(typeof(PagedControlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the current page.")]
        public virtual Page SelectedPage
        {
            get
            {
                return (selectedIndex == -1 ? null : Pages[selectedIndex]);
            }
            set
            {
                var oldPage = lastSelectedPage;
                var newPage = value;
                int oldSelectedIndex = selectedIndex;
                int newSelectedIndex = (newPage == null ? -1 : Pages.IndexOf(newPage));

                if (newPage != null && !Pages.Contains(newPage))
                    throw new ArgumentException("Page is not found in the page collection.");

                if (oldPage == value)
                    return;

                if (oldPage != null && oldPage.CausesValidation)
                {
                    PageValidatingEventArgs pve = new PageValidatingEventArgs(oldPage, oldSelectedIndex);
                    OnPageValidating(pve);
                    if (pve.Cancel) return;

                    OnPageValidated(new PageEventArgs(oldPage, oldSelectedIndex));
                }

                if (oldPage != null && newPage != null)
                {
                    PageChangingEventArgs pce = new PageChangingEventArgs(oldPage, oldSelectedIndex, newPage, newSelectedIndex);
                    OnCurrentPageChanging(pce);
                    if (pce.Cancel) return;
                }

                if (oldPage != null) oldPage.Visible = false;
                if (newPage != null) newPage.Visible = true;

                lastSelectedPage = newPage;
                selectedIndex = newSelectedIndex;

                OnUpdateUIControls(new EventArgs());
                UpdatePages();

                if (oldPage != null)
                    OnPageHidden(new PageEventArgs(oldPage, oldSelectedIndex));

                if (newPage != null)
                    OnPageShown(new PageEventArgs(newPage, newSelectedIndex));

                if (oldPage != null && newPage != null)
                    OnCurrentPageChanged(new PageChangedEventArgs(oldPage, oldSelectedIndex, newPage, newSelectedIndex));
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the current page.
        /// </summary>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the zero-based index of the current page.")]
        public virtual int SelectedIndex
        {
            get => selectedIndex;
            set { SelectedPage = (value == -1 ? null : Pages[value]); }
        }

        /// <summary>
        /// Gets or sets the collection of pages.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the collection of pages.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PageCollection Pages { get; }

        /// <summary>
        /// Gets or sets the drawing mode for the control.
        /// </summary>
        [Category("Behavior"), DefaultValue(false)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets the drawing mode for the control.")]
        public bool OwnerDraw { get; set; }

        /// <summary>
        /// Gets the rectangle that represents the client area of the control.
        /// </summary>
        [Browsable(false)]
        public new Rectangle ClientRectangle
        {
            get
            {
                var rect = base.ClientRectangle;
                if (BorderStyle != BorderStyle.None)
                    rect.Inflate(-1, -1);
                return rect;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), "Transparent")]
        [Description("Gets or sets the background color of the control.")]
        public override Color BackColor { get => backColor; set { backColor = value; Invalidate(); } }

        /// <summary>
        /// Gets or sets the border style of the control.
        /// </summary>
        [Category("Appearance"), DefaultValue(BorderStyle.FixedSingle)]
        [Description("Gets or sets the border style of the control.")]
        public BorderStyle BorderStyle { get => borderStyle; set { borderStyle = value; Invalidate(); } }

        /// <summary>
        /// Gets the size of the control when it is initially created.
        /// </summary>
        protected override Size DefaultSize => new Size(300, 200);

        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        /// <summary>
        /// Determines whether the control can navigate to the previous page.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanGoBack => (Pages.Count != 0) && (SelectedIndex != -1) && !(ReferenceEquals(SelectedPage, Pages[0]));

        /// <summary>
        /// Determines whether the control can navigate to the next page.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanGoNext => (Pages.Count != 0) && (SelectedIndex != -1) && !(ReferenceEquals(SelectedPage, Pages[Pages.Count - 1]));

        /// <summary>
        /// Gets the client rectangle where pages are located.
        /// </summary>
        [Browsable(false)]
        public override Rectangle DisplayRectangle => new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width, ClientRectangle.Height);

        /// <summary>
        /// Gets an array of UI controls hosted on the PagedControl.
        /// </summary>
        public virtual Control[] UIControls => new Control[0];
        #endregion

        #region Unused Methods - Hide From User
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls => base.Controls;

#pragma warning disable CS0067
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TextChanged;
#pragma warning restore CS0067
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericPagedControl"/> class.
        /// </summary>
        public PagedControl()
        {
            creatingUIControls = false;
            uiControlCount = 0;

            Pages = new PageCollection(this);
            selectedIndex = -1;
            lastSelectedPage = null;
            borderStyle = BorderStyle.FixedSingle;
            backColor = Color.Transparent;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;

            CreateChildControls();

            OnUpdateUIControls(new EventArgs());
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Navigates to the previous page.
        /// </summary>
        public void GoBack()
        {
            if (CanGoBack)
                SelectedIndex = SelectedIndex - 1;
        }

        /// <summary>
        /// Navigates to the next page.
        /// </summary>
        public void GoNext()
        {
            if (CanGoNext)
                SelectedIndex = SelectedIndex + 1;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Create the UI controls.
        /// </summary>
        private void CreateChildControls()
        {
            creatingUIControls = true;

            foreach (Control control in UIControls)
            {
                Controls.Add(control);
            }
            uiControlCount = UIControls.Length;

            creatingUIControls = false;
        }

        internal void UpdatePages()
        {
            var pageBounds = DisplayRectangle;

            for (int i = 0; i < Pages.Count; i++)
            {
                var page = Pages[i];
                page.SetBounds(pageBounds.Left, pageBounds.Top, pageBounds.Width, pageBounds.Height, BoundsSpecified.All);
                page.Visible = (i == SelectedIndex);
            }
        }
        #endregion

        #region Overriden Methods
        protected override ControlCollection CreateControlsInstance()
        {
            return new PagedControlControlCollection(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (BackColor != Color.Transparent)
            {
                using (Brush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(brush, base.ClientRectangle);
                }
            }

            if (BorderStyle == BorderStyle.FixedSingle)
                ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
            else if (BorderStyle == BorderStyle.Fixed3D)
                ControlPaint.DrawBorder3D(e.Graphics, base.ClientRectangle, Border3DStyle.SunkenOuter);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdatePages();
        }
        #endregion

        #region ControlCollection
        internal class PagedControlControlCollection : ControlCollection
        {
            private readonly PagedControl owner;

            public PagedControlControlCollection(PagedControl ownerControl) : base(ownerControl)
            {
                owner = ownerControl;
            }

            public override void Add(Control value)
            {
                if (owner.creatingUIControls)
                {
                    base.Add(value);
                    return;
                }

                if (!(value is Page))
                {
                    throw new ArgumentException("Only a Page can be hosted in a PagedControl.");
                }

                base.Add(value);
                if (owner.PageCount == 1) owner.SelectedIndex = 0;

                owner.UpdatePages();

                // site the page
                ISite site = owner.Site;
                if (site != null && value.Site == null)
                {
                    IContainer container = site.Container;
                    if (container != null)
                    {
                        container.Add(value);
                    }
                }

                owner.OnPageAdded(new PageEventArgs((Page)value, owner.SelectedIndex));
            }

            public override void Remove(Control value)
            {
                if (!base.Contains(value))
                {
                    throw new ArgumentException("Control not found in collection.", "value");
                }

                if (owner.creatingUIControls)
                {
                    base.Remove(value);
                    return;
                }

                if (!(value is Page))
                {
                    throw new ArgumentException("Only a Page can be hosted in a PagedControl.");
                }

                base.Remove(value);

                if (owner.PageCount == 0)
                    owner.SelectedIndex = -1;
                else if (owner.SelectedIndex > owner.PageCount - 1)
                    owner.SelectedIndex = 0;

                owner.UpdatePages();
                owner.OnPageRemoved(new PageEventArgs((Page)value, -1));
            }

            public override Control this[int index] => base[index];
        }
        #endregion

        #region UITypeEditor
        internal class PagedControlEditor : ObjectSelectorEditor
        {
            protected override void FillTreeWithData(Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
            {
                base.FillTreeWithData(selector, context, provider);

                var control = (PagedControl)context.Instance;

                foreach (var page in control.Pages)
                {
                    SelectorNode node = new SelectorNode(page.Name, page);
                    selector.Nodes.Add(node);

                    if (page == control.SelectedPage)
                        selector.SelectedNode = node;
                }
            }
        }
        #endregion
    }
}
