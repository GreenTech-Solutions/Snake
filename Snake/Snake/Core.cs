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
        public void Check()
        {
            if (Functions.CollidedWthFood())
            {
                Data.CollidedWthFood = true;
            }
            if (Data.CollidedWthFood)
            {
                Functions.OnCollidedWthFood();
            }
            if (Data.FoodEaten == true)
            {
                Functions.GenerateFood();
            }
        }

        /// <summary>
        /// Установка координат пикселя, изменение скорости, движение
        /// </summary>
        /// TODO движение работает отлично, но необходимо убрать привязку к 0, 9, и т.п.
        public void Do()
        {
            // Координаты головы
            int X = Data.snake.First.Value.X;
            int Y = Data.snake.First.Value.Y;

            Coord temp;
            Data.Tail = Data.snake.Last.Value;
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

            // Выделеие памяти для карты и змеи
            Data.map = new int[Data.MapSize.X, Data.MapSize.Y];
            Data.snake = new LinkedList<Coord>();

            // Создание змеи
            for (int i = 0; i < Data.Size; i++)
            {
                Data.snake.AddFirst(new Coord(/* TODO Значение выставляется не здесь */i + 3, 0));
            }
            // Заполнени карты
            for (int i = 0; i < Data.MapSize.Y; i++)
            {
                for (int j = 0; j < Data.MapSize.X; j++)
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

                // TODO Поменять значения 10,10 на автоматически выщитываемые относительно размера карты
                Console.SetCursorPosition(10, 10);

                var kInfo = Console.ReadKey();
                Functions.Clear();

                Input.KeyHandler(kInfo);
                Do();
                Check();
            }

            #endregion

        }
    }
}
