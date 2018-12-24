using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        /// <summary>
        /// Represent a toolbar button on the designer.
        /// </summary>
        protected internal class ButtonGlyph : BaseGlyph
        {
            #region Member Variables
            private Size iconSize;
            private Size textSize;
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets the array of graphics paths representing the button icon. Each point array in the list
            /// represents a separate closed path. The coordinates of the upper-left corner of the path should be 0, 0. 
            /// </summary>
            public PointF[][] Path { get; set; } = new PointF[][] { new PointF[0] };

            /// <summary>
            /// Gets or sets the button text.
            /// </summary>
            public string Text { get; set; } = "";

            /// <summary>
            /// Gets or sets the button tooltip text.
            /// </summary>
            public string ToolTipText { get; set; } = "";

            /// <summary>
            /// Gets or sets whether the button is enabled.
            /// </summary>
            public bool Enabled { get; set; } = true;

            /// <summary>
            /// Gets or sets whether the mouse cursor is over the button.
            /// </summary>
            internal bool IsHot { get; set; } = false;

            /// <summary>
            /// Gets or sets whether the tooltip text should be displayed.
            /// </summary>
            internal bool ShowToolTip { get; set; } = false;

            /// <summary>
            /// Gets the size of the button.
            /// </summary>
            public override Size Size
            {
                get
                {
                    bool hasIcon = (Path != null && Path.Length != 0);
                    bool hasText = !string.IsNullOrEmpty(Text);

                    iconSize = (hasIcon ? Parent.DefaultIconSize : Size.Empty);
                    textSize = (hasText ? TextRenderer.MeasureText(Text, Parent.Control.Font) : Size.Empty);

                    return new Size(iconSize.Width + textSize.Width + (hasIcon && hasText ? 2 : 0),
                        Math.Max(iconSize.Height, textSize.Height)) + Padding + Padding + new Size(2, 2);
                }
            }
            #endregion

            #region Events
            /// <summary>
            /// Occurs when the mouse is clicked when the cursor is over the button.
            /// </summary>
            public event EventHandler Click;

            /// <summary>
            /// Raises the <see cref="Click"/> event.
            /// </summary>
            /// <param name="e"></param>
            protected internal virtual void OnClick(EventArgs e)
            {
                Click?.Invoke(this, e);
            }
            #endregion

            #region Overriden Methods
            /// <summary>
            /// Paints the glyph. The base class paints the background only.
            /// </summary>
            /// <param name="pe">Paint event arguments.</param>
            public override void Paint(PaintEventArgs pe)
            {
                base.Paint(pe);

                using (Brush backBrush = new SolidBrush(IsHot ? Parent.HotButtonBackColor : Parent.ButtonBackColor))
                using (Pen borderPen = new Pen(IsHot ? Parent.HotButtonBorderColor : Parent.ButtonBorderColor))
                using (Brush pathBrush = new SolidBrush(IsHot ? Parent.HotButtonFillColor : !Enabled ? Parent.DisabledButtonFillColor : Parent.ButtonFillColor))
                using (Pen pathPen = new Pen(!Enabled ? Parent.DisabledButtonForeColor : Parent.ButtonForeColor))
                {
                    Rectangle bounds = Bounds;

                    pe.Graphics.FillRectangle(backBrush, bounds);
                    pe.Graphics.DrawRectangle(borderPen, bounds);

                    if (Path != null && Path.Length != 0)
                    {
                        foreach (var subPath in Path)
                        {
                            if (subPath != null && subPath.Length != 0)
                            {
                                Rectangle iconBounds = GetCenteredRectangle(iconSize);

                                var oldTrans = pe.Graphics.Transform;
                                pe.Graphics.TranslateTransform(iconBounds.Left, iconBounds.Top);
                                pe.Graphics.FillPolygon(pathBrush, subPath);
                                pe.Graphics.DrawPolygon(pathPen, subPath);
                                pe.Graphics.Transform = oldTrans;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Text))
                    {
                        Rectangle textBounds = GetCenteredRectangle(textSize);

                        TextRenderer.DrawText(pe.Graphics, Text, Parent.Control.Font, textBounds,
                            (!Enabled ? Parent.DisabledButtonForeColor : Parent.ButtonForeColor),
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                    }
                }
            }
            #endregion
        }
    }
}
