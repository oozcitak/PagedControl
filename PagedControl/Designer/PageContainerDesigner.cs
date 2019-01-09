using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected internal class PageContainerDesigner : ParentControlDesigner
        {
            #region Properties
            /// <summary>
            /// Gets the designer host.
            /// </summary>
            public IDesignerHost DesignerHost { get; private set; }

            /// <summary>
            /// Gets the selection service.
            /// </summary>
            public ISelectionService SelectionService { get; private set; }

            /// <summary>
            /// Gets the parent control.
            /// </summary>
            public new PagedControl Control => (PagedControl)base.Control;
            #endregion

            #region Initialize/Dispose
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);

                DesignerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
                SelectionService = (ISelectionService)GetService(typeof(ISelectionService));
            }

            public override void InitializeNewComponent(IDictionary defaultValues)
            {
                base.InitializeNewComponent(defaultValues);

                if (Control.Pages.Count == 0)
                {
                    // add two default pages
                    AddPageHandler(this, EventArgs.Empty);
                    AddPageHandler(this, EventArgs.Empty);

                    MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Controls"];
                    RaiseComponentChanging(member);
                    RaiseComponentChanged(member, null, null);

                    Control.SelectedIndex = 0;
                }
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Gets the designer of the current page.
            /// </summary>
            /// <returns>The designer of the page currently active in the designer.</returns>
            internal Page.PageDesigner CurrentPageDesigner
            {
                get
                {
                    var page = Control.SelectedPage;
                    if (page != null)
                    {
                        if (DesignerHost != null)
                            return (Page.PageDesigner)DesignerHost.GetDesigner(page);
                    }
                    return null;
                }
            }
            #endregion

            #region Verb Handlers
            /// <summary>
            /// Adds a new page.
            /// </summary>
            protected void AddPageHandler(object sender, EventArgs e)
            {
                if (DesignerHost != null)
                {
                    Page page = (Page)DesignerHost.CreateComponent(typeof(Page));
                    page.Text = string.Format("Page {0}", Control.Pages.Count + 1);
                    Control.Pages.Add(page);
                    Control.SelectedPage = page;

                    if (SelectionService != null)
                        SelectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
                }
            }

            /// <summary>
            /// Removes the current page.
            /// </summary>
            protected void RemovePageHandler(object sender, EventArgs e)
            {
                if (DesignerHost != null)
                {
                    if (Control.Pages.Count > 1)
                    {
                        Page page = Control.SelectedPage;
                        if (page != null)
                        {
                            int index = Control.SelectedIndex;

                            DesignerHost.DestroyComponent(page);
                            if (index == Control.Pages.Count)
                                index = Control.Pages.Count - 1;
                            Control.SelectedIndex = index;

                            if (SelectionService != null)
                                SelectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
                        }
                    }
                }
            }

            /// <summary>
            /// Navigates to the previous page.
            /// </summary>
            protected void NavigateBackHandler(object sender, EventArgs e)
            {
                Control.GoBack();

                SelectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
            }

            /// <summary>
            /// Navigates to the next page.
            /// </summary>
            protected void NavigateNextHandler(object sender, EventArgs e)
            {
                Control.GoNext();

                SelectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
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
                var pageDesigner = CurrentPageDesigner;
                if (pageDesigner == null)
                    base.OnDragEnter(de);
                else
                    pageDesigner.OnDragEnter(de);
            }

            protected override void OnDragOver(DragEventArgs de)
            {
                var pageDesigner = CurrentPageDesigner;
                if (pageDesigner == null)
                    base.OnDragOver(de);
                else
                {
                    Point pt = Control.PointToClient(new Point(de.X, de.Y));

                    if (pageDesigner.Control.DisplayRectangle.Contains(pt))
                        pageDesigner.OnDragOver(de);
                    else
                        de.Effect = DragDropEffects.None;
                }
            }

            protected override void OnDragLeave(EventArgs e)
            {
                var pageDesigner = CurrentPageDesigner;
                if (pageDesigner == null)
                    base.OnDragLeave(e);
                else
                    pageDesigner.OnDragLeave(e);
            }

            protected override void OnDragDrop(DragEventArgs de)
            {
                var pageDesigner = CurrentPageDesigner;
                if (pageDesigner == null)
                    base.OnDragDrop(de);
                else
                    pageDesigner.OnDragDrop(de);
            }

            protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
            {
                var pageDesigner = CurrentPageDesigner;
                if (pageDesigner == null)
                    base.OnGiveFeedback(e);
                else
                    pageDesigner.OnGiveFeedback(e);
            }

            protected override void OnDragComplete(DragEventArgs de)
            {
                var pageDesigner = CurrentPageDesigner;
                if (pageDesigner == null)
                    base.OnDragComplete(de);
                else
                    pageDesigner.OnDragComplete(de);
            }
            #endregion
        }
    }
}
