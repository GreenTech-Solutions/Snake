#define TEST
#undef TEST
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// TODO Не показывать еду после HeadCollide
// TODO Генерация еды сразу после HeadCollide
// TODO Возможность изменения стилей змеи (знака генерации тела и змеи)
// TODO Создать менеджер состояний

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
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
            // Запись начальных настроек в файл
            // TODO Заполнять файл только в случае его отсутствия
            // TODO Проверить файл настроек на повреждения
            string[] settingsForFile = { "10", "10"};
            FileInfo file = new FileInfo("settings.txt");
            if (file.Exists == false)
            {
                file.Create().Close();

                StreamWriter sw = file.CreateText();
                foreach (var s in settingsForFile)
                {
                    sw.WriteLineAsync(s);
                }
                sw.Close();
            }

            // Чтение настроек из файла
            StreamReader sr = file.OpenText();
            List<string> settingsLines = new List<string>();

            while (!sr.EndOfStream)
            {
                settingsLines.Add(sr.ReadLine());
            }
            sr.Close();

            int[] values = new int[settingsLines.Count];
            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = Convert.ToInt32(settingsLines[i]);
            }

            Menu Settings = new Menu("Settings");
            Settings.Add(new MenuItem("Map Width",values[0]));
            Settings.Add(new MenuItem("Map Height",values[1]));
            Settings.Add(new MenuItem("Back",true));

            Settings.Get(0).ValueChanged += (value) =>
            {
                StreamWriter sw = file.CreateText();
                sw.WriteLine(value);
                sw.WriteLine(settingsLines[1]);
                sw.Close();
            };

            Settings.Get(1).ValueChanged += (value) =>
            {
                StreamWriter sw = file.CreateText();
                sw.WriteLine(settingsLines[0]);
                sw.WriteLine(value);
                sw.Close();
            };

            Settings.Engage();
        }
    }
}
