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
        public static int Size = 3;

        /// <summary>
        /// Игровое поле
        /// </summary>
        public static int[,] map;

        /// <summary>
        /// Координаты ячеек змеи
        /// </summary>
        public static LinkedList<Coord> snake;

        /// <summary>
        /// Размер карты
        /// </summary>
        public static Coord MapSize = new Coord(10,10);

        public static Direction direction;

        /// <summary>
        /// Координаты хвоста змеи
        /// </summary>
        public static Coord Tail;

        public static Coord Food;

        public static bool CollidedWthFood;

        public static bool FoodEaten = true;


        private static int score = 0;
        public static int Score
        {
            get { return score; }
            set
            {
                score = value;
                ScoreChanged?.Invoke();
            }
        }

        public delegate void ValueChanged();
        public static event ValueChanged ScoreChanged;

        public int HighScore;
    }
}
