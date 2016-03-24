using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        static int Size = 3;
        private static int[,] map;
        private static Coord[] snake;
        static Coord MapSize = new Coord(10,10);

        enum KeyValue
        {
            right, down, left, up
        }

        static private KeyValue key;


        static void Main(string[] args)
        {
            ConsoleKeyInfo kInfo;
            map = new int[MapSize.x,MapSize.y];

            snake = new Coord[Size];

            for (int i = 0; i < Size; i++)
            {
                snake[i] = new Coord(i+3,0);
            }

            //Console.WriteLine("Snake\n");

            for (int i = 0; i < MapSize.y; i++)
            {
                //Console.Write('-');

                for (int j = 0; j < MapSize.x; j++)
                {
                    map[i,j] = 0;
                }
            }

            while (true)
            {
                int k = 0;

                for (int i = 0; i < MapSize.y; i++)
                {
                    for (int j = 0; j < MapSize.x; j++)
                    {
                        Console.SetCursorPosition(j,i);
                        Console.Write('-');
                    }
                }
                foreach (var cell in snake)
                {
                    Console.SetCursorPosition(cell.x, cell.y);
                    Console.Write('*');
                }
                for (int i = 0; i < Size; i++)
                {
                    Console.SetCursorPosition(snake[i].x, snake[i].y);
                    if (i == Size - 1)
                    {
                        Console.Write('©');
                    }
                    else
                    {
                        Console.Write('*');
                    }
                }

                Console.SetCursorPosition(10,10);
                kInfo = Console.ReadKey();
                Clear();

                int X = snake[Size - 1].x;
                int Y = snake[Size - 1].y;
                if (kInfo.KeyChar == 'd')
                {
                    X++;
                    if (X > 9)
                    {
                        X = 0;
                    }
                    Coord temp = new Coord(X,Y);
                    Move(temp);
                }
                if (kInfo.KeyChar == 'a')
                {
                    X--;
                    if (X < 0)
                    {
                        X = 9;
                    }
                    Coord temp = new Coord(X, Y);
                    Move(temp);
                }
                if (kInfo.KeyChar == 's')
                {
                    Y++;
                    if (Y > 9)
                    {
                        Y = 0;
                    }
                    Coord temp = new Coord(X, Y);
                    Move(temp);
                }
                if (kInfo.KeyChar == 'w')
                {
                    Y--;
                    if (Y < 0)
                    {
                        Y = 9;
                    }
                    Coord temp = new Coord(X, Y);
                    Move(temp);
                }


                if (kInfo.KeyChar == 'q')
                {
                    Environment.Exit(0);
                }
                //Console.Clear();
            }
            
        }

        public static void ClearTail()
        {
            Console.SetCursorPosition(snake[0].x,snake[0].y);
            Console.Write('-');
        }

        public static void ClearHead()
        {
            Console.SetCursorPosition(snake[Size-1].x, snake[Size-1].y);
            Console.Write('-');
        }

        static void Clear()
        {
            Console.SetCursorPosition(10,10);
            Console.Write(' ');
        }

        static void Move(Coord temp)
        {
            for (int i = 0; i < Size; i++)
            {
                if (i == Size - 1)
                {
                    snake[i] = temp;
                }
                else
                {
                    snake[i] = snake[i + 1];
                }
            }
        }
    }
}
