using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public class PagedControlDesigner : ParentControlDesigner
    {
        #region Member Variables
        private BehaviorService behaviorService;
        private ISelectionService selectionService;

        private DesignerVerb addPageVerb;
        private DesignerVerb removePageVerb;
        private DesignerVerb navigateBackVerb;
        private DesignerVerb navigateNextVerb;
        private DesignerVerbCollection verbs;

        private GlyphToolBar toolbar;
        private ButtonGlyph addPageButton;
        private ButtonGlyph removePageButton;
        private ButtonGlyph navigateBackButton;
        private ButtonGlyph navigateNextButton;
        private LabelGlyph currentPageLabel;

        private Adorner toolbarAdorner;
        #endregion

        #region Properties
        public override DesignerVerbCollection Verbs => verbs;

        public new PagedControl<Page> Control => (PagedControl<Page>)base.Control;

        public virtual Point ToolbarLocation => new Point(Control.Left + 8, Control.Bottom - 8 - toolbar.Bounds.Height);
        #endregion

        #region Glyph Icons
        private static PointF[] GetLeftArrowSign(float size)
        {
            float arrowHeadThickness = size;
            float arrowTailThickness = 0.375f * size;
            float arrowHeadLength = 0.5625f * size;
            float arrowTailLength = size - arrowHeadLength;

            return new PointF[] {
                    new PointF(0, size / 2f),
                    new PointF(arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                    new PointF(arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                    new PointF(arrowHeadLength + arrowTailLength, size / 2f - arrowTailThickness / 2f),
                    new PointF(arrowHeadLength + arrowTailLength, size / 2f + arrowTailThickness / 2f),
                    new PointF(arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                    new PointF(arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
                };
        }

        private static PointF[] GetRightArrowSign(float size)
        {
            float arrowHeadThickness = size;
            float arrowTailThickness = 0.375f * size;
            float arrowHeadLength = 0.5625f * size;
            float arrowTailLength = size - arrowHeadLength;

            return new PointF[] {
                    new PointF(size, size / 2f),
                    new PointF(size - arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                    new PointF(size - arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                    new PointF(size - arrowHeadLength - arrowTailLength, size / 2f - arrowTailThickness / 2f),
                    new PointF(size - arrowHeadLength - arrowTailLength, size / 2f + arrowTailThickness / 2f),
                    new PointF(size - arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                    new PointF(size - arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
                };
        }

        private static PointF[] GetPlusSign(float size)
        {
            float thickness = 0.375f * size;

            return new PointF[] {
                    new PointF(0, size / 2f - thickness / 2f),
                    new PointF(size / 2f - thickness / 2f, size / 2f - thickness / 2f),
                    new PointF(size / 2f - thickness / 2f, 0),
                    new PointF(size / 2f + thickness / 2f, 0),
                    new PointF(size / 2f + thickness / 2f, size / 2f - thickness / 2f),
                    new PointF(size, size / 2f - thickness / 2f),
                    new PointF(size, size / 2f + thickness / 2f),
                    new PointF(size / 2f + thickness / 2f, size / 2f + thickness / 2f),
                    new PointF(size / 2f + thickness / 2f, size),
                    new PointF(size / 2f - thickness / 2f, size),
                    new PointF(size / 2f - thickness / 2f, size / 2f + thickness / 2f),
                    new PointF(0, size / 2f + thickness / 2f),
                };
        }

        private static PointF[] GetMinusSign(float size)
        {
            float thickness = 0.375f * size;

            return new PointF[] {
                    new PointF(0, size / 2f - thickness / 2f),
                    new PointF(size, size / 2f - thickness / 2f),
                    new PointF(size, size / 2f + thickness / 2f),
                    new PointF(0, size / 2f + thickness / 2f),
                };
        }
        #endregion

        #region Initialize/Dispose
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            behaviorService = (BehaviorService)GetService(typeof(BehaviorService));
            selectionService = (ISelectionService)GetService(typeof(ISelectionService));

            CreateVerbs();
            CreateGlyphs();

            Control.ControlAdded += Control_ControlAdded;
            Control.ControlRemoved += Control_ControlRemoved;
            Control.Resize += Control_Resize;
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            // add a default page
            AddPageHandler(this, EventArgs.Empty);

            MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Controls"];
            RaiseComponentChanging(member);
            RaiseComponentChanged(member, null, null);

            Control.SelectedIndex = 0;

            UpdateGlyphs();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.PageChanged -= Control_CurrentPageChanged;
                Control.ControlAdded -= Control_ControlAdded;
                Control.ControlRemoved -= Control_ControlRemoved;
                Control.Resize -= Control_Resize;

                navigateBackButton.Click -= NavigateBackButton_Click;
                navigateNextButton.Click -= NavigateNextButton_Click;
                addPageButton.Click -= AddPageButton_Click;
                removePageButton.Click -= RemovePageButton_Click;

                if (behaviorService != null)
                    behaviorService.Adorners.Remove(toolbarAdorner);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates the designer verbs.
        /// </summary>
        private void CreateVerbs()
        {
            navigateBackVerb = new DesignerVerb("Previous page", new EventHandler(NavigateBackHandler));
            navigateNextVerb = new DesignerVerb("Next page", new EventHandler(NavigateNextHandler));
            addPageVerb = new DesignerVerb("Add page", new EventHandler(AddPageHandler));
            removePageVerb = new DesignerVerb("Remove page", new EventHandler(RemovePageHandler));

            verbs = new DesignerVerbCollection();
            verbs.AddRange(new DesignerVerb[] {
                    navigateBackVerb, navigateNextVerb, addPageVerb, removePageVerb
                });
        }

        /// <summary>
        /// Creates the glyphs for navigation and manipulating pages
        /// </summary>
        private void CreateGlyphs()
        {
            toolbarAdorner = new Adorner();
            behaviorService.Adorners.Add(toolbarAdorner);

            toolbar = new GlyphToolBar(behaviorService, this, toolbarAdorner);

            navigateBackButton = new ButtonGlyph();
            navigateBackButton.Path = GetLeftArrowSign(toolbar.DefaultIconSize.Height);
            navigateBackButton.ToolTipText = "Previous page";

            navigateNextButton = new ButtonGlyph();
            navigateNextButton.Path = GetRightArrowSign(toolbar.DefaultIconSize.Height);
            navigateNextButton.ToolTipText = "Next page";

            addPageButton = new ButtonGlyph();
            addPageButton.Path = GetPlusSign(toolbar.DefaultIconSize.Height);
            addPageButton.ToolTipText = "Add a new page";

            removePageButton = new ButtonGlyph();
            removePageButton.Path = GetMinusSign(toolbar.DefaultIconSize.Height);
            removePageButton.ToolTipText = "Remove current page";

            currentPageLabel = new LabelGlyph();
            currentPageLabel.Text = string.Format("Page {0} of {1}", Control.SelectedIndex + 1, Control.Pages.Count);

            navigateBackButton.Click += NavigateBackButton_Click;
            navigateNextButton.Click += NavigateNextButton_Click;
            addPageButton.Click += AddPageButton_Click;
            removePageButton.Click += RemovePageButton_Click;

            toolbar.AddButton(navigateBackButton);
            toolbar.AddButton(currentPageLabel);
            toolbar.AddButton(navigateNextButton);
            toolbar.AddButton(new SeparatorGlyph());
            toolbar.AddButton(addPageButton);
            toolbar.AddButton(removePageButton);

            toolbarAdorner.Glyphs.Add(toolbar);
        }

        /// <summary>
        /// Updates verbs and toolbar buttons when the current page is changed.
        /// </summary>
        private void Control_CurrentPageChanged(object sender, PagedControl<Page>.PageChangedEventArgs e)
        {
            UpdateGlyphs();
        }

        /// <summary>
        /// Updates verbs and toolbar buttons when a new page is added.
        /// </summary>
        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            UpdateGlyphs();
        }

        /// <summary>
        /// Updates verbs and toolbar buttons when a page is removed.
        /// </summary>
        private void Control_ControlRemoved(object sender, ControlEventArgs e)
        {
            UpdateGlyphs();
        }

        /// <summary>
        /// Relocate the toolbar when the control is resized.
        /// </summary>
        private void Control_Resize(object sender, EventArgs e)
        {
            toolbar.UpdateLayout();
            toolbar.Location = ToolbarLocation;
            toolbar.Refresh();
        }

        /// <summary>
        /// Gets the designer of the current page.
        /// </summary>
        /// <returns>The designer of the wizard page currently active in the designer.</returns>
        private Page.PageDesigner GetCurrentPageDesigner()
        {
            var page = Control.SelectedPage;
            if (page != null)
            {
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                if (host != null)
                    return (Page.PageDesigner)host.GetDesigner(page);
            }
            return null;
        }

        /// <summary>
        /// Updates the visual states of glyphs.
        /// </summary>
        private void UpdateGlyphs()
        {
            removePageVerb.Enabled = removePageButton.Enabled = (Control.Pages.Count > 1);
            navigateBackVerb.Enabled = navigateBackButton.Enabled = (Control.SelectedIndex > 0);
            navigateNextVerb.Enabled = navigateNextButton.Enabled = (Control.SelectedIndex < Control.Pages.Count - 1);
            currentPageLabel.Text = Control.Pages.Count == 0 ? "" : string.Format("Page {0} of {1}", Control.SelectedIndex + 1, Control.Pages.Count);

            toolbar.Refresh();
        }

        private void NavigateBackButton_Click(object sender, EventArgs e)
        {
            NavigateBackHandler(this, EventArgs.Empty);
        }

        private void NavigateNextButton_Click(object sender, EventArgs e)
        {
            NavigateNextHandler(this, EventArgs.Empty);
        }

        private void AddPageButton_Click(object sender, EventArgs e)
        {
            AddPageHandler(this, EventArgs.Empty);
        }

        private void RemovePageButton_Click(object sender, EventArgs e)
        {
            RemovePageHandler(this, EventArgs.Empty);
        }
        #endregion

        #region Verb Handlers
        /// <summary>
        /// Adds a new wizard page.
        /// </summary>
        protected void AddPageHandler(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

            if (host != null)
            {
                Page page = (Page)host.CreateComponent(typeof(Page));
                Control.Pages.Add(page);
                Control.SelectedPage = page;

                selectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
            }
        }

        /// <summary>
        /// Removes the current wizard page.
        /// </summary>
        protected void RemovePageHandler(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

            if (host != null)
            {
                if (Control.Pages.Count > 1)
                {
                    Page page = Control.SelectedPage;
                    if (page != null)
                    {
                        int index = Control.SelectedIndex;
                        //Control.Pages.Remove(page);
                        host.DestroyComponent(page);
                        if (index == Control.Pages.Count)
                            index = Control.Pages.Count - 1;
                        Control.SelectedIndex = index;

                        selectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
                    }
                }
            }
        }

        /// <summary>
        /// Navigates to the previous wizard page.
        /// </summary>
        protected void NavigateBackHandler(object sender, EventArgs e)
        {
            Control.GoBack();

            selectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
        }

        /// <summary>
        /// Navigates to the next wizard page.
        /// </summary>
        protected void NavigateNextHandler(object sender, EventArgs e)
        {
            Control.GoNext();

            selectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
        }
        #endregion

        #region Parent/Child Relation
        public override bool CanParent(Control control)
        {
            return (control is Page);
        }

        public override bool CanParent(ControlDesigner controlDesigner)
        {
            return (controlDesigner != null && controlDesigner.Component is Page);
        }
        #endregion

        #region Delegate All Drag Events To The Current Page
        protected override void OnDragEnter(DragEventArgs de)
        {
            GetCurrentPageDesigner().OnDragEnter(de);
        }

        protected override void OnDragOver(DragEventArgs de)
        {
            Point pt = Control.PointToClient(new Point(de.X, de.Y));

            if (GetCurrentPageDesigner().Control.DisplayRectangle.Contains(pt))
                GetCurrentPageDesigner().OnDragOver(de);
            else
                de.Effect = DragDropEffects.None;
        }

        protected override void OnDragLeave(EventArgs e)
        {
            GetCurrentPageDesigner().OnDragLeave(e);
        }

        protected override void OnDragDrop(DragEventArgs de)
        {
            GetCurrentPageDesigner().OnDragDrop(de);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            GetCurrentPageDesigner().OnGiveFeedback(e);
        }

        protected override void OnDragComplete(DragEventArgs de)
        {
            GetCurrentPageDesigner().OnDragComplete(de);
        }
        #endregion
    }
}
