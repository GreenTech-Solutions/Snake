using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Snake
{
    /// <summary>
    /// Главный модуль
    /// </summary>
    class Core
    {
        /// <summary>
        /// Экземпляр хранилища
        /// </summary>
        private readonly Data _data = new Data();

        /// <summary>
        /// Указывает может ли модуль завершить свою работу при возможности
        /// </summary>
        private bool _canExit;

        public Nullable<bool> IsAlive; 

        delegate void Collided(params dynamic []args);

        /// <summary>
        /// Происходит при Столкновении с пищей
        /// </summary>
        private event Collided CollidedWithFood;

        /// <summary>
        /// Происходит при столкновении с собственным телом или другим препятствием
        /// </summary>
        private event Collided CollidedWithObstacle;

        /// <summary>
        /// Проверка возможности поворота змеи, проверка проигрыша и съедания еды
        /// </summary>
        /// <returns></returns>
        private void Check()
        {
            List<Coord> obstaclesList = (from Cell cell in _data.Level.MapCellsInfo.cells where cell.CellType == CellType.Brick select new Coord(cell.X, cell.Y)).ToList();

            //Все кроме самой головы сталкиваются с частями змеи
            if (_data.Snake.SkipWhile(value => value==_data.Snake.First.Value).Any(coord => coord == _data.Snake.First.Value))
            {
                CollidedWithObstacle?.Invoke();
            }
            else if (_data.Snake.Any(coord => obstaclesList.Any(item => item == coord)))
            {
                CollidedWithObstacle?.Invoke();
            }

            if (_data.Snake.First.Value == _data.Food)
            {
                CollidedWithFood?.Invoke();
            }

            if (_data.CollidedWthFood)
            {
                if (_data.Tail == _data.FoodForEating)
                {
                    _data.FoodEaten = false;
                }
            }
        }

        /// <summary>
        /// Установка координат змеи, изменение скорости, движение
        /// </summary>
        private void Do()
        {
            // Координаты головы
            var x = _data.Snake.First.Value.X;
            var y = _data.Snake.First.Value.Y;

            var maxX = _data.Level.MapSize.X-1;
            var maxY = _data.Level.MapSize.Y-1;

            Coord temp;
            _data.Tail = _data.Snake.Last.Value;

            switch (_data.Direction)
            {
                case Direction.Right:
                    x++;
                    if (x > maxX)
                    {
                        x = 0;
                    }
                    temp = new Coord(x, y);
                    Functions.AddNewHead(temp, _data.Snake);
                    break;
                case Direction.Left:
                    x--;
                    if (x < 0)
                    {
                        x = maxX;
                    }
                    temp = new Coord(x, y);
                    Functions.AddNewHead(temp, _data.Snake);
                    break;
                case Direction.Down:
                    y++;
                    if (y > maxY)
                    {
                        y = 0;
                    }
                    temp = new Coord(x, y);
                    Functions.AddNewHead(temp,_data.Snake);
                    break;
                case Direction.Up:
                    y--;
                    if (y < 0)
                    {
                        y = maxY;
                    }
                    temp = new Coord(x, y);
                    Functions.AddNewHead(temp,_data.Snake);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Устанавливает количество очков, анализируя данные
        /// </summary>
        private void SetScore()
        {
            _data.Score = (_data.Snake.Count - 2)*10;
        }

        /// <summary>
        /// Корректирует направление движения, предотвращая разворот на 180
        /// </summary>
        /// <param name="newDirection">Новое напрвление</param>
        /// <returns>Скорректированное направление</returns>
        private Direction EvaulateDirection(Direction newDirection)
        {
            var oldDirection = _data.Direction;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(newDirection), newDirection, null);
            }

            return newDirection;
        }

        /// <summary>
        /// Начать игровой цикл
        /// </summary>
        private void Start()
        {
            Output.Clear();
            Output.DrawMap(_data.Level.MapSize, _data.Level.MapCellsInfo.cells);
            Output.DrawScores(_data.Score, _data.Level.MapSize);

            MusicManager.Play(_data.Level.BackgroundMusic.Name,SoundType.Music);

            //Timer timer = new Timer(60000);

            //timer.Elapsed += OnTimerElapsed;
            //timer.Start();
            while (true)
            {
                GameCycle();

                if (_canExit)
                {
                    MusicManager.StopAll();
                    return;
                }
            }
        }

        // TODO Экспериментальная функция нового игрового цикла
        private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            var timer = sender as Timer;
            timer.Interval = 60000;
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            Output.DrawPlayer(_data.Snake);
            Output.DrawFood(_data.Food);
        }

        Input input = new Input();

        private void GameCycle()
        {
            if (_data.FoodEaten)
            {
                Output.RedrawMapcell(_data.Tail);
            }
            else
            {
                _data.FoodEaten = true;
                _data.CollidedWthFood = false;
                Functions.Grow(ref _data.Snake, _data.FoodForEating);
            }

            Output.DrawPlayer(_data.Snake);
            Output.DrawFood(_data.Food);

            var action = input.AskForInput();

            switch (action)
            {
                case ActionType.Up:
                    _data.Direction = EvaulateDirection(Direction.Up);
                    break;
                case ActionType.Down:
                    _data.Direction = EvaulateDirection(Direction.Down);
                    break;
                case ActionType.Left:
                    _data.Direction = EvaulateDirection(Direction.Left);
                    break;
                case ActionType.Right:
                    _data.Direction = EvaulateDirection(Direction.Right);
                    break;
                case ActionType.Exit:
                    _canExit = true;
                    break;
                case ActionType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Do();
            Check();
            Thread.Sleep(_data.GetSpeed());
            //Output.WriteInfo(Utility.CalculateFrameRate().ToString());
        }

        /// <summary>
        /// Возвращает настройки приложения
        /// </summary>
        /// <returns>Настроки приложения</returns>
        private NameValueCollection GetAppSettings()
        {
            return ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// Начать игровой цикл с указанным уровнем
        /// </summary>
        /// <param name="level">Объект уровня</param>
        public void Start(Level level)
        {
            if (Equals(level, null))
            {
                var settings = GetAppSettings();

                var _sizeOfMap = new Coord(Convert.ToInt32(settings["MapWidth"]), Convert.ToInt32(settings["MapHeight"]));

                List<Cell> cells = new List<Cell>();
                int width = _sizeOfMap.X;
                int height = _sizeOfMap.Y;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i == 0 && j < 3)
                        {
                            cells.Add(new Cell(CellType.Player, j,i));
                            continue;
                        }
                        cells.Add(new Cell(CellType.Empty, j,i));
                    }
                }

                int speed = Convert.ToInt32(settings["Speed"]);

                CellsInfo cellsInfo = new CellsInfo(cells,width,height);
                level = new Level(cellsInfo,int.MaxValue,Direction.Right, new Audio(Resources.InGame), speed);
            }

            var sizeOfMap = level.MapSize;

            // Выделение памяти для карты и змеи
            _data.Snake = new LinkedList<Coord>();

            foreach (var playerInitCoord in level.PlayerInitCoords)
            {
                _data.Snake.AddFirst(playerInitCoord);
            }

            _data.Tail = _data.Snake.Last.Value;

            _data.Food = Functions.GenerateFood(_data.Snake, level.MapSize, level.MapCellsInfo.cells);

            _data.ScoreChanged += () => { Output.DrawScores(_data.Score, sizeOfMap); };

            MusicManager.Add(new Audio("Apple", Resources.apple), SoundType.Effect);

            CollidedWithFood += delegate
            {
                MusicManager.Play("Apple",SoundType.Effect);
                _data.CollidedWthFood = true;
                SetScore();
                _data.FoodForEating = _data.Food;
                _data.Food = Functions.GenerateFood(_data.Snake, sizeOfMap, level.MapCellsInfo.cells);
            };

            MusicManager.Add(new Audio("Lose",Resources.lose), SoundType.Effect);

            CollidedWithObstacle += delegate
            {
                MusicManager.Play("Lose",SoundType.Effect);
                Output.DrawGameover(sizeOfMap,false);
                IsAlive = false;
                _canExit = true;
            };

            _data.FinishingScoreReached += () =>
            {
                Output.DrawGameover(sizeOfMap, true);
                IsAlive = true;
                _canExit = true;
            };

            _data.SetSpeed(level.Speed);

            _data.Direction = level.Direction;

            MusicManager.Add(level.BackgroundMusic,SoundType.Music);
            _data.Level = level;

            IsAlive = true;

            Start();
        }
    }

    public class Utility
    {
        #region Basic Frame Counter

        private static int lastTick;
        private static int lastFrameRate;
        private static int frameRate;

        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate/10;
        }
        #endregion

    }
}
