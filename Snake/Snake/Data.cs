using System.Collections.Generic;

namespace Snake
{
    /// <summary>
    /// Игровое хранилище
    /// </summary>
    class Data
    {
        /// <summary>
        /// Размер змеи
        /// </summary>
        public int Size = 3;

        /// <summary>
        /// Игровое поле
        /// </summary>
        public Cell[,] Map;

        /// <summary>
        /// Координаты ячеек змеи
        /// </summary>
        public LinkedList<Coord> Snake;

        /// <summary>
        /// Размер карты
        /// </summary>
        public Coord MapSize;

        /// <summary>
        /// Направление движения
        /// </summary>
        public Direction Direction = Direction.Right;

        /// <summary>
        /// Координаты хвоста змеи
        /// </summary>
        public Coord Tail;

        /// <summary>
        /// Координаты пищи
        /// </summary>
        public Coord Food;

        /// <summary>
        /// Еда, которую змея начала поглощать
        /// </summary>
        public Coord FoodForEating;

        /// <summary>
        /// Указывает произошло ли столкновение с пищей
        /// </summary>
        public bool CollidedWthFood;

        /// <summary>
        /// Указывает, была ли съедена предыдущая еда(первое значение true)
        /// </summary>
        public bool FoodEaten = true;


        private int _score;
        /// <summary>
        /// Количество набранных очков
        /// </summary>
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                ScoreChanged?.Invoke();
                if (_score == FinishingScore)
                {
                    FinishingScoreReached?.Invoke();
                }
            }
        }

        /// <summary>
        /// Скорость перемещения змеи
        /// </summary>
        private double _speed;

        /// <summary>
        /// Установка значения скорости
        /// </summary>
        /// <param name="speed">Новое значение скорости</param>
        public void SetSpeed(double speed)
        {
            _speed = speed*100;
        }

        /// <summary>
        /// Получение значения скорости
        /// </summary>
        /// <returns>Скорость перемещения змеи</returns>
        public int GetSpeed()
        {
            return (int) _speed;
        }


        public delegate void ValueChanged();
        /// <summary>
        /// Происходит при изменении набранных очков
        /// </summary>
        public event ValueChanged ScoreChanged;

        public event ValueChanged FinishingScoreReached;

        public int FinishingScore;

        public Music BackgroundMusic;
    }
}
