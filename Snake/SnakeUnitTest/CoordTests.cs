using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snake;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Tests
{
    [TestClass()]
    public class CoordTests
    {
        [TestMethod()]
        public void CoordTest()
        {
            Coord testCoord = new Coord(0,0);
            Assert.IsNotNull(testCoord);
            Assert.IsNotNull(testCoord.X);
            Assert.IsNotNull(testCoord.Y);

            Assert.IsInstanceOfType(testCoord,typeof(Coord));
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Coord obj1 = new Coord(0,0);
            Coord obj2 = new Coord(0,0);

            if (!obj1.Equals(obj2) | obj1 != obj2)
            {
                Assert.Fail();
            }

            obj2 = new Coord(1,1);
            if (obj1.Equals(obj2) | obj1 == obj2)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void EqualsTest1()
        {
            object obj1 = new Coord(0, 0);
            object obj2 = null;
            obj1 = null;

            if (!Equals(obj1, obj2))
            {
                Assert.Fail();
            }

            obj1 = new Coord(0, 0);

            if (Equals(obj1,obj2))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Coord coord = new Coord(0,0);

            int hash = coord.GetHashCode();
            Assert.IsNotNull(hash);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Coord coord = new Coord(0,0);

            string line = coord.ToString();

            Assert.IsNotNull(line);
        }
    }
}