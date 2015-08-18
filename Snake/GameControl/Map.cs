using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameControl
{
    internal class Map
    {
        Dictionary<Coord, Image> map = new Dictionary<Coord, Image>();

        public Map()
        {
            Image img = null;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        map.Add(new Coord(j, i), img);
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
            map[xy] = img;
        }
    }
}
