using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Output
    {
        static public void DrawMap()
        {
            // Рисование игрового поля
            for (int i = 0; i < Data.MapSize.y; i++)
            {
                for (int j = 0; j < Data.MapSize.x; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write('-');
                }
            }
        }

        static public void DrawPlayer()
        {
            // Рисование змеи
            for (int i = 0; i < Data.Size; i++)
            {
                Console.SetCursorPosition(Data.snake[i].x, Data.snake[i].y);
                if (i == Data.Size - 1)
                {
                    Console.Write('©');
                }
                else
                {
                    Console.Write('*');
                }
            }
        }

        static public void RedrawMapcell(Coord cell)
        {
            if (cell == null)
            {
                return;
            }
            Console.SetCursorPosition(cell.x,cell.y);
            Console.Write('-');
        }
    }
}
