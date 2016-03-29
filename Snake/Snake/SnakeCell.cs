using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class SnakeCell
    {
        public SnakeCell Prev;
        public SnakeCell Next;

        public Coord Value;

        public SnakeCell(Coord v)
        {
            Value = v;
        }

        public SnakeCell(Coord v, SnakeCell p, SnakeCell n)
        {
            Value = v;
            Prev = p;
            Next = n;
        }

        public SnakeCell(SnakeCell cell)
        {
            Value = cell.Value;
            Next = cell.Next;
            Prev = cell.Prev;
        }
    }
}
