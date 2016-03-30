using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Coord : IEquatable<Coord>
    {
        public int x;
        public int y;

        public Coord(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public override bool Equals(object obj)
        {
            // Сравнение с null всегда возвращает false
            if (obj == null)
                return false;
            // Если сравниваемые объекты имеют разный тип, равенство не верно
            if (obj.GetType() != this.GetType())
                return false;
            // Вызываем специфический метод сравнения
            return Equals((Coord)obj);
        }

        // Реализация интерфейса IEquatable<T>
        public bool Equals(Coord other)
        {
            // Сравниваем поля по-одному
            return this.x == other.x
                && this.y == other.y;
        }

        public override int GetHashCode()
        {
            return (((int)x ^ (int)y) << 16) |
                (((int)x ^ (int)y) & 0x0000FFFF);
        }
        //public static bool operator ==(Coord obj1, Coord obj2)
        //{
        //    if (obj1 == null && obj2 == null)
        //    {
        //        return true;
        //    }
        //    if (obj1.x == obj2.x && obj1.y == obj2.y)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool operator !=(Coord obj1, Coord obj2)
        //{
        //    if (obj1 == null && obj2 == null)
        //    {
        //        return false;
        //    }
        //    if (obj1.x == obj2.x && obj1.y == obj2.y)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}
