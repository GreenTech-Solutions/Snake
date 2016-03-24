using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Core
    {
        /// <summary>
        /// Проверка возможности поворота змеи, проверка проигрыша и съедания еды
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            return false;
        }

        /// <summary>
        /// Установка координат пикселя, изменение скорости, движение
        /// </summary>
        public void Do()
        {
            
        }

        /// <summary>
        /// Инициализация, начало, конец, перезагрузка, подключение стилей
        /// </summary>
        public void Main()
        {
            #region Инициализация

            ConsoleKeyInfo kInfo;
            Data.map = new int[Data.MapSize.x, Data.MapSize.y];
            Data.snake = new Coord[Data.Size];

            for (int i = 0; i < Data.Size; i++)
            {
                Data.snake[i] = new Coord(i + 3, 0);
            }
            for (int i = 0; i < Data.MapSize.y; i++)
            {
                for (int j = 0; j < Data.MapSize.x; j++)
                {
                    Data.map[i, j] = 0;
                }
            }

            #endregion

            #region Игровой цикл

            while (true)
            {
                // Рисование игрового поля
                for (int i = 0; i < Data.MapSize.y; i++)
                {
                    for (int j = 0; j < Data.MapSize.x; j++)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write('-');
                    }
                }
                // Рисование змеи
                for (int i = 0; i < Data.Size; i++)
                {
                    Console.SetCursorPosition(Data.snake[i].x, Data.snake[i].y);
                    if (i == Data.Size - 1)
                    {
                        Console.Write('©');
                    }
                    else
                    {
                        Console.Write('*');
                    }
                }

                // Ожидание нажатия клавиши
                Console.SetCursorPosition(10, 10);
                kInfo = Console.ReadKey();
                Functions.Clear();

                KeyHandler(kInfo);
            }

            #endregion

        }

        /// <summary>
        /// Обработка нажатия клавиш
        /// </summary>
        public void KeyHandler(ConsoleKeyInfo kInfo)
        {
            // Обработка нажатия клавиши
            int X = Data.snake[Data.Size - 1].x;
            int Y = Data.snake[Data.Size - 1].y;
            if (kInfo.KeyChar == 'd')
            {
                X++;
                if (X > 9)
                {
                    X = 0;
                }
                Coord temp = new Coord(X, Y);
                Functions.Move(temp);
            }
            if (kInfo.KeyChar == 'a')
            {
                X--;
                if (X < 0)
                {
                    X = 9;
                }
                Coord temp = new Coord(X, Y);
                Functions.Move(temp);
            }
            if (kInfo.KeyChar == 's')
            {
                Y++;
                if (Y > 9)
                {
                    Y = 0;
                }
                Coord temp = new Coord(X, Y);
                Functions.Move(temp);
            }
            if (kInfo.KeyChar == 'w')
            {
                Y--;
                if (Y < 0)
                {
                    Y = 9;
                }
                Coord temp = new Coord(X, Y);
                Functions.Move(temp);
            }


            if (kInfo.KeyChar == 'q')
            {
                Environment.Exit(0);
            }
        }
    }
}
