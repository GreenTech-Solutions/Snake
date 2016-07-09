using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Класс для представления
    /// </summary>
    class Output
    {
        // TODO передавать в класс экземпляр Data для локального использования, во время приёма проверять его на валидность
        // Тогда не придётся отдельно проверять все поля

        /// <summary>
        /// Отрисовывает карту, находящуюся в хранилище
        /// </summary>
        /// TODO проверить ввод
        static public void DrawMap()
        {
            // Рисование игрового поля
            for (int i = 0; i < Data.MapSize.Y; i++)
            {
                for (int j = 0; j < Data.MapSize.X; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write('-');
                }
            }
        }

        /// <summary>
        /// Отрисовывает игрока, с помощью координат в Data.Snake
        /// </summary>
        /// TODO Проверка ввода
        static public void DrawPlayer()
        {
            // Рисование змеи
            LinkedListNode<Coord> cell = Data.snake.Last;
            while (cell!=null)
            {
                Console.SetCursorPosition(cell.Value.X, cell.Value.Y);
                Console.Write(cell == Data.snake.First ? '©' : '*');
                cell = cell.Previous;
            }
        }

        /// <summary>
        /// Отображает на указанных координатах знак -
        /// </summary>
        /// <param name="cell">Координаты</param>
        /// TODO Проверка ввода
        static public void RedrawMapcell(Coord cell)
        {
            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }
            Console.SetCursorPosition(cell.X,cell.Y);
            Console.Write('-');
        }

        /// <summary>
        /// Отрисовывает еду на координатах, хранящихся в Data.Food
        /// </summary>
        /// TODO Проверка ввода
        static public void DrawFood()
        {
            Console.SetCursorPosition(Data.Food.X,Data.Food.Y);
            Console.Write('+');
        }
    }
}
