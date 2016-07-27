using System;
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
                    Console.SetCursorPosition(j, i);
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
                Console.SetCursorPosition(cell.Value.X, cell.Value.Y);
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
            Console.SetCursorPosition(cell.X,cell.Y);
            Console.Write(MapCell);

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает еду на координатах, хранящихся в Data.Food
        /// </summary>
        static public void DrawFood(Coord coords)
        {
            if (coords == null) throw new ArgumentNullException(nameof(coords));

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(coords.X,coords.Y);
            Console.Write(Food);

            Console.ResetColor();
        }

        static public void DrawScores(int score, Coord mapSize)
        {
            int x = mapSize.X;
            int y = mapSize.Y;
            Console.SetCursorPosition(x + 1, 0);
            Console.Write($"{score}pts");
        }

        static public void DrawGameover()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game over!\n");
            Console.Write("Quit to main menu..");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
