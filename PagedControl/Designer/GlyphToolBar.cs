using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represent a toolbar on the designer.
    /// </summary>
    internal class GlyphToolBar : Glyph
    {
        #region Member Variables
        private readonly BehaviorService behaviorService;
        private readonly ControlDesigner designer;
        private readonly Adorner adorner;

        private readonly List<BaseGlyph> buttons = new List<BaseGlyph>();

        private Rectangle bounds;

        private readonly Timer tooltipTimer = new Timer();
        private readonly int tooltipDelay = 750;
        private ButtonGlyph lastHotButton = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the designed control.
        /// </summary>
        public Control Control { get; private set; }

        /// <summary>
        /// Gets or sets the padding around the toolbar buttons and the toolbar border.
        /// </summary>
        public Size Padding { get; set; } = new Size(2, 2);

        /// <summary>
        /// Gets the default size of button icons.
        /// </summary>
        public Size DefaultIconSize = new Size(16, 16);

        /// <summary>
        /// Gets the location of the toolbar in adorner coordinates.
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets the size of the toolbar.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Gets the bounding rectangle of the toolbar.
        /// </summary>
        public override Rectangle Bounds => bounds;

        /// <summary>
        /// Gets the background color of the toolbar.
        /// </summary>
        public Color BackColor { get; set; } = Color.FromArgb(207, 214, 229);
        /// <summary>
        /// Gets the border color of the toolbar.
        /// </summary>
        public Color BorderColor { get; set; } = Color.FromArgb(128, 128, 128);

        /// <summary>
        /// Gets the background color of toolbar buttons.
        /// </summary>
        public Color ButtonBackColor { get; set; } = Color.FromArgb(207, 214, 229);
        /// <summary>
        /// Gets the background color of hot toolbar buttons.
        /// </summary>
        public Color HotButtonBackColor { get; set; } = Color.FromArgb(253, 244, 191);

        /// <summary>
        /// Gets the border color of toolbar buttons.
        /// </summary>
        public Color ButtonBorderColor { get; set; } = Color.FromArgb(207, 214, 229);
        /// <summary>
        /// Gets the border color of hot toolbar buttons.
        /// </summary>
        public Color HotButtonBorderColor { get; set; } = Color.FromArgb(229, 195, 101);

        /// <summary>
        /// Gets the foreground color of toolbar button texts and icon border.
        /// </summary>
        public Color ButtonForeColor { get; set; } = Color.Black;
        /// <summary>
        /// Gets the foreground color of disabled toolbar button texts and icon border.
        /// </summary>
        public Color DisabledButtonForeColor { get; set; } = Color.FromArgb(109, 109, 109);
        /// <summary>
        /// Gets the fill color of toolbar button icons.
        /// </summary>
        public Color ButtonFillColor { get; set; } = Color.FromArgb(231, 186, 10);
        /// <summary>
        /// Gets the fill color of hot toolbar button icons.
        /// </summary>
        public Color HotButtonFillColor { get; set; } = Color.FromArgb(231, 186, 10);
        /// <summary>
        /// Gets the fill color of disabled toolbar button icons.
        /// </summary>
        public Color DisabledButtonFillColor { get; set; } = Color.FromArgb(186, 186, 186);

        /// <summary>
        /// Gets the color of toolbar separators.
        /// </summary>
        public Color SeparatorColor { get; set; } = Color.FromArgb(109, 109, 109);
        #endregion

        #region Constructor
        public GlyphToolBar(BehaviorService behaviorService, ControlDesigner designer, Adorner adorner)
            : base(new GlyphToolBarBehavior())
        {
            this.behaviorService = behaviorService;
            this.designer = designer;
            this.Control = (Control)designer.Component;
            this.adorner = adorner;

            tooltipTimer.Interval = tooltipDelay;
            tooltipTimer.Tick += TooltipTimer_Tick;
        }

        private void TooltipTimer_Tick(object sender, EventArgs e)
        {
            tooltipTimer.Stop();
            if (lastHotButton != null)
                lastHotButton.ShowToolTip = true;
            Refresh();
        }
        #endregion

        #region Behavior
        /// <summary>
        /// Represents the behaviour associated with toolbars.
        /// The behaviour raises a click event when a button is clicked.
        /// </summary>
        internal class GlyphToolBarBehavior : Behavior
        {
            public override bool OnMouseDown(Glyph g, MouseButtons mouseButton, Point mouseLoc)
            {
                if (mouseButton == MouseButtons.Left)
                {
                    GlyphToolBar toolbar = (GlyphToolBar)g;

                    foreach (var glyph in toolbar.buttons.OfType<ButtonGlyph>())
                    {
                        var button = glyph;
                        if (button.Enabled && button.Bounds.Contains(mouseLoc))
                        {
                            button.OnClick(EventArgs.Empty);
                            return true;
                        }

                    }
                }

                return false;
            }
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Adds a new toolbar glyph.
        /// </summary>
        /// <param name="button"></param>
        public void AddButton(BaseGlyph button)
        {
            button.Parent = this;
            buttons.Add(button);
        }

        /// <summary>
        /// Recalculates button locations and sizes.
        /// </summary>
        public void UpdateLayout()
        {
            Point pt = behaviorService.ControlToAdornerWindow(Control);

            // calculate toolbar size
            int width = 0;
            int height = 0;

            foreach (var button in buttons)
            {
                Size size = button.Size;

                height = Math.Max(height, size.Height);
                width += size.Width + 1;
            }

            Size = new Size(width - 1, height) + Padding + Padding + new Size(2, 2);
            bounds = new Rectangle(pt.X + Location.X, pt.Y + Location.Y, Size.Width, Size.Height);

            // update button locations
            int x = pt.X + Location.X + Padding.Width + 1;
            int y = pt.Y + Location.Y + Padding.Height + 1;

            foreach (var button in buttons)
            {
                Size size = button.Size;

                button.Bounds = new Rectangle(x, y, size.Width, height);
                x += size.Width + 1;
            }
        }

        /// <summary>
        /// Refreshes the toolbar.
        /// </summary>
        public void Refresh()
        {
            adorner.Invalidate();
        }
        #endregion

        #region Overriden Methods
        /// <summary>
        /// Determines whether the given point is over a toolbar button.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override Cursor GetHitTest(Point p)
        {
            bool insideToolbar = bounds.Contains(p);
            bool needsPaint = false;
            bool hasHit = false;
            ButtonGlyph currentHotButton = null;

            foreach (var button in buttons.OfType<ButtonGlyph>())
            {
                if (!button.Enabled && button.IsHot)
                {
                    button.IsHot = false;
                    needsPaint = true;
                }
                else if (button.Enabled)
                {
                    bool newIsHot = button.Bounds.Contains(p);

                    if (newIsHot)
                    {
                        hasHit = true;
                        currentHotButton = button;
                    }

                    if (button.IsHot != newIsHot)
                    {
                        button.IsHot = newIsHot;
                        needsPaint = true;
                    }
                }
            }

            if (currentHotButton != lastHotButton)
            {
                if (lastHotButton != null && lastHotButton.ShowToolTip)
                {
                    lastHotButton.ShowToolTip = false;
                    needsPaint = true;
                }

                tooltipTimer.Stop();

                if (currentHotButton != null)
                {
                    tooltipTimer.Start();
                }

                lastHotButton = currentHotButton;
            }

            if (needsPaint) Refresh();

            return insideToolbar || hasHit ? Cursors.Default : null;
        }

        /// <summary>
        /// Paints the toolbar.
        /// </summary>
        /// <param name="pe">Paint event arguments.</param>
        public override void Paint(PaintEventArgs pe)
        {
            UpdateLayout();
            Rectangle bounds = Bounds;

            if (!pe.ClipRectangle.IntersectsWith(bounds))
                return;

            using (Brush brush = new SolidBrush(BackColor))
            using (Pen pen = new Pen(BorderColor))
            {
                pe.Graphics.FillRectangle(brush, bounds);
                pe.Graphics.DrawRectangle(pen, bounds);
            }

            foreach (var button in buttons)
            {
                button.Paint(pe);
            }

            if (lastHotButton != null && lastHotButton.ShowToolTip)
            {
                using (Brush backBrush = new SolidBrush(HotButtonBackColor))
                using (Pen borderPen = new Pen(HotButtonBorderColor))
                {
                    Size toolTipSize = TextRenderer.MeasureText(lastHotButton.ToolTipText, Control.Font);

                    Rectangle toolTipBounds = new Rectangle(lastHotButton.Bounds.Left, lastHotButton.Bounds.Top - toolTipSize.Height - 2 * Padding.Width - 2 + 1,
                        toolTipSize.Width + 2 * Padding.Width + 2, toolTipSize.Height + 2 * Padding.Height + 2);

                    pe.Graphics.FillRectangle(backBrush, toolTipBounds);
                    pe.Graphics.DrawLines(borderPen, new Point[] {
                        new Point(toolTipBounds.Left, toolTipBounds.Bottom),
                        new Point(toolTipBounds.Left, toolTipBounds.Top),
                        new Point(toolTipBounds.Right, toolTipBounds.Top),
                        new Point(toolTipBounds.Right, toolTipBounds.Bottom),
                        new Point(lastHotButton.Bounds.Right, toolTipBounds.Bottom)
                    });

                    TextRenderer.DrawText(pe.Graphics, lastHotButton.ToolTipText, Control.Font, toolTipBounds,
                        ButtonForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                }
            }
        }
        #endregion
    }
}
