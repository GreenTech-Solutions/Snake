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

        static void Main(string[] args)
        {
            Core core = new Core();
            core.Main();
        }
    }
}
