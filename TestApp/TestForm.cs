using System;
using System.Windows.Forms;

namespace TestApp
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            pagedControl1.Pages.Add(new Manina.Windows.Forms.Page());
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (pagedControl1.SelectedPage != null)
                pagedControl1.Pages.Remove(pagedControl1.SelectedPage);
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            pagedControl1.GoBack();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            pagedControl1.GoNext();
        }

        private void Log(string message, params object[] args)
        {
            EventLog.Items.Add(string.Format(message, args));

            CurrentPageLabel.Text = string.Format("Current Page: {0}, Page Count: {1}", pagedControl1.SelectedIndex, pagedControl1.Pages.Count);
        }

        private void pagedControl1_PageChanged(object sender, Manina.Windows.Forms.PagedControl.PageChangedEventArgs e)
        {
            Log("Page Changed: {0} -> {1}", e.OldPageIndex, e.CurrentPageIndex);
        }

        private void pagedControl1_PageChanging(object sender, Manina.Windows.Forms.PagedControl.PageChangingEventArgs e)
        {
            Log("Page Changing: {0} -> {1}", e.CurrentPageIndex, e.NewPageIndex);
        }

        private void pagedControl1_PageHidden(object sender, Manina.Windows.Forms.PagedControl.PageEventArgs e)
        {
            Log("Page Hidden: {0}", e.PageIndex);
        }

        private void pagedControl1_PageAdded(object sender, Manina.Windows.Forms.PagedControl.PageEventArgs e)
        {
            Log("Page Added: {0}", e.PageIndex);
        }

        private void pagedControl1_PageRemoved(object sender, Manina.Windows.Forms.PagedControl.PageEventArgs e)
        {
            Log("Page Removed: {0}", e.PageIndex);
        }

        private void pagedControl1_PageShown(object sender, Manina.Windows.Forms.PagedControl.PageEventArgs e)
        {
            Log("Page Shown: {0}", e.PageIndex);
        }

        private void pagedControl1_PageValidated(object sender, Manina.Windows.Forms.PagedControl.PageEventArgs e)
        {
            Log("Page Validated: {0}", e.PageIndex);
        }

        private void pagedControl1_PageValidating(object sender, Manina.Windows.Forms.PagedControl.PageValidatingEventArgs e)
        {
            Log("Page Validating: {0}", e.PageIndex);
        }
    }
}
