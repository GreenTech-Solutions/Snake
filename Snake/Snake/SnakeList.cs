using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class SnakeList
    {
        public SnakeCell Head;
        public SnakeCell Tail;

        LinkedList<SnakeCell> snake;

        public int count = 0;


        public SnakeList()
        {
            snake = new LinkedList<SnakeCell>();
        }

        public void AddToBegin(Coord value)
        {
            if (count == 0)
            {
                Tail = new SnakeCell(value);
                Head = new SnakeCell(value);
                Tail.Next = Head;
                Head.Prev = Tail;
            }
            else if (count == 1)
            {
                Head.Value = value;
            }
            else
            {
                Head.Prev = Head;
                Head.Value = value;
                Head.Prev.Next= Head;
            }
            count++;
        }
    }
}
