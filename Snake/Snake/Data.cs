using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool FoodEaten;

        public int Score;

        public int HighScore;
    }
}
