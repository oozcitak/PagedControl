using System;
using Manina.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class NavigationEvents
    {
        [TestMethod]
        public void PageChanging()
        {
            var control = new PagedControl();
            Page currentPage = null;
            Page newPage = null;
            control.PageChanging += (sender, e) => { currentPage = e.CurrentPage; newPage = e.NewPage; };

            var page1 = new Page();
            control.Pages.Add(page1);
            Assert.AreEqual(null, currentPage);
            Assert.AreEqual(page1, newPage);

            control.Pages.Remove(page1);
            Assert.AreEqual(page1, currentPage);
            Assert.AreEqual(null, newPage);

            control.Pages.Add(page1);
            control.Pages.Clear();
            Assert.AreEqual(page1, currentPage);
            Assert.AreEqual(null, newPage);

            control.Pages.Add(page1);
            Assert.AreEqual(null, currentPage);
            Assert.AreEqual(page1, newPage);

            var page2 = new Page();
            control.Pages.Add(page2);
            control.SelectedPage = page2;
            Assert.AreEqual(page1, currentPage);
            Assert.AreEqual(page2, newPage);

            var page3 = new Page();
            control.Pages.Add(page3);
            control.SelectedIndex = 2;
            Assert.AreEqual(page2, currentPage);
            Assert.AreEqual(page3, newPage);
        }

        [TestMethod]
        public void PageChangingCancelNavigation()
        {
            var control = new PagedControl();

            // Cancel all page change events
            control.PageChanging += (sender, e) => { e.Cancel = true; };

            // While adding a page and if the control is empty the control automatically
            // sets the new page as the current page. This event cannot be cancelled as it would
            // leave the control in an indeterminate state. SelectedPage should always return a valid
            // page if there is at least one page in the control, and it should return null if there are
            // no pages.
            var page1 = new Page();
            Assert.AreEqual(null, control.SelectedPage);
            Assert.AreEqual(-1, control.SelectedIndex);
            control.Pages.Add(page1);
            Assert.AreEqual(page1, control.SelectedPage);
            Assert.AreEqual(0, control.SelectedIndex);

            // Otherwise it is OK to cancel the event
            var page2 = new Page();
            control.Pages.Add(page2);
            control.SelectedPage = page2;
            Assert.AreEqual(page1, control.SelectedPage);
            Assert.AreEqual(0, control.SelectedIndex);
        }

        [TestMethod]
        public void PageChanged()
        {
            var control = new PagedControl();
            Page oldPage = null;
            Page currentPage = null;
            control.PageChanged += (sender, e) => { oldPage = e.OldPage; currentPage = e.CurrentPage; };

            var page1 = new Page();
            control.Pages.Add(page1);
            Assert.AreEqual(null, oldPage);
            Assert.AreEqual(page1, currentPage);

            control.Pages.Remove(page1);
            Assert.AreEqual(page1, oldPage);
            Assert.AreEqual(null, currentPage);

            control.Pages.Add(page1);
            control.Pages.Clear();
            Assert.AreEqual(page1, oldPage);
            Assert.AreEqual(null, currentPage);

            control.Pages.Add(page1);
            Assert.AreEqual(null, oldPage);
            Assert.AreEqual(page1, currentPage);

            var page2 = new Page();
            control.Pages.Add(page2);
            control.SelectedPage = page2;
            Assert.AreEqual(page1, oldPage);
            Assert.AreEqual(page2, currentPage);

            var page3 = new Page();
            control.Pages.Add(page3);
            control.SelectedIndex = 2;
            Assert.AreEqual(page2, oldPage);
            Assert.AreEqual(page3, currentPage);
        }
    }
}