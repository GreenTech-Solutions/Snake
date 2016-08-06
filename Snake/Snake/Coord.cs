using System;

namespace Snake
{
    public class Coord : IEquatable<Coord>
    {
        public int X { get; }

        public int Y { get; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            // Сравнение с null всегда возвращает false
            if (obj == null)
                return false;
            // Если сравниваемые объекты имеют разный тип, равенство не верно
            if (obj.GetType() != GetType())
                return false;
            // Вызываем специфический метод сравнения
            return (((Coord) obj).X == X) & (((Coord) obj).Y == Y);
        }

        // Реализация интерфейса IEquatable<T>
        public bool Equals(Coord other)
        {
            if (other == null) return false;
            // Сравниваем поля по-одному
            return X == other.X & Y == other.Y;
        }

        public override int GetHashCode()
        {
            string s = string.Concat(X.ToString(), Y.ToString());
            return s.GetHashCode();
        }

        public static bool operator ==(Coord obj1, Coord obj2)
        {
            if (Equals(obj1,null) && Equals(obj2,null))
            {
                return true;
            }
            if (Equals(obj1, null) || Equals(obj2, null))
            {
                return false;
            }
            if (obj1.X.Equals(obj2.X) && obj1.Y.Equals(obj2.Y))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Coord obj1, Coord obj2)
        {
            if (obj1 == null) throw new ArgumentNullException(nameof(obj1));
            if (obj2 == null) throw new ArgumentNullException(nameof(obj2));
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            return $"({X};{Y})";
        }

        public static bool operator true(Coord obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return true;
        }

        public static bool operator false(Coord obj)
        {
            return obj==null;
        }
    }
}
