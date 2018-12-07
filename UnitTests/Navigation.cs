using Manina.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Navigation
    {
        [TestMethod]
        public void GoBack()
        {
            var control = new PagedControl();

            Assert.IsFalse(control.CanGoBack);
            var page1 = new Page();
            control.Pages.Add(page1);
            Assert.IsFalse(control.CanGoBack);

            var page2 = new Page();
            control.Pages.Add(page2);
            Assert.IsFalse(control.CanGoBack);

            control.SelectedPage = page2;
            Assert.IsTrue(control.CanGoBack);

            control.GoBack();
            Assert.AreEqual(page1, control.SelectedPage);
            Assert.IsFalse(control.CanGoBack);
        }

        [TestMethod]
        public void GoNext()
        {
            var control = new PagedControl();

            Assert.IsFalse(control.CanGoNext);
            var page1 = new Page();
            control.Pages.Add(page1);
            Assert.IsFalse(control.CanGoNext);

            var page2 = new Page();
            control.Pages.Add(page2);
            Assert.IsTrue(control.CanGoNext);

            control.GoNext();
            Assert.AreEqual(page2, control.SelectedPage);
            Assert.IsFalse(control.CanGoNext);
        }
    }
}
