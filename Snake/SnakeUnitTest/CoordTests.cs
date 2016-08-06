using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snake;

namespace SnakeUnitTest
{
    [TestClass]
    public class CoordTests
    {
        [TestMethod]
        public void CoordTest()
        {
            Coord testCoord = new Coord(0,0);
            Assert.IsNotNull(testCoord);
            Assert.IsNotNull(testCoord.X);
            Assert.IsNotNull(testCoord.Y);

            Assert.IsInstanceOfType(testCoord,typeof(Coord));
        }

        [TestMethod]
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

        [TestMethod]
        public void EqualsTest1()
        {
            object obj2 = null;
            object obj1 = null;

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

        [TestMethod]
        public void GetHashCodeTest()
        {
            var coord = new Coord(0,0);

            var hash = coord.GetHashCode();
            Assert.IsNotNull(hash);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var coord = new Coord(0,0);

            var line = coord.ToString();

            Assert.IsNotNull(line);
        }
    }
}