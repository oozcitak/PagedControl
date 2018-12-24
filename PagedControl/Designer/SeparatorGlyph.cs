using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        /// <summary>
        /// Represent a toolbar separator on the designer.
        /// </summary>
        protected internal class SeparatorGlyph : BaseGlyph
        {
            #region Member Variables
            private Size lineSize;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the size of the label.
            /// </summary>
            public override Size Size
            {
                get
                {
                    lineSize = new Size(1, Parent.DefaultIconSize.Height);

                    return lineSize + Padding + Padding;
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

                using (Pen linePen = new Pen(Parent.SeparatorColor))
                {
                    Rectangle lineBounds = GetCenteredRectangle(lineSize);
                    pe.Graphics.DrawLine(linePen, lineBounds.Left, lineBounds.Top, lineBounds.Left, lineBounds.Bottom);
                }
            }
            #endregion
        }
    }
}
