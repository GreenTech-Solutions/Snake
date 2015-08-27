using System;
using System.Windows.Forms;
using System.Drawing;

namespace StrawberryGameEngine
{
    namespace Core
    {
        public class Main_WF : App
        {
            // Главное окно
            Form Screen = new Form();
            
            // Отвечает за состояние окна
            bool _fullScreen = false;

            // Активирован ли полноэкранный режим
            public bool ISFullScreen
            {
                get
                {
                    return this._fullScreen;
                }
            }
            
            // Высота окна
            public int Height
            {
                get
                {
                    return Screen.Height;
                }

                set
                {
                    Screen.Height = value;
                }
            }
            
            // Ширина окна
            public int Width
            {
                get
                {
                    return Screen.Width;
                }

                set
                {
                    Screen.Width = value;
                }
            }

            // Заголовок окна
            public string WindowName
            {
                get
                {
                    return Screen.Text;
                }

                set
                {
                    Screen.Text = value;
                }
            }

            // Инициализация всех менеджеров
            public void Init()
            {
            }

            public void Init(WindowCreationInfo info)
            {
                this.ResizeWindow(info.size);
                this.WindowName = info.WindowName;
                this._fullScreen = info.FullScreen;
            }

            // Изменение размеров экрана
            public void ResizeWindow(int width, int Height)
            {
                if (width==0 || Height==0)
                {
                    throw new ArgumentNullException();
                }
                this.Width = width;
                this.Height = Height;
            }

            public void ResizeWindow(Size newSize)
            {
                if (newSize.IsEmpty)
                {
                    throw new ArgumentException();
                }
                this.Width = newSize.Width;
                this.Height = newSize.Height;
            }

            // Запуск игрового цикла
            public void Run()
            {
                Screen.Show();
            }

            // Выключение
            public void ShutDown()
            {
                Screen.Close();
            }

            // Переключение в полноэкранный режим и обратно
            public void ToggleFullscreen()                   // --- Beta
            {
                if (ISFullScreen)
                {
                    Screen.FormBorderStyle = FormBorderStyle.FixedSingle;
                    Screen.WindowState = FormWindowState.Normal;
                    _fullScreen = false;
                }
                else
                {
                    Screen.FormBorderStyle = FormBorderStyle.None;
                    Screen.WindowState = FormWindowState.Maximized;
                    _fullScreen = true;
                }
            }
        }

        // Информация о создании окна
        public class WindowCreationInfo
        {
            // Ширина, высота
            public Size size;

            // Название окна
            public string WindowName;

            // Глубина цвета
            public int BitForPx;

            // Возможность изменения размеров, использование OpenGL, режим полного экрана, использование аппаратного ускорения
            public bool CanResize, UseOpenGL, FullScreen, HardwareSurface;

            public WindowCreationInfo(Size size, string windowName, int bitForPX, bool CanResize, bool FullScreen)
            {
                this.size = size;
                this.WindowName = windowName;
                this.BitForPx = bitForPX;
                this.CanResize = CanResize;
                this.FullScreen = FullScreen;
            }

        }
    }
}
