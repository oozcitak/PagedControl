﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace TestApp
{
    public partial class TestForm : Form
    {
        private List<Tuple<string, Color>> messages = new List<Tuple<string, Color>>();

        public TestForm()
        {
            InitializeComponent();

            UpdatePageLabel();
        }

        private Page MakePage()
        {
            int i = 1;
            string name = "page" + i.ToString();
            while (pagedControl1.Pages.Any(p => p.Name == name))
            {
                i++;
                name = "page" + i.ToString();
            }

            Page page = new Page();
            page.Name = name;
            return page;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            pagedControl1.Pages.Add(MakePage());
            UpdatePageLabel();
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            pagedControl1.Pages.Insert(0, MakePage());
            UpdatePageLabel();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (pagedControl1.SelectedPage != null)
                pagedControl1.Pages.Remove(pagedControl1.SelectedPage);
            UpdatePageLabel();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            pagedControl1.Pages.Clear();
            UpdatePageLabel();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            pagedControl1.GoBack();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            pagedControl1.GoNext();
            UpdatePageLabel();
        }

        private void Log(string message, params object[] args)
        {
            Log(Color.Black, message, args);
        }

        private void Log(Color color, string message, params object[] args)
        {
            messages.Add(new Tuple<string, Color>(string.Format(message, args), color));
            UpdatePageLabel();
            pagedControl1.Invalidate();
        }

        private void UpdatePageLabel()
        {
            CurrentPageLabel.Text = string.Format("Current Page: {0}, Page Count: {1}", pagedControl1.SelectedIndex, pagedControl1.Pages.Count);
        }

        private void pagedControl1_PageChanged(object sender, PagedControl.PageChangedEventArgs e)
        {
            Log(Color.LawnGreen, "Page Changed: {0} -> {1}", e.OldPageIndex, e.CurrentPageIndex);
        }

        private void pagedControl1_PageChanging(object sender, PagedControl.PageChangingEventArgs e)
        {
            Log(Color.LawnGreen, "Page Changing: {0} -> {1}", e.CurrentPageIndex, e.NewPageIndex);
        }

        private void pagedControl1_PageHidden(object sender, PagedControl.PageEventArgs e)
        {
            Log(Color.Magenta, "Page Hidden: {0}", e.PageIndex);
        }

        private void pagedControl1_PageShown(object sender, PagedControl.PageEventArgs e)
        {
            Log(Color.Magenta, "Page Shown: {0}", e.PageIndex);
        }

        private void pagedControl1_PageAdded(object sender, PagedControl.PageEventArgs e)
        {
            Log(Color.OrangeRed, "Page Added: {0}", e.PageIndex);
        }

        private void pagedControl1_PageRemoved(object sender, PagedControl.PageEventArgs e)
        {
            Log(Color.OrangeRed, "Page Removed: {0}", e.PageIndex);
        }

        private void pagedControl1_PageValidated(object sender, PagedControl.PageEventArgs e)
        {
            Log(Color.BlueViolet, "Page Validated: {0}", e.PageIndex);
        }

        private void pagedControl1_PageValidating(object sender, PagedControl.PageValidatingEventArgs e)
        {
            Log(Color.BlueViolet, "Page Validating: {0}", e.PageIndex);
        }

        private void pagedControl1_PagePaint(object sender, PagedControl.PagePaintEventArgs e)
        {
            var bounds = e.Page.DisplayRectangle;
            bounds.Inflate(-10, -10);
            e.Graphics.DrawRectangle(Pens.Red, bounds);
            bounds.Inflate(-4, -4);
            e.Graphics.Clip = new Region(bounds);

            var y = bounds.Top + 6;
            var burn = 0f;
            var burnStep = 0.9f / (bounds.Height / (e.Graphics.MeasureString("M", e.Page.Font).Height + 4));
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var message = messages[i].Item1;
                var color = messages[i].Item2;
                var h = (int)e.Graphics.MeasureString(message, e.Page.Font).Height;
                using (var brush = new SolidBrush(Color.FromArgb((int)(color.R + (255 - color.R) * burn), (int)(color.G + (255 - color.G) * burn), (int)(color.B + (255 - color.B) * burn))))
                {
                    e.Graphics.DrawString(message, e.Page.Font, brush, 20, y);
                }
                y += h + 4;
                burn += burnStep;
                if (burn > 0.9f) burn = 0.9f;
            }

            string currentPageStr = string.Format("Selected page: {0}", (pagedControl1.SelectedPage != null) ? pagedControl1.SelectedPage.Name : "<none>");
            var size = e.Graphics.MeasureString(currentPageStr, e.Page.Font);
            e.Graphics.DrawString(currentPageStr, e.Page.Font, Brushes.Red, bounds.Right - 6 - size.Width, bounds.Top + 6);
        }
    }
}
