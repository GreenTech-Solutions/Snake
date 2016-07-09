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
        /// Стирает введённый символ на координатах 10,10
        /// </summary>
        /// TODO Избавиться вовсе или перенести в Output для автоматического выполнения
        public static void Clear()
        {
            Console.SetCursorPosition(10, 10);
            Console.Write(' ');
        }

        /// <summary>
        /// Добавляет точку в начало списка и удаляет с конца
        /// </summary>
        /// <param name="coord"></param>
        /// TODO Сделать проверку на неверные координаты
        public static void Move(Coord coord)
        {
            Data.snake.AddFirst(coord);
            Data.snake.RemoveLast();
        }

        /// <summary>
        /// Генерирует новую клетку с едой и помещает её на карту
        /// </summary>
        /// TODO Проверить реализацию
        public static void GenerateFood()
        {
            int x = Data.MapSize.X;
            int y = Data.MapSize.Y;
            Random r = new Random(DateTime.Now.Millisecond);
            x = r.Next(0, x - 1);
            y = r.Next(0, y - 1);
            Data.Food = new Coord(x,y);

            LinkedListNode<Coord> cell = Data.snake.Last;
            while (cell!=null)
            {
                if (cell.Value == Data.Food)
                {
                    GenerateFood();
                    return;
                }
                cell = cell.Previous;
            }
            Data.FoodEaten = false;
        }

        /// <summary>
        /// ПРоверка на столкновение с едой
        /// </summary>
        /// <returns></returns>
        /// TODO Проверить правильно ли сравниваются координаты
        public static bool CollidedWthFood()
        {
            if (Equals(Data.snake.First.Value, Data.Food))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Обработчик для события CollidedWthFood
        /// </summary>
        /// TODO Переделать алгоритм роста, потому как этот работает неадекватно, взращивая змею как ему вздумается
        public static void OnCollidedWthFood()
        {
            bool CanGrow = false;
            Coord tail = Data.snake.Last.Value;
            Coord food = Data.Food;
            Coord temp;
            temp = new Coord(tail.X-1,tail.Y);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }
            temp = new Coord(tail.X+1,tail.Y);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }
            temp = new Coord(tail.X, tail.Y+1);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }
            temp = new Coord(tail.X, tail.Y-1);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }

            if (CanGrow)
            {
                Grow(tail);
            }
        }

        /// <summary>
        /// Добавляет указанную точку в хвост змеи
        /// </summary>
        /// <param name="point"></param>
        public static void Grow(Coord point)
        {
            Data.snake.AddLast(point);
            Data.CollidedWthFood = false;
            Data.FoodEaten = true;
        }
    }
}
