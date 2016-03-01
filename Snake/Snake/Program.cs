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
        static void Main(string[] args)
        {
            ConsoleKeyInfo kInfo;

            while (true)
            {
                Console.WriteLine("Hello world\n");
                if (Console.ReadKey().KeyChar == 'Q')
                {
                    Environment.Exit(0);
                }
            }
            
        }
    }
}
