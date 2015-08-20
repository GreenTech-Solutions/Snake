using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace StrawberryGameEngine
{
    namespace Core
    {
        public class Main_SDL : App
        {
            // Название окна
            private string wndname;

            // Высота, ширина окна
            public int Height
            {
                get
                {
                    return this.Screen.Height;
                }
                set
                {
                    Screen = Video.SetVideoMode(this.Screen.Width, value);
                }
            }
            public int Width
            {
                get
                {
                    return this.Screen.Width;
                }
                set
                {
                    Screen = Video.SetVideoMode(value, this.Screen.Height);
                }
            }

            // Включён ли режим полного экрана
            public bool ISFullScreen
            {
                get
                {
                    return this.Screen.FullScreen;
                }
            }

            // Имя окна
            public string WindowName
            {
                get
                {
                    return this.wndname;
                }
                set
                {
                    this.wndname = value;
                }
            }

            // Переменная для работы с окном
            Surface Screen;

            // Инициализация окна
            public void Init() { }

            public void Init(WindowCreationInfo inf)
            {
                Screen = Video.SetVideoMode(inf.Width, inf.Height, inf.BitForPx, inf.CanResize, inf.UseOpenGL, inf.FullScreen, inf.HardwareSurface);
                Events.TargetFps = 30;
            }

            // Изменение размеров окна
            public void ResizeWindow(int width, int Height)
            {
                Screen = Video.SetVideoMode(width, Height);
            }

            // Запуск программы
            public void Run()
            {
                Events.Run();
            }

            // Выключение программы
            public void ShutDown()
            {
                Events.QuitApplication();
            }

            // Переключение в режим полного экрана
            public void ToggleFullscreen()
            {
            }
        }

        // Информация о создании окна
        public class WindowCreationInfo
        {
            // Ширина, высота
            public int Width, Height;

            // Название окна
            public string WindowName;

            // Глубина цвета
            public int BitForPx;

            // Возможность изменения размеров, использование OpenGL, режим полного экрана, исполльзование HardwareSurface
            public bool CanResize, UseOpenGL, FullScreen, HardwareSurface;

            public WindowCreationInfo(int width, int height, string windowName, int bitForPX, bool CanResize, bool UseOpenGL, bool FullScreen, bool HardwareSurface)
            {
                this.Width = width;
                this.Height = height;
                this.WindowName = windowName;
                this.BitForPx = bitForPX;
                this.CanResize = CanResize;
                this.UseOpenGL = UseOpenGL;
                this.FullScreen = FullScreen;
                this.HardwareSurface = HardwareSurface;
            }

        }
    }
}
