using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace TestApp
{
    internal class PagedControlTest : PagedControl
    {
        private Button PrevButton = new Button();
        private Button NextButton = new Button();
        private Label CurrentPageLabel = new Label();
        private Button AddButton = new Button();
        private Button RemoveButton = new Button();
        private Button ClearButton = new Button();
        private Button InsertButton = new Button();
        private Button ClearLogButton = new Button();

        private List<Tuple<string, Color>> messages = new List<Tuple<string, Color>>();

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            PaintInfo(e.Graphics, DisplayRectangle, SystemColors.Control);
        }

        protected override void OnPagePaint(PagePaintEventArgs e)
        {
            base.OnPagePaint(e);

            PaintInfo(e.Graphics, e.Page.ClientRectangle, Color.White);
        }

        private void PaintInfo(Graphics g, Rectangle bounds, Color backColor)
        {
            bounds.Inflate(-4, -4);
            g.Clip = new Region(bounds);

            var y = bounds.Top + 6;
            var burn = 0f;
            var burnStep = 0.9f / (bounds.Height / (g.MeasureString("M", Font).Height + 4));
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var message = messages[i].Item1;
                var color = messages[i].Item2;
                var h = (int)g.MeasureString(message, Font).Height;
                using (var brush = new SolidBrush(Color.FromArgb((int)(color.R + (255 - color.R) * burn), (int)(color.G + (255 - color.G) * burn), (int)(color.B + (255 - color.B) * burn))))
                {
                    g.DrawString(message, Font, brush, 20, y);
                }
                y += h + 4;
                burn += burnStep;
                if (burn > 0.9f) burn = 0.9f;
            }

            string currentPageStr = string.Format("Selected Page: {0}", (SelectedPage != null) ? SelectedPage.Name : "<none>");
            string currentIndexStr = string.Format("Selected Index: {0}", SelectedIndex);
            var size1 = g.MeasureString(currentPageStr, Font);
            var size2 = g.MeasureString(currentIndexStr, Font);
            g.DrawString(currentPageStr, Font, Brushes.Red, bounds.Right - 6 - Math.Max(size1.Width, size2.Width), bounds.Top + 6);
            g.DrawString(currentIndexStr, Font, Brushes.Red, bounds.Right - 6 - Math.Max(size1.Width, size2.Width), bounds.Top + 22);
        }

        protected override void OnCreateUIControls(CreateUIControlsEventArgs e)
        {
            int width = ClientRectangle.Width;
            int height = ClientRectangle.Height;

            // PrevButton
            PrevButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PrevButton.Location = new Point(194, height - 23 - 12);
            PrevButton.Size = new Size(23, 23);
            PrevButton.Text = "<";
            PrevButton.Click += PrevButton_Click;
            // NextButton
            NextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NextButton.Location = new Point(216, height - 23 - 12);
            NextButton.Size = new Size(23, 23);
            NextButton.Text = ">";
            NextButton.Click += NextButton_Click;
            // CurrentPageLabel
            CurrentPageLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CurrentPageLabel.AutoSize = true;
            CurrentPageLabel.Location = new Point(245, height - 13 - 12 - 5);
            CurrentPageLabel.Text = "<none>";
            // AddButton
            AddButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            AddButton.Location = new Point(13, height - 23 - 12);
            AddButton.Size = new Size(23, 23);
            AddButton.Text = "+";
            AddButton.Click += AddButton_Click;
            // RemoveButton
            RemoveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            RemoveButton.Location = new Point(99, height - 23 - 12);
            RemoveButton.Size = new Size(23, 23);
            RemoveButton.Text = "-";
            RemoveButton.Click += RemoveButton_Click;
            // ClearButton
            ClearButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ClearButton.Location = new Point(121, height - 23 - 12);
            ClearButton.Size = new Size(65, 23);
            ClearButton.Text = "Clear";
            ClearButton.Click += ClearButton_Click;
            // InsertButton
            InsertButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            InsertButton.Location = new Point(35, height - 23 - 12);
            InsertButton.Size = new Size(65, 23);
            InsertButton.Text = "Insert @ 0";
            InsertButton.Click += InsertButton_Click;
            // ClearLogButton
            ClearLogButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ClearLogButton.Location = new Point(width - 65 - 12, height - 23 - 12);
            ClearLogButton.Size = new Size(65, 23);
            ClearLogButton.Text = "Clear Log";
            ClearLogButton.Click += ClearLogButton_Click;

            e.Controls = new Control[] {
                PrevButton,
                NextButton,
                CurrentPageLabel,
                AddButton,
                RemoveButton,
                ClearButton,
                InsertButton,
                ClearLogButton
            };

            base.OnCreateUIControls(e);
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                var bounds = base.DisplayRectangle;
                bounds.Height -= 23 + 2 * 12;
                return bounds;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Pages.Add(MakePage());
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            Pages.Insert(0, MakePage());
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (SelectedPage != null)
                Pages.Remove(SelectedPage);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Pages.Clear();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            GoNext();
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            messages.Clear();
            Invalidate();
        }

        private void Log(string message, params object[] args)
        {
            Log(Color.Black, message, args);
        }

        private void Log(Color color, string message, params object[] args)
        {
            messages.Add(new Tuple<string, Color>(string.Format(message, args), color));
            Invalidate();
        }

        private Page MakePage()
        {
            int i = 1;
            string name = "page" + i.ToString();
            while (Pages.Any(p => p.Name == name))
            {
                i++;
                name = "page" + i.ToString();
            }

            Page page = new Page();
            page.Name = name;
            return page;
        }

        private string PageName(Page page) => string.Format("{0}", page?.Name ?? "<none>");

        protected override void OnPageChanged(PageChangedEventArgs e)
        {
            base.OnPageChanged(e);
            Log(Color.DarkGreen, "Page Changed: {0} -> {1}", PageName(e.OldPage), PageName(e.CurrentPage));
            CurrentPageLabel.Text = string.Format("Page {0} of {1}", SelectedIndex + 1, Pages.Count);
        }

        protected override void OnPageChanging(PageChangingEventArgs e)
        {
            base.OnPageChanging(e);
            Log(Color.DarkGreen, "Page Changing: {0} -> {1}", PageName(e.CurrentPage), PageName(e.NewPage));
        }

        protected override void OnPageHidden(PageEventArgs e)
        {
            base.OnPageHidden(e);
            Log(Color.Magenta, "Page Hidden: {0}", PageName(e.Page));
        }

        protected override void OnPageShown(PageEventArgs e)
        {
            base.OnPageShown(e);
            Log(Color.Magenta, "Page Shown: {0}", PageName(e.Page));
        }

        protected override void OnPageAdded(PageEventArgs e)
        {
            base.OnPageAdded(e);
            Log(Color.OrangeRed, "Page Added: {0}", PageName(e.Page));
        }

        protected override void OnPageRemoved(PageEventArgs e)
        {
            base.OnPageRemoved(e);
            Log(Color.OrangeRed, "Page Removed: {0}", PageName(e.Page));
        }

        protected override void OnPageValidated(PageEventArgs e)
        {
            base.OnPageValidated(e);
            Log(Color.BlueViolet, "Page Validated: {0}", PageName(e.Page));
        }

        protected override void OnPageValidating(PageValidatingEventArgs e)
        {
            base.OnPageValidating(e);
            Log(Color.BlueViolet, "Page Validating: {0}", PageName(e.Page));
        }
    }
}
