using Manina.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class PageCollectionEvents
    {
        [TestMethod]
        public void PageAdded()
        {
            var control = new PagedControl();
            Page pageAdded = null;
            control.PageAdded += (sender, e) => pageAdded = e.Page; ;

            var page1 = new Page();
            control.Pages.Add(page1);
            Assert.AreEqual(page1, pageAdded);

            var page2 = new Page();
            control.Pages.Insert(1, page2);
            Assert.AreEqual(page2, pageAdded);
        }

        [TestMethod]
        public void PageRemoved()
        {
            var control = new PagedControl();
            Page pageRemoved = null;
            control.PageRemoved += (sender, e) => pageRemoved = e.Page;

            var page1 = new Page();
            control.Pages.Add(page1);
            var page2 = new Page();
            control.Pages.Add(page2);

            control.Pages.Remove(page2);
            Assert.AreEqual(page2, pageRemoved);

            control.Pages.Remove(page1);
            Assert.AreEqual(page1, pageRemoved);
        }
    }
}
