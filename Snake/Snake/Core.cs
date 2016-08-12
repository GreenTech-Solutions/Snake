using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading;

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
        public void Check()
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
        public void Do()
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
        /// Возвращает настройки приложения
        /// </summary>
        /// <returns>Настроки приложения</returns>
        public NameValueCollection GetAppSettings()
        {
            return ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// Получает количество очков
        /// </summary>
        /// <returns>Количество очков</returns>
        public int GetScore()
        {
            return _data.Score;
        }

        /// <summary>
        /// Устанавливает количество очков, анализируя данные
        /// </summary>
        public void SetScore()
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

        public void Initialize()
        {
            #region Инициализация

            Level level;

            var settings = GetAppSettings();

            var sizeOfMap = new Coord(Convert.ToInt32(settings["MapWidth"]), Convert.ToInt32(settings["MapHeight"]));

            // Выделение памяти для карты и змеи
            var map = new List<Cell>();
            _data.Snake = new LinkedList<Coord>();

            // Создание змеи
            for (var i = 0; i < 3; i++)
            {
                _data.Snake.AddFirst(new Coord( /* TODO Значение выставляется не здесь */i + 3, 0));
            }

            _data.Tail = _data.Snake.Last.Value;

            // Заполнение карты
            for (var i = 0; i < sizeOfMap.Y; i++)
            {
                for (var j = 0; j < sizeOfMap.X; j++)
                {
                    map.Add(new Cell(CellType.Empty, j, i));
                }
            }

            CellsInfo cellsInfo = new CellsInfo(map,sizeOfMap.X,sizeOfMap.Y);

            _data.Food = Functions.GenerateFood(_data.Snake, sizeOfMap, map);

            _data.ScoreChanged += () => { Output.DrawScores(_data.Score, sizeOfMap); };

            CollidedWithFood += delegate
            {
                var apple = new Music(new Audio(Resources.apple));
                apple.PlayOnce();
                _data.CollidedWthFood = true;
                SetScore();
                _data.FoodForEating = _data.Food;
                _data.Food = Functions.GenerateFood(_data.Snake, sizeOfMap, map);
            };

            CollidedWithObstacle += delegate
            {
                var lose = new Music(new Audio(Resources.lose));
                lose.PlayOnce();
                Output.DrawGameover(sizeOfMap,false);
                _canExit = true;
            };

            _data.FinishingScoreReached += () =>
            {
                Output.DrawGameover(sizeOfMap, true);
                _canExit = true;
            };

            _data.SetSpeed(1);

            var backgroundMusic = new Audio(nameof(Resources.InGame),Resources.InGame);

            level = new Level(cellsInfo,int.MaxValue,_data.Direction,backgroundMusic);

            level.FinishingScore = Int32.MaxValue;

            _data.Level = level;

            _music = new Music(level.BackgroundMusic);

            #endregion
        }

        private Music _music;

        /// <summary>
        /// Начать игровой цикл
        /// </summary>
        public void Start()
        {
            var input = new Input();

            Output.Clear();
            Output.DrawMap(_data.Level.MapSize, _data.Level.MapCellsInfo.cells);
            Output.DrawScores(_data.Score, _data.Level.MapSize);

            _music.PlayLoop();

            while (true)
            {
                if (_canExit)
                {
                    _music.Stop();
                    return;
                }

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
            }
        }

        /// <summary>
        /// Начать игровой цикл с указанным уровнем
        /// </summary>
        /// <param name="level">Объект уровня</param>
        public void Start(Level level)
        {
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

            CollidedWithFood += delegate
            {
                var apple = new Music(new Audio(Resources.apple));
                apple.PlayOnce();
                _data.CollidedWthFood = true;
                SetScore();
                _data.FoodForEating = _data.Food;
                _data.Food = Functions.GenerateFood(_data.Snake, sizeOfMap, level.MapCellsInfo.cells);
            };

            CollidedWithObstacle += delegate
            {
                var lose = new Music(new Audio(Resources.lose));
                lose.PlayOnce();
                Output.DrawGameover(sizeOfMap,false);
                _canExit = true;
            };

            _data.FinishingScoreReached += () =>
            {
                Output.DrawGameover(sizeOfMap, true);
                _canExit = true;
            };

            _data.SetSpeed(1);

            _data.Direction = level.Direction;

            _music = new Music(level.BackgroundMusic);

            Start();
        }
    }
}
