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
        /// Интерпретирует нажатую клавишу и возвращает информациию о действии
        /// </summary>
        /// <param name="kInfo">Информация о клавише</param>
        public static ActionType KeyHandler(ConsoleKeyInfo kInfo)
        {
            switch (kInfo.KeyChar.ToString().ToLower())
            {
                case "d":
                    return ActionType.Right;
                case "a":
                    return ActionType.Left;
                case "w":
                    return ActionType.Up;
                case "s":
                    return ActionType.Down;
                case "q":
                    return ActionType.Exit;
                default:
                    return ActionType.None;
            }
        }

        /// <summary>
        /// Ожидает нажатия клавиши и возвращает ассоциативный тип действия
        /// </summary>
        /// <returns></returns>
        public static ActionType AskForInput()
        {
            ConsoleKeyInfo kInfo = Console.ReadKey(true);
            return KeyHandler(kInfo);
        }
    }
}
