using System;
using System.Linq;
using System.Windows.Forms;
using Manina.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class PageCollection
    {
        [TestMethod]
        public void Add()
        {
            var control = new PagedControl();
            int count = control.Pages.Count;
            var page = new Page();
            control.Pages.Add(page);
            Assert.AreEqual(count + 1, control.Pages.Count);
            Assert.AreEqual(page, control.Pages[control.Pages.Count - 1]);

            Assert.ThrowsException<ArgumentException>(() => control.Controls.Add(new Button()));
        }

        [TestMethod]
        public void Insert()
        {
            var control = new PagedControl();
            int count = control.Pages.Count;

            var page1 = new Page();
            control.Pages.Insert(0, page1);
            Assert.AreEqual(count + 1, control.Pages.Count);
            Assert.AreEqual(page1, control.Pages[0]);

            var page2 = new Page();
            control.Pages.Insert(1, page2);
            Assert.AreEqual(count + 2, control.Pages.Count);
            Assert.AreEqual(page2, control.Pages[1]);
        }

        [TestMethod]
        public void Remove()
        {
            var control = new PagedControl();
            control.Pages.Add(new Page());
            int count = control.Pages.Count;
            control.Pages.Remove(control.Pages[0]);
            Assert.AreEqual(count - 1, control.Pages.Count);

            Assert.ThrowsException<ArgumentException>(() => control.Controls.Remove(new Button()));
            Assert.ThrowsException<ArgumentException>(() => control.Controls.Remove(new Page()));
        }

        [TestMethod]
        public void RemoveAt()
        {
            var control = new PagedControl();
            control.Pages.Add(new Page());
            int count = control.Pages.Count;
            control.Pages.RemoveAt(0);
            Assert.AreEqual(count - 1, control.Pages.Count);
        }

        [TestMethod]
        public void Clear()
        {
            var control = new PagedControl();
            control.Pages.Add(new Page());
            control.Pages.Add(new Page());
            control.Pages.Add(new Page());
            control.Pages.Clear();
            Assert.AreEqual(0, control.Pages.Count);
        }

        [TestMethod]
        public void Contains()
        {
            var control = new PagedControl();
            Page page1 = new Page();
            control.Pages.Add(page1);
            Page page2 = new Page();
            control.Pages.Add(page2);
            Page page3 = new Page();
            control.Pages.Add(page3);

            Assert.IsTrue(control.Pages.Contains(page1));
            Assert.IsTrue(control.Pages.Contains(page2));
            Assert.IsTrue(control.Pages.Contains(page3));
        }

        [TestMethod]
        public void IndexOf()
        {
            var control = new PagedControl();
            Page page1 = new Page();
            control.Pages.Add(page1);
            Page page2 = new Page();
            control.Pages.Add(page2);
            Page page3 = new Page();
            control.Pages.Add(page3);

            Assert.AreEqual(0, control.Pages.IndexOf(page1));
            Assert.AreEqual(1, control.Pages.IndexOf(page2));
            Assert.AreEqual(2, control.Pages.IndexOf(page3));
        }

        [TestMethod]
        public void GetEnumerator()
        {
            var control = new PagedControl();
            Page page1 = new Page();
            control.Pages.Add(page1);
            Page page2 = new Page();
            control.Pages.Add(page2);
            Page page3 = new Page();
            control.Pages.Add(page3);

            CollectionAssert.AreEqual(new[] { page1, page2, page3 }, control.Pages.Take(3).ToArray());
        }

        [TestMethod]
        public void Count()
        {
            var control = new PagedControl();
            control.Pages.Add(new Page());
            control.Pages.Add(new Page());
            control.Pages.Add(new Page());
            Assert.AreEqual(3, control.Pages.Count);
        }

        [TestMethod]
        public void Indexer()
        {
            var control = new PagedControl();
            Page page1 = new Page();
            control.Pages.Add(page1);
            Page page2 = new Page();
            control.Pages.Add(page2);
            Page page3 = new Page();
            control.Pages.Add(page3);

            // Getter
            Assert.AreEqual(3, control.Pages.Count);
            Assert.AreEqual(page1, control.Pages[0]);
            Assert.AreEqual(page2, control.Pages[1]);
            Assert.AreEqual(page3, control.Pages[2]);
            Assert.IsTrue(control.Pages.Contains(page1));
            Assert.IsTrue(control.Pages.Contains(page2));
            Assert.IsTrue(control.Pages.Contains(page3));

            Page page4 = new Page();
            control.Pages[1] = page4;

            // Setter
            Assert.AreEqual(3, control.Pages.Count);
            Assert.AreEqual(page4, control.Pages[1]);
            Assert.IsTrue(control.Pages.Contains(page1));
            Assert.IsTrue(control.Pages.Contains(page4));
            Assert.IsTrue(control.Pages.Contains(page3));
            Assert.IsFalse(control.Pages.Contains(page2));
        }
    }
}