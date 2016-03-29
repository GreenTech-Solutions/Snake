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
            int X = Data.snake[Data.Size - 1].x;
            int Y = Data.snake[Data.Size - 1].y;
            Coord temp;
            switch (Data.direction)
            {
                case Direction.Right:
                    X++;
                    if (X > 9)
                    {
                        X = 0;
                    }
                    temp = new Coord(X, Y);
                    Functions.Move(temp);
                    break;
                case Direction.Left:
                    X--;
                    if (X < 0)
                    {
                        X = 9;
                    }
                    temp = new Coord(X, Y);
                    Functions.Move(temp);
                    break;
                case Direction.Down:
                    Y++;
                    if (Y > 9)
                    {
                        Y = 0;
                    }
                    temp = new Coord(X, Y);
                    Functions.Move(temp);
                    break;
                case Direction.Up:
                    Y--;
                    if (Y < 0)
                    {
                        Y = 9;
                    }
                    temp = new Coord(X, Y);
                    Functions.Move(temp);
                    break;
            }
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
                Output.DrawMap();
                Output.DrawPlayer();

                // Ожидание нажатия клавиши
                Console.SetCursorPosition(10, 10);
                kInfo = Console.ReadKey();
                Functions.Clear();

                KeyHandler(kInfo);
                Do();
            }

            #endregion

        }

        /// <summary>
        /// Обработка нажатия клавиш
        /// </summary>
        public void KeyHandler(ConsoleKeyInfo kInfo)
        {
            if (kInfo.KeyChar == 'd')
            {
                Data.direction = Direction.Right;
            }
            if (kInfo.KeyChar == 'a')
            {
                Data.direction = Direction.Left;

            }
            if (kInfo.KeyChar == 's')
            {
                Data.direction = Direction.Down;

            }
            if (kInfo.KeyChar == 'w')
            {
                Data.direction = Direction.Up;

            }


            if (kInfo.KeyChar == 'q')
            {
                Environment.Exit(0);
            }
        }
    }
}
