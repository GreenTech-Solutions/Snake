using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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
            static int _textureId;
            static int _textureSectionId;

            internal Form Window = new Form();

            internal Graphics G;

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
                    Window = window;
                    G = Window.CreateGraphics();
                    var t = LoadTextureFromMemory(Resources.Strawberry);
                    ChangeTextureInfo(t, new TextureInfo(0, 0));
                    Textures[t].Height = window.Height;
                    Textures[t].Width = window.Width;
                    DrawTexture(t);
                }
                catch
                {
                    throw;
                }
            }

            /// <summary>
            /// Словарь текстур
            /// </summary>
            internal Dictionary<int, Texture> Textures = new Dictionary<int, Texture>();

            /// <summary>
            /// Словарь усечённых текстур
            /// </summary>
            internal Dictionary<int, TextureSection> TextureSections = new Dictionary<int, TextureSection>();

            /// <summary>
            /// Список идентификаторов текстур, отрисованных на экране(Отрицательные для фрагментов)
            /// </summary>
            internal List<int> TexturesOnScreen = new List<int>();
            #endregion

            #region Service
            bool TextureExist(int id)
            {
                if (Textures.ContainsKey(id))
                {
                    return true;
                }
                return false;
            }

            bool TextureSectionExist(int id)
            {
                return TextureSections.ContainsKey(id);
            }

            #endregion

            #region Load & Create
            /// <summary>
            /// Загрузка изображения из файла
            /// </summary>
            /// <param name="filePath">Путь к файлу</param>
            public int LoadTextureFromFile(string filePath)
            {
                try
                {
                    if (filePath==null)
                    {
                        throw new ArgumentNullException();
                    }
                    Textures.Add(++_textureId, new Texture(filePath));
                    return _textureId;
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
                    Textures.Add(++_textureId, new Texture(img));
                    return _textureId;
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Создаёт фрагмент текстуры из указанной по заданным параметрам
            /// </summary>
            /// <param name="id">ID текстуры</param>
            /// <param name="section">Тип фрагмента</param>
            public int CreateTextureSection(int id, Section section)
            {
                try
                {
                    if (!Textures.ContainsKey(id))
                    {
                        throw new KeyNotFoundException();
                    }
                    if (id==0 || section == null)
                    {
                        throw new ArgumentNullException();
                    }
                    TextureSections.Add(++_textureSectionId, new TextureSection(Textures[id], section));
                    return _textureSectionId;
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
            /// <param name="id">ID текстуры</param>
            /// <param name="info">Новая информация</param>
            public void ChangeTextureInfo(int id, TextureInfo info)
            {
                try
                {
                    if (!Textures.ContainsKey(id))
                    {
                        throw new KeyNotFoundException();
                    }
                    Textures[id].Info = info;
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
                    var temp1 = scale * image.Width;
                    var temp2 = scale * image.Height;
                    var newWidth = (int)temp1;
                    var newHeight = (int)temp2;
                    var newSize = new Size(newWidth, newHeight);
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
            /// <param name="id">ID текстуры</param>
            /// <param name="newSize">Новый размер</param>
            public void SetTextureSize(int id, Size newSize)
            {
                try
                {
                    if (!TextureExist(id))
                    {
                        throw new KeyNotFoundException();
                    }
                    if (newSize==null)
                    {
                        throw new ArgumentNullException();
                    }
                    Textures[id].ChangeSize(newSize);
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
            /// <param name="id">ID текстуры</param>
            /// <param name="window">Ссылка на форму</param>
            public void DrawTexture(int id)
            {
                try
                {
                    if (!TextureExist(id))
                    {
                        throw new KeyNotFoundException();
                    }
                    var img = Textures[id].Image;
                    var inf = Textures[id].Info;
                    if (inf != null)
                    {
                        img = ScaleImage(img, inf.Scale / 100);
                        img.RotateFlip(inf.Rotation);
                        G.DrawImageUnscaled(img, new Point(inf.X, inf.Y));
                        TexturesOnScreen.Add(id);
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
            /// <param name="id">ID фрагмента</param>
            public void DrawTextureSection(int id)
            {
                try
                {
                    if (!TextureSectionExist(id))
                    {
                        throw new KeyNotFoundException();
                    }
                    var img = TextureSections[id].Fragment.Image;
                    var inf = TextureSections[id].Info;
                    if (inf != null)
                    {
                        img = ScaleImage(img, inf.Scale / 100);
                        img.RotateFlip(inf.Rotation);
                        G.DrawImageUnscaled(img, new Point(inf.X, inf.Y));
                        TexturesOnScreen.Add(-id);
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
                        G.Clear(Color.Black);
                    }
                    var temp = new List<int>();
                    temp.AddRange(TexturesOnScreen);
                    TexturesOnScreen.Clear();
                    foreach (var i in temp)
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
            /// <param name="id">ID текстуры</param>
            public void RemoveTexture(int id)
            {
                try
                {
                    if (!TextureExist(id))
                    {
                        throw new KeyNotFoundException();
                    }
                    var indexes = new List<int>();
                    foreach (var a in TextureSections)
                    {
                        if (a.Value.Id == id)
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
                    Textures.Remove(id);
                    TexturesOnScreen.Remove(id);
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Удаляет фрагмент текстуры
            /// </summary>
            /// <param name="id">ID фрагмента</param>
            public void RemoveTextureSection(int id)
            {
                try
                {
                    TextureSections.Remove(id);
                    TexturesOnScreen.Remove(-id);
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
                    _textureId = _textureSectionId = 0;
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
                    TexturesOnScreen.RemoveAll(x => x < 0);
                    TextureSections.Clear();
                    _textureSectionId = 0;
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
            /// <param name="id">ID текстуры</param>
            /// <returns></returns>
            public float GetTextureWidth(int id)
            {
                return Textures[id].Width;
            }

            /// <summary>
            /// Возвращает высоту текстуры
            /// </summary>
            /// <param name="id">ID текстуры</param>
            /// <returns></returns>
            public float GetTextureHeight(int id)
            {
                return Textures[id].Height;
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
                    return Image.Width;
                }
                set
                {
                    Image = new Bitmap(Image, new Size((int)value,Image.Height));
                }
            }

            /// <summary>
            /// Высота текстуры
            /// </summary>
            public float Height
            {
                get
                {
                    return Image.Height;
                }
                set
                {
                    Image = new Bitmap(Image, new Size(Image.Width, (int)value));
                }
            }

            /// <summary>
            /// Текстура
            /// </summary>
            public Bitmap Image;

            public Texture(string fileName)
            {
                try
                {
                    if (fileName == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Image = new Bitmap(fileName);
                    Width = Image.Width;
                    Height = Image.Height;
                }
                catch
                {

                    throw;
                }
            }

            public Texture(string fileName, TextureInfo info) : this(fileName)
            {
                try
                {
                    if (info == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Info = info;
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
                    Image = img;
                    Width = img.Width;
                    Height = img.Height;
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
                    Info = info;
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
                    Width = newSize.Width;
                    Height = newSize.Height;
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
            public int Id;

            /// <summary>
            /// Информация о фрагменте
            /// </summary>
            public TextureInfo Info = new TextureInfo();

            /// <summary>
            /// Фрагмент текстуры
            /// </summary>
            public Texture Fragment;

            /// <summary>
            /// Используемая секция
            /// </summary>
            public Section Section;

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
                    var cropRect = new Rectangle((int)section.UMin, (int)section.VMin, (int)(section.UMax - section.UMin), (int)(section.VMax - section.VMin));
                    var src = texture.Image;
                    var target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (var g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }


                    Fragment = new Texture(target);
                    Section = section;
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
                    Fragment.Info = info;
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
            public float UMin;

            /// <summary>
            /// Координата x(Нижняя правая точка)
            /// </summary>
            public float UMax;

            /// <summary>
            /// Координата y(Верхняя левая точка)
            /// </summary>
            public float VMin;

            /// <summary>
            /// Координата y(Нижняя правая точка)
            /// </summary>
            public float VMax;

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
                    UMin = newUmin;
                    UMax = newUmax;
                    VMin = newVmin;
                    VMax = newVmax;
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
            public int X;

            /// <summary>
            /// Расположение по Oy
            /// </summary>
            public int Y;

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
            /// <param name="scale">Масштабирование</param>
            /// <param name="rotation">Угол вращения</param>
            public TextureInfo(int x = 0, int y=0, float scale = 100, RotateFlipType rotation = 0) : this()
            {
                try
                {
                    X = x;
                    Y = y;
                    Scale = scale;
                    Rotation = rotation;
                }
                catch
                {

                    throw;
                }
            }
        }
    }
}
