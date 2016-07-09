using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Input
    {
        /// <summary>
        /// Определяет Direction в Data относительно переданной информации о клавише
        /// </summary>
        /// <param name="kInfo">Информация о клавише</param>
        /// TODO из-за такой хромой реализации можно нажимать любые кнопки, сделать контроль ввода
        public static void KeyHandler(ConsoleKeyInfo kInfo)
        {
            if (kInfo.KeyChar == 'd')
            {
                Data.direction = Direction.Right;
            }
            if (kInfo.KeyChar == 'a')
            {
                Data.direction = Direction.Left;

            }
            if (kInfo.KeyChar == 's')
            {
                Data.direction = Direction.Down;

            }
            if (kInfo.KeyChar == 'w')
            {
                Data.direction = Direction.Up;

            }


            if (kInfo.KeyChar == 'q')
            {
                Environment.Exit(0);
            }
        }
    }
}
