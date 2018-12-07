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
        }
    }
}
