#define TEST
#undef TEST
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

// TODO Изменение скорости
// TODO Добавить концепцию уровней
// TODO Настройки громкости
// TODO Возможность изменения стилей змеи (знака генерации тела и змеи)
// TODO Создать менеджер состояний

namespace Snake
{
    class Program
    {


        static void Configurate()
        {
            var settings = ConfigurationManager.AppSettings;
            Config.UpKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["ControlsUp"]);
            Config.DownKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["ControlsDown"]);
            Config.LeftKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["Controlsleft"]);
            Config.RightKey = (ConsoleKey) Enum.Parse(typeof (ConsoleKey), settings["ControlsRight"]);
        }

        const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleDisplayMode(IntPtr ConsoleHandle, uint Flags, IntPtr NewScreenBufferDimensions);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetConsoleFont(IntPtr hOut, uint dwFontSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CONSOLE_FONT_INFO_EX
        {
            public int cbSize;

            public int FontIndex;
            public COORD FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
        IntPtr ConsoleOutput,
        bool MaximumWindow,
        ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx
        );


        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShow);

        static void Main(string[] args)
        {
            try
            {
                var hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

                CONSOLE_FONT_INFO_EX fontInfo = new CONSOLE_FONT_INFO_EX();

                fontInfo.FontSize.X = 0;
                // TODO Управление шрифтом, можно применять только стандартные значения
                fontInfo.FontSize.Y = 22;
                fontInfo.cbSize = Marshal.SizeOf(typeof(CONSOLE_FONT_INFO_EX));
                fontInfo.FaceName = "Lucida Console";
                fontInfo.FontFamily = 54;
                fontInfo.FontIndex = 1;

                SetCurrentConsoleFontEx(hConsole, false, ref fontInfo);

                SetConsoleDisplayMode(hConsole, 1, IntPtr.Zero);
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                Cursor.Clip = new System.Drawing.Rectangle(SystemInformation.PrimaryMonitorSize.Width,
                    SystemInformation.PrimaryMonitorSize.Height,1,1);
                Cursor.Hide();

                

                Console.Title = "Snake v" + SnakeSettings.Default.Version;
                SnakeSettings.Default.open_sum++;
                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;

                Configurate();

                Music music = new Music(new Audio(Resources.MainMenu));

                Menu MainMenu = new Menu("Snake by Alex_Green ©");
                MainMenu.Add(new MenuItem("New Game", delegate
                {
                    var core = new Core();

                    music.Stop();
                    music = new Music(new Audio(Resources.InGame));
                    music.PlayLoop();
                    core.Start();
                    music.Stop();
                    music = new Music(new Audio(Resources.MainMenu));
                    music.PlayLoop();
                }));
                MainMenu.Add(new MenuItem("Settings", Settings));
                MainMenu.Add(new MenuItem("Exit", true));

                music.PlayLoop();
                MainMenu.Engage();
            }
                //catch
                //{
                //    throw;
                //}
            finally
            {
                
            }
        }

        static void Settings()
        {
            var settings = ConfigurationManager.AppSettings;

            Menu Settings = new Menu("Settings");
            Settings.Add(new MenuItem("Map Width",Convert.ToInt32(settings["MapWidth"])));
            Settings.Add(new MenuItem("Map Height",Convert.ToInt32(settings["MapHeight"])));
            Settings.Add(new MenuItem("Controls",Controls));
            Settings.Add(new MenuItem("Back",true));

            Settings.Get("Map Width").ValueChanged += (value) =>
            {
                settings["MapWidth"] = value.ToString();
            };

            Settings.Get("Map Height").ValueChanged += (value) =>
            {
                settings["MapWidth"] = value.ToString();
            };

            Settings.Engage();
        }

        static void Controls()
        {
            var settings = ConfigurationManager.AppSettings;

            Menu Controls = new Menu("Controls settings");

            Controls.Add(new MenuItem("Up", Convert.ToInt32(settings["ControlsUp"]), ControlsEditingFunction,ControlsConvertingFunction));
            Controls.Add(new MenuItem("Down",Convert.ToInt32(settings["ControlsDown"]), ControlsEditingFunction, ControlsConvertingFunction));
            Controls.Add(new MenuItem("Left",Convert.ToInt32(settings["ControlsLeft"]), ControlsEditingFunction, ControlsConvertingFunction));
            Controls.Add(new MenuItem("Right", Convert.ToInt32(settings["ControlsRight"]), ControlsEditingFunction, ControlsConvertingFunction));
            Controls.Add(new MenuItem("Back",true));

            Controls.Get("Up").ValueChanged += value => { settings["ControlsUp"] = value.ToString(); Configurate();};
            Controls.Get("Down").ValueChanged += value => { settings["ControlsDown"] = value.ToString(); Configurate(); };
            Controls.Get("Left").ValueChanged += value => { settings["ControlsLeft"] = value.ToString(); Configurate(); };
            Controls.Get("Right").ValueChanged += value => { settings["ControlsRight"] = value.ToString(); Configurate(); };

            Controls.Engage();
        }

        static string ControlsConvertingFunction(int value)
        {
            return Enum.Parse(typeof (ConsoleKey), value.ToString()).ToString();
        }

        static int ControlsEditingFunction()
        {
            return Convert.ToInt32(Console.ReadKey().Key);
        }
    }
}
