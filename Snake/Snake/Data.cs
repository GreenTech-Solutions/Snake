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
        public static Coord[] snake;

        /// <summary>
        /// Размер карты
        /// </summary>
        public static Coord MapSize = new Coord(10,10);

        public static Direction direction;

        public static Coord Tail;

        public int Score;

        public int HighScore;
    }
}
