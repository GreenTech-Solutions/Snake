using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// TODO Вывод результатов
// TODO Добавить коллизии с телом змеи
// TODO Возможность изменения стилей змеи (знака генерации тела и змеи)
// TODO Перенести игровые настройки в другой код
// TODO Создать менеджер состояний

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            int line = 1;
            string menu = "Snake by Alex_Green ©\n" +
                          "--> New Game\n" +
                          "    Settings\n" +
                          "    Exit\n";
            Console.Write(menu);

            while (true)
            {
                var kInfo = Console.ReadKey(true);
                switch (kInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (line<3)
                        Draw(line, ++line);
                        break;
                    case ConsoleKey.UpArrow:
                        if (line>1)
                        Draw(line,--line);
                        break;
                    case ConsoleKey.Enter:
                        if (line == 1)
                        {
                            new Core().Start();
                            Console.Clear();
                            Console.Write(menu);
                            Draw(1, 1);
                        }
                        else if (line == 2)
                        {
                            Settings();
                            Console.Clear();
                            Console.Write(menu);
                            Draw(1,2);
                        }
                        else if (line == 3)
                        {
                            Environment.Exit(0);
                        }
                        break;
                    default:
                        break;
                } 
            }
        }

        static void Settings()
        {
            Console.Clear();

            string settingsForFile = "10 10";

            FileInfo file = new FileInfo("settings.txt");
            if (file.Exists == false)
            {
                file.Create().Close();
            }
            StreamWriter sw = file.CreateText();
            sw.WriteLineAsync(settingsForFile);
            sw.Close();

            Console.Write("Settings\n");

            StreamReader sr = file.OpenText();
            List<string> settingsLines = new List<string>();

            while (!sr.EndOfStream)
            {
                settingsLines.Add(sr.ReadLine());
            }
            sr.Close();

            bool first = true;
            foreach (var settingsLine in settingsLines)
            {
                if (first)
                {
                    Console.Write("-->Map Size:" + settingsLine.Split(' ')[0] + "x" + settingsLine.Split(' ')[1] + "\n");
                    first = false;
                }
                else
                {
                    Console.Write("   " + settingsLine);
                }
            }

            Console.Write("   Back");

            int line = 1;

            while (true)
            {
                var kInfo = Console.ReadKey(true);

                switch (kInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (line < 2)
                            Draw(line, ++line);
                        break;
                    case ConsoleKey.UpArrow:
                        if (line > 1)
                            Draw(line, --line);
                        break;
                    case ConsoleKey.Enter:
                        if (line == 1)
                        {
                            Console.CursorVisible = true;

                            Console.SetCursorPosition(12,line);
                            string input = Console.ReadLine();

                            string[] values = input.Split(' ');


                            //int[] choice = new int[4];
                            //string key;
                            //int X = 12;

                            //// TODO Проверка ввода на неправильный размер карты
                            //for (int i = 0; i < 4; i++)
                            //{
                            //    INPUT:
                            //    key = Console.ReadKey().KeyChar.ToString();
                            //    if (!int.TryParse(key, out choice[i]))
                            //    {
                            //        goto INPUT;
                            //    }

                            //    if (X == 14)
                            //    {
                            //        X++;
                            //    }

                            //    Console.SetCursorPosition(X++, line);
                            //}

                            sw = file.CreateText();
                            sw.Write(input);
                            sw.Close();
                            Console.CursorVisible = false;
                        }
                        else if (line == 2)
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                } 
            }
        }

        static void Draw(int oldLine, int newLine)
        {
            Console.SetCursorPosition(0,oldLine);
            Console.Write("   ");
            Console.SetCursorPosition(0,newLine);
            Console.Write("-->");
        }
    }
}
