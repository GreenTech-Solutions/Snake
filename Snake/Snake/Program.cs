#define TEST
#undef TEST
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

// TODO Изменение скорости
// TODO Добавить концепцию уровней
// TODO Настройки громкости
// TODO Возможность изменения стилей змеи (знака генерации тела и змеи)
// TODO Создать менеджер состояний

namespace Snake
{
    class Program
    {
        //Переменные и методы для настройки отображения в консоли
        const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleDisplayMode(IntPtr ConsoleHandle, uint Flags, IntPtr NewScreenBufferDimensions);

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

        /// <summary>
        /// Настройка основных переменных из файла конфигурации
        /// </summary>
        static void Configurate()
        {
            var settings = ConfigurationManager.AppSettings;
            Config.UpKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["ControlsUp"]);
            Config.DownKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["ControlsDown"]);
            Config.LeftKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["Controlsleft"]);
            Config.RightKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), settings["ControlsRight"]);

        }

        /// <summary>
        /// Получение версии программы
        /// </summary>
        /// <returns>Версия программы</returns>
        private static string GetVersion()
        {
            string version = "0.0.5.2";

            version = SnakeSettings.Default.Version;

            return version;
        }

        /// <summary>
        /// Объект для работы с музыкой
        /// </summary>
        private static Music _music;

        static void Main(string[] args)
        {
            try
            {
                // Получение дескриптора окна
                var hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

                var fontInfo = new CONSOLE_FONT_INFO_EX
                {
                    FontSize =
                    {
                        X = 0,
                        Y = 22
                    },
                    cbSize = Marshal.SizeOf(typeof (CONSOLE_FONT_INFO_EX)),
                    FaceName = "Lucida Console",
                    FontFamily = 54,
                    FontIndex = 1
                };

                // TODO Управление шрифтом, можно применять только стандартные значения

                //Установка шрифта
                SetCurrentConsoleFontEx(hConsole, false, ref fontInfo);

                // Включение полноэкранного режима
                SetConsoleDisplayMode(hConsole, 1, IntPtr.Zero);
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                // Скрытие курсора мыши
                Cursor.Clip = new Rectangle(SystemInformation.PrimaryMonitorSize.Width,
                    SystemInformation.PrimaryMonitorSize.Height, 1, 1);
                Cursor.Hide();

                // Основные настройки системы
                Console.Title = "Snake v" + GetVersion();
                SnakeSettings.Default.open_sum++;
                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;

                Configurate();

                _music = new Music(new Audio(Resources.MainMenu));

                Menu MainMenu = new Menu("Snake by Alex_Green ©");
                MainMenu.Add(new MenuItem("New Game", () => StartLevel()));
                MainMenu.Add(new MenuItem("Levels",Levels));
                MainMenu.Add(new MenuItem("Settings", Settings));
                MainMenu.Add(new MenuItem("Exit", true));

                _music.PlayLoop();
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

        static void StartLevel(Level level = null)
        {
            var core = new Core();
            _music.Stop();
            _music.Load(new Audio(Resources.InGame));
            _music.PlayLoop();
            if (Equals(level, null))
            {
                core.Initialize();
                core.Start();
            }
            else
            {
                core.Start(level);
            }
            _music.Stop();
            _music.Load(new Audio(Resources.MainMenu));
            _music.PlayLoop();
        }

        static void Levels()
        {
            Menu Levels = new Menu("Levels");

            try
            {
                var folder = new DirectoryInfo(Application.StartupPath + @"\Levels");
                FileSystemInfo[] files = folder.GetFileSystemInfos("*.lvl");
                if (files.Length >= 1)
                {
                    foreach (var file in files)
                    {
                        Levels.Add(new MenuItem(file.Name.Split('.')[0], () => StartLevel(Level.GetLevelFromFile(file.FullName))));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Возникла проблема при открытии файлов уровней.");
            }
            Levels.Add(new MenuItem("Back",true));

            Levels.Engage();
        }

        /// <summary>
        /// Меню настроек
        /// </summary>
        static void Settings()
        {
            var settings = ConfigurationManager.AppSettings;

            Menu Settings = new Menu("Settings");
            Settings.Add(new MenuItem("Map Width",Convert.ToInt32(settings["MapWidth"])));
            Settings.Add(new MenuItem("Map Height",Convert.ToInt32(settings["MapHeight"])));
            Settings.Add(new MenuItem("Controls",Controls));
            Settings.Add(new MenuItem("Audio",Audio));
            Settings.Add(new MenuItem("Back",true));

            Settings.Get("Map Width").ValueChanged += value =>
            {
                settings["MapWidth"] = value.ToString();
            };

            Settings.Get("Map Height").ValueChanged += value =>
            {
                settings["MapWidth"] = value.ToString();
            };

            Settings.Engage();
        }

        /// <summary>
        /// Меню настроек управления
        /// </summary>
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

        /// <summary>
        /// Меню настроек звука
        /// </summary>
        static void Audio()
        {
            var settings = ConfigurationManager.AppSettings;

            Menu Audio = new Menu("Audio");
            Audio.Add(new MenuItem("Music",Convert.ToInt32(settings["AudioMusic"]), AudioEditingFunction,i => i > 0 ? "On" : "Off"));
            Audio.Add(new MenuItem("Back",true));

            Audio.Get("Music").ValueChanged += value => settings["AudioMusic"] = value.ToString();
            Audio.Engage();
        }

        static string ControlsConvertingFunction(int value)
        {
            return Enum.Parse(typeof (ConsoleKey), value.ToString()).ToString();
        }

        static int ControlsEditingFunction()
        {
            return Convert.ToInt32(Console.ReadKey().Key);
        }

        static int AudioEditingFunction()
        {
            var settings = ConfigurationManager.AppSettings;
            var value = Convert.ToInt32(settings["AudioMusic"]);
            var bvalue = value > 0;

            while (true)
            {
                var key = Console.ReadKey(true);
                var top = Console.CursorTop;
                var left = Console.CursorLeft;

                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow)
                {
                    bvalue = !bvalue;
                    Console.Write(bvalue ? "On " : "Off");
                    Console.SetCursorPosition(left, top);
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    _music.CanPlay = bvalue;
                    return bvalue ? 1 : 0;
                }
            }
        }
    }
}
