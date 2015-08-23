using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace StrawberryGameEngine
{
    namespace Graphics
    {
        public class TextureManager
        {
            static int TextureID = 0;
            static int TextureSectionID = 0;

            protected Dictionary<int, Texture> Textures = new Dictionary<int, Texture>();
            protected Dictionary<int, TextureSection> TextureSections = new Dictionary<int, TextureSection>();

            public void LoadTextureFromFile(string FilePath)
            {
                Textures.Add(TextureID++, new Texture(FilePath));
            }

            public void LoadTextureFromMemory(Image img)
            {
                Textures.Add(TextureID++, new Texture(img));
            }

            public void ReloadTextures()
            {
                //for (int i = 1; i <= TextureID; i++)
                //{
                //    DrawTexture(i,Textures[i].bit.)
                //}
            }

            public void CreateTextureSection(uint ID, Section section)
            {
                TextureSections.Add(TextureSectionID++, new TextureSection(ID,section));
            }

            public void RemoveTexture(int ID)
            {
                Textures.Remove(ID);
            }

            public void RemoveTextureSection(int ID)
            {
                TextureSections.Remove(ID);
            }

            public void DrawTexture(int ID, float x, float y, float Scale =1, float Rotation =0, float Red = 1, float Blue =1, float Green = 1)
            {

            }

            public void DrawTextureSection(int ID, float x, float y, float Scale = 1, float Rotation = 0, float Red = 1, float Blue = 1, float Green = 1)
            {

            }

            public float GetTextureWidth(int ID)
            {
                return (Textures[ID] as Texture).Width;
            }

            public float GetTextureHeight(int ID)
            {
                return (Textures[ID] as Texture).Height;
            }

            public void RemoveAllTextures()
            {
                Textures.Clear();
                TextureID = 0;
            }

            public void RemoveAllTextureSections()
            {
                TextureSections.Clear();
                TextureSectionID = 0;
            }
        }

        public class Texture
        {
            /// <summary>
            /// Название файла
            /// </summary>
            public string Filename;

            /// <summary>
            /// Ширина текстуры
            /// </summary>
            public float Width;

            /// <summary>
            /// Высота текстуры
            /// </summary>
            public float Height;

            /// <summary>
            /// Текстура
            /// </summary>
            public Image bit;

            public Texture(string FileName)
            {
                this.bit = new Bitmap(FileName);
                this.Filename = FileName;
                this.Width = bit.Width;
                this.Height = bit.Height;
            }

            public Texture(Image img)
            {
                this.bit = img;
                this.Filename = img.ToString();
                this.Width = img.Width;
                this.Height = img.Height;
            }
        }

        public class TextureSection
        {
            /// <summary>
            /// ID секции
            /// </summary>
            public uint ID;

            /// <summary>
            /// Используемая секция
            /// </summary>
            public Section section;

            /// <summary>
            /// Ширина секции
            /// </summary>
            public float Width;

            /// <summary>
            /// Высота секции
            /// </summary>
            public float Height;

            public TextureSection(uint ID, Section section)
            {
                this.section = section;
                this.ID = ID;

            }
        }

        public struct Section
        {
            /// <summary>
            /// Минимальная ширина
            /// </summary>
            public float uMin;

            /// <summary>
            /// Максимальная ширина
            /// </summary>
            public float uMax;

            /// <summary>
            ///  Минимальная высота
            /// </summary>
            public float vMin;

            /// <summary>
            /// Максимальная высота
            /// </summary>
            public float vMax;
        }
    }

}
