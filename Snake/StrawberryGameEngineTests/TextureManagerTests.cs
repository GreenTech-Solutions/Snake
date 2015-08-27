using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrawberryGameEngine.Video.Tests
{
    [TestClass]
    public class TextureManagerTests
    {
        [TestMethod]
        public TextureManager TextureManagerTest()
        {
            try
            {
                var m = new TextureManager(new Form());
                return m;
            }
            catch
            {

                Assert.Fail();
                return null;
            }
            
        }

        [TestMethod]
        public void LoadTextureFromFileTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = m.LoadTextureFromFile("D:\\Desktop\\Leva.png");
                Assert.AreEqual(t, 2);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public int LoadTextureFromMemoryTest(ref TextureManager n)
        {
            try
            {
                var m = TextureManagerTest();
                if (n!=null)
                {
                    m = n;
                }

                var img = Image.FromFile("D:\\Desktop\\Leva.png");
                var t = m.LoadTextureFromMemory(new Bitmap(img));
                return t;
            }
            catch
            {
                throw;
            }

        }

        [TestMethod]
        public int CreateTextureSectionTest(ref TextureManager n)
        {
            try
            {
                var m = TextureManagerTest();
                if (n!=null)
                {
                    m = n;
                }
                var t = LoadTextureFromMemoryTest(ref m);
                var f = m.CreateTextureSection(t, new Section(0, 0, m.GetTextureWidth(t), m.GetTextureHeight(t)));
                return f;
            }
            catch
            {
                throw;
            }
        }

        [TestMethod]
        public void ChangeTextureInfoTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                m.ChangeTextureInfo(t, new TextureInfo());
            }
            catch
            {
                throw;
            }
        }

        [TestMethod]
        public void SetTextureSizeTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                var oldSize = new Size((int)m.GetTextureWidth(t), (int)m.GetTextureHeight(t));
                m.SetTextureSize(t, new Size((int)m.GetTextureWidth(t) * 2, (int)m.GetTextureHeight(t) * 2));
                var doubleOldSize = new Size(oldSize.Width * 2, oldSize.Height * 2);
                Assert.AreEqual(doubleOldSize, new Size((int)m.GetTextureWidth(t), (int)m.GetTextureHeight(t)));
            }
            catch 
            {
                throw;
            }
        }

        [TestMethod]
        public void DrawTextureTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                m.DrawTexture(t);
            }
            catch
            {

                throw;
            }

        }

        [TestMethod]
        public void DrawTextureSectionTest()
        {
            try
            {
                var m = TextureManagerTest();
                var f = CreateTextureSectionTest(ref m);
                m.DrawTextureSection(f);

            }
            catch
            {

                throw;
            }
        }

        [TestMethod]
        public void ReloadTexturesTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                var f = CreateTextureSectionTest(ref m);
                m.DrawTexture(t);
                m.DrawTextureSection(f);
                m.ReloadTextures();
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod]
        public void RemoveTextureTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                m.RemoveTexture(t);

            }
            catch 
            {

                throw;
            }
        }

        [TestMethod]
        public void RemoveTextureSectionTest()
        {
            try
            {
                var m = TextureManagerTest();
                var f = CreateTextureSectionTest(ref m);
                m.RemoveTextureSection(f);
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod]
        public void RemoveAllTexturesTest()
        {
            try
            {
                var m = TextureManagerTest();
                for (var i = 0; i < 10; i++)
                {
                    LoadTextureFromMemoryTest(ref m);
                }
                m.RemoveAllTextures();
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod]
        public void RemoveAllTextureSectionsTest()
        {
            try
            {
                var m = TextureManagerTest();
                for (var i = 0; i < 10; i++)
                {
                    CreateTextureSectionTest(ref m);
                }
                m.RemoveAllTextureSections();
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod]
        public void GetTextureWidthTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                m.GetTextureWidth(t);
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod]
        public void GetTextureHeightTest()
        {
            try
            {
                var m = TextureManagerTest();
                var t = LoadTextureFromMemoryTest(ref m);
                m.GetTextureHeight(t);
            }
            catch
            {

                throw;
            }
        }
    }
}