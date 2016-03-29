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
            LinkedList<Coord> snake_ = new LinkedList<Coord>();

            snake_.AddFirst(temp);
            SnakeCell cell = Data.snake.Tail;
            Data.snake.Tail.Next = Data.snake.Tail;
            Data.snake.Tail.Value = temp;
            Data.snake.Head = Data.snake.Head.Prev;
        }

        public static void GenerateFood()
        {
            int x = Data.MapSize.x;
            int y = Data.MapSize.y;
            Random r = new Random(DateTime.Now.Millisecond);
            x = r.Next(0, x - 1);
            y = r.Next(0, y - 1);
            Data.Food = new Coord(x,y);

            SnakeCell cell = new SnakeCell(Data.snake.Tail.Value,null,Data.snake.Tail.Next);
            while (cell!=null)
            {
                if (cell.Value == Data.Food)
                {
                    GenerateFood();
                }
                cell = cell.Next;
            }
        }

        public static bool CollidedWthFood()
        {
            if (Data.snake.Head.Value == Data.Food)
            {
                return true;
            }
            return false;
        }
    }
}
