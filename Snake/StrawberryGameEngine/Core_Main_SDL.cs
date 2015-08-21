using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32API;



namespace StrawberryGameEngine
{
    namespace Core
    {
        public class Main_SDL : NativeWindow, App
        {
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
                    return Video.WindowCaption;
                }
                set
                {
                    Video.WindowCaption = value;
                }
            }

            // Переменная для работы с окном
            Surface Screen;

            // Инициализация окна
            public void Init() { }

            public void Init(WindowCreationInfo inf)
            {
                Screen = Video.SetVideoMode(inf.size.Width, inf.size.Height, inf.BitForPx, inf.CanResize, inf.UseOpenGL, inf.FullScreen, inf.HardwareSurface);
                Video.WindowCaption = inf.WindowName;


                Events.TargetFps = 30;
                Events.Quit += (QuitEventHandler);
            }



            // Constant values were found in the "windows.h" header file.
            private const int WS_CHILD = 0x40000000,
                              WS_VISIBLE = 0x10000000,
                              WM_ACTIVATEAPP = 0x001C;

            private int windowHandle;

            // Изменение размеров окна
            public void ResizeWindow(int width, int height)
            {
                try
                {
                    Form parent = new Form();
                    CreateParams cp = new CreateParams();

                    // Fill in the CreateParams details.
                    cp.Caption = "Hello from C#";
                    cp.ClassName = "Button";

                    // Set the position on the form
                    cp.X = 100;
                    cp.Y = 100;
                    cp.Height = 100;
                    cp.Width = 100;

                    // Specify the form as the parent.
                    cp.Parent = parent.Handle;

                    // Create as a child of the specified parent
                    cp.Style = WS_CHILD | WS_VISIBLE;

                    // Create the actual window
                    this.CreateHandle(cp);

                    Win32.SendMessage(this.Handle, WM_ACTIVATEAPP, IntPtr.Zero, IntPtr.Zero);
                    Win32.SendMessage(this.Handle, WS_VISIBLE, IntPtr.Zero, IntPtr.Zero);
                }
                catch
                {

                    throw;
                }

                
                //Screen = Video.Screen.CreateResizedSurface(new Size(width,height));
                //Video.Update();
                //IntPtr w = Video.Screen.Handle;
                //uint WM_CLOSE = 0x10;
                //SendMessage(w, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            }

            // Запуск игрового цикла
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

            private void QuitEventHandler(object sender, QuitEventArgs args)
            {
                ShutDown();
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

            public WindowCreationInfo(Size size, string windowName, int bitForPX, bool CanResize, bool UseOpenGL, bool FullScreen, bool HardwareSurface)
            {
                this.size = size;
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
