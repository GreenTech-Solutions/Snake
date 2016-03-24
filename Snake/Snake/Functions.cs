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
            for (int i = 0; i < Data.Size; i++)
            {
                if (i == Data.Size - 1)
                {
                    Data.snake[i] = temp;
                }
                else
                {
                    Data.snake[i] = Data.snake[i + 1];
                }
            }
        }
    }
}
