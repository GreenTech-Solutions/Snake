using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameControl
{
    internal class Map
    {
        Dictionary<Coord, Image> _map = new Dictionary<Coord, Image>();

        public Map()
        {
            Image img = null;
            try
            {
                for (var i = 0; i < 10; i++)
                {
                    for (var j = 0; j < 10; j++)
                    {
                        _map.Add(new Coord(j, i), img);
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public void Add(Coord xy, Image img)
        {
            _map[xy] = img;
        }
    }
}
