using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Input
    {
        public ConsoleKey UpKey;

        public ConsoleKey DownKey;

        public ConsoleKey LeftKey;

        public ConsoleKey RightKey;
        
        // TODO В дальнейшем создать отдельный класс с полями клавишами и передавать его в этот конструктор
        public Input(ConsoleKey upKey, ConsoleKey downKey, ConsoleKey leftKey, ConsoleKey rightKey)
        {
            UpKey = upKey;
            DownKey = downKey;
            LeftKey = leftKey;
            RightKey = rightKey;
        }

        public Input()
        {
            UpKey = Config.UpKey;
            DownKey = Config.DownKey;
            LeftKey = Config.LeftKey;
            RightKey = Config.RightKey;
        }

        /// <summary>
        /// Интерпретирует нажатую клавишу и возвращает информациию о действии
        /// </summary>
        /// <param name="kInfo">Информация о клавише</param>
        public ActionType KeyHandler(ConsoleKeyInfo kInfo)
        {
            if (kInfo.Key == RightKey)
            {
                return ActionType.Right;
            }
            else if (kInfo.Key == LeftKey)
            {
                return ActionType.Left;
            }
            else if (kInfo.Key == UpKey)
            {
                return ActionType.Up;
            }
            else if (kInfo.Key == DownKey)
            {
                return ActionType.Down;
            }
            else if (kInfo.Key == ConsoleKey.Escape)
            {
                return ActionType.Exit;
            }
            else
            {
                return ActionType.None;
            }
        }

        /// <summary>
        /// Ожидает нажатия клавиши и возвращает ассоциативный тип действия
        /// </summary>
        /// <returns></returns>
        public ActionType AskForInput()
        {
            ConsoleKeyInfo kInfo;
            if (Console.KeyAvailable == true)
            {
                kInfo = Console.ReadKey(true);
                return KeyHandler(kInfo);
            }
            else
            {
                return ActionType.None;
            }
        }
    }

    static class Config
    {
        public static ConsoleKey UpKey;

        public static ConsoleKey DownKey;

        public static ConsoleKey LeftKey;

        public static ConsoleKey RightKey;
    }
}
