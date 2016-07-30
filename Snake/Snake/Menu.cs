using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Menu
    {
        private int count => MenuItems.Count;

        private List<MenuItem> MenuItems = new List<MenuItem>();

        public string Title;
         
        public Menu(string title, int count = 1)
        {
            Title = title;
        }

        public void Add(MenuItem item)
        {
            if (MenuItems.Exists(x => x.Caption == item.Caption))
            {
                throw new ArgumentException("Каждый элемент меню должне иметь уникальное имя.");
            }
            MenuItems.Add(item);
        }

        public void Remove(string caption)
        {
            MenuItems.RemoveAll(value => value.Caption == caption);
        }

        public MenuItem Get(int index)
        {
            return MenuItems[index];
        }

        public MenuItem Get(string caption)
        {
            return MenuItems.Find(x => x.Caption == caption);
        }

        public void Out()
        {
            Console.Clear();
            Console.WriteLine(Title);
            foreach (var menuItem in MenuItems)
            {
                Console.Write($"   {menuItem} ");
                if (menuItem.HasValue)
                {
                    if (Equals(menuItem.ConvertingFunction, null))
                    {
                        Console.Write(menuItem.Value);
                    }
                    else
                    {
                        Console.Write(menuItem.ConvertingFunction.Invoke(menuItem.Value));
                    }
                }
                Console.Write("\n");
            }
        }

        private int previousline = 1;

        public void Engage()
        {
            int line = previousline;
            Out();
            DrawCursor(line);

            Music music = new Music(new Audio(Resources.click));

            while (true)
            {
                var kInfo = Console.ReadKey(true);

                music.PlayOnce();

                if (kInfo.Key == Config.DownKey || kInfo.Key == ConsoleKey.DownArrow)
                {
                    if (line < count)
                        DrawCursor(++line);
                }
                else if (kInfo.Key == Config.UpKey || kInfo.Key == ConsoleKey.UpArrow)
                {
                    if (line > 1)
                        DrawCursor(--line);
                }
                else if (kInfo.Key == ConsoleKey.Enter)
                {
                    if (MenuItems[line - 1].HasValue)
                    {
                        Edit(line - 1);
                    }
                    else if (MenuItems[line - 1].IsExitItem)
                    {
                        return;
                    }
                    else
                    {
                        MenuItems[line - 1].Function();
                    }
                    Console.Clear();
                    Out();
                    DrawCursor(line);
                }
                previousline = line;
            }
        }

        public void Edit(int number)
        {
            int line = number+1;
            string item = MenuItems[number].Caption;
            Console.SetCursorPosition(4 + item.Length, line);
            Console.CursorVisible = true;

            if (MenuItems[number].EditingFunction == null)
            {
                List<int> value = new List<int>();

                ConsoleKeyInfo key;
                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    char kChar = key.KeyChar;
                    try
                    {
                        value.Add((int) Convert.ToUInt32(kChar.ToString()));
                        Console.Write(value.Last());
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                MenuItems[number].Value = Convert.ToInt32(string.Concat(value));
            }
            else
            {
                MenuItems[number].Value = MenuItems[number].EditingFunction.Invoke();
            }

            Console.CursorVisible = false;
        } 

        void DrawCursor(int line)
        {
            Console.SetCursorPosition(0,previousline);
            Console.Write("   ");
            Console.SetCursorPosition(0, line);
            Console.Write("-->");
        }
    }

    class MenuItem
    {
        public string Caption;
        public Action Function = null;

        /// <summary>
        /// Метод, инкапсулирующий процесс редактирования значения элемента меню
        /// <returns>Результат преобразования ввода пользователя</returns>
        /// </summary>
        public Func<int> EditingFunction = null;

        /// <summary>
        /// Метод, переводящий число в дружелюбный формат
        /// </summary>
        public Func<int,string> ConvertingFunction = null; 

        private int value;

        public int Value
        {
            set
            {
                this.value = value; 
                ValueChanged?.Invoke(value);
            }
            get { return value; }
        }

        public bool IsExitItem = false;

        public bool HasValue = false;

        public MenuItem(string caption, Action function)
        {
            Caption = caption;
            Function = function;
        }

        public MenuItem(string caption, int value, Func<int> editingFunction = null, Func<int,string> convertingFunction = null)
            : this(caption, null)
        {
            Value = value;
            HasValue = true;
            EditingFunction = editingFunction;
            ConvertingFunction = convertingFunction;
        }

        public MenuItem(string caption, bool isExitItem)
            :this(caption,null)
        {
            IsExitItem = true;
        }

        public void Invoke()
        {
            Function?.Invoke();
        }

        public override string ToString()
        {
            return Caption;
        }

        public delegate void valueChanged(int newValue);

        public event valueChanged ValueChanged;
    }
}
