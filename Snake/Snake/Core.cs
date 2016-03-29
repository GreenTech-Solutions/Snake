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
            if (Functions.CollidedWthFood())
            {
                
            }
            return false;
        }

        /// <summary>
        /// Установка координат пикселя, изменение скорости, движение
        /// </summary>
        public void Do()
        {
            int X = Data.snake.Head.Value.x;
            int Y = Data.snake.Head.Value.y;
            Coord temp;
            Data.Tail = Data.snake.Tail.Value;
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
            Data.snake = new SnakeList();

            for (int i = 0; i < Data.Size; i++)
            {
                Data.snake.AddToBegin(new Coord(i + 3, 0));
            }
            for (int i = 0; i < Data.MapSize.y; i++)
            {
                for (int j = 0; j < Data.MapSize.x; j++)
                {
                    Data.map[i, j] = 0;
                }
            }

            Functions.GenerateFood();

            #endregion

            #region Игровой цикл

            Output.DrawMap();
            while (true)
            {
                //Output.DrawMap();
                Output.RedrawMapcell(Data.Tail);
                Output.DrawPlayer();
                Output.DrawFood();

                // Ожидание нажатия клавиши
                Console.SetCursorPosition(10, 10);
                kInfo = Console.ReadKey();
                Functions.Clear();

                Input.KeyHandler(kInfo);
                Do();
            }

            #endregion

        }
    }
}
