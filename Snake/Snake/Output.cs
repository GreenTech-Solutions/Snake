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

            // Рисование игрового поля
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write('-');
                }
            }
        }

        /// <summary>
        /// Отрисовывает игрока, с помощью указанных координат
        /// </summary>
        static public void DrawPlayer(LinkedList<Coord> playerCoords)
        {
            if (playerCoords == null) throw new ArgumentNullException(nameof(playerCoords));

            // Рисование змеи
            var cell = playerCoords.Last;
            while (cell!=null)
            {
                Console.SetCursorPosition(cell.Value.X, cell.Value.Y);
                Console.Write(cell == playerCoords.First ? '©' : '*');
                cell = cell.Previous;
            }
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
            Console.SetCursorPosition(cell.X,cell.Y);
            Console.Write('-');
        }

        /// <summary>
        /// Отрисовывает еду на координатах, хранящихся в Data.Food
        /// </summary>
        static public void DrawFood(Coord coords)
        {
            if (coords == null) throw new ArgumentNullException(nameof(coords));
            Console.SetCursorPosition(coords.X,coords.Y);
            Console.Write('+');
        }

        static public void DrawScores(int score, Coord mapSize)
        {
            int x = mapSize.X;
            int y = mapSize.Y;
            Console.SetCursorPosition(x + 1, 0);
            Console.Write($"{score}pts");
        }
    }
}
