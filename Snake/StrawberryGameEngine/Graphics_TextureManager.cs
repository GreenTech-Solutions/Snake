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
        /// <summary>
        /// Менеджер текстур
        /// </summary>
        public class TextureManager
        {
            #region General
            // Переменные для назначения уникального ID в словарях
            static int TextureID = 0;
            static int TextureSectionID = 0;

            Form window = new Form();

            Graphics g;

            /// <summary>
            /// Создаёт новый менеджер текстур для указанной формы Windows
            /// </summary>
            /// <param name="window">Форма</param>
            public TextureManager(Form window)
            {
                try
                {
                    if (window == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.window = window;
                    g = this.window.CreateGraphics();
                    int t = this.LoadTextureFromMemory(Resources.Strawberry);
                    this.ChangeTextureInfo(t, new TextureInfo(0, 0));
                    Textures[t].Height = SystemInformation.PrimaryMonitorSize.Height;
                    Textures[t].Width = SystemInformation.PrimaryMonitorSize.Width;
                    this.DrawTexture(t);
                }
                catch
                {
                    throw;
                }
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
            #endregion

            #region Service
            bool TextureExist(int ID)
            {
                if (this.Textures.ContainsKey(ID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region Load & Create
            /// <summary>
            /// Загрузка изображения из файла
            /// </summary>
            /// <param name="FilePath">Путь к файлу</param>
            public int LoadTextureFromFile(string FilePath)
            {
                try
                {
                    if (FilePath==null)
                    {
                        throw new ArgumentNullException();
                    }
                    Textures.Add(++TextureID, new Texture(FilePath));
                    return TextureID;
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Загрузка изображения из памяти
            /// </summary>
            /// <param name="img">Изображение</param>
            public int LoadTextureFromMemory(Bitmap img)
            {
                try
                {
                    if (img == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Textures.Add(++TextureID, new Texture(img));
                    return TextureID;
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Создаёт фрагмент текстуры из указанной по заданным параметрам
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="section">Тип фрагмента</param>
            public int CreateTextureSection(int ID, Section section)
            {
                try
                {
                    if (!Textures.ContainsKey(ID))
                    {
                        throw new KeyNotFoundException();
                    }
                    if (ID==0 || section == null)
                    {
                        throw new ArgumentNullException();
                    }
                    TextureSections.Add(++TextureSectionID, new TextureSection(Textures[ID], section));
                    return TextureSectionID;
                }
                catch
                {

                    throw;
                }
            }
            #endregion

            #region Changing
            /// <summary>
            /// Изменение информации о текстуре
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="info">Новая информация</param>
            public void ChangeTextureInfo(int ID, TextureInfo info)
            {
                try
                {
                    if (!Textures.ContainsKey(ID))
                    {
                        throw new KeyNotFoundException();
                    }
                    Textures[ID].Info = info;
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Масштабирование текстуры
            /// </summary>
            /// <param name="image">Исходное изображение</param>
            /// <param name="scale">Масштаб(1 - изначальный масштаб)</param>
            /// <returns></returns>
            private Bitmap ScaleImage(Bitmap image, float scale)
            {
                try
                {
                    if (image == null || scale == 0)
                    {
                        throw new ArgumentNullException();
                    }
                    float temp1 = scale * image.Width;
                    float temp2 = scale * image.Height;
                    int newWidth = (int)temp1;
                    int newHeight = (int)temp2;
                    Size newSize = new Size(newWidth, newHeight);
                    return new Bitmap(image, newSize);
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Изменяет размер указанной текстуры
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="newSize">Новый размер</param>
            public void SetTextureSize(int ID, Size newSize)
            {
                try
                {
                    if (!TextureExist(ID))
                    {
                        throw new KeyNotFoundException();
                    }
                    if (newSize==null)
                    {
                        throw new ArgumentNullException();
                    }
                    Textures[ID].ChangeSize(newSize);
                }
                catch
                {

                    throw;
                }
            }
            #endregion

            #region Draw
            /// <summary>
            /// Рисует текстуру
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <param name="window">Ссылка на форму</param>
            public void DrawTexture(int ID)
            {
                try
                {
                    if (!TextureExist(ID))
                    {
                        throw new KeyNotFoundException();
                    }
                    Bitmap img = Textures[ID].image;
                    TextureInfo inf = Textures[ID].Info;
                    if (inf != null)
                    {
                        img = ScaleImage(img, inf.Scale / 100);
                        img.RotateFlip(inf.Rotation);
                        g.DrawImageUnscaled(img, new Point((int)inf.x, (int)inf.y));
                        TexturesOnScreen.Add(ID);
                    }
                    else
                    {
                        throw new ArgumentNullException("Отсутствует информация о текстуре.");
                    }
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Рисует фрагмент текстуры
            /// </summary>
            /// <param name="ID">ID фрагмента</param>
            public void DrawTextureSection(int ID)
            {
                try
                {
                    if (!TextureExist(ID))
                    {
                        throw new KeyNotFoundException();
                    }
                    Bitmap img = TextureSections[ID].fragment.image;
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
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Перерисовывает все текстуры, находящиеся на экране
            /// </summary>
            public void ReloadTextures()
            {
                try
                {
                    if (TexturesOnScreen.Count == 0)
                    {
                        this.g.Clear(Color.Black);
                    }
                    List<int> Temp = new List<int>();
                    Temp.AddRange(TexturesOnScreen);
                    TexturesOnScreen.Clear();
                    foreach (int i in Temp)
                    {
                        if (i > 0)
                        {
                            DrawTexture(i);
                        }
                        else if (i < 0)
                        {
                            DrawTextureSection(-i);
                        }
                    }
                }
                catch
                {

                    throw;
                }
            }
            #endregion

            #region Remove
            /// <summary>
            /// Удаляет текстуру(Вместе со всеми её фрагментами)
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            public void RemoveTexture(int ID)
            {
                try
                {
                    if (!TextureExist(ID))
                    {
                        throw new KeyNotFoundException();
                    }
                    List<int> indexes = new List<int>();
                    foreach (var a in TextureSections)
                    {
                        if (a.Value.ID == ID)
                        {
                            indexes.Add(a.Key);
                        }
                    }
                    if (indexes.Count > 0)
                    {
                        foreach (var a in indexes)
                        {
                            TextureSections.Remove(a);
                        }
                    }
                    Textures.Remove(ID);
                    TexturesOnScreen.Remove(ID);
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Удаляет фрагмент текстуры
            /// </summary>
            /// <param name="ID">ID фрагмента</param>
            public void RemoveTextureSection(int ID)
            {
                try
                {
                    TextureSections.Remove(ID);
                    TexturesOnScreen.Remove(-ID);
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Удаляет все текстуры(Вместе с соответствующими фрагментами)
            /// </summary>
            public void RemoveAllTextures()
            {
                try
                {
                    Textures.Clear();
                    TextureSections.Clear();
                    TexturesOnScreen.Clear();
                    TextureID = TextureSectionID = 0;
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Удаляет все фрагменты
            /// </summary>
            public void RemoveAllTextureSections()
            {
                try
                {
                    TextureSections.Clear();
                    TexturesOnScreen.RemoveAll(x => x < 0);
                    TextureSectionID = 0;
                }
                catch
                {

                    throw;
                }
            }
            #endregion

            #region Measure
            /// <summary>
            /// Возвращает ширину текстуры
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <returns></returns>
            public float GetTextureWidth(int ID)
            {
                return (Textures[ID] as Texture).Width;
            }

            /// <summary>
            /// Возвращает высоту текстуры
            /// </summary>
            /// <param name="ID">ID текстуры</param>
            /// <returns></returns>
            public float GetTextureHeight(int ID)
            {
                return (Textures[ID] as Texture).Height;
            }
            #endregion
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
            public float Width
            {
                get
                {
                    return this.image.Width;
                }
                set
                {
                    image = new Bitmap(image, new Size((int)value,this.image.Height));
                }
            }

            /// <summary>
            /// Высота текстуры
            /// </summary>
            public float Height
            {
                get
                {
                    return this.image.Height;
                }
                set
                {
                    this.image = new Bitmap(image, new Size(this.image.Width, (int)value));
                }
            }

            /// <summary>
            /// Текстура
            /// </summary>
            public Bitmap image;

            public Texture(string FileName)
            {
                try
                {
                    if (FileName == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.image = new Bitmap(FileName);
                    this.Width = image.Width;
                    this.Height = image.Height;
                }
                catch
                {

                    throw;
                }
            }

            public Texture(string FileName, TextureInfo info) : this(FileName)
            {
                try
                {
                    if (info == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.Info = info;
                }
                catch
                {

                    throw;
                }
            }

            public Texture(Bitmap img)
            {
                try
                {
                    if (img == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.image = img;
                    this.Width = img.Width;
                    this.Height = img.Height;
                }
                catch
                {

                    throw;
                }
            }

            public Texture(Bitmap img, TextureInfo info) : this(img)
            {
                try
                {
                    if (info==null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.Info = info;
                }
                catch
                {

                    throw;
                }
            }

            public void ChangeSize(Size newSize)
            {
                try
                {
                    this.Width = newSize.Width;
                    this.Height = newSize.Height;
                }
                catch
                {

                    throw;
                }
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
            /// <param name="texture">Текстура</param>
            /// <param name="section">Тип фрагмента</param>
            public TextureSection(Texture texture, Section section)
            {
                try
                {
                    if (texture==null||section == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Rectangle cropRect = new Rectangle((int)section.uMin, (int)section.vMin, (int)(section.uMax - section.uMin), (int)(section.vMax - section.vMin));
                    Bitmap src = texture.image;
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }


                    this.fragment = new Texture(target);
                    this.section = section;
                }
                catch
                {
                    
                    throw;
                }
            }

            /// <summary>
            /// Создаёт новый фрагмент из указанной текстуры
            /// </summary>
            /// <param name="texture">Текстура</param>
            /// <param name="section">Тип фрагмента</param>
            /// <param name="info">Информация о фрагменте</param>
            public TextureSection(Texture texture, Section section, TextureInfo info) : this(texture,section)
            {
                try
                {
                    if (info == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.fragment.Info = info;
                }
                catch
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Координаты для создания фрагмента изображения
        /// </summary>
        public class Section
        {
            /// <summary>
            /// Координата x(Верхняя левая точка)
            /// </summary>
            public float uMin;

            /// <summary>
            /// Координата x(Нижняя правая точка)
            /// </summary>
            public float uMax;

            /// <summary>
            /// Координата y(Верхняя левая точка)
            /// </summary>
            public float vMin;

            /// <summary>
            /// Координата y(Нижняя правая точка)
            /// </summary>
            public float vMax;

            /// <summary>
            /// Создаёт экземпляр с данными о размере фрагмента
            /// </summary>
            /// <param name="newUmin">Координата x(Верхняя левая точка)</param>
            /// <param name="newUmax">Координата x(Нижняя правая точка)</param>
            /// <param name="newVmin">Координата y(Верхняя левая точка)</param>
            /// <param name="newVmax">Координата y(Нижняя правая точка)</param>
            public Section(float newUmin, float newVmin, float newUmax, float newVmax)
            {
                try
                {
                    if (newUmin>newUmax||newVmin>newVmax)
                    {
                        throw new ArgumentException("Минимальная координата {0} больше максимальной.", newUmin>newUmax?"x":"y");
                    } 
                    this.uMin = newUmin;
                    this.uMax = newUmax;
                    this.vMin = newVmin;
                    this.vMax = newVmax;
                }
                catch
                {

                    throw;
                }
            }
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
            public TextureInfo(int x = 0, int y=0, float Scale = 100, RotateFlipType rotation = 0) : this()
            {
                try
                {
                    this.x = x;
                    this.y = y;
                    this.Scale = Scale;
                    this.Rotation = rotation;
                }
                catch
                {

                    throw;
                }
            }
        }
    }
}
