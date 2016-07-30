﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Класс для представления
    /// </summary>
    class Output
    {
        private static char SnakeHead = (char)64;

        private static char SnakeBody = (char)42;

        private static char MapCell = (char)183;

        private static char Food = (char)35;

        private static ConsoleColor BackGround = ConsoleColor.DarkGreen;

        static public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Отрисовывает карту указанного размера
        /// </summary>
        static public void DrawMap(Coord size)
        {
            if (size == null) throw new ArgumentNullException(nameof(size));


            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Black;

            // Рисование игрового поля
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {
                    Console.SetCursorPosition(j + Padding, i + PaddingTop);
                    Console.Write(MapCell);
                }
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает игрока, с помощью указанных координат
        /// </summary>
        static public void DrawPlayer(LinkedList<Coord> playerCoords)
        {
            if (playerCoords == null) throw new ArgumentNullException(nameof(playerCoords));

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Red;

            // Рисование змеи
            var cell = playerCoords.Last;
            while (cell!=null)
            {
                Console.SetCursorPosition(cell.Value.X + Padding, cell.Value.Y + PaddingTop);
                Console.Write(cell == playerCoords.First ? SnakeHead : SnakeBody);
                cell = cell.Previous;
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Отображает на указанных координатах знак -
        /// </summary>
        /// <param name="cell">Координаты</param>
        static public void RedrawMapcell(Coord cell)
        {
            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(cell.X + Padding,cell.Y + PaddingTop);
            Console.Write(MapCell);

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает еду на указанных координатах
        /// </summary>
        static public void DrawFood(Coord coords)
        {
            if (coords == null) throw new ArgumentNullException(nameof(coords));

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(coords.X + Padding,coords.Y + PaddingTop);
            Console.Write(Food);

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает набранные очки
        /// </summary>
        /// <param name="score"></param>
        /// <param name="mapSize"></param>
        static public void DrawScores(int score, Coord mapSize)
        {
            int x = mapSize.X;
            int y = mapSize.Y;
            Console.SetCursorPosition(x + 1 + Padding, 0 + PaddingTop);
            Console.Write($"{score}pts");
        }

        /// <summary>
        /// Отрисовывает экран конца игры
        /// </summary>
        static public void DrawGameover(Coord MapSize)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(MapSize.X + 2 + Padding, 3 + PaddingTop);
            Console.Write("Game over!");

            Console.SetCursorPosition(MapSize.X + 2 + Padding, 4 + PaddingTop);
            Console.Write("Quit to main menu..");
            Console.ResetColor();
            Console.ReadKey();
        }

        static public void WriteLineCenter(string text)
        {
            var width = Console.WindowWidth;
            var padding = width / 2 + text.Length / 2;
            Console.SetCursorPosition(Padding,Console.CursorTop);
            Console.WriteLine(text);
        }

        static public void WriteCenter(string text)
        {
            var width = Console.WindowWidth;
            var padding = width / 2 + text.Length / 2;
            Console.SetCursorPosition(Padding,Console.CursorTop);
            Console.Write(text);
        }

        static public int Padding { get
            {
                var width = Console.WindowWidth;
                return width *3/ 8;
            } }

        public static int PaddingTop
        {
            get
            {
                var height = Console.WindowHeight;
                return height * 1/4;
            }
        }
    }
}
