using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected internal class PagedControlDesigner : PageContainerDesigner
        {
            #region Member Variables
            protected DesignerVerb addPageVerb;
            protected DesignerVerb removePageVerb;
            protected DesignerVerb navigateBackVerb;
            protected DesignerVerb navigateNextVerb;
            protected DesignerVerbCollection verbs;

            protected bool toolbarAtBottom = true;
            protected GlyphToolBar toolbar;
            protected ButtonGlyph moveToolbarButton;
            protected ButtonGlyph addPageButton;
            protected ButtonGlyph removePageButton;
            protected ButtonGlyph navigateBackButton;
            protected ButtonGlyph navigateNextButton;
            protected LabelGlyph currentPageLabel;

            private Adorner toolbarAdorner;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the list of designer verbs.
            /// </summary>
            public override DesignerVerbCollection Verbs => verbs;

            /// <summary>
            /// Gets the location of the designer toolbar relative to the parent control.
            /// </summary>
            public Point ToolbarLocation => new Point(8, toolbarAtBottom ? Control.Height - toolbar.Size.Height - 8 : 8);

            /// <summary>
            /// Gets the glyph toolbar of the designer.
            /// </summary>
            public GlyphToolBar ToolBar => toolbar;
            #endregion

            #region Glyph Icons
            private static PointF[][] GetUpDownArrowSign(float size)
            {
                float arrowSize = size;
                float arrowSeparator = 0.25f * size;

                return new PointF[][] {
                    new PointF[] {
                        new PointF(size / 2f, 0),
                        new PointF(size / 2f + arrowSize / 2f, size / 2f - arrowSeparator / 2f),
                        new PointF(size / 2f - arrowSize / 2f, size / 2f - arrowSeparator / 2f),
                    },
                    new PointF[] {
                        new PointF(size / 2f, size),
                        new PointF(size / 2f + arrowSize / 2f, size / 2f + arrowSeparator / 2f),
                        new PointF(size / 2f - arrowSize / 2f, size / 2f + arrowSeparator / 2f),
                    }
                };
            }

            private static PointF[][] GetLeftArrowSign(float size)
            {
                float arrowHeadThickness = size;
                float arrowTailThickness = 0.375f * size;
                float arrowHeadLength = 0.5625f * size;
                float arrowTailLength = size - arrowHeadLength;

                return new PointF[][] {
                    new PointF[] {
                        new PointF(0, size / 2f),
                        new PointF(arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                        new PointF(arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                        new PointF(arrowHeadLength + arrowTailLength, size / 2f - arrowTailThickness / 2f),
                        new PointF(arrowHeadLength + arrowTailLength, size / 2f + arrowTailThickness / 2f),
                        new PointF(arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                        new PointF(arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
                    }
                };
            }

            private static PointF[][] GetRightArrowSign(float size)
            {
                float arrowHeadThickness = size;
                float arrowTailThickness = 0.375f * size;
                float arrowHeadLength = 0.5625f * size;
                float arrowTailLength = size - arrowHeadLength;

                return new PointF[][] {
                    new PointF[] {
                        new PointF(size, size / 2f),
                        new PointF(size - arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                        new PointF(size - arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                        new PointF(size - arrowHeadLength - arrowTailLength, size / 2f - arrowTailThickness / 2f),
                        new PointF(size - arrowHeadLength - arrowTailLength, size / 2f + arrowTailThickness / 2f),
                        new PointF(size - arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                        new PointF(size - arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
                    }
                };
            }

            private static PointF[][] GetPlusSign(float size)
            {
                float thickness = 0.375f * size;

                return new PointF[][] {
                    new PointF[] {
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
                    }
                };
            }

            private static PointF[][] GetMinusSign(float size)
            {
                float thickness = 0.375f * size;

                return new PointF[][] {
                    new PointF[] {
                        new PointF(0, size / 2f - thickness / 2f),
                        new PointF(size, size / 2f - thickness / 2f),
                        new PointF(size, size / 2f + thickness / 2f),
                        new PointF(0, size / 2f + thickness / 2f),
                    }
                };
            }
            #endregion

            #region Initialize/Dispose
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);

                CreateVerbs();
                CreateGlyphs();

                Control.PageChanged += Control_CurrentPageChanged;
                Control.PageAdded += Control_PageAdded;
                Control.PageRemoved += Control_PageRemoved;
                Control.Resize += Control_Resize;
                Control.Move += Control_Move;

                if (SelectionService != null)
                    SelectionService.SelectionChanged += SelectionService_SelectionChanged;
            }

            public override void InitializeNewComponent(IDictionary defaultValues)
            {
                base.InitializeNewComponent(defaultValues);

                UpdateGlyphsAndVerbs();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Control.PageChanged -= Control_CurrentPageChanged;
                    Control.PageAdded -= Control_PageAdded;
                    Control.PageRemoved -= Control_PageRemoved;
                    Control.Resize -= Control_Resize;
                    Control.Move -= Control_Move;

                    navigateBackButton.Click -= NavigateBackButton_Click;
                    navigateNextButton.Click -= NavigateNextButton_Click;
                    addPageButton.Click -= AddPageButton_Click;
                    removePageButton.Click -= RemovePageButton_Click;

                    if (BehaviorService != null)
                        BehaviorService.Adorners.Remove(toolbarAdorner);

                    if (SelectionService != null)
                        SelectionService.SelectionChanged -= SelectionService_SelectionChanged;

                    toolbar.Dispose();
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
                if (BehaviorService != null)
                    BehaviorService.Adorners.Add(toolbarAdorner);

                toolbar = new GlyphToolBar(BehaviorService, this, toolbarAdorner);

                moveToolbarButton = new ButtonGlyph();
                moveToolbarButton.Path = GetUpDownArrowSign(toolbar.DefaultIconSize.Height);
                moveToolbarButton.ToolTipText = "Move toolbar";
                moveToolbarButton.Click += MoveToolbarButton_Click;

                navigateBackButton = new ButtonGlyph();
                navigateBackButton.Path = GetLeftArrowSign(toolbar.DefaultIconSize.Height);
                navigateBackButton.ToolTipText = "Previous page";
                navigateBackButton.Click += NavigateBackButton_Click;

                navigateNextButton = new ButtonGlyph();
                navigateNextButton.Path = GetRightArrowSign(toolbar.DefaultIconSize.Height);
                navigateNextButton.ToolTipText = "Next page";
                navigateNextButton.Click += NavigateNextButton_Click;

                addPageButton = new ButtonGlyph();
                addPageButton.Path = GetPlusSign(toolbar.DefaultIconSize.Height);
                addPageButton.ToolTipText = "Add a new page";
                addPageButton.Click += AddPageButton_Click;

                removePageButton = new ButtonGlyph();
                removePageButton.Path = GetMinusSign(toolbar.DefaultIconSize.Height);
                removePageButton.ToolTipText = "Remove current page";
                removePageButton.Click += RemovePageButton_Click;

                currentPageLabel = new LabelGlyph();
                currentPageLabel.Text = string.Format("Page {0} of {1}", Control.SelectedIndex + 1, Control.Pages.Count);

                toolbar.AddButton(moveToolbarButton);
                toolbar.AddButton(new SeparatorGlyph());
                toolbar.AddButton(navigateBackButton);
                toolbar.AddButton(currentPageLabel);
                toolbar.AddButton(navigateNextButton);
                toolbar.AddButton(new SeparatorGlyph());
                toolbar.AddButton(addPageButton);
                toolbar.AddButton(removePageButton);

                toolbarAdorner.Glyphs.Add(toolbar);
            }

            /// <summary>
            /// Updates the adorner when the selection is changed.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void SelectionService_SelectionChanged(object sender, EventArgs e)
            {
                bool showAdorner = false;

                if (SelectionService != null && SelectionService.PrimarySelection != null)
                {
                    if (SelectionService.PrimarySelection == Control)
                        showAdorner = true;
                    else if (SelectionService.PrimarySelection is Page page && page.Parent == Control)
                        showAdorner = true;
                }

                toolbarAdorner.Enabled = toolbar.Visible && showAdorner;
            }

            /// <summary>
            /// Updates verbs and toolbar buttons when the current page is changed.
            /// </summary>
            private void Control_CurrentPageChanged(object sender, PageChangedEventArgs e)
            {
                UpdateGlyphsAndVerbs();
            }

            /// <summary>
            /// Updates verbs and toolbar buttons when a new page is added.
            /// </summary>
            private void Control_PageAdded(object sender, PageEventArgs e)
            {
                UpdateGlyphsAndVerbs();
            }

            /// <summary>
            /// Updates verbs and toolbar buttons when a page is removed.
            /// </summary>
            private void Control_PageRemoved(object sender, PageEventArgs e)
            {
                UpdateGlyphsAndVerbs();
            }

            /// <summary>
            /// Relocate the toolbar when the control is resized.
            /// </summary>
            private void Control_Resize(object sender, EventArgs e)
            {
                UpdateGlyphsAndVerbs();
            }

            /// <summary>
            /// Relocate the toolbar when the control is moved.
            /// </summary>
            private void Control_Move(object sender, EventArgs e)
            {
                UpdateGlyphsAndVerbs();
            }

            /// <summary>
            /// Updates the visual states of the toolbar and its glyphs.
            /// </summary>
            private void UpdateGlyphsAndVerbs()
            {
                removePageVerb.Enabled = removePageButton.Enabled = (Control.Pages.Count > 1);
                navigateBackVerb.Enabled = navigateBackButton.Enabled = (Control.SelectedIndex > 0);
                navigateNextVerb.Enabled = navigateNextButton.Enabled = (Control.SelectedIndex < Control.Pages.Count - 1);
                currentPageLabel.Text = Control.Pages.Count == 0 ? "" : string.Format("Page {0} of {1}", Control.SelectedIndex + 1, Control.Pages.Count);

                toolbar.UpdateLayout();
                toolbar.Location = ToolbarLocation;
                toolbar.Refresh();
            }

            private void MoveToolbarButton_Click(object sender, EventArgs e)
            {
                toolbarAtBottom = !toolbarAtBottom;

                UpdateGlyphsAndVerbs();
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
        }
    }
}
