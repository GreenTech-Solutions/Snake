using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Functions
    {
        /// <summary>
        /// Добавляет точку в начало списка и удаляет с конца
        /// </summary>
        /// <param name="coord">Координаты</param>
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
        public static Coord GenerateFood(LinkedList<Coord> playerCoords, Coord MapSize)
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int randomNumber = r.Next(1, (MapSize.X*MapSize.Y) - playerCoords.Count);

            int h = 0;
            for (int i = 0; i < MapSize.Y; i++)
            {
                for (int j = 0; j < MapSize.X; j++)
                {
                    if (!playerCoords.Contains(new Coord(j, i)))
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


        public static void Grow(ref LinkedList<Coord> playerCoords, Coord foodCoords)
        {
            playerCoords.AddLast(foodCoords);
        }
    }
}
