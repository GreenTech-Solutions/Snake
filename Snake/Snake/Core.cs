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
            List<Coord> obstaclesList = new List<Coord>();

            foreach (var cell in _data.Map)
            {
                if (cell.CellType == CellType.Brick)
                {
                    obstaclesList.Add(new Coord(cell.Y, cell.X));
                }
            }
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

            var maxX = _data.MapSize.X-1;
            var maxY = _data.MapSize.Y-1;

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

            var settings = GetAppSettings();

            var sizeOfMap = new Coord(Convert.ToInt32(settings["MapWidth"]), Convert.ToInt32(settings["MapHeight"]));
            _data.MapSize = sizeOfMap;

            // Выделение памяти для карты и змеи
            _data.Map = new Cell[_data.MapSize.X, _data.MapSize.Y];
            _data.Snake = new LinkedList<Coord>();

            // Создание змеи
            for (var i = 0; i < _data.Size; i++)
            {
                _data.Snake.AddFirst(new Coord( /* TODO Значение выставляется не здесь */i + 3, 0));
            }

            _data.Tail = _data.Snake.Last.Value;

            // Заполнение карты
            for (var i = 0; i < _data.MapSize.Y; i++)
            {
                for (var j = 0; j < _data.MapSize.X; j++)
                {
                    _data.Map[j, i] = new Cell(CellType.Empty, j,i);
                }
            }

            _data.Food = Functions.GenerateFood(_data.Snake, _data.MapSize, _data.Map);

            _data.ScoreChanged += () => { Output.DrawScores(_data.Score, _data.MapSize); };

            var apple = new Music(new Audio(Resources.apple));
            CollidedWithFood += delegate
            {
                apple.PlayOnce();
                _data.CollidedWthFood = true;
                SetScore();
                _data.FoodForEating = _data.Food;
                _data.Food = Functions.GenerateFood(_data.Snake, _data.MapSize, _data.Map);
            };

            var lose = new Music(new Audio(Resources.lose));
            CollidedWithObstacle += delegate
            {
                lose.PlayOnce();
                Output.DrawGameover(_data.MapSize);
                _canExit = true;
            };

            _data.SetSpeed(1);

            #endregion
        }

        /// <summary>
        /// Начать игровой цикл
        /// </summary>
        public void Start()
        {
            var input = new Input();

            Output.Clear();
            Output.DrawMap(_data.MapSize, _data.Map);
            Output.DrawScores(_data.Score, _data.MapSize);

            while (true)
            {
                if (_canExit)
                {
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
            _data.MapSize = new Coord(level.MapWidth,level.MapHeight);

            // Выделение памяти для карты и змеи
            _data.Map = new Cell[_data.MapSize.X, _data.MapSize.Y];
            _data.Snake = new LinkedList<Coord>();


            foreach (var playerInitCoord in level.PlayerInitCoords)
            {
                _data.Snake.AddFirst(playerInitCoord);
            }

            _data.Tail = _data.Snake.Last.Value;

            // Заполнение карты
            for (var i = 0; i < _data.MapSize.Y; i++)
            {
                for (var j = 0; j < _data.MapSize.X; j++)
                {
                    _data.Map[j,i] = new Cell(level.CellsInfo.cells.FindAll(e => e.X == j && e.Y == i)[0].CellType,j,i);
                }
            }

            _data.Food = Functions.GenerateFood(_data.Snake, _data.MapSize, _data.Map);

            _data.ScoreChanged += () => { Output.DrawScores(_data.Score, _data.MapSize); };

            var apple = new Music(new Audio(Resources.apple));
            CollidedWithFood += delegate
            {
                apple.PlayOnce();
                _data.CollidedWthFood = true;
                SetScore();
                _data.FoodForEating = _data.Food;
                _data.Food = Functions.GenerateFood(_data.Snake, _data.MapSize, _data.Map);
            };

            var lose = new Music(new Audio(Resources.lose));
            CollidedWithObstacle += delegate
            {
                lose.PlayOnce();
                Output.DrawGameover(_data.MapSize);
                _canExit = true;
            };

            _data.SetSpeed(1);

            _data.Direction = level.Direction;

            Start();
        }
    }
}
