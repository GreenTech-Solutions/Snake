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
                    Console.Write(menuItem.Value);
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

            while (true)
            {
                var kInfo = Console.ReadKey(true);
                switch (kInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (line < count)
                            DrawCursor(++line);
                        break;
                    case ConsoleKey.UpArrow:
                        if (line > 1)
                            DrawCursor(--line);
                        break;
                    case ConsoleKey.Enter:
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
                        break;
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

            List<int> value = new List<int>();

            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                char kChar = key.KeyChar;
                try
                {
                    value.Add((int)Convert.ToUInt32(kChar.ToString()));
                    Console.Write(value.Last());
                }
                catch (Exception)
                {
                    continue;
                }
            }

            MenuItems[number].Value = Convert.ToInt32(string.Concat(value));

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

        public MenuItem(string caption, int value)
            : this(caption, null)
        {
            Value = value;
            HasValue = true;
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
