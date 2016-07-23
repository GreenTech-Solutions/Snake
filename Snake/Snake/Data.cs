using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Data
    {
        /// <summary>
        /// Размер змеи
        /// </summary>
        public int Size = 3;

        /// <summary>
        /// Игровое поле
        /// </summary>
        public int[,] map;

        /// <summary>
        /// Координаты ячеек змеи
        /// </summary>
        public LinkedList<Coord> snake;

        /// <summary>
        /// Размер карты
        /// </summary>
        public Coord MapSize;

        public Direction direction;

        /// <summary>
        /// Координаты хвоста змеи
        /// </summary>
        public Coord Tail;

        public Coord Food;

        public bool CollidedWthFood;

        public bool FoodEaten = true;


        private int score = 0;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                ScoreChanged?.Invoke();
            }
        }

        public delegate void ValueChanged();
        public event ValueChanged ScoreChanged;

        public int HighScore;
    }
}
