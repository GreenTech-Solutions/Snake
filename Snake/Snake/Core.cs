using System;
using System.Collections.Generic;
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
        delegate void dHeadCollidedWithFood(params dynamic []args);

        private event dHeadCollidedWithFood HeadCollidedWithFood;
        /// <summary>
        /// Проверка возможности поворота змеи, проверка проигрыша и съедания еды
        /// </summary>
        /// <returns></returns>
        public void Check()
        {
            if (Data.snake.First.Value == Data.Food)
            {
                HeadCollidedWithFood();
            }
            if (Data.CollidedWthFood)
            {
                if (Data.Tail == Data.Food)
                {
                    Data.FoodEaten = false;
                }
            }
        }


        /// <summary>
        /// Установка координат пикселя, изменение скорости, движение
        /// </summary>
        public void Do()
        {
            // Координаты головы
            int X = Data.snake.First.Value.X;
            int Y = Data.snake.First.Value.Y;

            int maxX, maxY, minX, minY;

            maxX = Data.MapSize.X-1;
            maxY = Data.MapSize.Y-1;

            Coord temp;
            Data.Tail = Data.snake.Last.Value;
            switch (Data.direction)
            {
                case Direction.Right:
                    X++;
                    if (X > maxX)
                    {
                        X = 0;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp, Data.snake);
                    break;
                case Direction.Left:
                    X--;
                    if (X < 0)
                    {
                        X = maxX;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp, Data.snake);
                    break;
                case Direction.Down:
                    Y++;
                    if (Y > maxY)
                    {
                        Y = 0;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp,Data.snake);
                    break;
                case Direction.Up:
                    Y--;
                    if (Y < 0)
                    {
                        Y = maxY;
                    }
                    temp = new Coord(X, Y);
                    Functions.AddNewHead(temp,Data.snake);
                    break;
            }
        }

        public List<string> GetSettings()
        {
            FileInfo file = new FileInfo("settings.txt");

            List<string> lines = new List<string>();
            StreamReader sr = file.OpenText();
            while (!sr.EndOfStream)
            {
                lines.Add(sr.ReadLine());
            }
            sr.Close();

            return lines;
        }

        public int GetScore()
        {
            return Data.Score;
        }

        public void SetScore()
        {
            Data.Score = (Data.snake.Count-2) * 10;
        }

        /// <summary>
        /// Инициализация, начало, конец, перезагрузка, подключение стилей
        /// </summary>
        public void Start()
        {
            #region Инициализация

            List<string> settings = GetSettings();

            Coord sizeOfMap = new Coord(Convert.ToInt32(settings[0].Split(' ')[0]),
                Convert.ToInt32(settings[0].Split(' ')[1]));
            Data.MapSize = sizeOfMap;

            // Выделеие памяти для карты и змеи
            Data.map = new int[Data.MapSize.X, Data.MapSize.Y];
            Data.snake = new LinkedList<Coord>();

            // Создание змеи
            for (int i = 0; i < Data.Size; i++)
            {
                Data.snake.AddFirst(new Coord(/* TODO Значение выставляется не здесь */i + 3, 0));
            }

            Data.Tail = Data.snake.Last.Value;

            // Заполнени карты
            for (int i = 0; i < Data.MapSize.Y; i++)
            {
                for (int j = 0; j < Data.MapSize.X; j++)
                {
                    Data.map[i, j] = 0;
                }
            }

            Data.Food = Functions.GenerateFood(Data.snake,Data.MapSize);

            Data.ScoreChanged += () => { Output.DrawScores(Data.Score, Data.MapSize); };
            HeadCollidedWithFood += delegate
            {
                Data.CollidedWthFood = true;
                SetScore();
            };

            #endregion

            #region Игровой цикл
            Output.Clear();
            Output.DrawMap(Data.MapSize);
            Output.DrawScores(Data.Score,Data.MapSize);            

            while (true)
            {
                if (Data.FoodEaten)
                {
                    Output.RedrawMapcell(Data.Tail);
                }
                else
                {
                    Data.FoodEaten = true;
                    Data.CollidedWthFood = false;
                    Functions.Grow(ref Data.snake,Data.Food);
                    Data.Food = Functions.GenerateFood(Data.snake,Data.MapSize);
                }


                Output.DrawPlayer(Data.snake);
                Output.DrawFood(Data.Food);

                ActionType action = Input.AskForInput();

                //ActionType action = ActionType.Right;

                switch (action)
                {
                        case ActionType.Up:
                        Data.direction = Direction.Up;
                        break;
                    case ActionType.Down:
                        Data.direction = Direction.Down;
                        break;
                    case ActionType.Left:
                        Data.direction = Direction.Left;
                        break;
                        case ActionType.Right:
                        Data.direction = Direction.Right;
                        break;
                    case ActionType.Exit:
                        return;
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
