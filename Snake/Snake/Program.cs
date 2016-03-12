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
        static int[] map = new int[10];
        private static int[] snake;

        static void Main(string[] args)
        {
            ConsoleKeyInfo kInfo;

            snake = new int[Size];
            
            


            for (int i = 0; i < Size; i++)
            {
                snake[i] = i + 3;
            }

            Console.WriteLine("Snake\n");

            for (int i = 0; i < 10; i++)
            {
                Console.Write('_');
                map[i] = 0;
            }

            while (true)
            {
                foreach (var cell in snake)
                {
                    Console.SetCursorPosition(cell,2);
                    Console.Write('*');
                }

                Console.SetCursorPosition(10,2);
                kInfo = Console.ReadKey();
                Clear();

                if (kInfo.KeyChar == 'd')
                {
                    ClearTail();
                    for (int i = 0; i < snake.Length; i++)
                    {
                        snake[i]++;
                        if (snake[i] > 9)
                        {
                            snake[i] = 0;
                        }
                    }
                }
                if (kInfo.KeyChar == 'a')
                {
                    ClearHead();
                    for (int i = 0; i < Size; i++)
                    {
                        snake[i]--;
                        if (snake[i] < 0)
                        {
                            snake[i] = 9;
                        }
                    }
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
            Console.SetCursorPosition(snake[0],2);
            Console.Write('_');
        }

        public static void ClearHead()
        {
            Console.SetCursorPosition(snake[Size-1], 2);
            Console.Write('_');
        }

        static void Clear()
        {
            Console.SetCursorPosition(10,2);
            Console.Write(' ');
        }
    }
}
