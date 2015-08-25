using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace StrawberryGameEngine
{
    namespace Video
    {
        public class TextureManager
        {
            // Переменные для назначения уникального ID в словарях
            static int TextureID = 0;
            static int TextureSectionID = 0;

            Form window = new Form();

            public TextureManager(Form window)
            {
                this.window = window;
                int t = this.LoadTextureFromMemory(Resources.Strawberry);
                this.DrawTexture(t);
            }

            /// <summary>
            /// Словарь текстур
            /// </summary>
            protected Dictionary<int, Texture> Textures = new Dictionary<int, Texture>();

            /// <summary>
            /// Словарь усечённых текстур
            /// </summary>
            protected Dictionary<int, TextureSection> TextureSections = new Dictionary<int, TextureSection>();

            /// <summary>
            /// Список идентификаторов текстур, отрисованных на экране(Отрицательные для фрагментов)
            /// </summary>
            protected List<int> TexturesOnScreen = new List<int>();

            /// <summary>
            /// Загрузка изображения из файла
            /// </summary>
            /// <param name="FilePath">Путь к файлу</param>
            public int LoadTextureFromFile(string FilePath)
            {
                Textures.Add(++TextureID, new Texture(FilePath));
                return TextureID;
            }

            /// <summary>
            /// Загрузка изображения из памяти
            /// </summary>
            /// <param name="img">Изображение</param>
            public int LoadTextureFromMemory(Bitmap img)
            {
                Textures.Add(++TextureID, new Texture(img));
                return TextureID;
            }

            /// <summary>
            /// Изменение информации о текстуре
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="info">Новая информация</param>
            public void ChangeTextureInfo(int ID, TextureInfo info)
            {
                Textures[ID].Info = info;
            }
            
            /// <summary>
            /// Создаёт фрагмент текстуры из указанной по заданным параметрам
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="section">Тип фрагмента</param>
            public void CreateTextureSection(int ID, Section section)
            {
                TextureSections.Add(TextureSectionID++, new TextureSection(Textures[ID],section));
            }

            /// <summary>
            /// Удаляет текстуру
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            public void RemoveTexture(int ID)
            {
                List<int> indexes = new List<int>();
                foreach (var a in TextureSections)
                {
                    if (a.Value.ID==ID)
                    {
                        indexes.Add(a.Key);
                    }
                }
                if (indexes.Count>0)
                {
                    foreach (var a in indexes)
                    {
                        TextureSections.Remove(a);
                    } 
                }
                Textures.Remove(ID);
                TexturesOnScreen.Remove(ID);
            }

            /// <summary>
            /// Удаляет фрагмент текстуры
            /// </summary>
            /// <param name="ID">ID фрагмента</param>
            public void RemoveTextureSection(int ID)
            {
                TextureSections.Remove(ID);
                TexturesOnScreen.Remove(ID);
            }

            /// <summary>
            /// Рисует текстуру на заданной форме
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="window">Ссылка на форму</param>
            public void DrawTexture(int ID)
            {
                Graphics g = this.window.CreateGraphics();
                Bitmap img = Textures[ID].image;
                TextureInfo inf = Textures[ID].Info;
                if (inf!=null)
                {
                    img = ScaleImage(img, inf.Scale/100);
                    img.RotateFlip(inf.Rotation);
                    g.DrawImageUnscaled(img, new Point((int)inf.x, (int)inf.y));
                    TexturesOnScreen.Add(ID);
                }
                else
                {
                    throw new ArgumentNullException("Отсутствуе информация о текстуре.");
                }
            }

            public void DrawTextureSection(int ID)
            {
                Graphics g = this.window.CreateGraphics();
                Bitmap img = Textures[ID].image;
                TextureInfo inf = TextureSections[ID].Info;
                if (inf != null)
                {
                    img = ScaleImage(img, inf.Scale / 100);
                    img.RotateFlip(inf.Rotation);
                    g.DrawImageUnscaled(img, new Point((int)inf.x, (int)inf.y));
                    TexturesOnScreen.Add(-ID);
                }
                else
                {
                    throw new ArgumentNullException("Отсутствуе информация о текстуре.");
                }
            }

            private Bitmap ScaleImage(Bitmap image, float scale)
            {
                float temp1 = scale * image.Width;
                float temp2 = scale * image.Height;
                int newWidth = (int)temp1;
                int newHeight = (int)temp2;
                Size newSize = new Size(newWidth,newHeight);
                return new Bitmap(image, newSize);
            }

            public void ReloadTextures(Form window)
            {
                foreach (int i in TexturesOnScreen)
                {
                    if (i>0)
                    {
                        DrawTexture(i);
                    }
                    else if (i<0)
                    {
                        DrawTextureSection(-i);
                    }
                }
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
                TexturesOnScreen.RemoveAll(x => x > 0);
                TextureID = 0;
            }

            public void RemoveAllTextureSections()
            {
                TextureSections.Clear();
                TexturesOnScreen.RemoveAll(x => x < 0);
                TextureSectionID = 0;
            }
        }

        /// <summary>
        /// Изображение и сведения о нём
        /// </summary>
        public class Texture
        {
            /// <summary>
            /// Информация о текстуре
            /// </summary>
            public TextureInfo Info = new TextureInfo();

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
            public Bitmap image;

            public Texture(string FileName)
            {
                this.image = new Bitmap(FileName);
                this.Width = image.Width;
                this.Height = image.Height;
            }

            public Texture(string FileName, TextureInfo info) : this(FileName)
            {
                this.Info = info;
            }

            public Texture(Bitmap img)
            {
                this.image = img;
                this.Width = img.Width;
                this.Height = img.Height;
            }

            public Texture(Bitmap img, TextureInfo info) : this(img)
            {
                this.Info = info;
            }
        }

        /// <summary>
        /// Сведения о фрагменте изображения
        /// </summary>
        public class TextureSection
        {
            /// <summary>
            /// ID текстуры
            /// </summary>
            public int ID;

            /// <summary>
            /// Информация о фрагменте
            /// </summary>
            public TextureInfo Info = new TextureInfo();

            /// <summary>
            /// Фрагмент текстуры
            /// </summary>
            public Texture fragment;

            /// <summary>
            /// Используемая секция
            /// </summary>
            public Section section;

            /// <summary>
            /// Ширина секции
            /// </summary>
            public int Width;

            /// <summary>
            /// Высота секции
            /// </summary>
            public int Height;

            /// <summary>
            /// Создаёт новый фрагмент из указанной текстуры
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="section">Тип фрагмента</param>
            public TextureSection(Texture texture, Section section)
            {
                Bitmap image = texture.image;
                image = image.Clone(new RectangleF(section.uMin, section.vMin, section.uMax - section.uMin, section.vMin - section.vMax), System.Drawing.Imaging.PixelFormat.Canonical);
                this.fragment.image = image;
                this.section = section;
            }
        }

        /// <summary>
        /// Координаты для создания фрагмента изображения
        /// </summary>
        public struct Section
        {
            /// <summary>
            /// Минимальная координата x(слева направо)
            /// </summary>
            public float uMin;

            /// <summary>
            /// Максимальная координата x(слева направо)
            /// </summary>
            public float uMax;

            /// <summary>
            ///  Минимальная координата y(сверху вниз)
            /// </summary>
            public float vMin;

            /// <summary>
            /// Максимальная координата y(сверху вниз)
            /// </summary>
            public float vMax;
        }

        /// <summary>
        /// Информация об изображении
        /// </summary>
        public class TextureInfo
        {
            /// <summary>
            /// Расположение по Ox
            /// </summary>
            public int x = 0;

            /// <summary>
            /// Расположение по Oy
            /// </summary>
            public int y = 0;

            /// <summary>
            /// Масштабирование
            /// </summary>
            public float Scale = 100;

            /// <summary>
            /// Угол вращения
            /// </summary>
            public RotateFlipType Rotation = RotateFlipType.RotateNoneFlipNone;

            /// <summary>
            /// Создаёт экземпляр TextureInfo со стандартными значениями
            /// </summary>
            public TextureInfo()
            {

            }

            /// <summary>
            /// Создаёт экземпляр TextureInfo
            /// </summary>
            /// <param name="x">Расположение по Ox</param>
            /// <param name="y">Расположение по Oy</param>
            /// <param name="Scale">Масштабирование</param>
            /// <param name="rotation">Угол вращения</param>
            public TextureInfo(int x, int y, float Scale = 100, RotateFlipType rotation = 0)
            {
                this.x = x;
                this.y = y;
                this.Scale = Scale;
                this.Rotation = rotation;
            }
        }
    }

}
