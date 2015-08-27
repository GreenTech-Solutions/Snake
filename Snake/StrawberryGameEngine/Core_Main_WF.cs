using System;
using System.Drawing;
using System.Windows.Forms;

namespace StrawberryGameEngine
{
    namespace Core
    {
        public class MainWf : IApp
        {
            // Главное окно
            Form _screen = new Form();
            
            // Отвечает за состояние окна
            bool _fullScreen;

            // Активирован ли полноэкранный режим
            public bool IsFullScreen => _fullScreen;

            // Высота окна
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
            
            // Ширина окна
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

            // Заголовок окна
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

            // Инициализация всех менеджеров
            public void Init()
            {
            }

            public void Init(WindowCreationInfo info)
            {
                ResizeWindow(info.Size);
                WindowName = info.WindowName;
                _fullScreen = info.FullScreen;
            }

            // Изменение размеров экрана
            public void ResizeWindow(int width, int height)
            {
                if (width==0 || height==0)
                {
                    throw new ArgumentNullException();
                }
                Width = width;
                Height = height;
            }

            public void ResizeWindow(Size newSize)
            {
                if (newSize.IsEmpty)
                {
                    throw new ArgumentException();
                }
                Width = newSize.Width;
                Height = newSize.Height;
            }

            // Запуск игрового цикла
            public void Run()
            {
                _screen.Show();
            }

            // Выключение
            public void ShutDown()
            {
                _screen.Close();
            }

            // Переключение в полноэкранный режим и обратно
            public void ToggleFullscreen()                   // --- Beta
            {
                if (IsFullScreen)
                {
                    _screen.FormBorderStyle = FormBorderStyle.FixedSingle;
                    _screen.WindowState = FormWindowState.Normal;
                    _fullScreen = false;
                }
                else
                {
                    _screen.FormBorderStyle = FormBorderStyle.None;
                    _screen.WindowState = FormWindowState.Maximized;
                    _fullScreen = true;
                }
            }
        }

        // Информация о создании окна
        public class WindowCreationInfo
        {
            // Ширина, высота
            public Size Size;

            // Название окна
            public string WindowName;

            // Глубина цвета
            public int BitForPx;

            // Возможность изменения размеров, использование OpenGL, режим полного экрана, использование аппаратного ускорения
            public bool CanResize, UseOpenGl, FullScreen, HardwareSurface;

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
