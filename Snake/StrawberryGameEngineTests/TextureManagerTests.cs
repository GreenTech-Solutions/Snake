using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;

namespace StrawberryGameEngine.Video.Tests
{
    [TestClass()]
    public class TextureManagerTests
    {
        [TestMethod()]
        public TextureManager TextureManagerTest()
        {
            try
            {
                TextureManager m = new TextureManager(new Form());
                return m;
            }
            catch
            {

                Assert.Fail();
                return null;
            }
            
        }

        [TestMethod()]
        public void LoadTextureFromFileTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = m.LoadTextureFromFile("D:\\Desktop\\Leva.png");
                Assert.AreEqual(t, 2);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public int LoadTextureFromMemoryTest(ref TextureManager n)
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                if (n!=null)
                {
                    m = n;
                }

                Image img = Image.FromFile("D:\\Desktop\\Leva.png");
                int t = m.LoadTextureFromMemory(new Bitmap(img));
                return t;
            }
            catch
            {
                throw;
            }

        }

        [TestMethod()]
        public int CreateTextureSectionTest(ref TextureManager n)
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                if (n!=null)
                {
                    m = n;
                }
                int t = this.LoadTextureFromMemoryTest(ref m);
                int f = m.CreateTextureSection(t, new Section(0, 0, m.GetTextureWidth(t), m.GetTextureHeight(t)));
                return f;
            }
            catch
            {
                throw;
            }
        }

        [TestMethod()]
        public void ChangeTextureInfoTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = this.LoadTextureFromMemoryTest(ref m);
                m.ChangeTextureInfo(t, new TextureInfo());
            }
            catch
            {
                throw;
            }
        }

        [TestMethod()]
        public void SetTextureSizeTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = this.LoadTextureFromMemoryTest(ref m);
                Size oldSize = new Size((int)m.GetTextureWidth(t), (int)m.GetTextureHeight(t));
                m.SetTextureSize(t, new Size((int)m.GetTextureWidth(t) * 2, (int)m.GetTextureHeight(t) * 2));
                Size DoubleOldSize = new Size(oldSize.Width * 2, oldSize.Height * 2);
                Assert.AreEqual(DoubleOldSize, new Size((int)m.GetTextureWidth(t), (int)m.GetTextureHeight(t)));
            }
            catch 
            {
                throw;
            }
        }

        [TestMethod()]
        public void DrawTextureTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = this.LoadTextureFromMemoryTest(ref m);
                m.DrawTexture(t);
            }
            catch
            {

                throw;
            }

        }

        [TestMethod()]
        public void DrawTextureSectionTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int f = this.CreateTextureSectionTest(ref m);
                m.DrawTextureSection(f);

            }
            catch
            {

                throw;
            }
        }

        [TestMethod()]
        public void ReloadTexturesTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = this.LoadTextureFromMemoryTest(ref m);
                int f = this.CreateTextureSectionTest(ref m);
                m.DrawTexture(t);
                m.DrawTextureSection(f);
                m.ReloadTextures();
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void RemoveTextureTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = LoadTextureFromMemoryTest(ref m);
                m.RemoveTexture(t);

            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void RemoveTextureSectionTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int f = this.CreateTextureSectionTest(ref m);
                m.RemoveTextureSection(f);
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void RemoveAllTexturesTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                for (int i = 0; i < 10; i++)
                {
                    this.LoadTextureFromMemoryTest(ref m);
                }
                m.RemoveAllTextures();
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void RemoveAllTextureSectionsTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                for (int i = 0; i < 10; i++)
                {
                    this.CreateTextureSectionTest(ref m);
                }
                m.RemoveAllTextureSections();
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void GetTextureWidthTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = this.LoadTextureFromMemoryTest(ref m);
                m.GetTextureWidth(t);
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void GetTextureHeightTest()
        {
            try
            {
                TextureManager m = this.TextureManagerTest();
                int t = this.LoadTextureFromMemoryTest(ref m);
                m.GetTextureHeight(t);
            }
            catch
            {

                throw;
            }
        }
    }
}