using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        /// <summary>
        /// Represent a toolbar label on the designer.
        /// </summary>
        protected internal class LabelGlyph : BaseGlyph
        {
            #region Member Variables
            private Size textSize;
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets the label text.
            /// </summary>
            public string Text { get; set; } = "";

            /// <summary>
            /// Gets the size of the label.
            /// </summary>
            public override Size Size
            {
                get
                {
                    bool hasText = !string.IsNullOrEmpty(Text);

                    textSize = (hasText ? TextRenderer.MeasureText(Text, Parent.Control.Font) : Size.Empty);

                    return textSize + Padding + Padding;
                }
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

                using (Brush backBrush = new SolidBrush(Parent.ButtonBackColor))
                using (Brush textBrush = new SolidBrush(Parent.ButtonForeColor))
                {
                    if (!string.IsNullOrEmpty(Text))
                    {
                        Rectangle textBounds = GetCenteredRectangle(textSize);
                        pe.Graphics.DrawString(Text, Parent.Control.Font, textBrush, textBounds);
                    }
                }
            }
            #endregion
        }
    }
}
