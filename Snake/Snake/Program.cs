#define TEST
#undef TEST
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

// TODO Настройка управления
// TODO Добавить звуки
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

        static void Main(string[] args)
        {
            Console.Title = "Snake v" + SnakeSettings.Default.Version;
            SnakeSettings.Default.open_sum++;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            Configurate();

            Menu MainMenu = new Menu("Snake by Alex_Green ©");
            MainMenu.Add(new MenuItem("New Game", delegate
            {
                var core = new Core();
                core.Start();
            }));
            MainMenu.Add(new MenuItem("Settings", Settings));
            MainMenu.Add(new MenuItem("Exit",true));
            
            MainMenu.Engage();
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
