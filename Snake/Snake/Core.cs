using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Snake
{
    class Core
    {
        private Data data = new Data();
        private bool CanExit = false;
        delegate void Collided(params dynamic []args);

        private event Collided CollidedWithFood;
        private event Collided CollidedWithBody;
        /// <summary>
        /// Проверка возможности поворота змеи, проверка проигрыша и съедания еды
        /// </summary>
        /// <returns></returns>
        public void Check()
        {
            if (data.snake.SkipWhile(value => value==data.snake.First.Value).Any(coord => coord == data.snake.First.Value))
            {
                CollidedWithBody?.Invoke();
            }

            if (data.snake.First.Value == data.Food)
            {
                CollidedWithFood?.Invoke();
            }

            if (data.CollidedWthFood)
            {
                if (data.Tail == data.FoodForEating)
                {
                    data.FoodEaten = false;
                }
            }
        }


        /// <summary>
        /// Установка координат пикселя, изменение скорости, движение
        /// </summary>
        public void Do()
        {
            // Координаты головы
            int X = data.snake.First.Value.X;
            int Y = data.snake.First.Value.Y;

            int maxX, maxY, minX, minY;

            maxX = data.MapSize.X-1;
            maxY = data.MapSize.Y-1;

            Coord temp;
            data.Tail = data.snake.Last.Value;

            switch (data.direction)
            {
                case Direction.Right:
                    X++;
                    if (X > maxX)
                    {
                        X = 0;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp, data.snake);
                    break;
                case Direction.Left:
                    X--;
                    if (X < 0)
                    {
                        X = maxX;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp, data.snake);
                    break;
                case Direction.Down:
                    Y++;
                    if (Y > maxY)
                    {
                        Y = 0;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp,data.snake);
                    break;
                case Direction.Up:
                    Y--;
                    if (Y < 0)
                    {
                        Y = maxY;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp,data.snake);
                    break;
            }
        }

        public NameValueCollection GetSettings()
        {
            return ConfigurationManager.AppSettings;
        }

        public int GetScore()
        {
            return data.Score;
        }

        public void SetScore()
        {
            data.Score = (data.snake.Count-2) * 10;
        }

        /// <summary>
        /// Проверяет старое направление с новым и выдаёт правильное
        /// </summary>
        /// <param name="newDirection"></param>
        /// <returns></returns>
        private Direction EvaulateDirection(Direction newDirection)
        {
            Direction oldDirection = data.direction;
            switch (newDirection)
            {
                case Direction.Up:
                    if (oldDirection == Direction.Down)
                    {
                        return oldDirection;
                    }
                    break;
                case Direction.Down:
                    if (oldDirection == Direction.Up)
                    {
                        return oldDirection;
                    }
                    break;
                case Direction.Left:
                    if (oldDirection == Direction.Right)
                    {
                        return oldDirection;
                    }
                    break;
                case Direction.Right:
                    if (oldDirection == Direction.Left)
                    {
                        return oldDirection;
                    }
                    break;
            }

            return newDirection;
        }

        /// <summary>
        /// Инициализация, начало, конец, перезагрузка, подключение стилей
        /// </summary>
        public void Start()
        {
            #region Инициализация

            var settings = GetSettings();

            Coord sizeOfMap = new Coord(Convert.ToInt32(settings["MapWidth"]),
                Convert.ToInt32(settings["MapHeight"]));
            data.MapSize = sizeOfMap;

            // Выделение памяти для карты и змеи
            data.map = new int[data.MapSize.X, data.MapSize.Y];
            data.snake = new LinkedList<Coord>();

            // Создание змеи
            for (int i = 0; i < data.Size; i++)
            {
                data.snake.AddFirst(new Coord(/* TODO Значение выставляется не здесь */i + 3, 0));
            }

            data.Tail = data.snake.Last.Value;

            // Заполнение карты
            for (int i = 0; i < data.MapSize.Y; i++)
            {
                for (int j = 0; j < data.MapSize.X; j++)
                {
                    data.map[j,i] = 0;
                }
            }

            data.Food = Functions.GenerateFood(data.snake,data.MapSize);

            data.ScoreChanged += () => { Output.DrawScores(data.Score, data.MapSize); };
            CollidedWithFood += delegate
            {

                new Music(new Audio(Resources.apple)).PlayOnce();
                data.CollidedWthFood = true;
                SetScore();
                data.FoodForEating = data.Food;
                data.Food = Functions.GenerateFood(data.snake, data.MapSize);

            };

            CollidedWithBody += delegate {
                Output.DrawGameover(data.MapSize);
                CanExit = true;
            };

            Input input = new Input();

            #endregion

            #region Игровой цикл
            Output.Clear();
            Output.DrawMap(data.MapSize);
            Output.DrawScores(data.Score,data.MapSize);            

            while (true)
            {
                if (CanExit)
                {
                    return;
                }

                if (data.FoodEaten)
                {
                    Output.RedrawMapcell(data.Tail);
                }
                else
                {
                    data.FoodEaten = true;
                    data.CollidedWthFood = false;
                    Functions.Grow(ref data.snake,data.FoodForEating);
                }


                Output.DrawPlayer(data.snake);
                Output.DrawFood(data.Food);

                ActionType action = input.AskForInput();

                switch (action)
                {
                        case ActionType.Up:
                        data.direction = EvaulateDirection(Direction.Up);
                        break;
                    case ActionType.Down:
                        data.direction = EvaulateDirection(Direction.Down);
                        break;
                    case ActionType.Left:
                        data.direction = EvaulateDirection(Direction.Left);
                        break;
                        case ActionType.Right:
                        data.direction = EvaulateDirection(Direction.Right);
                        break;
                    case ActionType.Exit:
                        CanExit = true;
                        break;
                    case ActionType.None:
                        break;
                }

                Do();
                Check();
                Thread.Sleep(100);
            }

            #endregion

        }
    }
}
