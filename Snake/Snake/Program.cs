#define TEST
#undef TEST
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

// TODO Хранить настройки в конфигурациооном файле
// TODO Настройка управления
// TODO Добавить звуки
// TODO Возможность изменения стилей змеи (знака генерации тела и змеи)
// TODO Создать менеджер состояний

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Snake v" + SnakeSettings.Default.Version;
            SnakeSettings.Default.open_sum++;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
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
            Settings.Add(new MenuItem("Back",true));

            Settings.Get(0).ValueChanged += (value) =>
            {
                settings["MapWidth"] = value.ToString();
            };

            Settings.Get(1).ValueChanged += (value) =>
            {
                settings["MapWidth"] = value.ToString();
            };

            Settings.Engage();
        }
    }
}
