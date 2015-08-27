using System;
using System.Drawing;
using System.Windows.Forms;

namespace StrawberryGameEngine
{
    namespace Core
    {
        /// <summary>
        /// Ядро управления графического движка
        /// </summary>
        public class MainWf : IApp
        {
            /// <summary>
            /// Главное окно
            /// </summary>
            Form _screen = new Form();

            /// <summary>
            /// Активирован ли полноэкранный режим
            /// </summary>
            public bool IsFullScreen { get; private set; }

            /// <summary>
            /// Высота окна
            /// </summary>
            public int Height
            {
                get
                {
                    return _screen.Height;
                }

                set
                {
                    _screen.Height = value;
                }
            }

            /// <summary>
            /// Ширина окна
            /// </summary>
            public int Width
            {
                get
                {
                    return _screen.Width;
                }

                set
                {
                    _screen.Width = value;
                }
            }

            /// <summary>
            /// Заголовок окна
            /// </summary>
            public string WindowName
            {
                get
                {
                    return _screen.Text;
                }

                set
                {
                    _screen.Text = value;
                }
            }

            /// <summary>
            /// Инициализация всех менеджеров
            /// </summary>
            public void Init()
            {
            }

            /// <summary>
            /// Инициализация всех менеджеров
            /// </summary>
            /// <param name="info">Информация о создаваемом окне</param>
            public void Init(WindowCreationInfo info)
            {
                ResizeWindow(info.Size);
                WindowName = info.WindowName;
                IsFullScreen = info.FullScreen;
            }

            /// <summary>
            /// Изменение размеров экрана
            /// </summary>
            /// <param name="newWidth">Новая ширина окна</param>
            /// <param name="newHeight">Новая высота окна</param>
            public void ResizeWindow(int newWidth, int newHeight)
            {
                if (newWidth==0 || newHeight==0)
                {
                    throw new ArgumentNullException();
                }
                Width = newWidth;
                Height = newHeight;
            }

            /// <summary>
            /// Изменение размеров экрана
            /// </summary>
            /// <param name="newSize">Новый размер экрана</param>
            public void ResizeWindow(Size newSize)
            {
                if (newSize.IsEmpty)
                {
                    throw new ArgumentException();
                }
                Width = newSize.Width;
                Height = newSize.Height;
            }

            /// <summary>
            /// Запуск игрового цикла
            /// </summary>
            public void Run()
            {
                _screen.Show();
            }

            /// <summary>
            /// Выключение
            /// </summary>
            public void ShutDown()
            {
                _screen.Close();
            }

            /// <summary>
            /// Переключение в полноэкранный режим и обратно
            /// </summary>
            public void ToggleFullscreen()                   // !++ Beta 
            {
                if (IsFullScreen)
                {
                    _screen.FormBorderStyle = FormBorderStyle.FixedSingle;
                    _screen.WindowState = FormWindowState.Normal;
                    IsFullScreen = false;
                }
                else
                {
                    _screen.FormBorderStyle = FormBorderStyle.None;
                    _screen.WindowState = FormWindowState.Maximized;
                    IsFullScreen = true;
                }
            }
        }

        /// <summary>
        /// Информация о создании окна
        /// </summary>
        public class WindowCreationInfo
        {
            /// <summary>
            /// Размер окна
            /// </summary>
            public Size Size;

            /// <summary>
            /// Заголовок окна
            /// </summary>
            public string WindowName;

            /// <summary>
            /// Глубина цвета
            /// </summary>
            public int BitForPx;        // !++ Реализовать

            /// <summary>
            /// Возможность изменения размеров
            /// </summary>
            public bool CanResize;      // !++ Реализовать

            /// <summary>
            /// Использование OpenGL
            /// </summary>
            public bool UseOpenGl;      // !++ Реализовать

            /// <summary>
            /// Режим полного экрана
            /// </summary>
            public bool FullScreen;

            /// <summary>
            /// Использование аппаратного ускорения
            /// </summary>
            public bool HardwareSurface;        // !++ Реализовать

            /// <summary>
            /// Создаёт новую информацию о создании окна
            /// </summary>
            /// <param name="size">Размер окна</param>
            /// <param name="windowName">Заголовок окна</param>
            /// <param name="bitForPx">Глубина цвета</param>
            /// <param name="canResize">Возможность изменения размеров</param>
            /// <param name="fullScreen">Полноэкранный режим</param>
            public WindowCreationInfo(Size size, string windowName, int bitForPx, bool canResize, bool fullScreen)
            {
                Size = size;
                WindowName = windowName;
                BitForPx = bitForPx;
                CanResize = canResize;
                FullScreen = fullScreen;
            }

        }
    }
}
