using System.Collections.Generic;

namespace Snake
{
    /// <summary>
    /// Игровое хранилище
    /// </summary>
    class Data
    {
        public Data()
        {
        }

        /// <summary>
        /// Размер змеи
        /// </summary>
        public int Size;

        /// <summary>
        /// Координаты ячеек змеи
        /// </summary>
        public LinkedList<Coord> Snake;

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
                if (_score == Level.FinishingScore)
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
        public void SetSpeed(int speed)
        {
            _speed = speed/10;
        }

        /// <summary>
        /// Получение значения скорости
        /// </summary>
        /// <returns>Скорость перемещения змеи</returns>
        public int GetSpeed()
        {
            return (int) _speed*10;
        }


        public delegate void ValueChanged();
        /// <summary>
        /// Происходит при изменении набранных очков
        /// </summary>
        public event ValueChanged ScoreChanged;

        public event ValueChanged FinishingScoreReached;

        public Level Level;
    }
}
