using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    /// <summary>
    /// Инкапсуляция механизма создания меню
    /// </summary>
    class Menu
    {
        /// <summary>
        /// Количество элементов меню
        /// </summary>
        private int Count => _menuItems.Count;

        /// <summary>
        /// Элементы меню
        /// </summary>
        private readonly List<MenuItem> _menuItems;

        /// <summary>
        /// Заголовок меню
        /// </summary>
        public string Title;
         
        /// <summary>
        /// Создаёт новое меню
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="count">Количество пунктов</param>
        public Menu(string title, int count = 1)
        {
            Title = title;
            _menuItems = new List<MenuItem>(count);
        }

        /// <summary>
        /// Добавить элемент меню в конец списка (Совпадения по названию не допускаются)
        /// </summary>
        /// <param name="item">Элемент меню</param>
        public void Add(MenuItem item)
        {
            if (_menuItems.Exists(x => x.Caption == item.Caption))
            {
                throw new ArgumentException("Каждый элемент меню должне иметь уникальное имя.");
            }
            _menuItems.Add(item);
        }

        /// <summary>
        /// Удалить элемент меню с указанным названием
        /// </summary>
        /// <param name="caption">Название</param>
        public void Remove(string caption)
        {
            _menuItems.RemoveAll(value => value.Caption == caption);
        }

        /// <summary>
        /// Получить элемент меню по указанному индексу(с нулевого)
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Элемент меню</returns>
        public MenuItem Get(int index)
        {
            return _menuItems[index];
        }

        /// <summary>
        /// Получить элемент меню по указанному названию
        /// </summary>
        /// <param name="caption">Название</param>
        /// <returns>Элемент меню</returns>
        public MenuItem Get(string caption)
        {
            return _menuItems.Find(x => x.Caption == caption);
        }

        /// <summary>
        /// Вывести меню в консоль
        /// </summary>
        public void Out()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.CursorLeft,Output.PaddingTop);
            Output.WriteLineCenter(Title);
            foreach (var menuItem in _menuItems)
            {
                Output.WriteCenter($"   {menuItem} ");
                if (menuItem.HasValue)
                {
                    if (Equals(menuItem.ConvertingFunction, null))
                    {
                        Console.Write(menuItem.Value.ToString());
                    }
                    else
                    {
                        Console.Write(menuItem.ConvertingFunction.Invoke(menuItem.Value));
                    }
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Номер предыдущего выделенного элемента
        /// </summary>
        private int previousline = 1;

        /// <summary>
        /// Вывести меню в консоль и включить его функционал
        /// </summary>
        public void Engage()
        {
            var line = previousline;
            Out();
            DrawCursor(line);

            MusicManager.Add(new Audio("Click",Resources.click),SoundType.Effect );

            while (true)
            {
                var kInfo = Console.ReadKey(true);

                if (kInfo.Key == Config.DownKey || kInfo.Key == ConsoleKey.DownArrow)
                {
                    MusicManager.Play("Click", SoundType.Effect);
                    if (line < Count)
                        DrawCursor(++line);
                }
                else if (kInfo.Key == Config.UpKey || kInfo.Key == ConsoleKey.UpArrow)
                {
                    MusicManager.Play("Click", SoundType.Effect);
                    if (line > 1)
                        DrawCursor(--line);
                }
                else if (kInfo.Key == ConsoleKey.Enter)
                {
                    MusicManager.Play("Click", SoundType.Effect);
                    if (_menuItems[line - 1].HasValue)
                    {
                        Edit(line - 1);
                    }
                    else if (_menuItems[line - 1].IsExitItem)
                    {
                        return;
                    }
                    else
                    {
                        _menuItems[line - 1].Function();
                    }
                    Console.Clear();
                    Out();
                    DrawCursor(line);
                }
                previousline = line;
            }
        }

        /// <summary>
        /// Редактирование значения элемента меню
        /// </summary>
        /// <param name="number">Номер элемента</param>
        public void Edit(int number)
        {
            var line = number+1;
            var item = _menuItems[number].Caption;
            Console.SetCursorPosition(4 + item.Length + Output.Padding, line + Output.PaddingTop);
            Console.CursorVisible = true;

            if (_menuItems[number].EditingFunction == null)
            {
                var value = new List<int>();

                ConsoleKeyInfo key;
                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    var kChar = key.KeyChar;
                    try
                    {
                        value.Add((int) Convert.ToUInt32(kChar.ToString()));
                        Console.Write(value.Last());
                    }
                    catch (Exception)
                    {
                    }
                }

                _menuItems[number].Value = Convert.ToInt32(string.Concat(value));
            }
            else
            {
                _menuItems[number].Value = _menuItems[number].EditingFunction.Invoke();
            }

            Console.CursorVisible = false;
        } 

        /// <summary>
        /// Устанавливает курсор на указанной линии
        /// </summary>
        /// <param name="line">Номер линии</param>
        void DrawCursor(int line)
        {
            Console.SetCursorPosition(Output.Padding,previousline + Output.PaddingTop);
            Console.Write("   ");
            Console.SetCursorPosition(Output.Padding, line + Output.PaddingTop);
            Console.Write("-->");
        }
    }

    /// <summary>
    /// Инкапсуляция элемента меню
    /// </summary>
    class MenuItem
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Caption;

        /// <summary>
        /// Функция, запускаемая при выборе
        /// </summary>
        public Action Function;

        /// <summary>
        /// Метод, инкапсулирующий процесс редактирования значения элемента меню
        /// <returns>Результат преобразования ввода пользователя</returns>
        /// </summary>
        public Func<int> EditingFunction;

        /// <summary>
        /// Метод, переводящий число в дружелюбный формат
        /// </summary>
        public Func<int,string> ConvertingFunction; 


        private int _value;
        /// <summary>
        /// Значение элемента меню
        /// </summary>
        public int Value
        {
            set
            {
                this._value = value; 
                ValueChanged?.Invoke(value);
            }
            get { return _value; }
        }

        /// <summary>
        /// Указывает является ли элемент меню командой выхода из меню
        /// </summary>
        public bool IsExitItem;

        /// <summary>
        /// Указывает, содержит ли элемент меню какое-либо значение
        /// </summary>
        public bool HasValue;

        /// <summary>
        /// Создаёт новый элемент меню
        /// </summary>
        /// <param name="caption">Название</param>
        /// <param name="function">Функция при выборе</param>
        public MenuItem(string caption, Action function)
        {
            Caption = caption;
            Function = function;
        }

        /// <summary>
        /// Создаёт новый элемент меню
        /// </summary>
        /// <param name="caption">Название</param>
        /// <param name="value">Значение</param>
        /// <param name="editingFunction">Функция, для редактирования значения</param>
        /// <param name="convertingFunction">Функция, для преобразования значения в дружелюбный формат</param>
        public MenuItem(string caption, int value, Func<int> editingFunction = null, Func<int,string> convertingFunction = null)
            : this(caption, null)
        {
            Value = value;
            HasValue = true;
            EditingFunction = editingFunction;
            ConvertingFunction = convertingFunction;
        }

        /// <summary>
        /// Создаёт новый элемент меню
        /// </summary>
        /// <param name="caption">Название</param>
        /// <param name="isExitItem">Любое значение будет означать, что элемент является элементом выхода из меню</param>
        public MenuItem(string caption, bool isExitItem)
            :this(caption,null)
        {
            IsExitItem = true;
        }

        /// <summary>
        /// Вызов функции элемента меню
        /// </summary>
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
