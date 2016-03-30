using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Functions
    {
        public static void Clear()
        {
            Console.SetCursorPosition(10, 10);
            Console.Write(' ');
        }

        public static void Move(Coord temp)
        {
            Data.snake.AddFirst(temp);
            Data.snake.RemoveLast();
        }

        public static void GenerateFood()
        {
            int x = Data.MapSize.x;
            int y = Data.MapSize.y;
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

        public static bool CollidedWthFood()
        {
            if (Equals(Data.snake.First.Value, Data.Food))
            {
                return true;
            }
            return false;
        }

        public static void OnCollidedWthFood()
        {
            bool CanGrow = false;
            Coord tail = Data.snake.Last.Value;
            Coord food = Data.Food;
            Coord temp;
            temp = new Coord(tail.x-1,tail.y);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }
            temp = new Coord(tail.x+1,tail.y);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }
            temp = new Coord(tail.x, tail.y+1);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }
            temp = new Coord(tail.x, tail.y-1);
            if (Equals(temp, food))
            {
                CanGrow = true;
            }

            if (CanGrow)
            {
                Grow(tail);
            }
        }

        public static void Grow(Coord temp)
        {
            Data.snake.AddLast(temp);
            Data.CollidedWthFood = false;
            Data.FoodEaten = true;
        }
    }
}
