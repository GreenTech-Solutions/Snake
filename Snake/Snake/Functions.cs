using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    /// <summary>
    /// Копилка независимых функций для расчётов в игровой логике
    /// </summary>
    class Functions
    {
        /// <summary>
        /// Добавляет точку в начало списка и удаляет с конца
        /// </summary>
        /// <param name="coord">Координаты</param>
        /// <param name="playerCoords">Координаты игрока</param>
        public static void AddNewHead(Coord coord, LinkedList<Coord> playerCoords)
        {
            if (coord == null) throw new ArgumentNullException(nameof(coord));
            if (playerCoords == null) throw new ArgumentNullException(nameof(playerCoords));
            playerCoords.AddFirst(coord);
            playerCoords.RemoveLast();
        }

        /// <summary>
        /// Возвращает случайные координаты клетки с едой или null, если свободное место найти не удалось
        /// </summary>
        public static Coord GenerateFood(LinkedList<Coord> playerCoords, Coord mapSize, List<Cell> Map)
        {
            var r = new Random(DateTime.Now.Millisecond);

            List<Coord> obstaclesList = (from Cell cell in Map where cell.CellType == CellType.Brick select new Coord(cell.X, cell.Y)).ToList();

            var randomNumber = r.Next(1, (mapSize.X*mapSize.Y) - playerCoords.Count - obstaclesList.Count);

            var h = 0;

            for (var i = 0; i < mapSize.Y; i++)
            {
                for (var j = 0; j < mapSize.X; j++)
                {
                    Coord tempCoord = new Coord(j,i);
                    if (!playerCoords.Contains(tempCoord) && !obstaclesList.Contains(tempCoord))
                    {
                        h++;
                    }

                    if (h == randomNumber)
                    {
                        return new Coord(j,i);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Увеличивает размер змеи на одну клетку
        /// </summary>
        /// <param name="playerCoords">Координаты ячеек игрока</param>
        /// <param name="foodCoords">Координаты пищи</param>
        public static void Grow(ref LinkedList<Coord> playerCoords, Coord foodCoords)
        {
            playerCoords.AddLast(foodCoords);
        }
    }
}
