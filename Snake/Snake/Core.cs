﻿using System;
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
                if (data.Tail == data.Food)
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
            return data.Score;
        }

        public void SetScore()
        {
            data.Score = (data.snake.Count-2) * 10;
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
            data.MapSize = sizeOfMap;

            // Выделеие памяти для карты и змеи
            data.map = new int[data.MapSize.X, data.MapSize.Y];
            data.snake = new LinkedList<Coord>();

            // Создание змеи
            for (int i = 0; i < data.Size; i++)
            {
                data.snake.AddFirst(new Coord(/* TODO Значение выставляется не здесь */i + 3, 0));
            }

            data.Tail = data.snake.Last.Value;

            // Заполнени карты
            for (int i = 0; i < data.MapSize.Y; i++)
            {
                for (int j = 0; j < data.MapSize.X; j++)
                {
                    data.map[i, j] = 0;
                }
            }

            data.Food = Functions.GenerateFood(data.snake,data.MapSize);

            data.ScoreChanged += () => { Output.DrawScores(data.Score, data.MapSize); };
            CollidedWithFood += delegate
            {
                data.CollidedWthFood = true;
                SetScore();
            };
            CollidedWithBody += delegate {
                Output.DrawGameover();
                                             CanExit = true;
            };

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
                    Functions.Grow(ref data.snake,data.Food);
                    data.Food = Functions.GenerateFood(data.snake,data.MapSize);
                }


                Output.DrawPlayer(data.snake);
                Output.DrawFood(data.Food);

                ActionType action = Input.AskForInput();

                //ActionType action = ActionType.Right;

                switch (action)
                {
                        case ActionType.Up:
                        data.direction = Direction.Up;
                        break;
                    case ActionType.Down:
                        data.direction = Direction.Down;
                        break;
                    case ActionType.Left:
                        data.direction = Direction.Left;
                        break;
                        case ActionType.Right:
                        data.direction = Direction.Right;
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
